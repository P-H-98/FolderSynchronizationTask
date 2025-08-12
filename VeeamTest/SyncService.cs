using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VeeamTest.Exceptions;

namespace VeeamTest
{
    /// <summary>
    /// Class <c>SyncService</c> brings functionalities needed to synchronize two directories together
    /// </summary>
    public class SyncService
    {
        private readonly Logger _logger;

        public SyncService(Logger logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Method <c>Synchronize</c> synchronizes two directories given as parameters
        /// </summary>
        /// <param name="sourcePath">Path to the source directory</param>
        /// <param name="replicaPath">Path to the replica directory</param>
        /// <exception cref="SourceDirectoryNotFoundException">Thrown when source directory does not exist</exception>
        public void Synchronize(string sourcePath, string replicaPath)
        {
            // Simple check whether source path exists. If not - log it and exit the function.
            if (!Directory.Exists(sourcePath))
            {
                throw new SourceDirectoryNotFoundException(sourcePath);
            }
            
            // Make sure the replica directory exists
            Directory.CreateDirectory(replicaPath);

            // Copy and update files and/or directories from source directory to the replica directory
            CopyAndUpdate(sourcePath, replicaPath);

            // Remove any files and/or directories in replica direcotry that are not part of origin directory
            RemoveExtra(replicaPath, sourcePath);
        }

        /// <summary>
        /// Method <c>CopyAndUpdate</c> copies new or updated files and/or directories from source directory to the replica directory
        /// </summary>
        /// <param name="sourcePath">Path to the source directory</param>
        /// <param name="replicaPath">Path to the replica directory</param>
        private void CopyAndUpdate(string sourcePath, string replicaPath)
        {
            // Process all files in the source directory
            foreach (string sourceFile in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFile);
                string replicaFile = Path.Combine(replicaPath, fileName);
                
                // Check if file does not exist, was edited since last synchronization or has different size in bytes 
                if (!File.Exists(replicaFile) ||
                    File.GetLastWriteTimeUtc(sourceFile) != File.GetLastWriteTimeUtc(replicaFile) ||
                    new FileInfo(sourceFile).Length != new FileInfo(replicaFile).Length)

                {
                    File.Copy(sourceFile, replicaFile, true);
                    _logger.Log($"Copied/Updated: {sourceFile}");
                }
            }

            // Process all directories and subdirectories (recursively)
            foreach (string sourceDir in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(sourceDir);
                string replicaDir = Path.Combine(replicaPath, dirName);

                // Create new directory (unless they already exist)
                if (!Directory.Exists(replicaDir))
                {
                    Directory.CreateDirectory(replicaDir);
                    _logger.Log($"Created directory: {replicaDir}");
                }

                // Act recursively for subdirectories
                CopyAndUpdate(sourceDir, replicaDir);
            }
        }

        /// <summary>
        /// Method <c>RemoveExtra</c> deletes all files and/or directories (including subdirectories) 
        /// existing in replica directory that are not part of origin directory
        /// </summary>
        /// <param name="replicaPath">Path to replica directory</param>
        /// <param name="sourcePath">Path to source directory</param>
        private void RemoveExtra(string replicaPath, string sourcePath)
        {
            // Delete unnecessary files in replica directory
            foreach (string replicaFile in Directory.GetFiles(replicaPath))
            {
                string fileName = Path.GetFileName(replicaFile);
                string sourceFile = Path.Combine(sourcePath, fileName);

                if(!File.Exists(sourceFile))
                {
                    File.Delete(replicaFile);
                    _logger.Log($"Deleted: {replicaFile}");
                }
            }

            // Delete unnecessary directories in replica directory
            foreach (string replicaDir in Directory.GetDirectories(replicaPath))
            {
                string dirName = Path.GetFileName(replicaDir);
                string sourceDir = Path.Combine(sourcePath, dirName);

                if(!Directory.Exists(sourceDir))
                {
                    Directory.Delete(replicaDir, true);
                    _logger.Log($"Deleted directory: {replicaDir}");
                }
                else
                {
                    // Act recursively for subdirectories
                    RemoveExtra(replicaDir, sourceDir);
                }
            }
        }
    }
}

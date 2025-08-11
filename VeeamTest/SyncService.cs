using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VeeamTest
{
    class SyncService
    {
        private readonly Logger _logger;

        public SyncService(Logger logger)
        {
            _logger = logger;
        }

        public void Synchronize(string sourcePath, string replicaPath)
        {
            // Simple check whether source path exists. If not - log it and exit the function.
            if (!Directory.Exists(sourcePath))
            {
                _logger.Log($"Source directory does not exist: {sourcePath}");
                return;
            }
            
            // Make sure the replica directory exists
            Directory.CreateDirectory(replicaPath);

            CopyAndUpdate(sourcePath, replicaPath);

            RemoveExtra(sourcePath, replicaPath);
            RemoveExtra(sourcePath, replicaPath);
        }

        private void CopyAndUpdate(string sourcePath, string replicaPath)
        {
            foreach (string sourceFile in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFile);
                string replicaFile = Path.Combine(replicaPath, sourceFile);
                if (!File.Exists(replicaFile) ||
                    File.GetLastWriteTimeUtc(sourceFile) != File.GetLastWriteTimeUtc(replicaFile) ||
                    new FileInfo(sourceFile).Length != new FileInfo(replicaFile).Length)

                {
                    File.Copy(sourceFile, replicaFile, true);
                    _logger.Log($"Copied/Updated: {sourceFile}");
                }
            }

            foreach (string sourceDir in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(sourceDir);
                string replicaDir = Path.Combine(replicaPath, dirName);

                Directory.CreateDirectory(replicaDir);
                CopyAndUpdate(sourceDir, replicaDir);
            }
        }

        private void RemoveExtra(string replicaPath, string sourcePath)
        {
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
                    RemoveExtra(replicaDir, sourceDir);
                }
            }
        }
    }
}

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.IO;
using VeeamTest.Exceptions;
using Xunit;

namespace VeeamTest.Tests
{
    public class SyncServiceTests
    {
        [Fact]
        public void Synchronize_CopiesFilesFromSourceToReplica()
        {
            //Arrange 
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string sourcePath = Path.Combine(tempDir, "source");
            string replicaPath = Path.Combine(tempDir, "replica");
            Directory.CreateDirectory(sourcePath);
            Directory.CreateDirectory(replicaPath);

            string filePath = Path.Combine(sourcePath, "testfile.txt");
            File.WriteAllText(filePath, "Test content");

            string logPath = Path.Combine(tempDir, "log.txt");
            Logger logger = new Logger(logPath);
            SyncService syncService = new SyncService(logger);

            //Act
            syncService.Synchronize(sourcePath, replicaPath);

            //Assert
            string copiedFile = Path.Combine(replicaPath, "testfile.txt");
            Assert.True(File.Exists(copiedFile), "File was not copied to the replica directory.");
            Assert.Equal("Test content", File.ReadAllText(copiedFile));
        }

        [Fact]
        public void Synchronize_RemovesExtraFilesInReplica()
        {
            //Arrange 
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string sourcePath = Path.Combine(tempDir, "source");
            string replicaPath = Path.Combine(tempDir, "replica");
            Directory.CreateDirectory(sourcePath);
            Directory.CreateDirectory(replicaPath);

            string extraFile = Path.Combine(replicaPath, "extrafile.txt");
            File.WriteAllText(extraFile, "Extra content");

            string logPath = Path.Combine(tempDir, "log.txt");
            Logger logger = new Logger(logPath);
            SyncService syncService = new SyncService(logger);

            //Act
            syncService.Synchronize(sourcePath, replicaPath);

            //Assert
            Assert.False(File.Exists(extraFile), "Extra file was not removed from the replica directory.");
        }

        [Fact]
        public void Synchronize_ThrowsException_WhenSourceDirectoryDoesNotExist()
        {
            //Arrange 
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string sourcePath = Path.Combine(tempDir, "missingSource");
            string replicaPath = Path.Combine(tempDir, "replica");
            string logPath = Path.Combine(tempDir, "log.txt");
            Logger logger = new Logger(logPath);
            SyncService syncService = new SyncService(logger);
            
            //Act & Assert
            Assert.Throws<SourceDirectoryNotFoundException>(() => 
                syncService.Synchronize(sourcePath, replicaPath));
        }
    }
}
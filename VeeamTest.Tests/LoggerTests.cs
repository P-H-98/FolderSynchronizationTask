using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.IO;
using Xunit;

namespace VeeamTest.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void Log_CreatesLogFileIfNotExists()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            string logFilePath = Path.Combine(tempDir, "log.txt");
            var logger = new Logger(logFilePath);
            // Act
            logger.Log("Test log entry");
            // Assert
            Assert.True(File.Exists(logFilePath), "Log file was not created.");
        }

    }
}
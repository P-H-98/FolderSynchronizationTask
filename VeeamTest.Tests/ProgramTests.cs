using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.IO;
using VeeamTest.Exceptions;
using Xunit;

namespace VeeamTest.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void ValidateArguments_ThrowsException_WhenInvalidNumberOfArguments()
        {
            // Arrange
            string[] args = new string[] { "sourcePath", "replicaPath" }; // Missing interval and log file
            // Act & Assert
            var ex = Assert.Throws<InvalidArgumentsException>(() => 
                Program.ValidateArguments(new string[] {"only one argument"}));
        }

        [Fact]
        public void ValidateArguments_ThrowsException_WhenIntervalIsInvalid()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            string[] args = new string[] { tempDir, "replicaPath", "invalidInterval", Path.Combine(tempDir, "log.txt") };

            // Act & Assert
            var ex = Assert.Throws<InvalidArgumentsException>(() => 
                Program.ValidateArguments(args));
        }
    }
}
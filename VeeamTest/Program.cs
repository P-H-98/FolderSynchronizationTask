using System;
using System.Threading;
using VeeamTest.Exceptions;

namespace VeeamTest
{
    class Program
    {
        /// <summary>
        /// Main method of the program
        /// </summary>
        /// <param name="args">Array of command-line arguments
        /// args[0] - Path to the source directory
        /// args[1] - Path to the replica directory
        /// args[2] - Synchronization interval (in seconds)
        /// args[3] - Path to the log file
        /// </param>
        static void Main(string[] args)
        {
            // Check the correct number of arguments
            if  (args.Length != 4)
            {
                Console.WriteLine("Usage: VeeamTest <sourcePath> <replicaPath> <intervalSeconds> <logFilePath>");
                return;
            }

            string sourcePath = args[0];
            string replicaPath = args[1];
            int intervalSeconds = int.Parse(args[2]);
            string logFilePath = args[3];

            Logger logger = new Logger(logFilePath);
            SyncService syncService = new SyncService(logger);

            logger.Log($"Starting folder synchronization from '{sourcePath}' to '{replicaPath}' every {intervalSeconds} seconds.");

            // Synchronization loop
            while (true)
            {
                try
                {
                    syncService.Synchronize(sourcePath, replicaPath);
                }
                catch (SourceDirectoryNotFoundException ex)
                {
                    logger.Log($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    logger.Log($"Unexpected error: {ex.Message}");
                }

                Thread.Sleep(intervalSeconds * 1000);
            }
        }
    }
}
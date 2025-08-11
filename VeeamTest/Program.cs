using System;
using System.Threading;
using VeeamTest.Exceptions;

namespace VeeamTest
{
    class Program
    {
        static void Main(string[] args)
        {
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
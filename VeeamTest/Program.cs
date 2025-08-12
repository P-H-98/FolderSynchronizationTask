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
            try
            {
                // call to a static function to Validate arguments
                // if they are valid, store them in variables
                // throws exception when arguments are invalid
                var(sourcePath, replicaPath, intervalSeconds, logFilePath) = ValidateArguments(args);

                Logger logger = new Logger(logFilePath);
                SyncService syncService = new SyncService(logger);

                logger.Log($"Starting directory synchronization from '{sourcePath}' to '{replicaPath}' every {intervalSeconds} seconds");

                // main loop
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
            catch (InvalidArgumentsException ex)
            {
                Console.WriteLine($"Invalid arguments: {ex.Message}");
                PrintUsage();
            }
            catch (LogFileAccessException ex)
            {
                Console.WriteLine($"Log file error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static (string sourcePath, string replicaPath, int intervalSeconds, string logFilePath) ValidateArguments(string[] args)
        {
            if (args.Length != 4)
            {
                throw new InvalidArgumentsException("Incorrect number of arguments.");
            }

            string sourcePath = args[0];
            string replicaPath = args[1];
            string intervalArg = args[2];
            string logFilePath = args[3];

            if(!int.TryParse(intervalArg, out int intervalSeconds) || intervalSeconds <= 0)
            {
                throw new InvalidArgumentsException("Interval must be a positive integer (seconds).");
            }

            if(!Directory.Exists(sourcePath))
            {
                throw new InvalidArgumentsException($"Source path does not exist: {sourcePath}");
            }

            try
            {
                Directory.CreateDirectory(replicaPath);
            } 
            catch (Exception ex)
            {
                throw new InvalidArgumentsException($"Could not create replica directory: {replicaPath}");
            }

            try
            {
                File.AppendAllText(logFilePath, $"Log initialized at {DateTime.Now}\n");
            }
            catch (Exception ex)
            {
                throw new LogFileAccessException($"Could not write to the log file: {logFilePath} ({ex.Message})");
            }

            return (sourcePath, replicaPath, intervalSeconds, logFilePath);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("VeeamTest <sourcePath> <replicaPath> <intervalSeconds> <logFilePath>");
            Console.WriteLine("Example: ");
            Console.WriteLine(@"VeeamTest C:\Data\Source C:\Data\Replica 60 C:\Logs\sync.log");
        }
    }
}
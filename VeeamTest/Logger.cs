using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VeeamTest
{
    /// <summary>
    /// Class <c>Logger</c> brings functionalities needed to log messages to a log file
    /// </summary>
    class Logger
    {
        private readonly string _logFilePath;

        public Logger(string _logFilePath)
        {
            _logFilePath = _logFilePath;
        }

        public void Log(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Console.WriteLine(logEntry);
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }
}

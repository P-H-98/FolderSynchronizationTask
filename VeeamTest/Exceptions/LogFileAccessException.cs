using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeeamTest.Exceptions
{
    /// <summary>
    /// Custom exception thrown when accessing or creating the log file is not possible
    /// </summary>
    public class LogFileAccessException : Exception
    {
        public LogFileAccessException(string message) : base(message)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeeamTest.Exceptions
{
    /// <summary>
    /// Custom exception thrown when command-line arguments provided are invalid
    /// </summary>
    public class InvalidArgumentsException : Exception
    {
        public InvalidArgumentsException (string message) : base(message)
        {

        }
    }
}

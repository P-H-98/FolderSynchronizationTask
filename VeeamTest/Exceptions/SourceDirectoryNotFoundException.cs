using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeeamTest.Exceptions
{
    /// <summary>
    /// Custom exception thrown when source directory does not exist
    /// </summary>
    internal class SourceDirectoryNotFoundException : Exception
    {
        public SourceDirectoryNotFoundException(string path) : base($"Source directory does not exist: {path}")
        {

        }
    }
}

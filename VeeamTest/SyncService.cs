using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VeeamTest
{
    class SyncService
    {
        private readonly Logger _logger;

        public SyncService(Logger logger)
        {
            _logger = logger;
        }
    }
}

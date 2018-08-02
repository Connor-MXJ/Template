using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.Logging;

namespace MXJ.Core.Logging.Log4Net
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            return Log4NetLogger.Instance;
        }
    }
}
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.Logging;

namespace MXJ.Core.Logging.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private static readonly ILogger _instance = new Log4NetLogger();
        private readonly ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        public static ILogger Instance
        {
            get { return _instance; }
        }

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return _logger.IsFatalEnabled;
                default: return false;
            }
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Information:
                    _logger.Info(args == null ? format : string.Format(format, args), exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(args == null ? format : string.Format(format, args), exception);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(args == null ? format : string.Format(format, args), exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(args == null ? format : string.Format(format, args), exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(args == null ? format : string.Format(format, args), exception);
                    break;
            }
        }
    }
}
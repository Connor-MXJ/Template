using System;

namespace MXJ.Core.Infrastructure.Logging
{
    /// <summary>
    /// ��־����
    /// </summary>
    public interface ILoggerFactory {
        /// <summary>
        /// ������־��
        /// </summary>
        /// <returns></returns>
        ILogger CreateLogger();
    }
}
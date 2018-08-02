using System;

namespace MXJ.Core.Infrastructure.Logging
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public interface ILoggerFactory {
        /// <summary>
        /// 创建日志类
        /// </summary>
        /// <returns></returns>
        ILogger CreateLogger();
    }
}
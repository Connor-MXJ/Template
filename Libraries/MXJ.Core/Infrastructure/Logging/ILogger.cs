using System;

namespace MXJ.Core.Infrastructure.Logging
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel {
        Debug,//调试
        Information,//信息
        Warning,//警告
        Error,//错误
        Fatal//严重错误
    }

    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// 是否记录该等级日志
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Log(LogLevel level, Exception exception, string format, params object[] args);
    }
}

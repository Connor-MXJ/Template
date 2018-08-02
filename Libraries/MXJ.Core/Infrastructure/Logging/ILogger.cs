using System;

namespace MXJ.Core.Infrastructure.Logging
{
    /// <summary>
    /// ��־����
    /// </summary>
    public enum LogLevel {
        Debug,//����
        Information,//��Ϣ
        Warning,//����
        Error,//����
        Fatal//���ش���
    }

    /// <summary>
    /// ��־�ӿ�
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// �Ƿ��¼�õȼ���־
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Log(LogLevel level, Exception exception, string format, params object[] args);
    }
}

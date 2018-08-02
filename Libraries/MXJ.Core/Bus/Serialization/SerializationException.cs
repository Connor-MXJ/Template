using System;
using System.Runtime.InteropServices;
using MXJ.Core.Infrastructure.Exceptions;

namespace MXJ.Core.Bus.Serialization
{
    /// <summary>
    /// 序列化/反序列化异常
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class SerializationException : InfrastructureException
    {
        #region 构造函数
        /// <summary>
        ///构造函数
        /// </summary>
        public SerializationException()
            : base()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public SerializationException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">异常明细</param>
        public SerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public SerializationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
        #endregion
    }
}

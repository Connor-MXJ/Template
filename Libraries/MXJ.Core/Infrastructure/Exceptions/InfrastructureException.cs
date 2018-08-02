using System;
using System.Runtime.InteropServices;

namespace MXJ.Core.Infrastructure.Exceptions
{
   /// <summary>
   /// 基础架构异常
   /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class InfrastructureException : SystemException
    {
        #region 构造函数
        /// <summary>
        ///构造函数
        /// </summary>
        public InfrastructureException()
            : base()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public InfrastructureException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">异常明细</param>
        public InfrastructureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public InfrastructureException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        #endregion
    }
}

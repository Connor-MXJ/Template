using System;

namespace MXJ.Core.Bus.Message
{
    /// <summary>
    /// 消息分发事件参数
    /// </summary>
    public class MessageDispatchEventArgs : EventArgs
    {
        #region 属性
        /// <summary>
        /// 消息
        /// </summary>
        public dynamic Message { get; set; }

        /// <summary>
        /// 处理器类型
        /// </summary>
        public Type HandlerType { get; set; }

        /// <summary>
        /// 处理器
        /// </summary>
        public object Handler { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageDispatchEventArgs()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handlerType"></param>
        /// <param name="handler"></param>
        public MessageDispatchEventArgs(dynamic message, Type handlerType, object handler)
        {
            this.Message = message;
            this.HandlerType = handlerType;
            this.Handler = handler;
        }
        #endregion
    }
}

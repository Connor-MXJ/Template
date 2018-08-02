using System;
using MXJ.Core.Domain.Handlers;

namespace MXJ.Core.Bus.Message
{
    /// <summary>
    /// 消息分发接口
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 清除注册的消息处理器
        /// </summary>
        void Clear();
       
        /// <summary>
        /// 分发消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        void DispatchMessage<T>(T message);
       
        /// <summary>
        /// 注册消息处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void Register<T>(IHandler<T> handler);
      
        /// <summary>
        /// 卸载消息处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void UnRegister<T>(IHandler<T> handler);
       
        /// <summary>
        /// 消息分发事件
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatching;
     
        /// <summary>
        /// 消息分发失败事件
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> DispatchFailed;
       
        /// <summary>
        /// 消息分发完成事件
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatched;
    }
}

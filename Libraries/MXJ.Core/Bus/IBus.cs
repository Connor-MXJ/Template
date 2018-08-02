using System;
using System.Collections.Generic;
using MXJ.Core.Domain.Repositories;

namespace MXJ.Core.Bus
{
    /// <summary>
    /// 消息总线接口
    /// </summary>
    public interface IBus : IUnitOfWork, IDisposable
    {
      /// <summary>
      /// 发布消息
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="message"></param>
        void Publish<T>(T message);
      
        /// <summary>
        /// 发布消息集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        void Publish<T>(IEnumerable<T> messages);
      
        /// <summary>
        /// 清空消息
        /// </summary>
        void Clear();
    }
}

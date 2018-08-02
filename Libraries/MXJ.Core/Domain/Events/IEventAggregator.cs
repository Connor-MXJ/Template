using System;
using System.Collections.Generic;

namespace MXJ.Core.Domain.Events
{
  /// <summary>
  /// 事件聚合接口
  /// </summary>
    public interface IEventAggregator
    {
        #region Methods
        /// <summary>
        /// 为事件订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandler"></param>
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 为事件订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 为事件订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerAction"></param>
        void Subscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent;

        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        void Subscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent;

        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        void Subscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandler"></param>
        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerAction"></param>
        void Unsubscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        void Unsubscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        void Unsubscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅某类事件的所有事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅所有处理器
        /// </summary>
        void UnsubscribeAll();

        /// <summary>
        /// 获取某类事件的事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        void Publish<TEvent>(TEvent @event, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent;
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 事件聚合实现
   /// </summary>
    public class EventAggregator : IEventAggregator
    {
        #region 私有属性
        private readonly object _sync = new object();
        private readonly Dictionary<Type, List<object>> _eventHandlers = new Dictionary<Type, List<object>>();
        private readonly MethodInfo _registerEventHandlerMethod;
        private readonly Func<object, object, bool> _eventHandlerEquals = (o1, o2) =>
        {
            var o1Type = o1.GetType();
            var o2Type = o2.GetType();
            if (o1Type.IsGenericType &&
                o1Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>) &&
                o2Type.IsGenericType &&
                o2Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>))
                return o1.Equals(o2);
            return o1Type == o2Type;
        };
        #endregion

        #region 构造函数
       /// <summary>
       /// 构造函数
       /// </summary>
        public EventAggregator()
        {
            _registerEventHandlerMethod = (from p in this.GetType().GetMethods()
                                          let methodName = p.Name
                                          let parameters = p.GetParameters()
                                          where methodName == "Subscribe" &&
                                          parameters != null &&
                                          parameters.Length == 1 &&
                                          parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                                          select p).First();
        }
     
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlers"></param>
        public EventAggregator(object[] handlers)
            : this()
        {
            foreach (var obj in handlers)
            {
                var type = obj.GetType();
                var implementedInterfaces = type.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType &&
                        implementedInterface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventType = implementedInterface.GetGenericArguments().First();
                        var method = _registerEventHandlerMethod.MakeGenericMethod(eventType);
                        method.Invoke(this, new object[] { obj });
                    }
                }
            }
        }
        #endregion

        #region 事件聚合接口成员
      /// <summary>
      /// 为事件订阅事件处理器
      /// </summary>
      /// <typeparam name="TEvent"></typeparam>
      /// <param name="eventHandler"></param>
        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (_sync)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                    {
                        if (!handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                            handlers.Add(eventHandler);
                    }
                    else
                    {
                        handlers = new List<object>();
                        handlers.Add(eventHandler);
                    }
                }
                else
                    _eventHandlers.Add(eventType, new List<object> { eventHandler });
            }
        }
      
        /// <summary>
        /// 为事件订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }

      /// <summary>
      /// 为事件订阅事件处理器
      /// </summary>
      /// <typeparam name="TEvent"></typeparam>
      /// <param name="eventHandlers"></param>
        public void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }
      
        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerAction"></param>
        public void Subscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent
        {
            Subscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerAction));
        }
      
        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        public void Subscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerAction in eventHandlerActions)
                Subscribe<TEvent>(eventHandlerAction);
        }
     
        /// <summary>
        /// 为事件订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        public void Subscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerAction in eventHandlerActions)
                Subscribe<TEvent>(eventHandlerAction);
        }
       
        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandler"></param>
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (_sync)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null &&
                        handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(deh => _eventHandlerEquals(deh, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }

        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        public void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }
      
        /// <summary>
        /// 取消订阅事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        public void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }
      
        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerAction"></param>
        public void Unsubscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent
        {
            Unsubscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerAction));
        }
       
        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        public void Unsubscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerAction in eventHandlerActions)
                Unsubscribe<TEvent>(eventHandlerAction);
        }
       
        /// <summary>
        /// 取消订阅事件处理方法
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerActions"></param>
        public void Unsubscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerAction in eventHandlerActions)
                Unsubscribe<TEvent>(eventHandlerAction);
        }
       
        /// <summary>
        /// 取消订阅某类事件的所有事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        public void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent
        {
            lock (_sync)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                        handlers.Clear();
                }
            }
        }
       
        /// <summary>
        /// 取消订阅所有处理器
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (_sync)
            {
                _eventHandlers.Clear();
            }
        }
      
        /// <summary>
        /// 获取某类事件的事件处理器
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            if (_eventHandlers.ContainsKey(eventType))
            {
                var handlers = _eventHandlers[eventType];
                if (handlers != null)
                    return handlers.Select(p => p as IEventHandler<TEvent>).ToList();
                else
                    return null;
            }
            else
                return null;
        }
       
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        public void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            if (@event == null)
                throw new ArgumentNullException("evnt");
            var eventType = @event.GetType();
            if (_eventHandlers.ContainsKey(eventType) &&
                _eventHandlers[eventType] != null &&
                _eventHandlers[eventType].Count > 0)
            {
                var handlers = _eventHandlers[eventType];
                foreach (var handler in handlers)
                {
                    var eventHandler = handler as IEventHandler<TEvent>;
                    if (eventHandler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                    {
                        Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), @event);
                    }
                    else
                    {
                        eventHandler.Handle(@event);
                    }
                }
            }
        }
      
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public void Publish<TEvent>(TEvent @event, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent
        {
            if (@event == null)
                throw new ArgumentNullException("evnt");
            var eventType = @event.GetType();
            if (_eventHandlers.ContainsKey(eventType) &&
                _eventHandlers[eventType] != null &&
                _eventHandlers[eventType].Count > 0)
            {
                var handlers = _eventHandlers[eventType];
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), @event));
                        }
                        else
                        {
                            eventHandler.Handle(@event);
                        }
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(@event, true, null);
                }
                catch (Exception ex)
                {
                    callback(@event, false, ex);
                }
            }
            else
                callback(@event, false, null);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using MXJ.Core.Config;
using MXJ.Core.Domain.Handlers;

namespace MXJ.Core.Bus.Message
{
    /// <summary>
    /// 消息分发类
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        #region 私有属性
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();
        #endregion

        #region 私有方法
       /// <summary>
       /// 将指定类型的消息处理器注册到消息分发器
       /// </summary>
       /// <param name="messageDispatcher"></param>
       /// <param name="handlerType"></param>
        private static void RegisterType(IMessageDispatcher messageDispatcher, Type handlerType)
        {
            MethodInfo methodInfo = messageDispatcher.GetType().GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);

            var handlerIntfTypeQuery = from p in handlerType.GetInterfaces()
                                       where p.IsGenericType &&
                                       p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))
                                       select p;
            if (handlerIntfTypeQuery != null)
            {
                foreach (var handlerIntfType in handlerIntfTypeQuery)
                {
                    object handlerInstance = Activator.CreateInstance(handlerType);
                    Type messageType = handlerIntfType.GetGenericArguments().First();
                    MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(messageType);
                    genericMethodInfo.Invoke(messageDispatcher, new object[] { handlerInstance });
                }
            }
        }
       
        /// <summary>
        /// 将程序集的所有消息处理器注册到消息分发器
        /// </summary>
        /// <param name="messageDispatcher"></param>
        /// <param name="assembly"></param>
        private static void RegisterAssembly(IMessageDispatcher messageDispatcher, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                var intfs = type.GetInterfaces();
                if (intfs.Any(p =>
                    p.IsGenericType &&
                    p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))) &&
                    intfs.Any(p =>
                    p.IsDefined(typeof(RegisterDispatchAttribute), true)))
                {
                    RegisterType(messageDispatcher, type);
                }
            }
        }
        #endregion

        #region Protected Methods
      /// <summary>
      /// 消息分发时间处理函数
      /// </summary>
      /// <param name="e"></param>
        protected virtual void OnDispatching(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatching;
            if (temp != null)
                temp(this, e);
        }
        
        /// <summary>
        /// 消息分发失败处理函数
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDispatchFailed(MessageDispatchEventArgs e)
        {
            var temp = this.DispatchFailed;
            if (temp != null)
                temp(this, e);
        }
      
        /// <summary>
        /// 消息分发完成处理函数
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDispatched(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatched;
            if (temp != null)
                temp(this, e);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a message dispatcher and registers all the message handlers
        /// specified in the <see cref="Apworks.Config.IConfigSource"/> instance.
        /// </summary>
        /// <param name="configSource">The <see cref="Apworks.Config.IConfigSource"/> instance
        /// that contains the definitions for message handlers.</param>
        /// <param name="messageDispatcherType">The type of the message dispatcher.</param>
        /// <param name="args">The arguments that is used for initializing the message dispatcher.</param>
        /// <returns>A <see cref="Apworks.Bus.IMessageDispatcher"/> instance.</returns>
        //public static IMessageDispatcher CreateAndRegister(IConfigSource configSource, 
        //    Type messageDispatcherType,
        //    params object[] args)
        //{
        //    IMessageDispatcher messageDispatcher = (IMessageDispatcher)Activator.CreateInstance(messageDispatcherType,
        //        args);

        //    HandlerElementCollection handlerElementCollection = configSource.Config.Handlers;
        //    foreach (HandlerElement handlerElement in handlerElementCollection)
        //    {
        //        switch(handlerElement.SourceType)
        //        {
        //            case HandlerSourceType.Type:
        //                string typeName = handlerElement.Source;
        //                Type handlerType = Type.GetType(typeName);
        //                RegisterType(messageDispatcher, handlerType);
        //                break;
        //            case HandlerSourceType.Assembly:
        //                string assemblyString = handlerElement.Source;
        //                Assembly assembly = Assembly.Load(assemblyString);
        //                RegisterAssembly(messageDispatcher, assembly);
        //                break;
        //        }
        //    }
        //    return messageDispatcher;
        //}
        #endregion

        #region 接口成员
     /// <summary>
     /// 清空消息处理器
     /// </summary>
        public virtual void Clear()
        {
            _handlers.Clear();
        }
      
        /// <summary>
        /// 分发消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public virtual void DispatchMessage<T>(T message)
        {
            Type messageType = typeof(T);
            if (_handlers.ContainsKey(messageType))
            {
                var messageHandlers = _handlers[messageType];
                foreach (var messageHandler in messageHandlers)
                {
                    var dynMessageHandler = (IHandler<T>)messageHandler;
                    var evtArgs = new MessageDispatchEventArgs(message, messageHandler.GetType(), messageHandler);
                    this.OnDispatching(evtArgs);
                    try
                    {
                        dynMessageHandler.Handle(message);
                        this.OnDispatched(evtArgs);
                    }
                    catch
                    {
                        this.OnDispatchFailed(evtArgs);
                    }
                }
            }
        }
       
        /// <summary>
        /// 注册消息处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public virtual void Register<T>(IHandler<T> handler)
        {
            Type keyType = typeof(T);

            if (_handlers.ContainsKey(keyType))
            {
                List<object> registeredHandlers = _handlers[keyType];
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                        registeredHandlers.Add(handler);
                }
                else
                {
                    registeredHandlers = new List<object>();
                    registeredHandlers.Add(handler);
                    _handlers.Add(keyType, registeredHandlers);
                    
                }
            }
            else
            {
                List<object> registeredHandlers = new List<object>();
                registeredHandlers.Add(handler);
                _handlers.Add(keyType, registeredHandlers);
            }
        }
      
        /// <summary>
        /// 卸载消息处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public virtual void UnRegister<T>(IHandler<T> handler)
        {
            var keyType = typeof(T);
            if (_handlers.ContainsKey(keyType) &&
                _handlers[keyType] != null &&
                _handlers[keyType].Count > 0 &&
                _handlers[keyType].Contains(handler))
            {
                _handlers[keyType].Remove(handler);
            }
        }
      
        /// <summary>
        /// 消息分发事件
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatching;
     
        /// <summary>
        /// 消息分发失败事件
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> DispatchFailed;
      
        /// <summary>
        /// 消息分发完成事件
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatched;

        #endregion
    }
}

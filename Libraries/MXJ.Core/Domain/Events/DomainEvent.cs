using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MXJ.Core.Domain.Models;

namespace MXJ.Core.Domain.Events
{
    /// <summary>
    ///     领域事件基类
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        #region 事件接口成员

        /// <summary>
        ///     事件发生时间
        /// </summary>
        public virtual DateTime Timestamp { get; set; }

        #endregion

        #region 实体接口成员

        /// <summary>
        ///     实体唯一标识
        /// </summary>
        public virtual Guid AggregateRootId { get; set; }

        #endregion

        #region 方法

        /// <summary>
        ///     获取领域事件Hash值
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     判断两个领域事件是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as DomainEvent;
            if (other == null)
                return false;
            return AggregateRootId == other.AggregateRootId;
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        public DomainEvent()
        {
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="source">事件源实体</param>
        public DomainEvent(IAggregateRoot source)
        {
            Source = source;
        }

        #endregion

        #region 领域事件接口成员

        /// <summary>
        ///     领域事件源
        /// </summary>
        [XmlIgnore]
        [SoapIgnore]
        [IgnoreDataMember]
        public IAggregateRoot Source { get; set; }

        /// <summary>
        ///     事件版本
        /// </summary>
        public virtual long Version { get; set; }

        /// <summary>
        ///     事件版本所属分支
        /// </summary>
        public virtual long Branch { get; set; }

        /// <summary>
        ///     事件全局限定名
        /// </summary>
        public virtual string AssemblyQualifiedEventType { get; set; }

        #endregion

        #region 静态方法

        /// <summary>
        ///     发布时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domainEvent"></param>
        public static void Publish<T>(T domainEvent)
            where T : class, IDomainEvent
        {
            //IEnumerable<IDomainEventHandler<T>> handlers = ServiceLocator
            //    .Instance
            //    .ResolveAll<IDomainEventHandler<T>>();
            //foreach (var handler in handlers)
            //{
            //    if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
            //        Task.Factory.StartNew(() => handler.Handle(domainEvent));
            //    else
            //        handler.Handle(domainEvent);
            //}
        }

        /// <summary>
        ///     发布事件
        /// </summary>
        /// <typeparam name="TDomainEvent"></typeparam>
        /// <param name="domainEvent"></param>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public static void Publish<T>(T domainEvent, Action<T, bool, Exception> callback, TimeSpan? timeout = null)
            where T : class, IDomainEvent
        {
            //IEnumerable<IDomainEventHandler<T>> handlers = ServiceLocator
            //    .Instance
            //    .ResolveAll<IDomainEventHandler<T>>();
            //if (handlers != null && handlers.Count() > 0)
            //{
            //    List<Task> tasks = new List<Task>();
            //    try
            //    {
            //        foreach (var handler in handlers)
            //        {
            //            if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
            //            {
            //                tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent)));
            //            }
            //            else
            //                handler.Handle(domainEvent);
            //        }
            //        if (tasks.Count > 0)
            //        {
            //            if (timeout == null)
            //                Task.WaitAll(tasks.ToArray());
            //            else
            //                Task.WaitAll(tasks.ToArray(), timeout.Value);
            //        }
            //        callback(domainEvent, true, null);
            //    }
            //    catch (Exception ex)
            //    {
            //        callback(domainEvent, false, ex);
            //    }
            //}
            //else
            //    callback(domainEvent, false, null);
        }

        #endregion
    }
}
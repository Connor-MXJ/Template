using System;
using MXJ.Core.Domain.Models;

namespace MXJ.Core.Domain.Events
{
    /// <summary>
    ///事件基础接口
    /// </summary>
    public interface IEvent : IAggregateRoot
    {
       /// <summary>
       /// 事件产生时间
       /// </summary>
        DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 事件的程序集完全限定名
        /// </summary>
        string AssemblyQualifiedEventType { get; set; }
    }
}

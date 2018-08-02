using System;
using System.Xml.Serialization;
using MXJ.Core.Domain.Models;
namespace MXJ.Core.Domain.Events
{
 /// <summary>
 /// 领域事件接口
 /// </summary>
    public interface IDomainEvent : IEvent
    {
      /// <summary>
      /// 领域事件产生实体
      /// </summary>
        IAggregateRoot Source { get; set; }
       
        /// <summary>
        /// 事件版本号
        /// </summary>
        long Version { get; set; }
     
        /// <summary>
        /// 事件所属分支
        /// </summary>
        long Branch { get; set; }
    }
}

using System;

namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 领域事件属性
   /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class HandlesAttribute : System.Attribute
    {
        #region 属性
       /// <summary>
       /// 领域事件类型
       /// </summary>
        public Type DomainEventType { get; set; }
        #endregion

        #region 构造函数
       /// <summary>
       /// 构造函数
       /// </summary>
       /// <param name="domainEventType"></param>
        public HandlesAttribute(Type domainEventType)
        {
            this.DomainEventType = domainEventType;
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Reflection;
//using MXJ.Core.Properties;

namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 领域事件处理器
   /// </summary>
   /// <typeparam name="TDomainEvent"></typeparam>
    public sealed class InlineDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        #region 私有属性
        private readonly Type _domainEventType;
        private readonly Action<TDomainEvent> _action;
        #endregion

        #region 构造函数
       ///// <summary>
       ///// 构造函数
       ///// </summary>
       ///// <param name="aggregateRoot"></param>
       ///// <param name="mi"></param>
       // public InlineDomainEventHandler(ISourcedAggregateRoot aggregateRoot, MethodInfo mi)
       // {
       //     ParameterInfo[] parameters = mi.GetParameters();
       //     if (parameters == null || parameters.Count() == 0)
       //     {
       //         throw new ArgumentException(string.Format(Resources.EX_INVALID_HANDLER_SIGNATURE, mi.Name), "mi");
       //     }
       //     domainEventType = parameters[0].ParameterType;
       //     action = domainEvent =>
       //     {
       //         try
       //         {
       //             mi.Invoke(aggregateRoot, new object[] { domainEvent });
       //         }
       //         catch {  }
       //     };
       // }
       // #endregion

       // #region 方法
       ///// <summary>
       ///// 判断对象是否相同
       ///// </summary>
       ///// <param name="obj"></param>
       ///// <returns></returns>
       // public override bool Equals(object obj)
       // {
       //     if (ReferenceEquals(this, obj))
       //         return true;
       //     if (obj == (object)null)
       //         return false;
       //     InlineDomainEventHandler<TDomainEvent> other = obj as InlineDomainEventHandler<TDomainEvent>;
       //     if ((object)other == (object)null)
       //         return false;
       //     return Delegate.Equals(this.action, other.action);
       // }
       
       // /// <summary>
       // /// 获取对象Hash值
       // /// </summary>
       // /// <returns></returns>
       // public override int GetHashCode()
       // {
       //     if (this.action != null && this.domainEventType != null)
       //         return Utils.GetHashCode(this.action.GetHashCode(),
       //             this.domainEventType.GetHashCode());
       //     return base.GetHashCode();
       // }
        #endregion

        #region 事件处理器接口成员
       /// <summary>
       /// 处理领域事件
       /// </summary>
       /// <param name="message"></param>
        public void Handle(TDomainEvent message)
        {
            _action(message);
        }

        #endregion
    }
}

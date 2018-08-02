namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 领域事件处理器接口
   /// </summary>
   /// <typeparam name="T"></typeparam>
    public interface IDomainEventHandler<in T> : IEventHandler<T>
        where T : class, IDomainEvent
    {

    }
}

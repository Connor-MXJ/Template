using MXJ.Core.Bus.Message;
using MXJ.Core.Domain.Handlers;

namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 事件处理器接口
   /// </summary>
   /// <typeparam name="TEvent"></typeparam>
    [RegisterDispatch]
    public interface IEventHandler<in TEvent> : IHandler<TEvent>
        where TEvent : class, IEvent
    {
    }
}

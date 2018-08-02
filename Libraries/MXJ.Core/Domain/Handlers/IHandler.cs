namespace MXJ.Core.Domain.Handlers
{
   /// <summary>
   /// 消息处理器
   /// </summary>
   /// <typeparam name="T"></typeparam>
    public interface IHandler<in T>
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message"></param>
        void Handle(T message);
    }
}

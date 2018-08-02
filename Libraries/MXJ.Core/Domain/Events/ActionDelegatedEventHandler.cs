using System;

namespace  MXJ.Core.Domain.Events
{
   /// <summary>
   /// 事件处理器通用类
   /// </summary>
   /// <typeparam name="T"></typeparam>
    public sealed class ActionDelegatedEventHandler<T> : IEventHandler<T>
        where T : class, IEvent
    {
        #region 私有属性
        private readonly Action<T> _action;
        #endregion

        #region 构造函数
       /// <summary>
       /// 构造函数
       /// </summary>
       /// <param name="action"></param>
        public ActionDelegatedEventHandler(Action<T> action)
        {
            this._action = action;
        }
        #endregion

        #region 方法
       /// <summary>
       /// 判断两个处理器是否相同
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            ActionDelegatedEventHandler<T> other = obj as ActionDelegatedEventHandler<T>;
            if (other == null)
                return false;
            return Delegate.Equals(this._action, other._action);
        }
      /// <summary>
      /// 获取当前处理器的Hash值
      /// </summary>
      /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return Utils.GetHashCode(this.action.GetHashCode());
        //}
        #endregion

        #region 接口成员
       /// <summary>
       /// 处理消息
       /// </summary>
       /// <param name="message"></param>
        public void Handle(T message)
        {
            _action(message);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MXJ.Core.Infrastructure.LifeTime;
using Microsoft.Practices.Unity;

namespace MXJ.Core.IOC.Unity
{
    /// <summary>
    /// 每次请求一个实例
    /// </summary>
    public class UnityPerRequestLifetimeManager : LifetimeManager
    {
        #region Private Fields
        private readonly Guid _key = Guid.NewGuid();
        #endregion

        public UnityPerRequestLifetimeManager() : this(Guid.NewGuid()) { }

        UnityPerRequestLifetimeManager(Guid key)
        {
            if (key == Guid.Empty)
                throw new ArgumentException("键为空.");

            this._key = key;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return PerRequestManager.GetValue(_key.ToString());
        }
        /// <summary>
        /// 移除值
        /// </summary>
        public override void RemoveValue()
        {
            PerRequestManager.RemoveValue(_key.ToString());
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="newValue"></param>
        public override void SetValue(object newValue)
        {
            PerRequestManager.SetValue(_key.ToString(), newValue);
        }
    }
}

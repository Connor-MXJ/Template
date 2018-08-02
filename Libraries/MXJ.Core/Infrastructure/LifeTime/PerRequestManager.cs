using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MXJ.Core.Infrastructure.LifeTime
{
    public static class PerRequestManager
    {
        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            object result = null;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[key] != null)
                    result = HttpContext.Current.Items[key];
            }
            else
            {
                result = CallContext.GetData(key);
            }

            return result;
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveValue(string key)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[key] != null)
                    HttpContext.Current.Items[key] = null;
            }
            else
            {
                CallContext.FreeNamedDataSlot(key);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public static void SetValue(string key, object newValue)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[key] == null)
                    HttpContext.Current.Items[key] = newValue;
            }
            else
            {
                CallContext.SetData(key, newValue);
            }
        }
    }
}

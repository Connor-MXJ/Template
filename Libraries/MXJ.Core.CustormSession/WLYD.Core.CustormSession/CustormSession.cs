using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace MXJ.Core.CustormSession
{
    /// <summary>
    /// 自定义重写seesion
    /// </summary>
    public class CustormSession : ICustormSession
    {
        /// <summary>
        /// 添加一个键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 获取值对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// 移除键
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}

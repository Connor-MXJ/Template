using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.CustormSession
{
    /// <summary>
    /// 自定义重写seesion接口
    /// </summary>
    public interface ICustormSession 
    {
        /// <summary>
        /// 添加一个键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);

        /// <summary>
        /// 获取值对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        object Get(string key);

        /// <summary>
        /// 移除键
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}

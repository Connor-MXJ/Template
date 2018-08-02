using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Infrastructure.Caching
{
    public interface ICacheFactory
    {
        /// <summary>
        /// 获取缓存管理器
        /// </summary>
        /// <returns></returns>
        ICacheManager GetCacheManager();
    }
}

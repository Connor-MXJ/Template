using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.Caching;

namespace MXJ.Core.Caching.Redis
{
    public class RedisCacheFactory : ICacheFactory
    {
        /// <summary>
        /// 获取缓存管理器
        /// </summary>
        /// <returns></returns>
        public ICacheManager GetCacheManager()
        {
            return RedisCacheManager.Instance;
        }
    }
}

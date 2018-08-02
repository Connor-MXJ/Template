using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Infrastructure.Caching
{
    /// <summary>
    /// 缓存扩展方法
    /// </summary>
   public static class CacheExtensions
    {
       /// <summary>
       /// 获取缓存
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="cacheManager"></param>
       /// <param name="key"></param>
       /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key)
        {
            return (T)cacheManager.Get(key);
        }

       /// <summary>
       /// 获取缓存，如果缓存不存在，则执行相应的方法获取数据
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="cacheManager"></param>
       /// <param name="key"></param>
       /// <param name="factory"></param>
       /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> factory)
        {
            var result = cacheManager.Get(key);
            if (result == null)
            {
                var computed = factory();
                cacheManager.Put(key, computed);
                return computed;
            }
         return (T)result;
        }

       /// <summary>
        ///  获取缓存，如果缓存不存在，则执行相应的方法获取数据
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="cacheManager"></param>
       /// <param name="key"></param>
       /// <param name="factory"></param>
       /// <param name="validFor"></param>
       /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> factory, TimeSpan validFor)
        {
            var result = cacheManager.Get(key);
            if (result == null)
            {
                var computed = factory();
                if (computed != null)
                {
                    cacheManager.Put(key, computed, validFor);
                }
                return computed;
            }
           
            return (T)result;
        }
    }
}

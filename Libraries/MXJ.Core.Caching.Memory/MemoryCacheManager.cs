using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MXJ.Core.Infrastructure.Caching;

namespace MXJ.Core.Caching.Memory
{
    public class MemoryCacheManager : ICacheManager
    {
        private static readonly ICacheManager _instance = new MemoryCacheManager();
        public static ICacheManager Instance
        {
            get { return _instance; }
        }

        public void Put(string key, object value)
        {
            HttpRuntime.Cache.Insert(
                key,
                value,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal,
                null);
        }

        public void Put(string key, object value, TimeSpan validFor)
        {
            HttpRuntime.Cache.Insert(
                key,
                value,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                validFor,
                System.Web.Caching.CacheItemPriority.Normal,
                null);
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 删除满足表达式的缓存key
        /// </summary>
        /// <param name="regex">缓存key表达式</param>
        public void RemoveByRegex(string regexKey)
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (CacheEnum.MoveNext())
            {
                Regex regex = new Regex(regexKey, RegexOptions.IgnoreCase);
                if (regex.IsMatch(CacheEnum.Key.ToString()))
                {
                    HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
                }
            }
        }

        public void Clear()
        {
            var all = HttpRuntime.Cache
                .AsParallel()
                .Cast<DictionaryEntry>()
                .Select(x => x.Key.ToString())
                .ToList();

            foreach (var key in all)
            {
                Remove(key);
            }
        }

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }
    }
}

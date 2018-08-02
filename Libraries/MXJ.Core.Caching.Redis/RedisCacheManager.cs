using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MXJ.Core.Infrastructure.Caching;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace MXJ.Core.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private static readonly ICacheManager _instance = new RedisCacheManager();
        public static ICacheManager Instance
        {
            get { return _instance; }
        }
        private ConnectionMultiplexer _rediscon;
        public ConnectionMultiplexer RedisCon { get { return _rediscon; } }
        public RedisCacheManager()
        {
            _rediscon = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisServer"]);
        }
        public void Put(string key, object value)
        {
            var db = _rediscon.GetDatabase();
            var jsonvalue = JsonConvert.SerializeObject(value);
            db.StringSet(key, jsonvalue);
        }

        public void Put(string key, object value, TimeSpan validFor)
        {
            var db = _rediscon.GetDatabase();
            var jsonvalue = JsonConvert.SerializeObject(value);
            db.StringSet(key, jsonvalue);
            db.KeyExpire(key, validFor);
              }

        public void Remove(string key)
        {
            var db = _rediscon.GetDatabase();
            db.KeyDelete(key);
        }

        /// <summary>
        /// 删除满足表达式的缓存key
        /// </summary>
        /// <param name="regex">缓存key表达式</param>
        public void RemoveByRegex(string regexKey)
        {
            var db = _rediscon.GetDatabase();
            var it = _rediscon.GetServer(ConfigurationManager.AppSettings["RedisServer"]).Keys().GetEnumerator();
            while (it.MoveNext())
            {
                Regex regex = new Regex(regexKey, RegexOptions.IgnoreCase);
                if (regex.IsMatch(it.Current.ToString()))
                {
                    db.KeyDelete(it.Current);
                }

            }
        }
        public void Clear()
        {
            var db = _rediscon.GetDatabase();
            var it = _rediscon.GetServer(ConfigurationManager.AppSettings["RedisServer"]).Keys().GetEnumerator();
            while (it.MoveNext())
            {
                db.KeyDelete(it.Current);
            }
        }

        public object Get(string key)
        {
            var db = _rediscon.GetDatabase();
            var jsonvalue = db.StringGet(key);
            return JsonConvert.DeserializeObject(jsonvalue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Infrastructure.Singleton
{
    /// <summary>
    /// 获取和设置单态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : SingletonBase
    {
        static T _instance;

        public static T Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
}

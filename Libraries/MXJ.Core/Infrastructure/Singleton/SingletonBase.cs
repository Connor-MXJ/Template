using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Infrastructure.Singleton
{
    /// <summary>
    /// 单态基类
    /// </summary>
   public class SingletonBase
   {
       static SingletonBase()
       {
           allSingletons = new Dictionary<Type, object>();
       }

       static readonly IDictionary<Type, object> allSingletons;

       public static IDictionary<Type, object> AllSingletons
       {
           get { return allSingletons; }
       }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Persistence.PersistenceAttribute
{
    /// <summary>
    /// 标识该属性不进行持久化
    /// </summary>
   [AttributeUsage(AttributeTargets.Property)]
    public class PersistenceIgnoreAttribute : Attribute
    {
    }
}

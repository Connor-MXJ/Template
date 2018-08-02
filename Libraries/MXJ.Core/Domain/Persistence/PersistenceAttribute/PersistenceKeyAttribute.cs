using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Persistence.PersistenceAttribute
{
    /// <summary>
    /// 标识该属性为主键
    /// </summary>
     [AttributeUsage(AttributeTargets.Property)]
    class PersistenceKeyAttribute : Attribute
    {
    }
}

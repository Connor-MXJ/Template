using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Persistence.PersistenceAttribute
{
    /// <summary>
    /// 标识该类映射的表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
   public class PersistenceTableAttribute:Attribute
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Models
{
    /// <summary>
    /// 聚合根
    /// </summary>
   public interface IAggregateRoot
    {
        /// <summary>
        /// 聚合根标识
        /// </summary>
       Guid AggregateRootId { get; set; }
    }
}

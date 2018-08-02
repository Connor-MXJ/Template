using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Repositories
{
    /// <summary>
    /// 事务处理程序
    /// </summary>
    public interface ITransactionHandle : IDisposable 
     { 
        /// <summary>
        /// 在释放资源后触发
        /// </summary>
         event EventHandler<EventArgs> Disposed; 
 
        /// <summary>
        /// 获取隔离级别
        /// </summary>
         IsolationLevel IsolationLevel { get; } 
     } 

}

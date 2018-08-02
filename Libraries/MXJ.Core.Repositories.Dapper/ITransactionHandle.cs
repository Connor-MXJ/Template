using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.Dapper
{
    /// <summary>
    /// 事务处理程序接口
    /// </summary>
    public interface ITransactionHandle : IDisposable
    {
        /// <summary> 
        /// 释放事务资源时触发
        /// </summary> 
        event EventHandler<EventArgs> Disposed;


        /// <summary> 
        /// 获取事务隔离级别 
        /// </summary> 
        IsolationLevel IsolationLevel { get; }
    }

}

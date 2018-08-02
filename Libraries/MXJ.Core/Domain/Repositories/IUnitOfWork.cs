using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Repositories
{
    /// <summary>
    /// 单元工作模式
    /// </summary>
    public interface IUnitOfWork
    {
       /// <summary>
       /// 是否支持分布式事务
       /// </summary>
       // bool DistributedTransactionSupported { get; }

        /// <summary>
        /// 获取当前单元操作是否已被提交
        /// </summary>
        bool IsCommitted { get; }
     
        /// <summary>
        /// 提交
        /// </summary>
        int Commit();
      
        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
    }
}

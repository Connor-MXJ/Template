using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.Dapper
{
    /// <summary>
    /// 事务处理程序
    /// </summary>
    internal class TransactionHandle : ITransactionHandle
    {
        private IDbTransaction _transaction;

        /// <summary> 
        /// 资源释放事件
        /// </summary> 
        public event EventHandler<EventArgs> Disposed;

        /// <summary> 
        /// 事务隔离级别
        /// </summary> 
        public IsolationLevel IsolationLevel { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transaction"></param>
        public TransactionHandle(IDbTransaction transaction)
        {
            _transaction = transaction;
            IsolationLevel = transaction.IsolationLevel;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();


                OnDisposed();


                _transaction = null;
            }
        }

        /// <summary>
        /// 释放资源事件
        /// </summary>
        protected virtual void OnDisposed()
        {
            var handler = Disposed;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}

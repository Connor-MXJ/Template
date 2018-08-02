using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;
using MXJ.Core.Domain.Repositories;
using MXJ.Core.Utility.Helper;
namespace MXJ.Core.Repositories.Dapper
{
    public class DapperRepositoryContext// : IRepositoryContext
    {
      //  private IDbConnection _connection;
      //  private bool _disposed;
      //  private IDbTransaction _transaction;

      //  public IDbConnection Connection { get { return new SqlConnection(ConfigHelper.ConnectionString); } }

      //  public DapperRepositoryContext()
      //  {
      //      _connection = Connection;
      //  }
      //  /// <summary>
      //  /// 构造函数
      //  /// </summary>
      //  /// <param name="connection"></param>
      //  public DapperRepositoryContext(IDbConnection connection)
      //  {
      //      EnsureAnOpenConnection(connection);
      //  }

      //  /// <summary>
      //  /// 开始事务处理
      //  /// </summary>
      //  /// <param name="isolationLevel"></param>
      //  /// <returns></returns>
      //  public ITransactionHandle Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
      //  {
      //      if (_disposed)
      //      {
      //          throw new ObjectDisposedException(typeof(IUnitOfWork).Name, "资源已经释放");
      //      }
      //      return EnsureTransaction(isolationLevel);
      //  }

      //  /// <summary>
      //  /// 标记为新建
      //  /// </summary>
      //  /// <typeparam name="TEntity"></typeparam>
      //  /// <typeparam name="TKey"></typeparam>
      //  /// <param name="obj"></param>
      // public void RegisterCreate<TEntity>(TEntity obj)
      //      where TEntity : BaseEntity
      //  { }

      //  /// <summary>
      //  /// 标记为修改
      //  /// </summary>
      //  /// <typeparam name="TEntity"></typeparam>
      //  /// <typeparam name="TKey"></typeparam>
      //  /// <param name="obj"></param>
      // public void RegisterUpdate<TEntity>(TEntity obj)
      //      where TEntity : BaseEntity
      //  { }

      //  /// <summary>
      //  /// 标记为删除
      //  /// </summary>
      //  /// <typeparam name="TEntity"></typeparam>
      //  /// <typeparam name="TKey"></typeparam>
      //  /// <param name="obj"></param>
      //public  void RegisterDelete<TEntity>(TEntity obj)
      //      where TEntity : BaseEntity
      //  { }

      //  /// <summary>
      //  /// 执行SQL语句
      //  /// </summary>
      //  /// <param name="sql"></param>
      //  /// <param name="param"></param>
      //  /// <returns></returns>
      //  public int Execute(string sql, dynamic param = null) 
      //   { 
      //       return SqlMapper.Execute(_connection, sql, param, _transaction, null, null); 
      //   }

      //  public IEnumerable<TEntity> Query<TEntity>(string sql, dynamic param = null)
      //  {
      //     return SqlMapper.Query<TEntity>(_connection, sql, param, null, true, null, null);
      //  }

      //  /// <summary>
      //  /// 提交事务
      //  /// </summary>
      //  public void Commit()
      //  {
      //      if (_transaction != null)
      //      {
      //          _transaction.Commit();
      //          _transaction = null;
      //      }
      //      _connection.Close();
      //  }

      //  /// <summary>
      //  /// 回滚事务
      //  /// </summary>
      //  public void Rollback()
      //  {
      //      if (_transaction != null)
      //      {
      //          _transaction.Rollback();
      //          _transaction.Dispose();
      //          _transaction = null;
      //      }
      //  }

      //  /// <summary>
      //  /// 释放资源
      //  /// </summary>
      //  public void Dispose()
      //  {
      //      if (_disposed)
      //      {
      //          return;
      //      }
      //      if (_connection != null)
      //      {
      //          if (_transaction != null)
      //          {
      //              _transaction.Commit();
      //              _transaction = null;
      //          }
      //          _connection.Dispose();
      //          _connection = null;
      //      }
      //      _disposed = true;
      //  }

      //  /// <summary>
      //  /// 确保连接打开
      //  /// </summary>
      //  /// <param name="connection"></param>
      //  private void EnsureAnOpenConnection(IDbConnection connection = null)
      //  {
      //      if (connection != null)
      //      {
      //          _connection = connection;
      //      }
      //      if (_connection.State != ConnectionState.Open)
      //      {
      //          _connection.Open();
      //      }
      //  }

      //  /// <summary>
      //  /// 确保事务打开
      //  /// </summary>
      //  /// <param name="isolationLevel"></param>
      //  /// <returns></returns>
      //  protected virtual ITransactionHandle EnsureTransaction(IsolationLevel isolationLevel)
      //  {
      //      EnsureAnOpenConnection();
      //      if (_transaction == null)
      //      {
      //          _transaction = _connection.BeginTransaction(isolationLevel); 
      //      }
      //      var handle = new TransactionHandle(_transaction);
      //      handle.Disposed += (o, e) => Commit();
      //      return handle;
      //  }
    }
}

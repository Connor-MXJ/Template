using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;

namespace MXJ.Core.Domain.Repositories
{
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 对外公布的上下文
        /// </summary>
        DbContext DbContext { get; }

        #region 方法

        /// <summary>
        /// 标记为新建
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="obj"></param>
        void RegisterCreate<TEntity, TKey>(TEntity obj)
            where TEntity : BaseEntity<TKey>;

        /// <summary>
        /// 标记为修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="obj"></param>
        void RegisterUpdate<TEntity, TKey>(TEntity obj)
            where TEntity : BaseEntity<TKey>;

        /// <summary>
        /// 标记为删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="obj"></param>
        void RegisterDelete<TEntity, TKey>(TEntity obj)
            where TEntity : BaseEntity<TKey>;

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        void RegisterPhysicalDelete<TEntity, TKey>(TEntity entity)
            where TEntity : BaseEntity<TKey>;

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        void DetachedEntity<TEntity, TKey>(TEntity entity)
            where TEntity : BaseEntity<TKey>;
        #endregion
    }
}

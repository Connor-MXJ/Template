using System.Linq;
using MXJ.Core.Domain.Models;
using MXJ.Core.Domain.Repositories;

namespace MXJ.Core.Repositories.EF
{
    /// <summary>
    ///     EntityFramework仓储实现基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EfRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region 属性

        protected abstract EfRepositoryContext EfContext { get; }

        /// <summary>
        ///     获取当前实体数据集合
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get { return EfContext.Set<TEntity, TKey>().Where(r => r.IsDeleted == false); }
        }

        /// <summary>
        ///     获取当前实体数据集合（包括删除的）
        /// </summary>
        public virtual IQueryable<TEntity> AllEntities
        {
            get { return EfContext.Set<TEntity, TKey>(); }
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Create(TEntity entity)
        {
            EfContext.RegisterCreate<TEntity, TKey>(entity);
            return EfContext.Commit();
        }

        /// <summary>
        ///     更新实体记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity)
        {
            EfContext.RegisterUpdate<TEntity, TKey>(entity);
            return EfContext.Commit();
        }


        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int Delete(TKey id)
        {
            var entity = EfContext.Set<TEntity, TKey>().Find(id);
            return entity != null ? Delete(entity) : 0;
        }

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity)
        {
            EfContext.RegisterDelete<TEntity, TKey>(entity);
            return EfContext.Commit();
        }


        /// <summary>
        ///     物理删除指定编号的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int PhysicalDelete(TKey id)
        {
            var entity = EfContext.Set<TEntity, TKey>().Find(id);
            return entity != null ? PhysicalDelete(entity) : 0;
        }

        /// <summary>
        ///     物理删除实体记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int PhysicalDelete(TEntity entity)
        {
            EfContext.RegisterPhysicalDelete<TEntity, TKey>(entity);
            return EfContext.Commit();
        }

        /// <summary>
        ///     查找指定主键的实体记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TEntity GetByKey(TKey key)
        {
            var entity = EfContext.Set<TEntity, TKey>().Find(key);
            return entity == null ? null : (entity.IsDeleted ? null : entity);
        }

        #endregion
    }
}
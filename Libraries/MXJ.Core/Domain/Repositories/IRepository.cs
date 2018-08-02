using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;

namespace MXJ.Core.Domain.Repositories
{
   public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region 属性

        /// <summary>
        /// 返回实体可查询对象
        /// </summary>
        IQueryable<TEntity> Entities { get; }


        /// <summary>
        /// 返回实体可查询对象（包括删除的）
        /// </summary>
        IQueryable<TEntity> AllEntities { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Create(TEntity entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(TKey id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int PhysicalDelete(TKey id);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int PhysicalDelete(TEntity entity);

        /// <summary>
        /// 根据标识id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByKey(TKey id);



        #endregion

    }
}
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;
using MXJ.Core.Domain.Repositories;
using MXJ.Core.Domain.Persistence.Generator;

namespace MXJ.Core.Repositories.Dapper
{
    public class DapperRepository//<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
//        private readonly IRepositoryContext repoContext;

//        /// <summary>
//        /// 仓储上下文
//        /// </summary>
//        protected IRepositoryContext RepoContext
//        {
//            get { return repoContext; }
//        }

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="context"></param>
//        protected DapperRepository(IRepositoryContext context)
//        {
//            repoContext = context;
//        }

//        /// <summary>
//        /// 新增
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//       public int Create(TEntity entity)
//        {

//            var propertyContainer = SQLPairsGenerator.ParseProperties(entity);
//            var sql = string.Format(@"INSERT INTO [{0}] ({1}) 
//            VALUES (@{2}) ",
//                typeof(TEntity).Name,
//                string.Join(", ", propertyContainer.ValueNames),
//                string.Join(", @", propertyContainer.ValueNames));
//          return  repoContext.Execute(sql,propertyContainer.ValuePairs);
        

//        }

//        /// <summary>
//        /// 修改
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//       public int Update(TEntity entity)
//        {
//            var propertyContainer = SQLPairsGenerator.ParseProperties(entity);
//            var sqlIdPairs = SQLPairsGenerator.GetSqlPairs(propertyContainer.IdNames);
//            var sqlValuePairs = SQLPairsGenerator.GetSqlPairs(propertyContainer.ValueNames);
//            var sql = string.Format(@"UPDATE [{0}] 
//            SET {1} WHERE {2}", typeof(TEntity).Name, sqlValuePairs, sqlIdPairs);
//                return  repoContext.Execute(sql,propertyContainer.AllPairs);
//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//       public int Delete(TEntity entity)
//        {
//            var propertyContainer = SQLPairsGenerator.ParseProperties(entity);
//            var sqlIdPairs = SQLPairsGenerator.GetSqlPairs(propertyContainer.IdNames);
//            var sql = string.Format(@"DELETE FROM [{0}] 
//            WHERE {1}", typeof(TEntity).Name, sqlIdPairs);
//            return  repoContext.Execute(sql,propertyContainer.IdPairs);

//        }

//       /// <summary>
//       /// 获取实体
//       /// </summary>
//       /// <param name="entity"></param>
//       /// <returns></returns>
//      public TEntity GetByKey(Guid id)
//       {
//           var sql = string.Format(@"select * FROM [{0}] 
//            WHERE {1}={2}", typeof(TEntity).Name, SQLPairsGenerator.GetPrimeKey<TEntity>(),id);
//           return repoContext.Query<TEntity>(sql, null).FirstOrDefault();
//       }
    }
}

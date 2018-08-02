using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Extension
{
    public static class DbExtension
    {
        /// <summary>
        /// 不存在时添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="set"></param>
        /// <param name="entities"></param>
        public static void AddIfNonExist<TEntity>(this IDbSet<TEntity> set, Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities) where TEntity : class
        {
            var identifierFunc = identifierExpression.Compile();
            var list = set.Select(identifierFunc).ToList();
            foreach (var entity in entities)
            {
                var identifier = identifierFunc(entity).ToString();
                if (!list.Any(p => p.ToString() == identifier))
                {
                    set.Add(entity);
                }
            }
        }
    }
}

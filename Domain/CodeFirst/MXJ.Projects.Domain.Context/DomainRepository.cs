using MXJ.Core.Domain.Models;
using MXJ.Core.Repositories.EF;

namespace MXJ.Projects.Domain.Context
{
    public class DomainRepository<TEntity, TKey> : EfRepository<TEntity, TKey>, IDomainRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        public DomainRepository(DomainRepositoryContext efContext)
        {
            EfContext = efContext;
        }

        /// <summary>
        ///     获取当前使用的数据访问上下文对象
        /// </summary>
        protected override EfRepositoryContext EfContext { get; }
    }
}
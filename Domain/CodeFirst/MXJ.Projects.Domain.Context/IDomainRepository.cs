using MXJ.Core.Domain.Models;
using MXJ.Core.Domain.Repositories;

namespace MXJ.Projects.Domain.Context
{ 
    /// <summary>
    /// 领域仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IDomainRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
    }
}

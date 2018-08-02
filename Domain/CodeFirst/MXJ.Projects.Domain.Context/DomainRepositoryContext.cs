using MXJ.Core.Repositories.EF;
using System.Data.Entity; 

namespace MXJ.Projects.Domain.Context
{
    public class DomainRepositoryContext : EfRepositoryContext, IDomainRepositoryContext
    {
        #region 属性

        /// <summary>
        ///     获取当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context { get; } = new DomainContext();

        #endregion
    }
}
using System;

namespace MXJ.Core.Domain.Models
{
    /// <summary>
    /// 领域实体基础接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体标识
        /// </summary>
        Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using MXJ.Core.Domain.Models;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     角色用户关系表
    /// </summary>
    public class UserRoleRef : BaseEntity<Guid>
    {
        #region Public Properties

        /// <summary>
        /// 关系ID
        /// </summary>
        public Guid RefId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid SysRoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid SysUserId { get; set; }

        #endregion
    }
}
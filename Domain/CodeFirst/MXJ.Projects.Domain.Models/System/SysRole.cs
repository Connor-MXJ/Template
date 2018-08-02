using System;
using System.Collections.Generic;
using MXJ.Core.Domain.Models;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     角色
    /// </summary>
    public class SysRole : BaseEntity<Guid>
    {
        #region Public Properties

        /// <summary>
        ///     是否系统内置角色
        /// </summary>
        public bool IsSystemRole { get; set; }
        
        /// <summary>
        ///     角色描述
        /// </summary>
        public string RoleDesc { get; set; }

        /// <summary>
        ///     角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///     权限菜单集合
        /// </summary>
        public string SysMenuIDs { get; set; }

        /// <summary>
        /// Action集合
        /// </summary>

        public string SysActionIDs { get; set; }
        
        /// <summary>
        ///     角色ID
        /// </summary>
        public Guid SysRoleId { get; set; }
        
        #endregion
    }
}
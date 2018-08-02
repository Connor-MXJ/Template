using System;
using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.DataTransferObjects
{
    public class SysRoleDto : BaseEntityDto<Guid>
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

        /// <summary>
        /// 用户ID集合
        /// </summary>
        public string SysUserIDs { get; set; }
        /// <summary>
        /// 用户名称集合
        /// </summary>
        public string SysUserNames { get; set; }

        #endregion
    }

    public class SysRoleSearchDto : SearchRequestBase
    {
        /// <summary>
        ///     角色名称
        /// </summary>
        public string RoleName { get; set; }

    }
}
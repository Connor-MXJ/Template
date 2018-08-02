using System;
using System.Collections.Generic;
using MXJ.Core.Domain.Models;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     系统菜单
    /// </summary>
    public class SysMenu : BaseEntity<Guid>
    {
        #region Public Properties
          
        /// <summary>
        ///     菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        public int MenuOrder { get; set; }
         
        /// <summary>
        ///     菜单链接地址，仅叶子结点菜单有值
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        ///     菜单样式名字, 仅父节点有值
        /// </summary>
        public string MenuStyleName { get; set; }

        /// <summary>
        ///     父菜单，一级菜单值为0
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        ///     菜单ID
        /// </summary>
        public Guid SysMenuId { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.DataTransferObjects
{
    public class SysMenuDto : TreeBaseDto
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
        ///     父菜单，一级菜单值为0
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     父菜单，一级菜单值为0
        /// </summary>
        public string ParentMenuName { get; set; }


        /// <summary>
        ///     父菜单序号，一级菜单值为0
        /// </summary>
        public int ParentMenuOrder { get; set; }
        /// <summary>
        ///     菜单样式名字, 仅父节点有值
        /// </summary>
        public string MenuStyleName { get; set; }

        /// <summary>
        ///     菜单ID
        /// </summary>
        public Guid SysMenuId { get; set; }
        /// <summary>
        ///     子菜单
        /// </summary>
        [DataMember]
        public IList<SysMenuDto> ChildMenuList { get; set; }

        #endregion
    }

    public class SysMenuSearchDto : SearchRequestBase
    {
        /// <summary>
        ///     菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        ///     父菜单，一级菜单值为0
        /// </summary>
        public string ParentMenuName { get; set; }
    }
}
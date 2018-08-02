using System;
using System.Collections.Generic;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.DataTransferObjects
{
    public class SysUserDto : BaseEntityDto<Guid>
    {
        #region 基本属性

        /// <summary>
        ///     系统用户ID
        /// </summary>
        public Guid SysUserId { get; set; }

        /// <summary>
        ///     登陆账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     登陆密码
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        ///     状态编码
        /// </summary>
        public StatusCode UserStatus { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        public List<Guid> RoleIDs { get; set; }

        public List<string> RoleNames { get; set; }

        public List<string> MenuIds { get; set; }
        public List<string> MenuNames { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }

        public List<SysActionDto> Actions { get; set; }

        #endregion
    }
}
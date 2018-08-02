using System;
using MXJ.Core.Domain.Models;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     系统用户
    /// </summary>
    public class SysUser : BaseEntity<Guid>
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
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }

        #endregion
    }
}
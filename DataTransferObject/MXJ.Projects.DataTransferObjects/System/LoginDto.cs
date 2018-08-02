using MXJ.Projects.Models.DataTransferObjects;
using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.DataTransferObjects
{
    public class LoginDto : BaseEntityDto<Guid>
    {
        #region Public Properties
        /// <summary>
        /// 系统用户ID
        /// </summary>
        [DataMember]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [DataMember]
        public string UserPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember]
        public string VerificationCode { get; set; }

        /// <summary>
        /// 跳转
        /// </summary>
        public string RedirectUrl { get; set; }
        #endregion
    }
}
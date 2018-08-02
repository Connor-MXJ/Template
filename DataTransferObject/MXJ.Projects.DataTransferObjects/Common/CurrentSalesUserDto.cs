using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MXJ.Projects.Models.DataTransferObjects
{
    /// <summary>
    /// 当前店小二用户信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class CurrentSalesUserDto
    {
        #region Public Properties


        /// <summary>
        ///     用户手机号码
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }


        /// <summary>
        ///     用户名称
        /// </summary>
        [DataMember]
        public string RealName { get; set; }


        /// <summary>
        ///     角色
        /// </summary>
        [DataMember]
        public List<Guid> RoleIDs { get; set; }

        public List<string> RoleNames { get; set; }

        public List<string> MenuIds { get; set; }
        public List<string> MenuNames { get; set; }
 

        /// <summary>
        ///     用户标识
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        #endregion
    }
}

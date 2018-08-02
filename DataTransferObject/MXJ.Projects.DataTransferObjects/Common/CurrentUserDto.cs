// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentUserDTO.cs" company="四川物联亿达科技有限公司">
//   四川物联亿达科技有限公司---研发一部智慧社区小组出品
// </copyright>
// <summary>
//   当前用户信息
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MXJ.Projects.Models.DataTransferObjects
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Runtime.Serialization;

    /// <summary>
    /// 当前系统用户信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class CurrentUserDto
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
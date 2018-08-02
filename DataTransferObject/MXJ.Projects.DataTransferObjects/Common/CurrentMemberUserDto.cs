using MXJ.Projects.Domain.Enums;
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
    public class CurrentMemberUserDto
    {
        #region Public Properties


        /// <summary>
        /// 主键
        /// </summary>
        public Guid MemberId { set; get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public MemberTypeCode MemberType { set; get; }

        /// <summary>
        /// 本账户在云仓品台的Key
        /// </summary>
        public int MemberPlatformId { set; get; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        public string NickName { set; get; }

        /// <summary>
        /// 省份编码
        /// </summary>
        public int? ProvCode { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        public int? CityCode { get; set; }

        /// <summary>
        /// 县/区编码
        /// </summary>
        public int? AreaCode { get; set; }

        /// <summary>
        /// 会员所在地详情
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public string MemberNo { get; set; }

        #endregion
    }
}

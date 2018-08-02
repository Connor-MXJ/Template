using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     产品状态
    /// </summary>
    [Serializable]
    [DataContract]
    public enum ProductStatusCode
    {
        /// <summary>
        /// 供应商和店小二共享状态
        /// </summary>
        [EnumMember]
        草稿 = 100,

        /// <summary>
        /// 等待店小二审核
        /// </summary>
        [EnumMember]
        待提交 = 500,

        /// <summary>
        /// 等待网站后台运营审核
        /// </summary>
        [EnumMember]
        待审核 = 200,

        /// <summary>
        /// 正在前台销售中的产品
        /// </summary>
        [EnumMember]
        已上架 = 300,

        /// <summary>
        /// 已经取消销售
        /// </summary>
        [EnumMember]
        已下架 = 400,

        /// <summary>
        /// 未通过后台审核
        /// </summary>
        [EnumMember]
        未通过 = 600,
    }
}
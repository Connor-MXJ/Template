using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     用户类型
    /// </summary>
    [Serializable]
    [DataContract]
    public enum MemberTypeCode
    {
        [EnumMember]
        普通会员 = 1, 
        [EnumMember]
        供应商 = 3, 
    }
}
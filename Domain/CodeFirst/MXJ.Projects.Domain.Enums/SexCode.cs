using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     员工性别
    /// </summary>
    [Serializable]
    [DataContract]
    public enum SexCode
    {
        [EnumMember] 男 = 100,
        [EnumMember] 女 = 200,
        [EnumMember] 不详 = 0
    }
}
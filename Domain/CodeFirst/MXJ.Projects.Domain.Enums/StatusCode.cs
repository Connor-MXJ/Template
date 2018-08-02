using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     用户状态
    /// </summary>
    [Serializable]
    [DataContract]
    public enum StatusCode
    {
        [EnumMember] 正常 = 100,
        [EnumMember] 锁定 = 400
    }
}
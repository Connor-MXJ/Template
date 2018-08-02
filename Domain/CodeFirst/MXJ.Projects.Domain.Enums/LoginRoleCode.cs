using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    /// 登录角色
    /// </summary>
    [Serializable]
    [DataContract]
    public enum LoginRoleCode
    {
        [EnumMember]
        店小二 = 100,
        [EnumMember]
        供应商 = 200

    }
}

using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     数据字典父级
    /// </summary>
    [Serializable]
    [DataContract]
    public enum DictionaryParentCode
    {
        [EnumMember] 证件类型 = 100,
        [EnumMember] 银行 = 200
    }
}
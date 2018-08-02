using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     计量单位
    /// </summary>
    [Serializable]
    [DataContract]
    public enum MeansureUnitCode
    {
        [EnumMember]
        个 = 1,
        [EnumMember]
        包 = 2,
        [EnumMember]
        卷 = 3,
        [EnumMember]
        套 = 4,
        [EnumMember]
        件 = 5,
        [EnumMember]
        箱 = 6,
        [EnumMember]
        辆 = 7,
        [EnumMember]
        PCS = 8,
        [EnumMember]
        台 = 9
    }
}
 
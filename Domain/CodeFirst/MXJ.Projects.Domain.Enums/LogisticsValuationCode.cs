using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     计量单位
    /// </summary>
    [Serializable]
    [DataContract]
    public enum LogisticsValuationCode
    {
        [EnumMember]
        按重量 = 1,
        [EnumMember]
        按体积 = 2
    }
}

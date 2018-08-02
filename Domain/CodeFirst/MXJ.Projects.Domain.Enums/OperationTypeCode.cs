using System;
using System.Runtime.Serialization;

namespace MXJ.Projects.Domain.Enums
{
    /// <summary>
    ///     日志操作类型
    /// </summary>
    [Serializable]
    [DataContract]
    public enum OperationTypeCode
    {
        [EnumMember]
        登录 = 100,
        [EnumMember]
        操作 = 200
    }
}
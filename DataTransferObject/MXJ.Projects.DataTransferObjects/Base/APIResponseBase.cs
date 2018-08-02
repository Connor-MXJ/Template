namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// 响应结果类型
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class APIResponseBase
    {
        /// <summary>
        ///  操作结果
        /// </summary>
        [DataMember]
        public bool IsSuccess { get; set; }

        /// <summary>
        ///  操作描述
        /// </summary>
        [DataMember]
        public string OperationDesc { get; set; }
    }

    /// <summary>
    /// 响应结果类型
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class APIResponseBase<T> : APIResponseBase where T : class
    {
        /// <summary>
        ///  结果
        /// </summary>
        [JsonProperty]
        public T Result { get; set; }
    }
}

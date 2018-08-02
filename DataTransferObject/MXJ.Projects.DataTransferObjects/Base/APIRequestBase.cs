namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Runtime.Serialization;
    using Newtonsoft.Json;
    /// <summary>
    /// 操作请求基类
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class APIRequestBase
    {
        /// <summary>
        /// 用于请求的Token
        /// </summary>
        public string AccessToken { get; set; }
    }

    /// <summary>
    /// 响应结果类型
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class APIRequestBase<T> : APIRequestBase
    {
        #region Public Properties
        /// <summary>
        /// 操作结果
        /// </summary>
        public T Content { get; set; }

        #endregion
    }
}
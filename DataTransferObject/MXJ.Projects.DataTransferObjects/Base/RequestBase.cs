namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Runtime.Serialization;

    /// <summary>
    ///     操作请求基类
    /// </summary>
    [Serializable]
    [DataContract]
    public class RequestBase
    {
        #region Public Properties

        ///// <summary>
        /////     手机客户端请求的基准请求ID
        ///// </summary>
        //[DataMember]
        //public int ClientBaseID { get; set; }

        ///// <summary>
        /////     客户端屏幕滑动方向
        ///// </summary>
        //[DataMember]
        //public bool IsClientUp { get; set; }

        ///// <summary>
        /////     用于请求的Guid
        ///// </summary>
        //[DataMember]
        //public Guid RequestValGuid { get; set; }

        #endregion
    }
}
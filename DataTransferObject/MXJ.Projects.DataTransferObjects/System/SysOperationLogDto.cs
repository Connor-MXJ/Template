namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Runtime.Serialization;
    using Domain.Enums; 
    using MXJ.Projects.Models.DataTransferObjects;
    using MXJ.Core.Utility.Extensions;
    #region DTO

    /// <summary>
    ///     系统操作日志
    /// </summary>
    [Serializable]
    [DataContract]
    public class SysOperationLogDto
    {
        #region Public Properties

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public string DisplayCreatedTime { get { return CreatedTime.ToDisplayDateTime(); } }

        /// <summary>
        ///     操作人IP
        /// </summary>
        [DataMember]
        public string OperationIP { get; set; }

        /// <summary>
        ///     操作类型编码
        /// </summary>
        [DataMember]
        public OperationTypeCode OperationTypeCode { get; set; }

        /// <summary>
        ///     操作类型编码
        /// </summary>
        [DataMember]
        public string DisplayOperationTypeCode
        {
            get
            {
                return this.OperationTypeCode.ToString();
            }
        }

        /// <summary>
        ///     操作链接
        /// </summary>
        [DataMember]
        public string OperationUrl { get; set; }

        /// <summary>
        ///  请求URL参数
        /// </summary>
        [DataMember]
        public string UrlParameter { get; set; }

        /// <summary>
        ///  Form表单参数
        /// </summary>
        [DataMember]
        public string FormParameter { get; set; }

        /// <summary>
        ///  POST请求参数
        /// </summary>
        [DataMember]
        public string PostParameter { get; set; }

        /// <summary>
        ///     操作记录ID
        /// </summary>
        [DataMember]
        public Guid SysOperationLogID { get; set; }

        /// <summary>
        ///     操作人
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>

        [DataMember]
        public string OperationContent { get; set; }

        #endregion
    }

    #endregion

    #region SearchDTO

    /// <summary>
    ///     系统操作日志查询类
    /// </summary>
    [Serializable]
    [DataContract]
    public class SysOperationLogSearchDTO : SearchRequestBase
    {
        #region Public Properties

        // <summary>
        /// 操作类型编码
        /// </summary>
        public string DisplayOperationTypeCode
        {
            get
            {
                return this.OperationTypeCode.ToString();
            }
        }

        // <summary>
        /// 操作类型编码
        /// </summary>
        public OperationTypeCode OperationTypeCode { get; set; }

        /// <summary>
        ///     操作记录ID
        /// </summary>
        public Guid SysOperationLogID { get; set; }


        // <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

        public DateTime operateDate { get; set; }

        #endregion
    }

    #endregion
}
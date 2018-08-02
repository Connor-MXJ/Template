namespace MXJ.Projects.Domain.Models
{
    using global::System;
    using Core.Domain.Models;
    using Enums;
    /// <summary>
    ///     系统操作日志
    /// </summary>
    public class SysOperationLog : BaseEntity<Guid>
    {
        #region Public Properties
        /// <summary>
        ///     操作记录ID
        /// </summary>
        public Guid SysOperationLogID { get; set; }

        /// <summary>
        ///     操作人IP
        /// </summary>
        public string OperationIP { get; set; }

        /// <summary>
        ///     操作类型编码
        /// </summary>
        public OperationTypeCode OperationTypeCode { get; set; }

        /// <summary>
        ///     操作链接
        /// </summary>
        public string OperationUrl { get; set; }

        /// <summary>
        ///  请求URL参数
        /// </summary>
        public string UrlParameter { get; set; }

        /// <summary>
        ///  Form表单参数
        /// </summary>
        public string FormParameter { get; set; }

        /// <summary>
        ///  POST请求参数
        /// </summary>
        public string PostParameter { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>

        public string OperationContent { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }
        
        #endregion
    }
}
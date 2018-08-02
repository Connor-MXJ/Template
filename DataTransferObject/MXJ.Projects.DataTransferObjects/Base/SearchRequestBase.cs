namespace MXJ.Projects.Models.DataTransferObjects
{
    using global::System;
    using global::System.Runtime.Serialization;
    using MXJ.Projects.DataTransferObjects;
    using MXJ.Projects.Domain.Enums;

    /// <summary>
    ///     查询基类
    /// </summary>
    [Serializable]
    [DataContract]
    public class SearchRequestBase : RequestBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestBase"/> class. 
        ///     构造函数
        /// </summary>
        public SearchRequestBase()
        {
            this.Page = 1;
            this.Rows = 12;
            this.IsPage = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 创建开始时间小于
        /// </summary>
        [DataMember]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int Page { get; set; }

        /// <summary>
        ///  每页条数
        /// </summary>
        [DataMember]
        public int Rows { get; set; }

        /// <summary>
        /// 创建开始时间大于
        /// </summary>
        [DataMember]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        [DataMember]
        public bool IsPage { get; set; }

        #endregion

        public LoginRoleCode UserRole { get; set; }

        public Guid? UserId { get; set; }
    }
}
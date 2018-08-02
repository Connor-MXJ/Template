namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    /// 操作响应基类
    /// </summary>
    public class ResponseBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseBase()
        {
            IsSuccess = true;
            IsLogin = true;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDesc { get; set; }
        /// <summary>
        /// 附加描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string RedirectUrl { get; set; }
        #endregion
    }

    /// <summary>
    /// 响应结果类型
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ResponseBase<T> : ResponseBase
    {
        #region Public Properties
        /// <summary>
        /// 操作结果
        /// </summary>
        public T Result { get; set; }

        #endregion
    }

    /// <summary>
    /// 响应结果类型
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ResultResponseBase<T> : ResponseBase
        where T : class
    {
        #region Public Properties
        /// <summary>
        /// 操作结果
        /// </summary>
        public T Result { get; set; }
        #endregion

        public ResultResponseBase()
        {
            Result = default(T);
        }
    }
}
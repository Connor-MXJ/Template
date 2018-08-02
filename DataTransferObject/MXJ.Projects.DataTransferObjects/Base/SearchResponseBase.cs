namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Runtime.Serialization;
    

    /// <summary>
    /// 查询响应基类
    /// </summary>
    [Serializable]
    [DataContract]
    public class SearchResponseBase : ResponseBase
    {
        #region Fields

        /// <summary>
        /// The _page.
        /// </summary>
        private int _page;

        #endregion

        #region Public Properties

        /// <summary>
        ///     当前页
        /// </summary>
        [DataMember]
        public int Page
        {
            get
            {
                return this._page;
            }

            set
            {
                if ((value - 1) * this.Rows >= this.TotalRecordCount)
                {
                    this._page = (int)Math.Ceiling(this.TotalRecordCount / (double)this.Rows);
                }
                else
                {
                    this._page = value;
                }

                if (this._page <= 0)
                {
                    this._page = 1;
                }
            }
        }

        /// <summary>
        ///     每页条数
        /// </summary>
        [DataMember]
        public int Rows { get; set; }

        /// <summary>
        ///     总记录数
        /// </summary>
        [DataMember]
        public int TotalRecordCount { get; set; }

        #endregion
    }

    /// <summary>
    /// 响应结果类型
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    [DataContract]
    public class SearchResponseBase<T> : SearchResponseBase
        where T : class
    {
        #region Public Properties

        /// <summary>
        ///     操作结果
        /// </summary>
        [DataMember]
        public T Result { get; set; }

        #endregion
    }
}
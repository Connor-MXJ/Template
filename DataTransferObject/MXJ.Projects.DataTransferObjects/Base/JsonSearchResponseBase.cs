using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    /// 响应结果类型
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    [DataContract]
    public class ResultSearchResponseBase<T> : SearchResponseBase
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

    /// <summary>
    /// app请求操作响应基类
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    [DataContract]
    public class JsonSearchResponseBase<T>
        where T : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSearchResponseBase{T}"/> class. 
        ///     构造函数
        /// </summary>
        public JsonSearchResponseBase()
        {
            this.Json = new ResultSearchResponseBase<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     响应json字符串
        /// </summary>
        [DataMember]
        public ResultSearchResponseBase<T> Json { get; set; }

        #endregion
    }

    /// <summary>
    ///     app请求操作响应基类
    /// </summary>
    [Serializable]
    [DataContract]
    public class JsonSearchResponseBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSearchResponseBase"/> class. 
        ///     构造函数
        /// </summary>
        public JsonSearchResponseBase()
        {
            this.Json = new SearchResponseBase();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     响应json字符串
        /// </summary>
        [DataMember]
        public SearchResponseBase Json { get; set; }

        #endregion
    }
}

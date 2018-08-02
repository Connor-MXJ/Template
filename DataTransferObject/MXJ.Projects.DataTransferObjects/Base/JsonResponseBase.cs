using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.DataTransferObjects
{
   
    /// <summary>
    /// app请求操作响应基类
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    public class JsonResponseBase<T>
        where T : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResponseBase{T}"/> class. 
        ///     构造函数
        /// </summary>
        public JsonResponseBase()
        {
            this.Json = new ResultResponseBase<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     响应json字符串
        /// </summary>
        public ResultResponseBase<T> Json { get; set; }

        #endregion
    }

    /// <summary>
    ///     app请求操作响应基类
    /// </summary>
    [Serializable]
    public class JsonResponseBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResponseBase"/> class. 
        ///     构造函数
        /// </summary>
        public JsonResponseBase()
        {
            this.Json = new ResponseBase();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     响应json字符串
        /// </summary>
        public ResponseBase Json { get; set; }

        #endregion
    }
}

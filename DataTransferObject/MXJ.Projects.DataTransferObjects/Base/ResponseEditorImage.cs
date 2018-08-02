namespace MXJ.Projects.DataTransferObjects
{
  
    /// <summary>
    /// 响应富文本图片类
    /// </summary>
    public class ResponseEditorFile : ResponseBase
    {
        #region Public Properties
        /// <summary>
        /// 0为正确，不为0 则提示错误信息
        /// </summary>
        public string error { get; set; }

     
        public string message { get; set; }

        public string url { get; set; }
        #endregion
    }
}

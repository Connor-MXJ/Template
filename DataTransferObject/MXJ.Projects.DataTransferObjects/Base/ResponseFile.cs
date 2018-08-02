namespace MXJ.Projects.DataTransferObjects
{
  
    /// <summary>
    /// 响应附件类
    /// </summary>
    public class ResponseFile : ResponseBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the attachment url.
        /// </summary>
        public string AttachmentURL { get; set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }
        #endregion
    }
}

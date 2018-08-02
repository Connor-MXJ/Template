namespace MXJ.Projects.Models.DataTransferObjects
{
    using global::System;

    using global::System.Runtime.Serialization;

    /// <summary>
    ///     文件上传DTO
    /// </summary>
    [Serializable]
    [DataContract]
    public class FileBaseDto
    {
        #region Enums

        /// <summary>
        /// The file type enum.
        /// </summary>
        public enum FileTypeEnum
        {
            /// <summary>
            /// The image.
            /// </summary>
            Image = 100, 

            /// <summary>
            /// The document.
            /// </summary>
            Document = 200, 

            /// <summary>
            /// The pdf.
            /// </summary>
            Pdf = 300, 

            /// <summary>
            /// The video.
            /// </summary>
            Video = 400, 

            /// <summary>
            /// The flash.
            /// </summary>
            Flash = 500, 

            /// <summary>
            /// The file.
            /// </summary>
            File = 600
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     上传文件内容长度
        /// </summary>
        [DataMember]
        public int FileContentLength { get; set; }

        /// <summary>
        ///     上传文件类型
        /// </summary>
        [DataMember]
        public string FileContentType { get; set; }

        /// <summary>
        ///     上传文件名后缀
        /// </summary>
        [DataMember]
        public string FileExtension { get; set; }

        /// <summary>
        ///     上传文件保存路径
        /// </summary>
        [DataMember]
        public string FileFolderPath { get; set; }

        /// <summary>
        ///     上传文件名
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        [DataMember]
        public FileTypeEnum FileType { get; set; }

        /// <summary>
        ///     文件夹类型
        /// </summary>
        [DataMember]
        public string FileTypeFolder { get; set; }

        /// <summary>
        ///     上传文件地址
        /// </summary>
        [DataMember]
        public string FileUrl { get; set; }

        /// <summary>
        ///     文件符合验证
        /// </summary>
        [DataMember]
        public bool IsValid { get; set; }

        #endregion
    }
}
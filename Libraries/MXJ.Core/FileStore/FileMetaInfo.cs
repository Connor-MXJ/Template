using System;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件信息，注释中带*号表示，保存时需要
    /// </summary>
    public class FileMetaInfo
    {
        /// <summary>
        /// 文件id，保存后赋值
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文件名*
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大小*
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 文件类型*
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件的保存路径，保存后赋值
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension
        {
            get
            {
                if (Name == null) return string.Empty;
                var rindex = Name.LastIndexOf(".", StringComparison.Ordinal);
                return rindex > 0 ? Name.Substring(rindex + 1) : string.Empty;
            }
        }

        public string DirPath
        {
            get { return System.IO.Path.GetDirectoryName(Path); }
        }

        /// <summary>
        /// 创建世界
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

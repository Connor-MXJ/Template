using System.Drawing;
using ImageProcessor.Imaging;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// ����ͼ����
    /// </summary>
    public class ThumbnailConfig
    {
        /// <summary>
        /// ����ͼ��С
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// ����ͼ����
        /// </summary>
        public ResizeMode ResizeMode { get; set; }
    }
}
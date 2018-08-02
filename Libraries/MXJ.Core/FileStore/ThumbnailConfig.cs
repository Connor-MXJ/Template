using System.Drawing;
using ImageProcessor.Imaging;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// ËõÂÔÍ¼ÅäÖÃ
    /// </summary>
    public class ThumbnailConfig
    {
        /// <summary>
        /// ËõÂÔÍ¼´óĞ¡
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// ËõÂÔÍ¼ÀàĞÍ
        /// </summary>
        public ResizeMode ResizeMode { get; set; }
    }
}
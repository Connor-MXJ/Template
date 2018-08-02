using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageProcessor.Imaging;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// ͼƬ�洢����
    /// </summary>
    public class ImageStoreRule : FileStoreRule
    {
        private Dictionary<string, ThumbnailConfig> _thumbnailSizes;

        /// <summary>
        /// �Ƿ񱣴�ԭʼͼƬ
        /// </summary>
        public bool SaveOriginalImage { get; set; }

        /// <summary>
        /// �Ƿ����ˮӡ
        /// </summary>
        public bool AddWaterMarker { get; set; }

        /// <summary>
        /// ����ͼ�����ַ���
        /// </summary>
        public string ThumbailString { get; set; }

        /// <summary>
        /// ��ȡ����ͼ����
        /// </summary>
        public Dictionary<string, ThumbnailConfig> ThumbnailSizes
        {
            get { return _thumbnailSizes ?? (_thumbnailSizes = ParseThumbailString(ThumbailString)); }
        }


        /// <summary>
        /// ��������ͼ����
        /// </summary>
        /// <param name="thumbStr"></param>
        /// <returns></returns>
        public Dictionary<string, ThumbnailConfig> ParseThumbailString(string thumbStr)
        {
            if (string.IsNullOrWhiteSpace(thumbStr)) return new Dictionary<string, ThumbnailConfig>();
            return thumbStr.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                .Select(
                    x =>
                        new
                        {
                            Key = x[0],
                            SizeStr = x[1].Split("x".ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                            ResizeStr=x.Length==3?x[2]:"Max"
                        })
                .Select(
                    x =>
                        new
                        {
                            Key = x.Key.Trim(),
                            Config =new ThumbnailConfig()
                            {
                                Size= new Size(Convert.ToInt32(x.SizeStr[0]), Convert.ToInt32(x.SizeStr[1])),
                                ResizeMode = (ResizeMode)Enum.Parse(typeof(ResizeMode), x.ResizeStr,true)
                            }
                        })
                .ToDictionary(x => x.Key, x => x.Config);
        }
    }
}
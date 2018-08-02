using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace MXJ.Core.Utility.Helper
{
    public class ImageHelper
    {
        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailMode mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;


            switch (mode)
            {
                case ThumbnailMode.UsrHeightWidth: //指定高宽缩放（可能变形）
                    break;
                case ThumbnailMode.UsrHeightWidthBound: //指定高宽缩放（可能变形）（过小则不变）
                    if (originalImage.Width <= width && originalImage.Height <= height)
                    {
                        return originalImage;
                    }
                    if (originalImage.Width < width)
                    {
                        towidth = originalImage.Width;
                    }
                    if (originalImage.Height < height)
                    {
                        toheight = originalImage.Height;
                    }
                    break;
                case ThumbnailMode.UsrWidth: //指定宽，高按比例
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.UsrWidthBound: //指定宽（过小则不变），高按比例
                    if (originalImage.Width <= width)
                    {
                        return originalImage;
                    }
                    else
                    {
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    break;
                case ThumbnailMode.UsrHeight: //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.UsrHeightBound: //指定高（过小则不变），宽按比例
                    if (originalImage.Height <= height)
                    {
                        return originalImage;
                    }
                    else
                    {
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    break;
                case ThumbnailMode.Cut: //指定高宽裁减（不变形）
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            //g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);
            g.Dispose();
            return bitmap;
        }

        //保存图片
        public  static void SaveFile(Image img, string phyPath)
        {

            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            try
            {

                img.Save(phyPath);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);

            }
        }

        //保存压缩图片
        public static void SaveCompressedFile(Image img, string childPath, string saveName)
        {

            long quality = 100L;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            double length = ms.Length / ((float)1024 * 1024);
            if (length >= 2)
                quality = 10L;
            else if (length >= 1.5)
                quality = 15L;
            else if (length >= 1)
                quality = 20L;
            else if (length >= 0.8)
                quality = 30L;
            else if (length >= 0.6)
                quality = 40L;
            else if (length >= 0.4)
                quality = 50L;
            else if (length >= 0.3)
                quality = 80L;
            SaveFile(img, childPath, saveName, quality);
            ms.Dispose();
        }

        //保存图片
        public static void SaveFile(Image img, string phyPath, string saveName, long quality)
        {
           // string phyPath = Request.MapPath(fullPath + childPath);
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            Encoder encoder = Encoder.Quality;
            EncoderParameter myCoder = new EncoderParameter(encoder, quality);
            EncoderParameters myCoders = new EncoderParameters(1);
            myCoders.Param[0] = myCoder;
            ImageFormat format = GetFormat(saveName.Substring(saveName.LastIndexOf(".") + 1));
            ImageCodecInfo imgInfo = GetEncoder(format);
            img.Save(phyPath + saveName, imgInfo, myCoders);
        }

        //保存图片
        public static void SaveFile(HttpPostedFileBase postedFile, string phyPath, string saveName)
        {

            //string phyPath = Request.MapPath(fullPath + childPath);
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            try
            {
                postedFile.SaveAs(phyPath + saveName);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);

            }
        }
        //删除文件
        public static void DelFile(string fileName, string phyPath)
        {
            try
            {
                FileInfo fi = new FileInfo(phyPath + fileName);
                if (fi.Exists)
                    fi.Delete();
            }
            catch
            { }
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="childPath"></param>
        /// <param name="saveName"></param>
        public static void SaveMiniImage(HttpPostedFileBase postedFile, int miniImageWidth, int miniImageHeight, string phyPath, string saveName)
        {
            try
            {
                Image oldimg = Image.FromStream(postedFile.InputStream);
               // int intSize = int.Parse(ConfigurationManager.AppSettings["MiniImageSize"]);
                if (oldimg.Height > miniImageHeight || oldimg.Width > miniImageWidth)
                {
                    double wBlance = oldimg.Width / (double)miniImageWidth;
                    double hBlance = oldimg.Height / (double)miniImageHeight;
                    ThumbnailMode tm = wBlance > hBlance ? ThumbnailMode.UsrWidth : ThumbnailMode.UsrHeight;
                    Image img = ImageHelper.MakeThumbnail(oldimg, miniImageWidth, miniImageHeight,tm);
                    SaveFile(img, phyPath, saveName, 100L);
                }
                else
                    SaveFile(postedFile, phyPath, saveName);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
        /// <summary>
        /// //获取特定的图像编解码信息
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
         public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

         public static  ImageFormat GetFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                case "icon":
                    return ImageFormat.Icon;
                case "bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Jpeg;
            }

        }



    }
    public enum ThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        UsrHeightWidth, 
        /// <summary>
        /// 指定高宽缩放（可能变形）（过小则不变）
        /// </summary>
        UsrHeightWidthBound,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        UsrWidth,
        /// <summary>
        /// 指定宽（过小则不变），高按比例
        /// </summary>
        UsrWidthBound,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        UsrHeight,
        /// <summary>
        /// 指定高（过小则不变），宽按比例
        /// </summary>
        UsrHeightBound,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut
    }
}

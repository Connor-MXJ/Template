using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MXJ.Core.FileStore
{
    public static class ImageHelper
    {
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="img">要加水印的原图﻿(System.Drawing)</param>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置1=左上 2=中上 3=右上 4=左中  5=中中 6=右中 7=左下 8=右中 9=右下</param>
        /// <param name="quality">加水印后的质量0~100,数字越大质量越高</param>
        /// <param name="watermarkTransparency">水印图片的透明度1~10,数字越小越透明,10为不透明</param>
        public static void ImageWaterMarkPic(this Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            var g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                return;

            var imageAttributes = new ImageAttributes();
            var colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            var transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            var colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            var xpos = 0;
            var ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            var codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (var codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            var encoderParams = new EncoderParameters();
            var qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            var encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        public static Stream AddWatermark(this Image img, Image watermakeImage, int watermarkStatus, int quality, int watermarkTransparency)
        {
            MemoryStream ms;
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    //设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    var watermark = watermakeImage;

                    if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                    {
                        return img.ToStream();
                        //throw  new ApplicationException("watermark size grater than source image");
                    }

                    var colorMap = new ColorMap
                    {
                        OldColor = Color.FromArgb(255, 0, 255, 0),
                        NewColor = Color.FromArgb(0, 0, 0, 0)
                    };

                    ColorMap[] remapTable = { colorMap };

                    imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                    var transparency = 0.5F;
                    if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                        transparency = (watermarkTransparency / 10.0F);


                    float[][] colorMatrixElements = {
                        new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                        new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                    };

                    var colorMatrix = new ColorMatrix(colorMatrixElements);

                    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    var xpos = 0;
                    var ypos = 0;

                    switch (watermarkStatus)
                    {
                        case 1:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)(img.Height * (float).01);
                            break;
                        case 2:
                            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                            ypos = (int)(img.Height * (float).01);
                            break;
                        case 3:
                            xpos = (int)((img.Width * (float).99) - (watermark.Width));
                            ypos = (int)(img.Height * (float).01);
                            break;
                        case 4:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                            break;
                        case 5:
                            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                            break;
                        case 6:
                            xpos = (int)((img.Width * (float).99) - (watermark.Width));
                            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                            break;
                        case 7:
                            xpos = (int)(img.Width * (float).01);
                            ypos = (int)((img.Height * (float).99) - watermark.Height);
                            break;
                        case 8:
                            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                            ypos = (int)((img.Height * (float).99) - watermark.Height);
                            break;
                        case 9:
                            xpos = (int)((img.Width * (float).99) - (watermark.Width));
                            ypos = (int)((img.Height * (float).99) - watermark.Height);
                            break;
                    }

                    g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

                    var codecs = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (var codec in codecs)
                    {
                        if (codec.MimeType.IndexOf("jpeg", StringComparison.Ordinal) > -1)
                            ici = codec;
                    }
                    var encoderParams = new EncoderParameters();
                    var qualityParam = new long[1];
                    if (quality < 0 || quality > 100)
                        quality = 80;

                    qualityParam[0] = quality;

                    var encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
                    encoderParams.Param[0] = encoderParam;

                    ms = new MemoryStream();
                    if (ici != null)
                        img.Save(ms, ici, encoderParams);
                    else
                        img.Save(ms, img.RawFormat);

                }
                //img.Dispose();
                //watermark.Dispose();
            }
            ms.Position = 0;
            return ms;
        }

        public static Stream ToStream(this Image image)
        {
            var stream = new MemoryStream();
            try
            {
                image.Save(stream, image.RawFormat);
            }
            catch
            {
                stream.Position = 0;
                image.Save(stream, ImageFormat.Jpeg);
            }
            stream.Position = 0;
            return stream;
        }
        public static string CombineImageUrl(string url, string defaultPic = null, string category = "")
        {
            if (string.IsNullOrWhiteSpace(url)) return defaultPic;
            var imageHost = ConfigurationManager.AppSettings["ImageWebHost"];
            if (!string.IsNullOrWhiteSpace(category))
            {

                var rindex = url.LastIndexOf(".");
                if (rindex > 0)
                {
                    var p1 = url.Substring(0, rindex);
                    p1 = p1 + "_" + category;
                    var p2 = url.Substring(rindex);
                    url = p1 + p2;

                }
            }
            return Path.Combine(imageHost, url);
        }

        public static Stream Duplicate(this Stream stream)
        {
            var bs = new List<byte>();
            var buf = new byte[8192];
            var rd = 0;
            while ((rd = stream.Read(buf, 0, buf.Length)) > 0)
            {
                bs.AddRange(buf.Take(rd));
            }
            stream.Position = 0;
            var ms = new MemoryStream(bs.ToArray());
            return ms;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Security
{
   public static  class VerificationCode
   {
       private static int _length = 4;
       private static int _fontSize = 25;
       private static int _padding = 2;
       private static bool _chaos = true;
       private static Color _chaosColor = Color.LightGray;
       private static Color _backgroundColor = Color.White;
       private static Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
       private static string[] _fonts = { "Arial", "Georgia" };
       private static string _codeSerial = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

       private const double Pi = 3.1415926535897932384626433832795;
       private const double Pi2 = 6.283185307179586476925286766559;

       /// <summary>
       /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
       /// </summary>
       /// <param name="srcBmp">图片路径</param>
       /// <param name="bXDir">如果扭曲则选择为True</param>
       /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
       /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
       /// <returns></returns>
       private static System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
       {
           System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
           // 将位图背景填充为白色
           System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
           graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
           graph.Dispose();
           double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
           for (int i = 0; i < destBmp.Width; i++)
           {
               for (int j = 0; j < destBmp.Height; j++)
               {
                   double dx = 0;
                   dx = bXDir ? (Pi2 * (double)j) / dBaseAxisLen : (Pi2 * (double)i) / dBaseAxisLen;
                   dx += dPhase;
                   double dy = Math.Sin(dx);
                   // 取得当前点的颜色
                   int nOldX = 0, nOldY = 0;
                   nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                   nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                   System.Drawing.Color color = srcBmp.GetPixel(i, j);
                   if (nOldX >= 0 && nOldX < destBmp.Width
                    && nOldY >= 0 && nOldY < destBmp.Height)
                   {
                       destBmp.SetPixel(nOldX, nOldY, color);
                   }
               }
           }

           return destBmp;
       }

       /// <summary>
       /// 获取验证码
       /// </summary>
       /// <param name="codeLen"></param>
       /// <returns></returns>
       public static string CreateVerifyCode(int codeLen)
       {
           if (codeLen == 0)
           {
               codeLen = _length;
           }
           string[] arr = _codeSerial.Split(',');
           string code = "";
           int randValue = -1;
           Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
           for (int i = 0; i < codeLen; i++)
           {
               randValue = rand.Next(0, arr.Length - 1);
               code += arr[randValue];
           }
           return code;
       }


       /// <summary>
       /// 保存验证码,确保路径已经存创建且具有访问权限
       /// </summary>
       /// <param name="storePathFileName"></param>
       /// <returns>验证码</returns>
       public static string SaveVerifyCodeStream(string storePathFileName)
       {
           string code = CreateVerifyCode(_length);
           using (var ms = new MemoryStream())
           {

               var image = CreateImageCode(code);
               //将图像保存到指定的流
               image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
               FileInfo file = new FileInfo(storePathFileName);
               image.Save(storePathFileName);
               image.Dispose();
           }
           return code;
       }


       /// <summary>
       /// 生成校验码图片
       /// </summary>
       /// <param name="code">随机码</param>
       /// <returns></returns>
       public static Image CreateImageCode(string code)
       {
           int fSize = _fontSize;
           int fWidth = fSize + _padding;
           int imageWidth = (int)(code.Length * fWidth) + 4 + _padding * 2;
           int imageHeight = fSize * 2 + _padding;
           System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);
           Graphics g = Graphics.FromImage(image);
           g.Clear(_backgroundColor);
           Random rand = new Random();
           //给背景添加随机生成的燥点
           if (_chaos)
           {
               Pen pen = new Pen(_chaosColor, 0);
               int c = imageWidth * imageHeight * 10 / 100;
               for (int i = 0; i < c; i++)
               {
                   int x = rand.Next(image.Width);
                   int y = rand.Next(image.Height);
                   g.DrawRectangle(pen, x, y, 1, 1);
               }
           }

           int left = 0, top = 0, top1 = 1, top2 = 1;
           int n1 = (imageHeight - _fontSize - _padding * 2);
           int n2 = n1 / 4;
           top1 = n2;
           top2 = n2 * 2;
           Font f;
           Brush b;
           int cindex, findex;
           //随机字体和颜色的验证码字符
           for (int i = 0; i < code.Length; i++)
           {
               cindex = rand.Next(_colors.Length - 1);
               findex = rand.Next(_fonts.Length - 1);
               f = new System.Drawing.Font(_fonts[findex], fSize, System.Drawing.FontStyle.Bold);
               b = new System.Drawing.SolidBrush(_colors[cindex]);
               if (i % 2 == 1)
               {
                   top = top2;
               }
               else
               {
                   top = top1;
               }
               left = i * fWidth;
               g.DrawString(code.Substring(i, 1), f, b, left, top);
           }

           //画一个边框 边框颜色为Color.Gainsboro
           g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
           g.Dispose();
           //产生波形
           image = TwistImage(image, true, 0, 4);
           return image;
       }
   }
}

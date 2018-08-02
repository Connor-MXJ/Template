using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Security
{
    /// <summary>
    /// TripleDES 加密 解密
    /// </summary>
    public static class TripleDes
    {
        /// <summary>
        /// 3DES加密KEY
        /// </summary>
        private static byte[] Key
        {
            get
            {
                var key = new byte[24];
                for (int i = 0; i < key.Length; i++)
                {
                    key[i] = (byte)(i + new Random(60).Next());
                }
                return key;
            }
        }

        /// <summary>
        /// 3DES加密IV
        /// </summary>
        private static byte[] Iv
        {
            get
            {
                var iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                return iv;
            }
        }

        private static readonly string Deskey = "SCDESKEY";

        private static readonly string Desiv = "SCDES_IV";

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="plainText">需要加密字串</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            var plainTextArray = Encoding.Default.GetBytes(plainText);
            using (var mStream = new MemoryStream())
            {
                using (var cStream = new CryptoStream(mStream,
                      new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                      CryptoStreamMode.Write))
                {
                    cStream.Write(plainTextArray, 0, plainTextArray.Length);
                    cStream.FlushFinalBlock();
                    byte[] ret = mStream.ToArray();
                    cStream.Close();
                    mStream.Close();
                    return Convert.ToBase64String(ret);
                }
            }
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="plainText">需要加密字串</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string EncryptDES(string data, string desKey, string desIv)
        {
            byte[] key = new byte[8];
            byte[] iv = new byte[8];
            Encoding.ASCII.GetBytes(desKey).CopyTo(key, 0);
            Encoding.ASCII.GetBytes(desIv).CopyTo(iv, 0);
            DES des = DES.Create();
            //这行代码很重要,需要根据不同的字符串选择不同的转换格式  
            byte[] tmp = Encoding.Unicode.GetBytes(data);
            byte[] encryptoData;
            ICryptoTransform encryptor = des.CreateEncryptor(key, iv);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var cs = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        writer.Write(data);
                        writer.Flush();
                    }
                }
                encryptoData = memoryStream.ToArray();
            }
            des.Clear();
            return Convert.ToBase64String(encryptoData);
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="plainText">需要加密字串</param>
        /// <returns></returns>
        public static string EncryptDES(string data)
        {
            return EncryptDES(data, Deskey, Desiv);
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="plainText">需要加密字串</param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, Key, Iv);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="EncryptedDataArray">被加密字串</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string Decrypt(string encryptedData, byte[] key, byte[] iv)
        {
            var encryptedDataArray = Convert.FromBase64String(encryptedData);
            using (var msDecrypt = new MemoryStream(encryptedDataArray))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt,
                      new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
                      CryptoStreamMode.Read))
                {
                    byte[] decryptDataArray = new byte[encryptedDataArray.Length];
                    csDecrypt.Read(decryptDataArray, 0, decryptDataArray.Length);
                    msDecrypt.Close();
                    csDecrypt.Close();
                    string retString = Encoding.Default.GetString(decryptDataArray);
                    if(!string.IsNullOrEmpty(retString))
                    {
                        return retString.Replace("\0", "");
                    }
                    else
                    {
                        return retString;
                    }
                }
            }
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="EncryptedDataArray">被加密字串</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string DecryptDES(string data, string desKey, string desIv)
        {
            byte[] key = new byte[8];
            byte[] iv = new byte[8];
            Encoding.ASCII.GetBytes(desKey).CopyTo(key, 0);
            Encoding.ASCII.GetBytes(desIv).CopyTo(iv, 0);
            string resultData = string.Empty;
            byte[] tmpData = Convert.FromBase64String(data);//转换的格式挺重要  
            DES des = DES.Create();
            ICryptoTransform decryptor = des.CreateDecryptor(key, iv);
            using (var memoryStream = new MemoryStream(tmpData))
            {
                using (var cs = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    StreamReader reader = new StreamReader(cs);
                    resultData = reader.ReadLine();
                }
            }
            return resultData;
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="EncryptedDataArray">被加密字串</param>
        /// <returns></returns>
        public static string DecryptDES(string data)
        {
            return DecryptDES(data, Deskey, Desiv);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="EncryptedDataArray">被加密字串</param>
        /// <returns></returns>
        public static string Decrypt(string encryptedData)
        {
            return Decrypt(encryptedData, Key, Iv);
        }
    }
}

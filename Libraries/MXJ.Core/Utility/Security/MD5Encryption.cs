using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Security
{
    /// <summary>
    /// md5 加密
    /// </summary>
    public static class Md5Encryption
    {
        /// <summary>
        /// MD5散列加密
        /// </summary>
        /// <param name="pwdToEncryption">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string pwdToEncryption)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(pwdToEncryption));
            StringBuilder ret = new StringBuilder();
            foreach (byte b in encryptedBytes.ToArray())
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }
    }
}

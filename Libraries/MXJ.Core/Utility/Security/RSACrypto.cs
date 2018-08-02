using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace MXJ.Core.Utility.Security
{
    /// <summary>
    /// RSA加密解密
    /// </summary>
    public static class RsaCrypto
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns>经过base64加密后的字符串</returns>
        public static string RSAEncrypt(string publicKey, string content)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(publicKey);
                byte[] encryptedData = rsaProvider.Encrypt(Encoding.GetEncoding("GBK").GetBytes(content), false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content">Base64加密后的字符串</param>
        /// <returns>解密后字符串</returns>
        public static string RSADecrypt(string privateKey, string encryptStr)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(privateKey);
                byte[] decryptedData = rsaProvider.Decrypt(Convert.FromBase64String(encryptStr), false);
                return Encoding.GetEncoding("GBK").GetString(decryptedData);
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="rsaPublicKeyInfo">公钥</param>
        /// <param name="content">字符串</param>
        /// <param name="DoOAEPPadding">是否使用OAEP</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(RSAParameters rsaPublicKeyInfo, string content, bool doOaepPadding)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.ImportParameters(rsaPublicKeyInfo);
                byte[] encryptedData = rsaProvider.Encrypt(Encoding.GetEncoding("GBK").GetBytes(content), doOaepPadding);
                return encryptedData;
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="rsaPrivateKeyInfo">私钥</param>
        /// <param name="encryptedData">加密后的字节数组</param>
        /// <param name="DoOAEPPadding">是否使用OAEP</param>
        /// <returns></returns>
        public static string RSADecrypt(RSAParameters rsaPrivateKeyInfo, byte[] encryptedData, bool doOaepPadding)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.ImportParameters(rsaPrivateKeyInfo);
                byte[] decryptedData = rsaProvider.Decrypt(encryptedData, doOaepPadding);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        /// <summary>
        /// 生成密钥
        /// </summary>
        public static RsaKeys BuildingKeys()
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                RsaKeys keys = new RsaKeys();
                rsaProvider.KeySize = 1024;
                keys.PublicKey = ParseKeyRsaKeyValue<RsaPublicKeyValue>(rsaProvider.ToXmlString(false));
                keys.PrivateKey = ParseKeyRsaKeyValue<RsaPrivateKeyValue>(rsaProvider.ToXmlString(true));
                return keys;
            }
        }

        /// <summary>
        /// 将密钥装换成RSA键值
        /// </summary>
        /// <param name="keyStr"></param>
        /// <returns></returns>
        public static T ParseKeyRsaKeyValue<T>(string keyXml) where T : RsaPublicKeyValue
        {
            using (StringReader sr = new StringReader(keyXml))
            {
                try
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    T t = (T)xz.Deserialize(sr);
                    return t;
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        [Serializable, XmlRoot("RSAKeyValue", Namespace = "", IsNullable = false)]
        public class RsaPublicKeyValue
        {
            [XmlElement("Modulus")]
            public string Modulus { get; set; }
            [XmlElement("Exponent")]
            public string Exponent { get; set; }

            public string XmlToBase64String()
            {
                Encoding byteConverter = Encoding.GetEncoding("GBK");
                string xml = ToString();
                xml = Convert.ToBase64String(byteConverter.GetBytes(xml));
                return xml;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Clear();
                builder.Append("<RSAKeyValue>");
                builder.Append("<Modulus>");
                builder.Append(Modulus.Trim());
                builder.Append("</Modulus>");
                builder.Append("<Exponent>");
                builder.Append(Exponent.Trim());
                builder.Append("</Exponent>");
                builder.Append("</RSAKeyValue>");
                return builder.ToString();
            }
        }

        [Serializable, XmlRoot("RSAKeyValue", Namespace = "", IsNullable = false)]
        public class RsaPrivateKeyValue : RsaPublicKeyValue
        {
            [XmlElement("P")]
            public string P { get; set; }
            [XmlElement("Q")]
            public string Q { get; set; }
            [XmlElement("DP")]
            public string Dp { get; set; }
            [XmlElement("DQ")]
            public string Dq { get; set; }
            [XmlElement("InverseQ")]
            public string InverseQ { get; set; }
            [XmlElement("D")]
            public string D { get; set; }

            public new string XmlToBase64String()
            {
                return base.XmlToBase64String();
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Clear();
                builder.Append("<RSAKeyValue>");
                builder.Append("<Modulus>");
                builder.Append(Modulus.Trim());
                builder.Append("</Modulus>");

                builder.Append("<Exponent>");
                builder.Append(Exponent.Trim());
                builder.Append("</Exponent>");

                builder.Append("<P>");
                builder.Append(P.Trim());
                builder.Append("</P>");

                builder.Append("<Q>");
                builder.Append(Q.Trim());
                builder.Append("</Q>");

                builder.Append("<DP>");
                builder.Append(Dp.Trim());
                builder.Append("</DP>");

                builder.Append("<DQ>");
                builder.Append(Dq.Trim());
                builder.Append("</DQ>");

                builder.Append("<InverseQ>");
                builder.Append(InverseQ.Trim());
                builder.Append("</InverseQ>");

                builder.Append("<D>");
                builder.Append(D.Trim());
                builder.Append("</D>");

                builder.Append("</RSAKeyValue>");
                return builder.ToString();
            }
        }

        [Serializable]
        public class RsaKeys
        {
            public RsaPublicKeyValue PublicKey { get; set; }
            public RsaPrivateKeyValue PrivateKey { get; set; }
        }

    }

    /// <summary>
    /// RSA转换公钥私钥
    /// </summary>
    public static class RsaKeysConvert
    {
        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKey">java生成的RSA私钥</param>
        /// <returns></returns>
        public static string RsaPrivateKeyJavaToDotNet(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA公钥格式转换，java->.net
        /// </summary>
        /// <param name="publicKey">java生成的公钥</param>
        /// <returns></returns>
        public static string RsaPublicKeyJavaToDotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA私钥格式转换，.net->java
        /// </summary>
        /// <param name="privateKey">.net生成的私钥</param>
        /// <returns></returns>
        public static string RsaPrivateKeyDotNetToJava(string privateKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(privateKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger exp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            BigInteger d = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
            BigInteger q = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
            BigInteger dp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
            BigInteger dq = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
            BigInteger qinv = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));
            RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>
        /// RSA公钥格式转换，.net->java
        /// </summary>
        /// <param name="publicKey">.net生成的公钥</param>
        /// <returns></returns>
        public static string RsaPublicKeyDotNetToJava(string publicKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(publicKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            RsaKeyParameters pub = new RsaKeyParameters(false, m, p);
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }
    }
}

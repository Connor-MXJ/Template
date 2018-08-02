using System;
using System.IO;
using NSXmlSerialization = System.Xml.Serialization;

namespace MXJ.Core.Bus.Serialization
{
    /// <summary>
    /// 序列化为xml
    /// </summary>
    public class ObjectXmlSerializer : IObjectSerializer
    {
        #region IObjectSerializer Members
       /// <summary>
       /// 序列化
       /// </summary>
       /// <typeparam name="TObject"></typeparam>
       /// <param name="obj"></param>
       /// <returns></returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            Type graphType = obj.GetType();
            NSXmlSerialization.XmlSerializer xmlSerializer = new NSXmlSerialization.XmlSerializer(graphType);
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                ret = ms.ToArray();
                ms.Close();
            }
            return ret;
        }
       
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            NSXmlSerialization.XmlSerializer xmlSerializer = new NSXmlSerialization.XmlSerializer(typeof(TObject));
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)xmlSerializer.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}

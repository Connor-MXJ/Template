using System;
using System.IO;
using System.Runtime.Serialization;

namespace MXJ.Core.Bus.Serialization
{
   /// <summary>
   /// 序列化为xml流
   /// </summary>
    public class ObjectDataContractSerializer : IObjectSerializer
    {
        #region IObjectSerializer接口成员
      /// <summary>
      /// 序列化
      /// </summary>
      /// <typeparam name="TObject"></typeparam>
      /// <param name="obj"></param>
      /// <returns></returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            Type graphType = obj.GetType();
            DataContractSerializer js = new DataContractSerializer(graphType);
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, obj);
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
            DataContractSerializer js = new DataContractSerializer(typeof(TObject));
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)js.ReadObject(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}

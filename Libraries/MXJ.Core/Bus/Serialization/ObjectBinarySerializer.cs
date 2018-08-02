using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MXJ.Core.Bus.Serialization
{
   /// <summary>
   /// 对象序列化为二进制
   /// </summary>
    public class ObjectBinarySerializer : IObjectSerializer
    {
        #region 私有变量
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        #endregion

        #region IObjectSerializer接口成员
       /// <summary>
       /// 序列化
       /// </summary>
       /// <typeparam name="TObject"></typeparam>
       /// <param name="obj"></param>
       /// <returns></returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, obj);
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
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)_binaryFormatter.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}

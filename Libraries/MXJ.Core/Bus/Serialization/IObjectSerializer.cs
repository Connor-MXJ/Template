using System;

namespace MXJ.Core.Bus.Serialization
{
   /// <summary>
   /// 序列化接口
   /// </summary>
    public interface IObjectSerializer
    {
       /// <summary>
       /// 序列化
       /// </summary>
       /// <typeparam name="TObject"></typeparam>
       /// <param name="obj"></param>
       /// <returns></returns>
        byte[] Serialize<TObject>(TObject obj);
       
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        TObject Deserialize<TObject>(byte[] stream);
    }
}

using System;

namespace MXJ.Core.Bus.Message
{
   /// <summary>
   /// 消息分发属性
   /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RegisterDispatchAttribute : Attribute
    {
    }
}

using System;

namespace MXJ.Core.Domain.Events
{
   /// <summary>
   /// 并行执行属性
   /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class ParallelExecutionAttribute : Attribute
    {

    }
}

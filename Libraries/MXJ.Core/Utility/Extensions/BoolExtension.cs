using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Extensions
{
    /// <summary>
    /// bool 扩展
    /// </summary>
    public static class BoolExtension
    {
        /// <summary>
        /// bool 显示为是否，否
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToDisplayBool(this bool b)
        {
            return b ? "是" : "否";
        }
    }
}

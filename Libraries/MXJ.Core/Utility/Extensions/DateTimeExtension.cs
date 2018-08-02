using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Extensions
{
    /// <summary>
    /// 日期扩展
    /// </summary>
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

       /// <summary>
       /// 是否为日期
       /// </summary>
       /// <param name="target"></param>
       /// <returns></returns>
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        /// <summary>
        /// 转化为日期时间格式，如2014-09-10 10:20:30
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDisplayDateTime(this DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return string.Empty;
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 转化为日期格式，如2014-09-10
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDisplayDate(this DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return string.Empty;
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转化为日期时间格式，如2014-09-10 10:20:30
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDisplayDateTime(this DateTime? dt)
        {
            if (dt.HasValue && dt.Value != DateTime.MinValue)
                return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Empty;
        }

        /// <summary>
        /// 转化为日期格式，如2014-09-10
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDisplayDate(this DateTime? dt)
        {
            if (dt.HasValue && dt.Value != DateTime.MinValue)
                return dt.Value.ToString("yyyy-MM-dd");
            return string.Empty;
        }

    }
}

using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace MXJ.Core.Utility.Extensions
{
    /// <summary>
    /// SQLMySqlDataReader帮助类
    /// </summary>
    public static class MySqlDataReaderExtension
    {
        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetNullableString(this MySqlDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (obj == null || obj == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(obj);
        }
        /// <summary>
        /// 获取整数值
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetNullableInt32(this MySqlDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (obj == null || obj == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 获取小数值
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static Decimal GetNullableDecimal(this MySqlDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (obj == null || obj == DBNull.Value)
            {
                return default(Decimal);
            }

            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// 获取可空的时间
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DateTime? GetNullableDataTime(this MySqlDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this MySqlDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (Enum.IsDefined(typeof(T), obj))
            {
                return (T)obj;
            }

            throw new NotImplementedException("枚举中没有定义。");
        }

    }
}

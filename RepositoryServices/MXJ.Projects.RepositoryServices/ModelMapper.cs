using System;
using System.Collections.Generic;
using System.Data;

namespace MXJ.Projects.RepositoryServices
{
    /// <summary>
    /// 数据映射
    /// </summary>
    public static class ModelMapper
    {
        public static T GetValueOrDefault<T>(this IDataReader reader, string field, T defaultValue)
        {
            if (!reader.ColumnExists(field))
            {
                return defaultValue;
            }

            var value = reader.GetValue(reader.GetOrdinal(field));
            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            object rtn = null;
            try
            {
                rtn = Convert.ChangeType(value, typeof (T));
            }
            catch (Exception e)
            {
                if (typeof (T) == typeof (string))
                {
                    rtn = value.ToString();
                }
                else
                {
                    var nullableType = Nullable.GetUnderlyingType(typeof (T));
                    if (nullableType == null)
                    {
                        return (T) value;
                    }

                    if (nullableType.IsEnum && value.GetType().IsValueType)
                    {
                        var enm = Enum.ToObject(nullableType, value);
                        var enmval = Convert.ChangeType(enm, nullableType);
                        return (T) enmval;
                    }


                    try
                    {
                        rtn = Convert.ChangeType(value, nullableType);
                        return (T) rtn;
                    }
                    catch
                    {
                        //
                    }
                    
                    return defaultValue;
                }
            }

            return (T) rtn;
        }

        public static int? GetIntValue(this IDataReader reader, string field)
        {
            if (!reader.ColumnExists(field)) return null;
            var value = reader.GetValue(reader.GetOrdinal(field));
            if (value == DBNull.Value) return null;
            return Convert.ToInt32(value);
        }

        public static decimal? GetDecimalValue(this IDataReader reader, string field)
        {
            if (!reader.ColumnExists(field)) return null;
            var value = reader.GetValue(reader.GetOrdinal(field));
            if (value == DBNull.Value) return null;
            return Convert.ToDecimal(value);
        }

        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> ReadAll<T>(this IDataReader reader, Func<IDataReader, T> mapFunc)
        {
            while (reader.Read())
            {
                yield return mapFunc(reader);
            }
        }

        public static T GetValueOrDefault<T>(this DataRow row, string field, T defaultValue)
        {
            var value = row[field];
            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            var rtn = Convert.ChangeType(value, typeof (T));
            return (T) rtn;
        }
    }
}
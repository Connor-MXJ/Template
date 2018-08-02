using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Extensions
{
    /// <summary>
    /// 键值对DTO
    /// </summary>
    public class KeyValueDto
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        public static IEnumerable<Type> EnumTypes { get; set; }

        /// <summary>
        /// 转化为集合
        /// </summary>
        /// <param name="implementAssemblyFile">枚举程序集名称的长格式</param>
        /// <param name="enumName">枚举名称</param>
        /// <returns>对象集合</returns>
        public static List<KeyValueDto> GetKeyValues(string assemblyString, string enumName)
        {
            if (EnumTypes == null)
            {
                EnumTypes = from t in Assembly.Load(assemblyString).GetTypes()
                            where (((Type)t).IsAssignableFrom(t) && ((Type)t).IsEnum)
                            select t;
            }
            var list = new List<KeyValueDto>();
            if (null != EnumTypes && EnumTypes.Count() > 0)
            {
                var e = EnumTypes.SingleOrDefault(m => m.Name == enumName);
                if (null != e)
                {
                    list = GetKeyValues(e);
                }
            }
            return list;
        }

        /// <summary>
        /// 转化为集合
        /// </summary>
        /// <param name="enumType">枚举Type</param>
        /// <returns>对象集合</returns>
        public static List<KeyValueDto> GetKeyValues(Type enumType)
        {
            var list = new List<KeyValueDto>();
            foreach (int i in Enum.GetValues(enumType))
            {
                list.Add(new KeyValueDto()
                {
                    Key = i,
                    Value = Enum.GetName(enumType, i)
                });
            }
            return list;
        }

        /// <summary>
        /// 转化为集合
        /// </summary>
        /// <param name="e">枚举类</param>
        /// <returns>对象集合</returns>
        public static List<KeyValueDto> GetKeyValues(Enum e)
        {
            var list = new List<KeyValueDto>();
            foreach (int i in Enum.GetValues(e.GetType()))
            {
                list.Add(new KeyValueDto()
                {
                    Key = i,
                    Value = Enum.GetName(e.GetType(), i)
                });
            }
            return list;
        }

        /// <summary>
        /// 转化为键值对集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>键值对集合</returns>
        public static Dictionary<int, string> GetDictionary<T>() where T : IComparable
        {
            Dictionary<int, string> dics = new Dictionary<int, string>();
            foreach (int s in Enum.GetValues(typeof(T)))
            {
                dics.Add(s, Enum.GetName(typeof(T), s));
            }
            return dics;
        }

        /// <summary>
        /// 转换为枚举集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> GetEnumKeys<T>() where T : IComparable
        {
            IList<T> list = new List<T>();
            foreach (T key in Enum.GetValues(typeof(T)))
            {
                list.Add(key);
            }
            return list;
        }

        /// <summary>
        /// 获取枚举属性上的描述特性内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns>描述内容</returns>
        public static string GetEnumDescriptionAttribute(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attribute = fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false)
                   .Cast<System.ComponentModel.DescriptionAttribute>()
                   .FirstOrDefault();
            if (attribute != null)
                return attribute.Description;
            return value.ToString();
        }
    }
}

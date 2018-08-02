using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Persistence.PersistenceAttribute;

namespace MXJ.Core.Domain.Persistence.Generator
{
   public class SqlPairsGenerator
    {
        #region 反射

       /// <summary>
       /// 获取主键
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <returns></returns>
       public static string GetPrimeKey<T>()
       {

           var typeName = typeof(T).Name;
           var validKeyNames = new[] { "Id", 
            string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                if (property.IsDefined(typeof(PersistenceKeyAttribute), false) || validKeyNames.Contains(name))
                {
                    return name;
                }
            }
            return string.Empty;

       }
      /// <summary>
      /// 生成属性集合
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="obj"></param>
      /// <returns></returns>
        public static PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[] { "Id", 
            string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                 if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                if (property.GetSetMethod() == null)
                    continue;

                if (property.IsDefined(typeof(PersistenceIgnoreAttribute), false))
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(PersistenceKeyAttribute), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

       /// <summary>
       /// 获取键值对
       /// </summary>
       /// <param name="keys"></param>
       /// <param name="separator"></param>
       /// <returns></returns>
        public static string GetSqlPairs
        (IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }

        public void SetId<T>(T obj, int id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(obj, id, null);
                }
            }
        }

        #endregion
       
    }
}

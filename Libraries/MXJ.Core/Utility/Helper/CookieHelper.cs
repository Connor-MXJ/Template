using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MXJ.Core.Utility.Helper
{
   public static class CookieHelper
    {
        #region Cookie 操作
        /// <summary>
        /// 根据cookie名字获取cookie，如果不存在该cookie则创建
        /// </summary>
        /// <param name="cookieName">cookie名字</param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName.Trim()];
            if (null == cookie)
            {
                cookie = new HttpCookie(cookieName.Trim());
                //var dt = DateTime.Now;
                //var ts = new TimeSpan(0, 0, 10, 0, 0);//过期时间为10分钟
                //cookie.Expires = dt.Add(ts);//设置过期时间
            }
            return cookie;
        }

        /// <summary>
        /// 为cookie输入值，如果key存在则覆盖该key的值，如果不存在则添加键值
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool PutValue(this HttpCookie cookie, string key, string value)
        {
            if (null != cookie)
            {
                if (cookie.Values.AllKeys.Contains(key.Trim()))
                {
                    cookie.Values.Set(key.Trim(), value.Trim());
                }
                else
                {
                    cookie.Values.Add(key.Trim(), value.Trim());
                }
                HttpContext.Current.Response.AppendCookie(cookie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据key获得Cookie中的字典项的值
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(this HttpCookie cookie, string key)
        {
            if (null != cookie && cookie.Values.Count > 0)
            {
                var value = cookie.Values.Get(key.Trim());
                if (!string.IsNullOrEmpty(value))
                    return value.Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获得Cookie中的全部值
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>字典项集合</returns>
        public static IDictionary<string, string> GetValues(this HttpCookie cookie)
        {
            IDictionary<string, string> items = new Dictionary<string, string>();
            if (null != cookie && cookie.Values.Count > 0)
            {
                foreach (var key in cookie.Values.Keys)
                {
                    items.Add(key + string.Empty, cookie.Values.Get(key + string.Empty));
                }
            }
            return items;
        }

        /// <summary>
        /// 获得Cookie中的全部值
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>返回一个对象</returns>
        public static T GetValues<T>(this HttpCookie cookie, T model) where T : class,new()
        {
            if (null != cookie && cookie.Values.Count > 0)
            {
                var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    var value = cookie.Values.Get(p.Name.Trim());
                    if (null != value)
                    {
                        p.SetValue(model, ConvertValue(p.PropertyType, value.Trim()));
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 为cookie添加字典项
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="values">字典项</param>
        public static void PutValues(this HttpCookie cookie, IDictionary<string, string> values)
        {
            if (null != cookie)
            {
                IDictionary<string, string> items = GetValues(cookie);
                foreach (var item in values)
                {
                    if (items.Keys.Contains(item.Key.Trim()))
                    {
                        cookie.Values.Set(item.Key.Trim(), item.Value.Trim());
                    }
                    else
                    {
                        cookie.Values.Add(item.Key.Trim(), item.Value.Trim());
                    }
                }
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 为cookie添加字典项
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="model">对象</param>
        public static void PutValues<T>(this HttpCookie cookie, T model) where T : class,new()
        {
            if (null != cookie)
            {
                if (null != model)
                {
                    var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (properties.Length > 0)
                    {
                        var items = cookie.GetValues();
                        foreach (var p in properties)
                        {
                            var type = p.PropertyType;
                            if (!type.IsInterface && !type.IsArray && !type.IsGenericType && !type.IsGenericParameter && !type.IsPointer)
                            {
                                var name = p.Name.Trim();
                                var value = p.GetValue(model);
                                if (null != value)
                                {
                                    if (items.Keys.Contains(name))
                                    {
                                        cookie.Values.Set(name, value.ToString().Trim());
                                    }
                                    else
                                    {
                                        cookie.Values.Add(name, value.ToString().Trim());
                                    }
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 清空cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static bool ClearCookie(this HttpCookie cookie)
        {
            if (null != cookie)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cookie.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
                HttpContext.Current.Response.AppendCookie(cookie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据key移除cookie中的值
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveValue(this HttpCookie cookie, string key)
        {
            if (null != cookie && cookie.Values.Count > 0)
            {
                if (cookie.Values.AllKeys.Contains(key.Trim()))
                {
                    cookie.Values.Remove(key.Trim());
                    HttpContext.Current.Response.AppendCookie(cookie);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 移除所有值
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static bool RemoveValues(this HttpCookie cookie)
        {
            if (null != cookie && cookie.Values.Count > 0)
            {
                foreach (var key in cookie.Values.AllKeys)
                {
                    cookie.Values.Remove(key.Trim());
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ConvertValue(Type type, string value)
        {
            if (null != type && type.IsPublic)
            {
                var name = type.Name;
                type = type.UnderlyingSystemType;
                if (name.Equals("Nullable`1"))
                {
                    type = Nullable.GetUnderlyingType(type).UnderlyingSystemType;
                }
                if (type == typeof(Int16))
                {
                    return Convert.ToInt16(value);
                }
                if (type == typeof(Int32))
                {
                    return Convert.ToInt32(value);
                }
                if (type == typeof(Int64))
                {
                    return Convert.ToInt64(value);
                }
                if (type == typeof(UInt16))
                {
                    return Convert.ToUInt16(value);
                }
                if (type == typeof(UInt32))
                {
                    return Convert.ToUInt32(value);
                }
                if (type == typeof(UInt64))
                {
                    return Convert.ToUInt64(value);
                }
                if (type == typeof(int))
                {
                    return Convert.ToInt32(value);
                }
                if (type == typeof(sbyte))
                {
                    return Convert.ToSByte(value);
                }
                if (type == typeof(double))
                {
                    return Convert.ToDouble(value);
                }
                if (type == typeof(long))
                {
                    return Convert.ToDouble(value);
                }
                if (type == typeof(float))
                {
                    return Convert.ToSingle(value);
                }
                if (type == typeof(decimal))
                {
                    return Convert.ToDecimal(value);
                }
                if (type == typeof(bool))
                {
                    return Convert.ToBoolean(value);
                }
                if (type == typeof(byte))
                {
                    return Convert.ToByte(value);
                }
                if (type == typeof(char))
                {
                    return Convert.ToChar(value);
                }
                if (type == typeof(DateTime))
                {
                    return Convert.ToDateTime(value);
                }
                if (type == typeof(string))
                {
                    return Convert.ToString(value);
                }
                if (type.IsEnum)
                {
                    return Convert.ToInt32(value);
                }
            }
            return value;
        }
        #endregion
    }
}

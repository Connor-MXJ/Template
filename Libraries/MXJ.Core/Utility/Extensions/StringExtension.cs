using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MXJ.Core.Utility.Helper;

namespace MXJ.Core.Utility.Extensions
{
    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(
            @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex EmailExpression =
            new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        private static readonly char[] IllegalUrlCharacters =
        {
            ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',',
            '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘',
            '’', '“', '”', '»', '«'
        };

        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string FormatWith(this string target, params object[] args)
        {
            ArgumentChecker.IsNotEmpty(target, "target");

            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        public static string Hash(this string target)
        {
            ArgumentChecker.IsNotEmpty(target, "target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        public static string WrapAt(this string target, int index)
        {
            const int dotCount = 3;

            ArgumentChecker.IsNotEmpty(target, "target");
            ArgumentChecker.IsNotNegativeOrZero(index, "index");

            return (target.Length <= index)
                ? target
                : string.Concat(target.Substring(0, index - dotCount), new string('.', dotCount));
        }

        public static string StripHtml(this string target)
        {
            return StripHtmlExpression.Replace(target, string.Empty);
        }

        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }

        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T) Enum.Parse(typeof (T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        /// <summary>
        ///     去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码   </param>
        /// <returns>已经去除后的文字</returns>
        public static string FilterHtml(this string target)
        {
            //删除脚本
            target = Regex.Replace(target, @"<script[^>]*?>.*?</script>", "",
                RegexOptions.IgnoreCase);
            //删除HTML
            target = Regex.Replace(target, @"<(.[^>]*)>", "",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"([\r\n])[\s]+", "",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"-->", "", RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"<!--.*", "", RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(quot|#34);", "\"",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(amp|#38);", "&",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(lt|#60);", "<",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(gt|#62);", ">",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(nbsp|#160);", "   ",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(iexcl|#161);", "\xa1",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(cent|#162);", "\xa2",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(pound|#163);", "\xa3",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&(copy|#169);", "\xa9",
                RegexOptions.IgnoreCase);
            target = Regex.Replace(target, @"&#(\d+);", "",
                RegexOptions.IgnoreCase);
            target.Replace("<", "");
            target.Replace(">", "");
            target.Replace("\r\n", "");
            target = HttpContext.Current.Server.HtmlEncode(target).Trim();

            return target;
        }

    }
}
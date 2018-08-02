using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Helper
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public static string DependencyResolverTypeName { get; private set; }

        /// <summary>
        /// 连接串
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public static string SiteUrl { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static ConfigHelper()
        {
            DependencyResolverTypeName = ConfigurationManager.AppSettings["DependencyResolverTypeName"].Trim();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                SiteUrl = ConfigurationManager.AppSettings["SiteUrl"].Trim().TrimEnd('/');
            }
            ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString.Trim();
        }

    }
}

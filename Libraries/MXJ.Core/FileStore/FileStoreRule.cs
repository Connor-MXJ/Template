using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件存储规则
    /// </summary>
    public abstract class FileStoreRule
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 保存目录
        /// </summary>
        public string StoreDir { get; set; }

        /// <summary>
        /// 子目录模板
        /// </summary>
        public string SubDirTemplate { get; set; }

        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <returns></returns>
        public string GetSaveDir()
        {
            var rgx = new Regex("{(?<val>.+?)}");
            var sub = rgx.Replace(SubDirTemplate, GetTempleValue);
            return Path.Combine(StoreDir, sub);
        }


        /// <summary>
        /// 解析模板数据
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string GetTempleValue(Match m)
        {
            var dt = DateTime.Now;
            var val = m.Result("${val}");
            switch (val)
            {
                case "year":
                    return dt.ToString("yyyy");
                case "month":
                    return dt.ToString("MM");
                case "day":
                    return dt.ToString("dd");
                case "guid":
                    return Guid.NewGuid().ToString();
                case "GUID":
                    return Guid.NewGuid().ToString("N");
            }
            throw new NotSupportedException("unsurport templ param:" + val);
        }
    }
}

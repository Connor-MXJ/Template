using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// �ļ��洢����
    /// </summary>
    public abstract class FileStoreRule
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public string StoreDir { get; set; }

        /// <summary>
        /// ��Ŀ¼ģ��
        /// </summary>
        public string SubDirTemplate { get; set; }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns></returns>
        public string GetSaveDir()
        {
            var rgx = new Regex("{(?<val>.+?)}");
            var sub = rgx.Replace(SubDirTemplate, GetTempleValue);
            return Path.Combine(StoreDir, sub);
        }


        /// <summary>
        /// ����ģ������
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core
{
    public class ConfigManager
    {
        private static string _fileStoreBaseDir;
        private static Dictionary<Size, Image> _watermarkSet;

        public static string DbConnectString => ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        public string FileStoreBaseDir
        {
            get
            {
                return _fileStoreBaseDir ?? (_fileStoreBaseDir = ConfigurationManager.AppSettings["FileStoreBaseDir"]);
            }
        }

        /// <summary>
        /// 水印配置
        /// </summary>
        public Dictionary<Size, Image> WatermarkSet
        {
            get
            {
                if (_watermarkSet != null) return _watermarkSet;

                var watermarkSet = ConfigurationManager.AppSettings["WatermarkSet"];
                _watermarkSet = watermarkSet.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    .Select(
                        x =>
                            new
                            {
                                Size = x[0].Split("x".ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                                Path = x[1]
                            })
                    .Select(
                        x =>
                            new
                            {
                                Size = new Size(Convert.ToInt32(x.Size[0]), Convert.ToInt32(x.Size[1])),
                                Image = Image.FromFile(Path.Combine(FileStoreBaseDir, x.Path))
                            })
                    .ToDictionary(x => x.Size, x => x.Image);
                return _watermarkSet;
            }
        }
    }
}

using System.Drawing;
using System.Linq;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件存储规则管理器
    /// </summary>
    public abstract class FileStoreRuleManager : IFileStoreRuleManager
    {
        protected ConfigManager ConfigManager;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configManager"></param>
        protected FileStoreRuleManager(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        /// <summary>
        /// 根据规则名获取文件保存规则
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract FileStoreRule GetRuleByName(string name);


        /// <summary>
        /// 获取水印图片
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public Image GetWatermarkImage(Size size)
        {
            var match = ConfigManager.WatermarkSet.Where(x => x.Key.Height < size.Height && x.Key.Width < size.Width)
                .OrderByDescending(x => x.Key.Width)
                .FirstOrDefault();
            return match.Value ?? ConfigManager.WatermarkSet[new Size(0, 0)];
        }
    }
}
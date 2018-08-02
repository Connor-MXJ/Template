using System.Drawing;
using System.Linq;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// �ļ��洢���������
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
        /// ���ݹ�������ȡ�ļ��������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract FileStoreRule GetRuleByName(string name);


        /// <summary>
        /// ��ȡˮӡͼƬ
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
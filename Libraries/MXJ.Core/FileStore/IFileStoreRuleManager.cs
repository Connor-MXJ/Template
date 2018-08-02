using System.Drawing;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件存储规则管理器接口
    /// </summary>
    public interface IFileStoreRuleManager
    {
        /// <summary>
        /// 根据规则名获取文件保存规则
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        FileStoreRule GetRuleByName(string name);

        /// <summary>
        /// 获取水印图片
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Image GetWatermarkImage(Size size);
    }
}
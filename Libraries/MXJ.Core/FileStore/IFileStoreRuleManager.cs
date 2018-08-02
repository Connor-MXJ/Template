using System.Drawing;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// �ļ��洢����������ӿ�
    /// </summary>
    public interface IFileStoreRuleManager
    {
        /// <summary>
        /// ���ݹ�������ȡ�ļ��������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        FileStoreRule GetRuleByName(string name);

        /// <summary>
        /// ��ȡˮӡͼƬ
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Image GetWatermarkImage(Size size);
    }
}
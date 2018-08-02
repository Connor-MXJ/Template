using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// �ļ��洢
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="fileInfo">�ļ���Ϣ</param>
        /// <param name="stream">������ļ���</param>
        /// <param name="extraArgs">����Ĳ����ֵ䣬���ڵ�ʵ�ֱ��봫context����</param>
        /// <returns></returns>
        Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs);

        /// <summary>
        /// �ֶα����ļ���δʵ��
        /// </summary>
        /// <param name="sameFileIndentity"></param>
        /// <param name="fileInfo"></param>
        /// <param name="stream"></param>
        /// <param name="index"></param>
        /// <param name="total"></param>
        /// <param name="extraArgs"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        Task SavePartAsync(string sameFileIndentity, FileMetaInfo fileInfo, Stream stream, int index, int total, IDictionary<string, object> extraArgs, Action<FileMetaInfo> completeCallback = null);


        /// <summary>
        /// �Ƿ����ָ���ļ���δʵ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(string id);

        /// <summary>
        /// ��ȡָ���ļ���δʵ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Tuple<FileMetaInfo, Stream>> GetAsync(string id);

        Task<Tuple<FileMetaInfo, Stream>> GetOriginalAsync(string id);
    }
}

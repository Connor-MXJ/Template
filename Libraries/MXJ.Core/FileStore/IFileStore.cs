using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件存储
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="stream">保存的文件流</param>
        /// <param name="extraArgs">额外的参数字典，现在的实现必须传context参数</param>
        /// <returns></returns>
        Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs);

        /// <summary>
        /// 分段保存文件，未实现
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
        /// 是否存在指定文件，未实现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(string id);

        /// <summary>
        /// 获取指定文件，未实现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Tuple<FileMetaInfo, Stream>> GetAsync(string id);

        Task<Tuple<FileMetaInfo, Stream>> GetOriginalAsync(string id);
    }
}

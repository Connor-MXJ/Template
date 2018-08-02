using System;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 元数据保存接口
    /// </summary>
    public interface IFileMetaInfoStore
    {
        Task SaveAsync(FileMetaInfo info,string ruleId);
        Task<FileMetaInfo> GetByIdAsync(Guid fileId);
    }
}
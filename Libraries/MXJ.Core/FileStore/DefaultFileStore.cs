using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 默认文件存储,自动区分普通文件和图片
    /// </summary>
    public class DefaultFileStore:IFileStore
    {
        private static CommonFileStore _commonFileStore;
        private static ImageFileStore _imageFileStore;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configManager"></param>
        /// <param name="fileStoreRuleManager"></param>
        /// <param name="metaInfoStore"></param>
        public DefaultFileStore(ConfigManager configManager, IFileStoreRuleManager fileStoreRuleManager, IFileMetaInfoStore metaInfoStore)
        {
            _commonFileStore=new CommonFileStore(configManager,fileStoreRuleManager,metaInfoStore);
            _imageFileStore=new ImageFileStore(configManager,fileStoreRuleManager,metaInfoStore);
        }

        bool IsImageFile(FileMetaInfo info)
        {
            return info.ContentType.ToLower().StartsWith("image");
        }
        public async Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs)
        {
            return await (IsImageFile(fileInfo)
                ? _imageFileStore.SaveAsync(fileInfo, stream, extraArgs)
                : _commonFileStore.SaveAsync(fileInfo, stream, extraArgs));
        }

        public Task SavePartAsync(string sameFileIndentity, FileMetaInfo fileInfo, Stream stream, int index, int total,
            IDictionary<string, object> extraArgs, Action<FileMetaInfo> completeCallback = null)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<FileMetaInfo, Stream>> GetAsync(string id)
        {
            return _commonFileStore.GetAsync(id);
        }

        public Task<Tuple<FileMetaInfo, Stream>> GetOriginalAsync(string id)
        {
            return _commonFileStore.GetOriginalAsync(id);
        }
    }
}

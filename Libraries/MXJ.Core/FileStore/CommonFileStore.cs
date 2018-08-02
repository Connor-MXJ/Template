using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks; 

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 通用文件存储
    /// </summary>
    public class CommonFileStore : FileSystemFileStore
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configManager"></param>
        /// <param name="fileStoreRuleManager"></param>
        /// <param name="metaInfoStore"></param>
        public CommonFileStore(ConfigManager configManager, IFileStoreRuleManager fileStoreRuleManager, IFileMetaInfoStore metaInfoStore)
            : base(configManager, fileStoreRuleManager, metaInfoStore)
        {
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="stream"></param>
        /// <param name="extraArgs"></param>
        /// <returns></returns>
        public override async Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs)
        {
            if (extraArgs == null || !extraArgs.ContainsKey("context"))
                throw new ArgumentException("extraArgs 参数需要context键值");

            var context = extraArgs["context"].ToString();
            var rule = FileStoreRuleManager.GetRuleByName(context) as ImageStoreRule;

            if (rule == null) throw new Exception("未找到指定规则或默认规则:"+context);

            fileInfo.Id = Guid.NewGuid().ToString();
            var name = GetInnerFileName(fileInfo);
            var dir = rule.GetSaveDir();
            await SaveFileAsync(dir, name, stream);

            fileInfo.Path = Path.Combine(rule.GetSaveDir(), name).Replace("\\", "/");

            //保存文件信息
            await MetaInfoStore.SaveAsync(fileInfo,rule.Id);

            return fileInfo;
        }

        public override Task SavePartAsync(string sameFileIndentity, FileMetaInfo fileInfo, Stream stream, int index, int total, IDictionary<string, object> extraArgs, Action<FileMetaInfo> completeCallback = null)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string name)
        {
            throw new NotImplementedException();
        }

    }
}

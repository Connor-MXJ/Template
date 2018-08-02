using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks; 

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// ͨ���ļ��洢
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
        /// �����ļ�
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="stream"></param>
        /// <param name="extraArgs"></param>
        /// <returns></returns>
        public override async Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs)
        {
            if (extraArgs == null || !extraArgs.ContainsKey("context"))
                throw new ArgumentException("extraArgs ������Ҫcontext��ֵ");

            var context = extraArgs["context"].ToString();
            var rule = FileStoreRuleManager.GetRuleByName(context) as ImageStoreRule;

            if (rule == null) throw new Exception("δ�ҵ�ָ�������Ĭ�Ϲ���:"+context);

            fileInfo.Id = Guid.NewGuid().ToString();
            var name = GetInnerFileName(fileInfo);
            var dir = rule.GetSaveDir();
            await SaveFileAsync(dir, name, stream);

            fileInfo.Path = Path.Combine(rule.GetSaveDir(), name).Replace("\\", "/");

            //�����ļ���Ϣ
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

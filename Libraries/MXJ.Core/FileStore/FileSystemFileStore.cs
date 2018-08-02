using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 文件系统存储
    /// </summary>
    public abstract class FileSystemFileStore : IFileStore
    {
        /// <summary>
        /// 文件存储路径
        /// </summary>
        protected string BaseStoreDirectory;
        /// <summary>
        /// 文件存储规则
        /// </summary>
        protected IFileStoreRuleManager FileStoreRuleManager;
        /// <summary>
        /// 元数据保存
        /// </summary>
        protected IFileMetaInfoStore MetaInfoStore;

        /// <summary>
        ///
        /// </summary>
        protected string OriginalImageBaseDir;


        /// <summary>
        ///
        /// </summary>
        /// <param name="configManager"></param>
        /// <param name="fileStoreRuleManager"></param>
        /// <param name="metaInfoStore"></param>
        protected FileSystemFileStore(ConfigManager configManager, IFileStoreRuleManager fileStoreRuleManager, IFileMetaInfoStore metaInfoStore)
        {
            BaseStoreDirectory = configManager.FileStoreBaseDir;
            FileStoreRuleManager = fileStoreRuleManager;
            MetaInfoStore = metaInfoStore;
            OriginalImageBaseDir = ConfigurationManager.AppSettings["OriginalImageBaseDir"];
        }

        /// <summary>
        /// 获取内部保存用文件名
        /// </summary>
        /// <param name="info"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected string GetInnerFileName(FileMetaInfo info, string suffix = null)
        {
            var suffixReal = "";
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                suffixReal = "_" + suffix;
            }
            return string.Format("{0}{2}.{1}", info.Id, info.Extension, suffixReal);
        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="name"></param>
        /// <param name="fileStream"></param>
        /// <param name="isOrg"></param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">OriginalImageBaseDir 配置错误</exception>
        protected async Task<string> SaveFileAsync(string dir, string name, Stream fileStream,bool isOrg=false)
        {
            if (isOrg)
            {
                if (string.IsNullOrWhiteSpace(OriginalImageBaseDir)) throw new ConfigurationErrorsException("OriginalImageBaseDir 配置错误");
                dir = Path.Combine(OriginalImageBaseDir, dir);
            }
            else
            {
                dir = Path.Combine(BaseStoreDirectory, dir);
            }

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, name);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                await fileStream.CopyToAsync(fs);
            }
            return Path.Combine(dir, name);
        }

        public abstract Task<FileMetaInfo> SaveAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs);

        public abstract Task SavePartAsync(string sameFileIndentity, FileMetaInfo fileInfo, Stream stream, int index, int total, IDictionary<string, object> extraArgs, Action<FileMetaInfo> completeCallback = null);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool Exists(string name);

        /// <summary>
        /// 根据 Id 获取文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<FileMetaInfo, Stream>> GetAsync(string id)
        {
            var fileId = Guid.Parse(id);
            var fileMeta= await MetaInfoStore.GetByIdAsync(fileId);
            var filePath = Path.Combine(BaseStoreDirectory, fileMeta.Path);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件未找到",filePath);
            }
            var buf = new byte[fileMeta.Size];
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Tuple.Create(fileMeta, (Stream)fs);
        }

        public virtual async Task<Tuple<FileMetaInfo, Stream>> GetOriginalAsync(string id)
        {
            var fileId = Guid.Parse(id);
            var fileMeta = await MetaInfoStore.GetByIdAsync(fileId);
            var p= new Regex("\\." + fileMeta.Extension + "$").Replace(fileMeta.Path, "_OriginalImage." + fileMeta.Extension);
            var filePath = Path.Combine(OriginalImageBaseDir, p);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件未找到", filePath);
            }
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Tuple.Create(fileMeta, (Stream) fs);
        }
    }
}

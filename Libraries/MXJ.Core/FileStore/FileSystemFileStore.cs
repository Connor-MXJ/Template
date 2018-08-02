using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// �ļ�ϵͳ�洢
    /// </summary>
    public abstract class FileSystemFileStore : IFileStore
    {
        /// <summary>
        /// �ļ��洢·��
        /// </summary>
        protected string BaseStoreDirectory;
        /// <summary>
        /// �ļ��洢����
        /// </summary>
        protected IFileStoreRuleManager FileStoreRuleManager;
        /// <summary>
        /// Ԫ���ݱ���
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
        /// ��ȡ�ڲ��������ļ���
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
        /// �����ļ�
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="name"></param>
        /// <param name="fileStream"></param>
        /// <param name="isOrg"></param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">OriginalImageBaseDir ���ô���</exception>
        protected async Task<string> SaveFileAsync(string dir, string name, Stream fileStream,bool isOrg=false)
        {
            if (isOrg)
            {
                if (string.IsNullOrWhiteSpace(OriginalImageBaseDir)) throw new ConfigurationErrorsException("OriginalImageBaseDir ���ô���");
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
        /// �Ƿ����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool Exists(string name);

        /// <summary>
        /// ���� Id ��ȡ�ļ�
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
                throw new FileNotFoundException("�ļ�δ�ҵ�",filePath);
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
                throw new FileNotFoundException("�ļ�δ�ҵ�", filePath);
            }
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Tuple.Create(fileMeta, (Stream) fs);
        }
    }
}

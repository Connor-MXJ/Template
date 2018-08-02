using ImageProcessor;
using ImageProcessor.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MXJ.Core.FileStore
{
    /// <summary>
    /// 图片存储
    /// </summary>
    public class ImageFileStore : FileSystemFileStore
    {

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configManager"></param>
        /// <param name="fileStoreRuleManager"></param>
        /// <param name="metaInfoStore"></param>
        public ImageFileStore(ConfigManager configManager, IFileStoreRuleManager fileStoreRuleManager, IFileMetaInfoStore metaInfoStore)
            : base(configManager, fileStoreRuleManager, metaInfoStore)
        {
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private Stream GenThumbnail(Stream imageStream, ThumbnailConfig config)
        {
            Stream stream = new MemoryStream();
            using (var imageFactory=new ImageFactory())
            {
                var resizeLayer = new ResizeLayer(config.Size, resizeMode: config.ResizeMode);
                var imageF = imageFactory.Load(imageStream);
                imageF.AutoRotate();
                var image = imageF.Image;
                if (image.Height > config.Size.Height && image.Width > config.Size.Width)
                {
                    imageF.Resize(resizeLayer);
                    imageF.Save(stream);
                }
                else
                {
                    imageStream.Position = 0;
                    stream = imageStream;
                }

            }
            return stream;
        }


        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="imageStream"></param>
        /// <returns></returns>
        private async Task<Stream> AddWaterMarkAsync(Stream imageStream)
        {
            imageStream.Position = 0;
            var stream=new MemoryStream();
            using (var imageF = new ImageFactory())
            {
                imageF.Load(imageStream);
                var image = imageF.Image;
                var watermarkImage = FileStoreRuleManager.GetWatermarkImage(image.Size);

                var imgl = new ImageLayer();
                imgl.Image = watermarkImage;
                imageF.AutoRotate();
                imageF.Overlay(imgl);

                imageF.Save(stream);
                return stream;
            }
            using (var image = Image.FromStream(imageStream.Duplicate()))
            {
                var watermarkImage = FileStoreRuleManager.GetWatermarkImage(image.Size);
                return image.AddWatermark((Image)watermarkImage.Clone(), 5, 100, 10);
            }

        }


        /// <summary>
        /// 保存
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

            Guid fileId ;
            bool modify = false;
;
            if (extraArgs.ContainsKey("fileid") && Guid.TryParse(extraArgs["fileid"].ToString(), out fileId))
            {
                int rotate;
                if (extraArgs.ContainsKey("rotate") && int.TryParse(extraArgs["rotate"].ToString(), out rotate))
                {
                    modify = true;
                    var fi = await GetOriginalAsync(fileId.ToString());
                    fileInfo = fi.Item1;
                    var fileStream = fi.Item2;

                    var imageFactory = new ImageFactory();
                    var ms = new MemoryStream();
                    imageFactory.Load(fileStream).AutoRotate().Rotate(rotate).Save(ms);
                    ms.Position = 0;
                    stream = ms;
                    fileStream.Dispose();
                }


            }
            else if(!fileInfo.Extension.ToLower().EndsWith("gif"))
            {


                var ms = new MemoryStream();
                var imageF = new ImageFactory();
                imageF.Load(stream).AutoRotate().Save(ms);
                ms.Position = 0;
                stream.Position = 0;
                stream = ms;
            }
            ;

            if (string.IsNullOrWhiteSpace(fileInfo.Id) || fileInfo.Id==default(Guid).ToString())
            {
                fileInfo.Id = Guid.NewGuid().ToString();
            }
            await SaveInternalAsync(fileInfo, stream, extraArgs, rule,modify);

            return fileInfo;
        }

        private async Task SaveInternalAsync(FileMetaInfo fileInfo, Stream stream, IDictionary<string, object> extraArgs, ImageStoreRule rule,bool modify=false)
        {
            if (!fileInfo.ContentType.ToLower().StartsWith("image")) throw new NotSupportedException("传入文件类型不是有效的图片类型");

            var needSaveFiles = new Dictionary<string, Stream>();

            var name = GetInnerFileName(fileInfo);


            var savedir = modify ? fileInfo.DirPath : rule.GetSaveDir();
            //保存原始图片
            if (rule.SaveOriginalImage)
            {
                var dir = savedir;
                var svname = rule.AddWaterMarker ? GetInnerFileName(fileInfo, "OriginalImage") : name;
                var fileStream = stream;
                await SaveFileAsync(dir, svname, fileStream, rule.AddWaterMarker);
                if (!rule.ThumbnailSizes.Any())
                {
                    stream.Position = 0;
                    needSaveFiles.Add(name, stream);
                }
            }

            //生成缩略图
            if (rule.ThumbnailSizes.Any())
            {
                foreach (var thumbConfig in rule.ThumbnailSizes.OrderBy(x => x.Value.Size.Height*x.Value.Size.Width))
                {
                    var key = thumbConfig.Key;
                    var config = thumbConfig.Value;
                    var thumbStream = GenThumbnail(stream, config);
                    var thumbName = GetInnerFileName(fileInfo, key);
                    needSaveFiles.Add(thumbName, thumbStream);
                    stream.Position = 0;
                }

                //org comp
                if (rule.SaveOriginalImage && rule.AddWaterMarker && needSaveFiles.Any())
                {
                    var orgInstead = needSaveFiles.Last();
                    var dir = savedir;
                    var stm = await AddWaterMarkAsync(orgInstead.Value);
                    await SaveFileAsync(dir, name, stm);
                }
            }

            //添加水印
            if (rule.AddWaterMarker)
            {
                foreach (var images in needSaveFiles.ToDictionary(x => x.Key, x => x.Value))
                {
                    needSaveFiles[images.Key] = await AddWaterMarkAsync(images.Value);
                }
            }

            //持久化
            foreach (var images in needSaveFiles)
            {
                var dir = savedir;
                var svname = images.Key;
                var fileStream = images.Value;
                await SaveFileAsync(dir, svname, fileStream);
                if (fileStream != stream)
                {
                    fileStream.Dispose();
                }
            }
            if (extraArgs.ContainsKey("suffix"))
            {
                fileInfo.Path =
                    Path.Combine(savedir, GetInnerFileName(fileInfo, extraArgs["suffix"].ToString()))
                        .Replace("\\", "/");
            }
            else if (!rule.SaveOriginalImage && rule.ThumbnailSizes.ContainsKey("org"))
            {
                fileInfo.Path = Path.Combine(savedir, GetInnerFileName(fileInfo, "org")).Replace("\\", "/");
            }
            else
            {
                fileInfo.Path = Path.Combine(savedir, name).Replace("\\", "/");
            }


            //保存文件信息
            await MetaInfoStore.SaveAsync(fileInfo, rule.Id);
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

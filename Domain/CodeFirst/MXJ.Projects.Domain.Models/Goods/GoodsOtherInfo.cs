using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     商品价格
    /// </summary>
    public class GoodsPrice
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid GoodsPriceId { set; get; }

        /// <summary>
        ///     起订数量(开始)
        /// </summary>
        public int StartNumber { set; get; }

        /// <summary>
        ///     起订数量(结束)
        /// </summary>
        public int EndNumber { set; get; }

        /// <summary>
        ///     价格
        /// </summary>
        public decimal Price { set; get; }
    }

    /// <summary>
    /// 配送区域及运费
    /// </summary>
    public class DeliveryInfo
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid DeliveryInfoId { set; get; }

        /// <summary>
        /// 配送区域省编码
        /// </summary>
        public int ProvinceCode { set; get; }

        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { set; get; }

        /// <summary>
        /// 配送区域省简称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 首重(kg)
        /// </summary>
        public float FirstWeight { get; set; }

        /// <summary>
        /// 首费(元)
        /// </summary>
        public decimal FirstFee{ get; set; }

        /// <summary>
        /// 续重(kg)
        /// </summary>
        public float ContinueWeight { get; set; }

        /// <summary>
        /// 续费(元)
        /// </summary>
        public decimal ContinueFee { get; set; }
    }

    /// <summary>
    /// 商品图片
    /// </summary>
    public class GoodsPicture
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid GoodsPictureId { set; get; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl { set; get; }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string SmallFilePath { set; get; }

        /// <summary>
        /// 原图地址
        /// </summary>
        public string FilePath { set; get; }

        /// <summary>	
        /// 是否主图		
        /// </summary>			
        public bool IsPrimary { get; set; }
    }
}

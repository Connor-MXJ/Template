using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     商品
    /// </summary>
    public class Goods : BaseEntity<Guid>
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid GoodsId { set; get; }

        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { set; get; }

        /// <summary>
        ///  销售数量
        /// </summary>
        public int AlreadySalesCount { set; get; }
         
        /// <summary>
        ///  页面浏览量
        /// </summary>
        public int ScaningCount { set; get; }

        /// <summary>
        /// 随机生成一段数字，唯一
        /// </summary>
        public long GoodsNumber { set; get; }

        /// <summary>
        /// 最小价格，冗余字段，做查询使用
        /// </summary>
        public decimal MinPrice { set; get; }

        /// <summary>
        ///     SKU编号
        /// </summary>
        public string Sku { set; get; }

        /// <summary>
        /// 系统存储SKU编码(唯一)
        /// </summary>
        public string SystemSku { get; set; }

        /// <summary>
        /// sku规格（由所选销售属性值拼接）
        /// </summary>
        public string SkuStandard { get; set; }

        /// <summary>
        ///     详情页地址
        /// </summary>
        public string LinkStr { set; get; }

        /// <summary>
        ///     价格区间
        /// </summary>
        public List<GoodsPrice> Prices { set; get; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public ProductProperty ProductProperty { set; get; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public virtual List<GoodsAttributeValue> GoodsAttributeValues { set; get; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public virtual List<GoodsPicture> Pictures { set; get; }
    }
}

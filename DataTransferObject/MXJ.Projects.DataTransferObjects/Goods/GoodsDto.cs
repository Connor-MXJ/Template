using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;
using MXJ.Core.Utility.Extensions;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    ///     商品
    /// </summary>
    public class GoodsDto : BaseEntity<Guid>
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
        /// 最小价格，冗余字段，做查询使用
        /// </summary>
        public decimal MinPrice { set; get; }

        /// <summary>
        ///    最小价格，冗余字段，做查询使用
        /// </summary>
        public string DisplayMinPrice
        {
            get { return MinPrice.ToDisplayMoney(); }
        }

        /// <summary>
        ///     SKU编号
        /// </summary>
        public string Sku { set; get; }

        /// <summary>
        /// 系统存储SKU编码(唯一)
        /// </summary>
        public string SystemSku { get; set; }

        /// <summary>
        /// 随机生成一段数字，唯一
        /// </summary>
        public int GoodsNumber { set; get; }

        /// <summary>
        ///  销售数量
        /// </summary>
        public int AlreadySalesCount { set; get; }

        /// <summary>
        ///  页面浏览量
        /// </summary>
        public int ScaningCount { set; get; }

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
        public List<GoodsPriceDto> Prices { set; get; }

        /// <summary>
        /// 同product其他产品
        /// </summary>
        public List<GoodsDto> Brother { set; get; }

        /// <summary>
        /// 同分类下的其他产品
        /// </summary>
        public List<GoodsDto> CousinBrother { set; get; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public ProductPropertyDto ProductProperty { set; get; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public virtual List<GoodsPictureDto> Pictures { set; get; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public virtual List<GoodsAttributeValueDto> GoodsAttributeValues { set; get; } 
    }

    /// <summary>
    ///   实例化前选中的属性及属性值 
    /// </summary>
    public class GoodsAttributeValueDto
    {
        /// <summary>
        ///   主键 
        /// </summary>
        public Guid GoodsAttributeValueId { set; get; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid AttributeId { set; get; }

        /// <summary>
        /// 所选属性值Id
        /// </summary>
        public Guid AttributeValueId { set; get; }

        /// <summary>
        /// 属性编号
        /// </summary>
        public int AttributeNumber { set; get; }

        /// <summary>
        /// 属性值
        /// </summary>
        public int AttributeValueNumber { set; get; }
    }


    public class GoodsSearchDto : SearchRequestBase
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid? ProductId { set; get; }

        /// <summary>
        /// 商品品Id
        /// </summary>
        public Guid? GoodsId { set; get; }

        /// <summary>
        /// 随机生成一段数字，唯一
        /// </summary>
        public int? GoodsNumber { set; get; } 

        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid? ProductCategoryId { set; get; }

        /// <summary>
        /// 是否加载伙伴商品信息 
        /// </summary>
        public bool IsLoadBotherGoods { set; get; }

        /// <summary>
        /// 是否加载同类其他商品信息 
        /// </summary>
        public bool IsLoadCousinBrotherGoods { set; get; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public List<ProductStatusCode> Status { get; set; }

        /// <summary>
        /// 商品名称、商品型号
        /// 商品SKU、商品供应商
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 最低价格
        /// </summary>
        public int? PirceMin { get; set; }

        /// <summary>
        /// 最高价格
        /// </summary>
        public int? PirceMax { get; set; }

        /// <summary>
        /// 属性值过滤
        /// </summary>
        public string AttribteFilter { get; set; }
         
        /// <summary>
        /// 可配送区域
        /// </summary>
        public string DelieveryArea { get; set; }

        /// <summary>
        /// 排序方式(0：综合，1：价格降序，-1：价格升序，2：销量降序，-2：销量升序)
        /// </summary>
        public int? OrderCode { get; set; }
    }
}

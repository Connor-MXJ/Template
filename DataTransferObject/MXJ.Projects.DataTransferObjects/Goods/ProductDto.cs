using System;
using System.Collections.Generic; 
using MXJ.Projects.Domain.Enums;
using MXJ.Core.Domain.Models;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    ///     产品
    /// </summary>
    public class ProductDto : BaseEntity<Guid>
    { 
        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { set; get; }
        public bool IsEdit { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public ProductPropertyDto ProductProperty { set; get; }

        /// <summary>
        ///     价格区间
        /// </summary>
        public List<GoodsPriceDto> Prices { set; get; }

        /// <summary>
        /// 未上架商品的临时存储
        /// </summary>
        public List<TempGoodsDto> TempGoods { set; get; }

        /// <summary>
        /// 前台展示的临时Dto
        /// </summary>
        public List<TempGoodsDto> DisplayTempGoods { get; set; }

        /// <summary>
        /// 存放选定的属性及属性值
        /// </summary>
        public List<ProductSelectedAttibutesDto> ProductSelectedAttibutes { set; get; }

        /// <summary>
        /// 通用属性集合
        /// </summary>
        public List<ProductAttributeDto> GeneralAttributes { get; set; }

        /// <summary>
        /// 销售属性集合
        /// </summary>
        public List<ProductAttributeDto> SalesAttributes { get; set; }

        /// <summary>
        /// 配送区域及运费
        /// </summary>
        public List<DeliveryInfoDto> DeliveryInfo { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SubmitedTime { get; set; }

        /// <summary>
        /// 提交用户
        /// </summary>
        public Guid? SubmitedUser { get; set; }
    }

    /// <summary>
    /// 存放选定的属性及属性值
    /// </summary>
    public class ProductSelectedAttibutesDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ProductToAttibutesId { set; get; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid ProductId { set; get; }

        /// <summary>
        /// 产品
        /// </summary>
        public ProductDto Product { set; get; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid ProductAttributeId { set; get; }

        /// <summary>
        /// 属性name
        /// </summary>
        public string ProductAttibuteName { set; get; }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public Guid ProductAttributeValueId { set; get; }

        /// <summary>
        /// 属性值name
        /// </summary>
        public string ProductAttributeValueName { set; get; }
    }

    /// <summary>
    /// 存放选定的属性及属性值
    /// </summary>
    public class SelectedAttibutesDto
    {
        /// <summary>
        /// TempGoodsId
        /// </summary>
        public Guid? TempGoodsId { set; get; }

        /// <summary>
        /// TempGoodsId
        /// </summary>
        public Guid? GoodsId { set; get; }
        
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid ProductAttributeId { set; get; }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public Guid ProductAttributeValueId { set; get; }
    }

    /// <summary>
    /// 临时存储
    /// </summary>
    public class TempGoodsDto
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid TempGoodsId { set; get; }

        /// <summary>
        ///     产品
        /// </summary>
        public ProductDto Product { set; get; }

        /// <summary>
        ///     产品
        /// </summary>
        public Guid ProductId { set; get; }

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
        /// 存储前台选中的销售属性的选项及值.存储格式(属性1.Id:值1.Id|属性2.Id:值2.Id)
        /// </summary>
        public string SelectedAttibutesAndValues { set; get; }

        /// <summary>
        /// 存放选定的属性及属性值（销售属性）
        /// </summary>
        public List<SelectedAttibutesDto> TempGoodsSelectedSalesAttibutes { set; get; }

        /// <summary>
        ///     价格区间
        /// </summary>
        public List<GoodsPriceDto> Prices { set; get; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public virtual List<GoodsPictureDto> Pictures { set; get; }
    }

    public class ProductSearchDto : SearchRequestBase
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid? ProductId { set; get; }
        /// <summary>
        /// 是否加载产品选择属性
        /// </summary>
        public bool IsLoadSelectedAttibutes { get; set; }

        /// <summary>
        /// 是否加TempGoods信息
        /// </summary>
        public bool IsLoadTempGoods { set; get; }

        /// <summary>
        /// 是否加载图片信息
        /// </summary>
        public bool IsLoadPictures { set; get; }

        /// <summary>
        /// 是否加载价格信息
        /// </summary>
        public bool IsLoadPirces { set; get; }

        /// <summary>
        ///  是否加载配送信息
        /// </summary>
        public bool IsLoadDelivery { set; get; }

        /// <summary>
        ///     产品名称
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        ///     产品编号
        /// </summary>
        public string ProductNumber { set; get; }

        /// <summary>
        ///     供应商编号
        /// </summary>
        public string SupplierNumber { set; get; }

        /// <summary>
        ///     供应商名称
        /// </summary>
        public string SupplierName { set; get; }




        /// <summary>
        /// 产品状态
        /// </summary>
        public List<ProductStatusCode> Status { get; set; }

        /// <summary>
        /// 供应商、商品名称、商品型号
        /// </summary>
        public string Filter { get; set; }
    }
}
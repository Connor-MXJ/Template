using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MXJ.Core.Domain.Models;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     产品
    /// </summary>
    public class Product : BaseEntity<Guid>
    {
        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { set; get; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public ProductProperty ProductProperty { set; get; }

        /// <summary>
        ///     默认价格区间
        /// </summary>
        public List<GoodsPrice> Prices { set; get; }

        /// <summary>
        /// 未上架商品的临时存储
        /// </summary>
        public List<TempGoods> TempGoods { set; get; }

        /// <summary>
        /// 存放选定的属性及属性值
        /// </summary>
        public List<ProductSelectedAttibutes> ProductSelectedAttibutes { set; get; }

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
    public class ProductSelectedAttibutes
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
        public Product Product { set; get; }

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
    public class TempGoods
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid TempGoodsId { set; get; }

        /// <summary>
        ///     产品
        /// </summary>
        public Product Product { set; get; }

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
        /// 商品属性
        /// </summary>
        public virtual List<GoodsAttributeValue> TempGoodsAttributeValues { set; get; }

        /// <summary>
        ///     价格区间
        /// </summary>
        public List<GoodsPrice> Prices { set; get; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public virtual List<GoodsPicture> Pictures { set; get; }
    }

    /// <summary>
    ///   实例化前选中的属性及属性值 
    /// </summary>
    public class GoodsAttributeValue
    {
        /// <summary>
        ///   主键 
        /// </summary>
        public Guid GoodsAttributeValueId { set; get; }
        
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid ProductAttributeId { set; get; }

        /// <summary>
        /// 所选属性值Id
        /// </summary>
        public Guid AttributeValueId { set; get; }

        /// <summary>
        /// 属性编号
        /// </summary>
        public int ProductAttributeNumber { set; get; }

        /// <summary>
        /// 属性值
        /// </summary>
        public int AttributeValueNumber { set; get; } 
    }
}
using MXJ.Projects.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    /// 商品属性类
    /// </summary>
    public class ProductAttributeDto : BaseEntityDto<Guid>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid AttributeId { set; get; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { set; get; }

        /// <summary>
        /// 是否允许过滤
        /// </summary>
        public bool IsAllowFilter { set; get; }

        /// <summary>
        /// 是否为销售属性(SKU)
        /// </summary>
        public bool IsSaleAttribute { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int AttributeNumber { get; set; }

        /// <summary>
        /// 产品种类Id
        /// </summary>
        public Guid ProductCategoryId { set; get; }

        /// <summary>
        /// 产品种类
        /// </summary>
        public ProductCategoryDto ProductCategory { set; get; }

        /// <summary>
        /// 商品属性的属性值集合
        /// </summary>
        public List<AttributeValuesDto> AttributeValues { set; get; }

        /// <summary>
        /// 所选择的属性值
        /// </summary>
        public Guid SelectedAttributeValue { get; set; }

        public string DisplayAttValues
        {
            get
            {
                string val = "";
                if (AttributeValues != null && AttributeValues.Count > 0)
                {
                    foreach (var item in AttributeValues)
                    {
                        val += item.AttributeValue + " ,";
                    }
                    return val;
                }
                else return "";
            }
        }

    }

    /// <summary>
    /// 商品属性查询类
    /// </summary>
    public class ProductAttributeSearchDto : SearchRequestBase
    {
        /// <summary>
        /// 类目ID
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid? AttributeID { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeName { get; set; }
    }

    /// <summary>
    /// 系统管理的属性值
    /// </summary>
    public class AttributeValuesDto : BaseEntityDto<Guid>
    {
        /// <summary>
        /// 属性值主键
        /// </summary>
        public Guid AttributeValueId { set; get; }

        /// <summary>
        /// 属性值名称
        /// </summary>
        public string AttributeValue { set; get; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid ProductAttributeId { set; get; }

        /// <summary>
        /// 属性
        /// </summary>
        public ProductAttributeDto ProductAttribute { set; get; }

        /// <summary>
        /// 品牌logo图片地址
        /// </summary>
        public string LogoUrl { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int AttributeValueNumber { get; set; }
        
        /// <summary>
        /// 是否选中（前台筛选页面使用）
        /// </summary>
        public bool IsSelected { get; set; }
    }
}

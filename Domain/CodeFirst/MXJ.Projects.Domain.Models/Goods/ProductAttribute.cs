using MXJ.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    /// 产品分类的通用及销售属性
    /// </summary>
    public class ProductAttribute : BaseEntity<Guid>
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
        /// 属性具有的属性值
        /// </summary>
        public virtual List<AttributeValues> Values { set; get; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public virtual List<ProductCategory> ProductCategories { set; get; }
    }
    
    /// <summary>
    /// 系统管理的属性值
    /// </summary>
    public class AttributeValues : BaseEntity<Guid>
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
        public ProductAttribute ProductAttribute { set; get; }
        
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid ProductAttributeId { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int AttributeValueNumber { get; set; }

    }
}

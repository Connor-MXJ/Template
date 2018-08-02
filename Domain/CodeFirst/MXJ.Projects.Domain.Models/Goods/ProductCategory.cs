using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    /// 产品种类
    /// </summary>
    public class ProductCategory : BaseEntity<Guid>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid CategoryId { set; get; }

        /// <summary>
        /// 种类名称
        /// </summary>
        public string CategoryName { set; get; }
        
        /// <summary>
        /// 是否自营，Ture即为本类别是属于本公司自家销售
        /// </summary>
        public bool IsOurSale { set; get; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int CategoryNumber { get; set; }

        /// <summary>
        /// 产品分类路径,用id_表示
        /// </summary>
        public string ProductPath { set; get; }

        /// <summary>
        /// 父类主键
        /// </summary>
        public Guid? ParentCategoryId { set; get; }

        /// <summary>
        /// 种类所在树的级别
        /// </summary>
        public int CategoryLevel { set; get; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        public virtual List<ProductAttribute> ProductAttributes { set; get; }
    }

}

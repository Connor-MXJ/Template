using System;
using System.Collections.Generic;

namespace MXJ.Projects.DataTransferObjects
{
    /// <summary>
    ///     产品种类
    /// </summary>
    public class ProductCategoryDto : TreeBaseDto
    {
        /// <summary>
        ///     主键
        /// </summary>
        public Guid CategoryId { set; get; }

        /// <summary>
        ///     种类名称
        /// </summary>
        public string CategoryName { set; get; }

        /// <summary>
        ///     是否自营，Ture即为本类别是属于本公司自家销售
        /// </summary>
        public bool IsOurSale { set; get; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        /// 产品分类路径,用id_表示
        /// </summary>
        public string ProductPath { set; get; }

        /// <summary>
        ///     父类主键
        /// </summary>
        public Guid? ParentCategoryId { set; get; }

        /// <summary>
        ///     子类
        /// </summary>
        public List<ProductCategoryDto> ProductCategoryChildren { set; get; }

        /// <summary>
        ///     种类所在树的级别
        /// </summary>
        public int CategoryLevel { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int CategoryNumber { get; set; }

        /// <summary>
        /// 属性及属性值集合
        /// </summary>
        public List<ProductAttributeDto> ProductAttributes { get; set; }

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
    }

    /// <summary>
    ///     产品种类查询
    /// </summary>
    public class ProductCategorySearchDto
    {
        /// <summary>
        ///     主键查询。若有值，代表单个查询
        /// </summary>
        public Guid? CategoryId { set; get; }

        /// <summary>
        ///     种类名称
        /// </summary>
        public string CategoryName { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public int? CategoryNumber { set; get; }

        /// <summary>
        ///     父类主键
        /// </summary>
        public Guid? ParentCategoryId { set; get; }

        /// <summary>
        ///     种类所在树的级别
        /// </summary>
        public int? CategoryLevel { set; get; }
    }
}
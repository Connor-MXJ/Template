using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.DataTransferObjects
{
    public class ProductPropertyDto
    {
        /// <summary>
        ///     产品名称
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        ///     产品编号
        /// </summary>
        public string ProductNumber { set; get; }

        /// <summary>
        ///     产品分类
        /// </summary>
        public Guid ProductCategoryId { set; get; }


        /// <summary>
        ///     产品分类名称
        /// </summary>
        public string ProductCategoryName { set; get; }

        /// <summary>
        ///     供应商
        /// </summary>
        public Guid SupplierId { set; get; }

        /// <summary>
        ///     供应商编号
        /// </summary>
        public string SupplierNumber { set; get; }

        /// <summary>
        ///     供应商名称
        /// </summary>
        public string SupplierName { set; get; }

        /// <summary>
        ///     产品型号
        /// </summary>
        public string ProductModelType { set; get; }

        /// <summary>
        ///     操作状态
        /// </summary>
        public ProductStatusCode ProductStatus { set; get; }

        public string DisplayStatus
        {
            get
            {
                if (ProductStatus == ProductStatusCode.待提交)
                    return "店小二审核中";
                else if (ProductStatus == ProductStatusCode.待审核)
                    return "运营审核中";
                else
                    return ProductStatus.ToString();
            }
        }

        /// <summary>
        ///     SEO搜索关键字
        /// </summary>
        public string SeoKeyWords { set; get; }

        /// <summary>
        ///     SEO搜索描述
        /// </summary>
        public string SeoDesc { set; get; }

        /// <summary>
        ///     计量单位
        /// </summary>
        public MeansureUnitCode MeansureUnit { set; get; }

        /// <summary>
        /// 显示计量单位
        /// </summary>
        public string DisplayMeansureUnit
        {
            get
            {
                return MeansureUnit.ToString();
            }
        }
        /// <summary>
        ///     预计发货周期
        /// </summary>
        public string DeliveryCycle { set; get; }

        /// <summary>
        ///     单个重量(kg)
        /// </summary>
        public float Weight { set; get; }

        /// <summary>
        ///     单个体积(㎡)
        /// </summary>
        public float Volume { set; get; }

        /// <summary>
        ///     服务保障(内容固定，存储方式为1,2,3,4)
        /// </summary>
        public string ServiceAssurance { set; get; }

        /// <summary>
        ///     规格参数
        /// </summary>
        public string Spec { set; get; }

        /// <summary>
        ///     商品说明
        /// </summary>
        public string Desc { set; get; }

        /// <summary>
        ///     物流计价方式(按重量（kg） 按体积（㎡）)
        /// </summary>
        public LogisticsValuationCode LogisticsValuation { set; get; }

        /// <summary>
        ///     运费说明
        /// </summary>
        public string DeliveryDesc { set; get; }

        /// <summary>
        /// 未通过后台审核原因
        /// </summary>
        public string NoPassReason { get; set; }
    }
}

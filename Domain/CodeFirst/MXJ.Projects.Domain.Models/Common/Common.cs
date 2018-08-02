using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    /// 省
    /// </summary>
    public class Province
    {
        /// <summary>
        /// 省编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// GPS经度
        /// </summary>
        public decimal? GpsLon { get; set; }

        /// <summary>
        /// GPS纬度
        /// </summary>
        public decimal? GpsLat { get; set; }

        /// <summary>
        /// 首字母拼音
        /// </summary>
        public string CNPY { get; set; }

        /// <summary>
        /// 全拼音
        /// </summary>
        public string CNPinyin { get; set; }

        /// <summary>
        /// SEOTitel
        /// </summary>
        public string SEOTitle { get; set; }

        /// <summary>
        /// SEO关键字
        /// </summary>
        public string SEOKeywords { get; set; }
    }

    /// <summary>
    /// 市
    /// </summary>
    public class City
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 省编码
        /// </summary>
        public int ProvinCode { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// GPS经度
        /// </summary>
        public decimal? GpsLon { get; set; }

        /// <summary>
        /// GPS纬度
        /// </summary>
        public decimal? GpsLat { get; set; }
    }

    /// <summary>
    /// 区
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 市编码
        /// </summary>
        public int CityCode { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// GPS经度
        /// </summary>
        public decimal? GpsLon { get; set; }

        /// <summary>
        /// GPS纬度
        /// </summary>
        public decimal? GpsLat { get; set; }
    }
}

namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Runtime.Serialization;
    /// <summary>
    /// 地址
    /// </summary>
    [DataContract]
    public class AddressDto
    {
        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember]
        public string DetailAddress { get; set; }

        /// <summary>
        /// 省ID
        /// </summary>
        [DataMember]
        public int ProvinceID { get; set; }

        /// <summary>
        /// 市ID
        /// </summary>
        [DataMember]
        public int CityID { get; set; }

        /// <summary>
        /// 县ID
        /// </summary>
        [DataMember]
        public int CountyID { get; set; }

        /// <summary>
        /// 乡镇ID
        /// </summary>
        [DataMember]
        public int TownID { get; set; }

        /// <summary>
        /// 省名
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市名
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// 县名
        /// </summary>
        [DataMember]
        public string CountyName { get; set; }
        /// <summary>
        /// 乡镇
        /// </summary>
        [DataMember]
        public string TownName { get; set; }
    }

    /// <summary>
    /// 经纬度坐标
    /// </summary>
    public struct GeoPosition
    {
        public GeoPosition(double lat, double lon)
        {
            this.Longitude = lon;
            this.Latitude = lat;
        }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude;

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude;

        #region Overrides of ValueType

        /// <summary>
        /// 返回该实例的完全限定类型名。
        /// </summary>
        /// <returns>
        /// 包含完全限定类型名的 <see cref="T:System.String"/>。
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0},{1}", Longitude, Latitude);
        }

        #endregion
    }


    [Serializable]
    public class DistrictItemDto
    {
        public DistrictItemDto()
        {
            Children = new List<DistrictItemDto>();
        }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 长名称
        /// </summary>
        public string FullName { get; set; }

        public string Pinyin { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<DistrictItemDto> Children { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 中心位置
        /// </summary>
        public GeoPosition Position { get; set; }

        public int Level { get; set; }

        private void Copy(DistrictItemDto src, DistrictItemDto dst)
        {
            dst.Id = src.Id;
            dst.Name = src.Name;
            dst.FullName = src.FullName;
            dst.Position = src.Position;
            dst.Level = src.Level;
            dst.Pinyin = src.Pinyin;
            if (src.Children == null) return;
            var children = new List<DistrictItemDto>();
            foreach (var dist in src.Children)
            {
                var child = new DistrictItemDto();
                Copy(dist, child);
                children.Add(child);
            }
            dst.Children = children;
        }

        public DistrictItemDto DeepClone()
        {
            var rtn = new DistrictItemDto();
            Copy(this, rtn);
            return rtn;
        }
    }
}

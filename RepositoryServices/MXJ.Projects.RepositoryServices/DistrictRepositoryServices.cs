using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.IRepositoryServices;
using MXJ.Core;

namespace MXJ.Projects.RepositoryServices
{
    /// <summary>
    /// 省市区持久化层
    /// </summary>
    public class DistrictRepositoryServices : BaseRepositoryService, IDistrictRepositoryServices
    {
        /// <summary>
        /// 加载所有的省市区
        /// </summary>
        /// <returns></returns>
        public List<DistrictItemDto> LoadData()
        {
            var psql = @"
SELECT   Code AS id, name,1 as level,GpsLat,GpsLon,CNPinyin as pinyin
FROM      province
UNION
SELECT   Code AS id, name,2 as level,GpsLat,GpsLon,'' as pinyin
FROM      city
UNION
SELECT   Code AS id, name,3 as level,GpsLat,GpsLon,'' as pinyin
FROM      area";

            List<DistrictItemDto> district = new List<DistrictItemDto>();
            using (var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, psql))
            {
                while (reader.Read())
                {
                    var item = new DistrictItemDto
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        Pinyin = reader["pinyin"].ToString(),
                        Level = reader.GetValueOrDefault("level", 0),
                        Position = new GeoPosition()
                        {
                            Latitude = reader.GetValueOrDefault("GpsLat", 0d),
                            Longitude = reader.GetValueOrDefault("GpsLon", 0d)
                        }
                    };

                    district.Add(item);
                }
            }

            return district;
        }
    }
}

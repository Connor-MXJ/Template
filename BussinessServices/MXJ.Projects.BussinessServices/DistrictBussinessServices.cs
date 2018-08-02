using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.IBussinessServices;
using MXJ.Projects.IRepositoryServices;
using MXJ.Core.Infrastructure.Caching;

namespace MXJ.Projects.BussinessServices
{
    /// <summary>
    /// 省市区业务层
    /// </summary>
    public class DistrictBussinessServices : BaseBussinessService, IDistrictBussinessServices
    {
        protected IDistrictRepositoryServices DistrictRepo { set; get; } 
        public DistrictBussinessServices(IDistrictRepositoryServices districtRepo)
        {
            DistrictRepo = districtRepo;
        }

        /// <summary>
        /// 获取所有种类
        /// </summary>
        /// <returns></returns>
        public ResponseBase<List<DistrictItemDto>> LoadData()
        {
            var response = new ResponseBase<List<DistrictItemDto>>();
            response.Result = CacheManager.Get(CacheKeyConstantVariable.District, () =>
            {
                return DistrictRepo.LoadData();
            }, TimeSpan.FromDays(10));
             
            response.IsSuccess = true;

            return response;
        }

        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns></returns>
        public ResponseBase<List<DistrictItemDto>> LoadProvinceData()
        {
            var response = new ResponseBase<List<DistrictItemDto>>();
            response.Result = LoadData().Result.Where(t => t.Level == 1).ToList();
            response.IsSuccess = true;
            return response;
        }
    }
}

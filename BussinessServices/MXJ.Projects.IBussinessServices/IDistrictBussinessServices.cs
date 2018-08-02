using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Projects.DataTransferObjects;

namespace MXJ.Projects.IBussinessServices
{
    /// <summary>
    /// 省市区业务层
    /// </summary>
    public interface IDistrictBussinessServices : IBaseBussinessService
    {
        /// <summary>
        /// 获取所有种类
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<DistrictItemDto>> LoadData();

        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<DistrictItemDto>> LoadProvinceData();
    }
}

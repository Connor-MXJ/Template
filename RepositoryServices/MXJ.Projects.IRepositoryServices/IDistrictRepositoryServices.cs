using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;

namespace MXJ.Projects.IRepositoryServices
{
    /// <summary>
    /// 省市区持久化层
    /// </summary>
    public interface IDistrictRepositoryServices : IBaseRepositoryService
    {
        /// <summary>
        /// 加载所有的省市区
        /// </summary>
        /// <returns></returns>
        List<DistrictItemDto> LoadData();
    }
}
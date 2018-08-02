using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Application.Services;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.IBussinessServices
{
    /// <summary>
    /// 业务层
    /// </summary>
    public interface IBaseBussinessService : IBussinessService
    {
        /// <summary>
        ///      获取当前用户.
        /// </summary>
        /// <returns>
        ///     The <see cref="CurrentUserDTO" />.
        /// </returns>
        CurrentMemberUserDto GetCurrentMemberUser();
    }
}

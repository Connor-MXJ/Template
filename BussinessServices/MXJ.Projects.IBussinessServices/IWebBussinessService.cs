using System;
using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.IBussinessServices
{
    /// <summary>
    ///     系统管理业务层
    /// </summary>
    public interface IWebBussinessService : IBaseBussinessService
    {
        #region 云仓
        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ResponseBase<object> GetValidateCode(string key, string type);
 


        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ResponseBase<object> GetSMSCode(string key, string mobile, string validCode, string validType);
      
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponseBase<string> ValidateRegister(string mobile, string smsCode, string password);
   

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <returns></returns>
        ResponseBase<object> ValidateLogin(string userName, string password);

        /// <summary>
        /// 同步租户
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase SyncMemberInfos();

        #endregion
    }
}
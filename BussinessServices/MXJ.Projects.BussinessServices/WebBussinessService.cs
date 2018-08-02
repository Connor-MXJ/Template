using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Models;
using MXJ.Projects.IBussinessServices;
using MXJ.Projects.IRepositoryServices;
using MXJ.Core.ValueInjector;
using MXJ.Projects.Domain.Enums;
using MXJ.Core.Utility.Security;
using MXJ.Core.Infrastructure.Caching;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MXJ.Projects.BussinessServices
{
    /// <summary>
    ///     调用远程交互接口
    /// </summary>
    public class WebBussinessService : BaseBussinessService, IWebBussinessService
    {
        #region 常量
        public string YCDomain = ConfigurationManager.AppSettings["YCDomain"];
        #endregion
 

        #region 云仓
        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseBase<object> GetValidateCode(string key, string type)
        {
            var response = new ResponseBase<object>();
            try
            {
                var client = new HttpClient();
                var result = client.GetAsync(YCDomain + "/api/app/v2/getvalidcode?key=" + key + "&type=" + type + "").Result;
                var responseJson = result.Content.ReadAsByteArrayAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    response.IsSuccess = true;
                    response.Result = responseJson;
                }
                else
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "验证码连接失败";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
                WriteLogException(ex); // 记录日志信息  
            }
            return response;
        }


        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseBase<object> GetSMSCode(string key, string mobile, string validCode, string validType)
        {
            var response = new ResponseBase<object>();
            try
            {
                var client = new HttpClient();
                var requestJson = JsonConvert.SerializeObject(new { key = key, mobile = mobile, validCode = validCode, validType = validType });
                var result = client.PostAsync(YCDomain + "/api/app/v2/getsmscode", new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")).Result;
                var responseJson = result.Content.ReadAsStringAsync().Result;
                var apiResponse = (JObject)JsonConvert.DeserializeObject(responseJson);
                response.IsSuccess = apiResponse.GetValue("code").Value<string>().Equals("0000");
                response.OperationDesc = apiResponse.GetValue("msg").Value<string>();
                response.Result = apiResponse.GetValue("result");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = ex.Message;
                WriteLogException(ex); // 记录日志信息  
            }
            return response;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseBase<string> ValidateRegister(string mobile, string smsCode, string password)
        {
            var response = new ResponseBase<string>();
            try
            {
                var client = new HttpClient();
                var requestJson = JsonConvert.SerializeObject(new { mobile = mobile, smsCode = smsCode, password = TripleDes.EncryptDES(password, "50yc_czm", "12345678") });
                var result = client.PostAsync(YCDomain + "/api/app/v2/member/register", new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")).Result;
                var responseJson = result.Content.ReadAsStringAsync().Result;
                var apiResponse = (JObject)JsonConvert.DeserializeObject(responseJson);
                response.IsSuccess = apiResponse.GetValue("code").Value<string>().Equals("0000");
                response.OperationDesc = apiResponse.GetValue("msg").Value<string>();
                if (response.IsSuccess)
                {
                    response.Result = apiResponse.GetValue("result").Value<string>("memberNo");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = "请求错误";
                WriteLogException(ex); // 记录日志信息  
            }
            return response;
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <returns></returns>
        public ResponseBase<object> ValidateLogin(string userName, string password)
        {
            var response = new ResponseBase<object>();
            try
            {
                var client = new HttpClient();
                var requestJson = JsonConvert.SerializeObject(new { Account = userName, Password = TripleDes.EncryptDES(password, "50yc_czm", "12345678") });

                var result = client.PostAsync(YCDomain + "/api/app/v2/member/signin", new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")).Result;
                var responseJson = result.Content.ReadAsStringAsync().Result;
                var apiResponse = (JObject)JsonConvert.DeserializeObject(responseJson);
                response.IsSuccess = apiResponse.GetValue("code").Value<string>().Equals("0000");
                response.OperationDesc = apiResponse.GetValue("msg").Value<string>();
                response.Result = apiResponse.GetValue("result");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
                WriteLogException(ex); // 记录日志信息  
            }
            return response;
        }

        int _syncCount = 0;
        int _syncSUM = 0;
        /// <summary>
        /// 同步租户
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase SyncMemberInfos()
        {
            var response = new ResponseBase();
            try
            {
               GetSyncMember(out response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = "请求错误";
                WriteLogException(ex); // 记录日志信息  
            }
            if (response.IsSuccess)
            {
                response.OperationDesc = "会员同步成功，共" + _syncSUM + "条数据";
            }
            _syncCount = 0;
            _syncSUM = 0;
            return response;
        }

        private void GetSyncMember(out ResponseBase response)
        {
            response = new ResponseBase();
        }
        #endregion


    }
}

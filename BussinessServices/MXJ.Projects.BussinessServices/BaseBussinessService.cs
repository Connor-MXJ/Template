using System;
using MXJ.Core.Infrastructure.Caching;
using MXJ.Core.Infrastructure.Engine;
using MXJ.Core.Infrastructure.LifeTime;
using MXJ.Core.Infrastructure.Logging;
using MXJ.Core.ValueInjector;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.IBussinessServices;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.BussinessServices
{
    /// <summary>
    ///     业务层
    /// </summary>
    public class BaseBussinessService : IBaseBussinessService
    {
        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        protected ICacheManager CacheManager { get; private set; }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// 当前网站管理后台登录用户
        /// </summary>
        protected SysUserDto CurrentAdminLogonUser { get; set; }

 
        public BaseBussinessService()
        {
            this.CacheManager = EngineContext.Current.DependencyContainer.Resolve<ICacheFactory>().GetCacheManager();
            this.Logger = EngineContext.Current.DependencyContainer.Resolve<ILoggerFactory>().CreateLogger();
        }


        #region 公共方法
        /// <summary>
        ///      获取当前用户.
        /// </summary>
        /// <returns>
        ///     The <see cref="CurrentUserDTO" />.
        /// </returns>
        public CurrentMemberUserDto GetCurrentMemberUser()
        {
            CurrentMemberUserDto currentUser = new CurrentMemberUserDto();
            Guid currentUserId = Guid.Empty;
            string userId = ((string)PerRequestManager.GetValue("CurrentUserID"));
            string key = userId + CacheKeyConstantVariable.CurrentUser;
            userId = userId ?? string.Empty;
            //if (Guid.TryParse(userId, out currentUserId))
            //{
            //    currentUser = this.CacheManager.Get(
            //        key,
            //        () =>
            //        {
            //            var memberBussiness = EngineContext.Current.DependencyContainer.Resolve<IMemberBussinessService>();
            //            var response = memberBussiness.GetMemberById(currentUserId);
            //            if (response.IsSuccess)
            //            {
            //                var user = response.Result;
            //                if (user != null)
            //                {
            //                    currentUser.Inject(user);
            //                    return currentUser;
            //                }
            //            }
            //            return null;
            //        },
            //        TimeSpan.FromDays(1));
            //}
            return currentUser;
        }

        #region 日志记录
        public void WriteLogInfo(string info)
        {
            Logger.Information(info);
        }

        public void WriteDebugInfo(string info)
        {
            Logger.Debug(info);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="error"></param>
        public void WriteLogError(string error)
        {
            Logger.Error(error);
        }

        public string WriteLogException(Exception exc)
        {
            string error = GetInnerExceptionErrorMessage(exc);
            Logger.Error(error, exc);
            return error;
        }

        private string GetInnerExceptionErrorMessage(Exception exc)
        {
            var error = string.Empty;
            SetInnerExceptionErrorMessage(exc, ref error);
            return error;
        }

        private void SetInnerExceptionErrorMessage(Exception exc, ref string error)
        {
            if (null != exc)
            {
                error += exc.Message + ";";
                SetInnerExceptionErrorMessage(exc.InnerException, ref error);
            }
        }

        CurrentMemberUserDto IBaseBussinessService.GetCurrentMemberUser()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}
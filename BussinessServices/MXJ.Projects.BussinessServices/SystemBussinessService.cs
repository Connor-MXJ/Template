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
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.BussinessServices
{
    /// <summary>
    ///     系统管理业务层
    /// </summary>
    public class SystemBussinessService : BaseBussinessService, ISystemBussinessService
    {
        protected ISystemRepositoryService SystemRepo { set; get; }

        public SystemBussinessService(ISystemRepositoryService systemRepo)
        {
            SystemRepo = systemRepo;
        }

        #region 登录/登出
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">登录条件</param>
        /// <returns>是否成功</returns>
        public ResponseBase<CurrentUserDto> Login(LoginDto model)
        {
            var rp = new ResponseBase<CurrentUserDto>();
            rp.IsLogin = false;
            if (null != model)
            {
                var userName = model.UserName;
                var loginUser = SystemRepo.GetUserByName(userName);
                if (null == loginUser)
                {
                    rp.IsSuccess = false;
                    rp.OperationDesc = "用户不存在";
                }
                else if (loginUser.UserStatus == StatusCode.锁定)
                {
                    rp.IsSuccess = false;
                    rp.OperationDesc = "该用户已被锁定";
                }
                else if (loginUser.RoleIDs == null || loginUser.RoleIDs.Count() <= 0)
                {
                    rp.IsSuccess = false;
                    rp.OperationDesc = "该用户无角色";
                }
                else
                {
                    var password = Md5Encryption.Encrypt(model.UserPassword);
                    if (password == loginUser.UserPassword)
                    {
                        CurrentUserDto currentUser = new CurrentUserDto();
                        currentUser.UserID = loginUser.SysUserId;
                        currentUser.UserName = loginUser.UserName;
                        //currentUser.Actions = loginUser.Actions;
                        currentUser.RealName = loginUser.RealName;
                        currentUser.RoleIDs = loginUser.RoleIDs;
                        currentUser.RoleNames = loginUser.RoleNames;
                        currentUser.MenuIds = loginUser.MenuIds;
                        rp.Result = currentUser;
                        rp.IsLogin = true;
                        rp.IsSuccess = true;
                        rp.OperationDesc = "登录成功";
                        WriteLogInfo("用户:" + model.UserName + ", 登录系统");
                        var dto = new SysOperationLogDto
                        {
                            UserName = loginUser.UserName,
                            OperationTypeCode = OperationTypeCode.操作,
                            OperationUrl = "/Account/Login",
                            OperationContent = "登录成功",
                        };
                        CreateSysOperationLog(dto);
                    }
                    else
                    {
                        rp.IsSuccess = false;
                        rp.OperationDesc = "登录失败，密码错误";
                    }
                }
            }
            else
            {
                rp.IsSuccess = false;
                rp.OperationDesc = "登录失败，提交数据为空";
            }
            return rp;
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        //public ResponseBase Logout()
        //{
        //    var rp = new ResponseBase();
        //    rp.IsLogin = false;
        //    rp.IsSuccess = false;
        //    rp.OperationDesc = "退出登录";
        //    WriteLogInfo("用户:" + GetCurrentUser().UserName + ", 退出系统");
        //    PerRequestManager.RemoveValue("CurrentUserID");
        //    return rp;
        //}
        #endregion

        #region 菜单
        /// <summary>
        ///     创建一个新的菜单
        /// </summary>
        /// <param name="sysMenuDto"></param>
        /// <returns></returns>
        public ResponseBase CreateSysMenu(SysMenuDto sysMenuDto)
        {
            var response = ValidationMenu(sysMenuDto);
            if (response.IsSuccess)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var sysMenu = new SysMenu();
                        sysMenu.Inject(sysMenuDto);
                        var result = SystemRepo.CreateSysMenu(sysMenuDto);
                        transactionScope.Complete();
                        response.IsSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    WriteLogException(ex);
                }
            }
            response.OperationDesc = "添加菜单" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        public ResponseBase<List<SysMenuDto>> GetMenus(Guid? parentId, int? pageNumber, int? pageIndex)
        {
            var response = new ResponseBase<List<SysMenuDto>>();
            var list = new List<SysMenuDto>();
            var result = SystemRepo.GetMenus(parentId, pageNumber, pageIndex);
            foreach (var item in result)
            {
                var obj = new SysMenuDto();
                obj.Inject(item);
                list.Add(obj);
            }

            response.Result = list;
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        public SearchResponseBase<List<SysMenuDto>> SearchSysMenuList(SysMenuSearchDto search)
        {
            var response = new SearchResponseBase<List<SysMenuDto>>();
            var list = new List<SysMenuDto>();
            try
            {
                response.Result = SystemRepo.SearchSysMenuList(search);
                response.Rows = search.Rows;
                response.Page = search.Page;
                response.TotalRecordCount = response.Result.Count();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                WriteLogException(ex);
                response.IsSuccess = false;
            }
            response.OperationDesc = "查询菜单列表" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>是否成功</returns>
        public ResponseBase DeleteSysMenu(IList<Guid> ids)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var result = SystemRepo.DeleteSysMenu(ids);
                if (result > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                WriteLogException(ex);
            }
            response.OperationDesc = "删除菜单" + (response.IsSuccess ? "成功" : "失败");
            return response;


        }

        /// <summary>
        /// 编辑菜单序号
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        public ResponseBase EditSysMenuOrder(SysMenuDto model)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var result = SystemRepo.EditSysMenu(model);
                if (result > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                WriteLogException(ex);
            }
            response.OperationDesc = "编辑菜单序号" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        public ResponseBase EditSysMenu(SysMenuDto model)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var result = SystemRepo.EditSysMenu(model);
                if (result > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                WriteLogException(ex);
            }
            response.OperationDesc = "编辑菜单" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单信息</returns>
        public ResponseBase<SysMenuDto> GetSysMenuByID(Guid id)
        {
            ResponseBase<SysMenuDto> response = new ResponseBase<SysMenuDto>();
            try
            {
                var result = SystemRepo.GetSysMenuByID(id);
                response.Result = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                WriteLogException(ex);
            }
            response.OperationDesc = "获取菜单" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        private ResponseBase ValidationMenu(SysMenuDto model)
        {
            var response = new ResponseBase();
            response.IsSuccess = false;
            if (string.IsNullOrEmpty(model.MenuName))
            {
                response.OperationDesc = "菜单名不能为空";
            }

            if (SystemRepo.IsExisMenu(model))
            {
                response.OperationDesc = "菜单名已存在";
            }
            else { response.IsSuccess = true; }
            return response;
        }

        /// <summary>
        /// 获取当前用户菜单树
        /// </summary>
        /// <returns>用户菜单列表</returns>
        public ResponseBase<List<SysMenuDto>> GetCurrentUserMenu(CurrentUserDto currentUserDto)
        {
            var response = new ResponseBase<List<SysMenuDto>>();
            var model = new List<SysMenuDto>();
            try
            {
                model = CacheManager.Get(currentUserDto.UserID.ToString() + CacheKeyConstantVariable.CurrentUserMenu, () =>
                  {
                      if (null != currentUserDto.MenuIds)
                      {

                          model = SystemRepo.GetUserMenuByIdList(currentUserDto.MenuIds);
                      }
                      return model;
                  }, TimeSpan.FromDays(1));
                response.Result = model;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
                WriteLogException(ex);
            }
            return response;
        }
        #endregion

        #region 用户



        #endregion

        #region 操作日志
        /// <summary>
        /// 获取操作日志列表
        /// </summary>
        /// <param name="search">操作日志查询参数</param>
        /// <returns>操作日志列表</returns>
        public SearchResponseBase<List<SysOperationLogDto>> SearchSysOperationLogList(SysOperationLogSearchDTO search)
        {
            SearchResponseBase<List<SysOperationLogDto>> response = new SearchResponseBase<List<SysOperationLogDto>>();
            try
            {
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                WriteLogException(ex);
                response.IsSuccess = false;
            }
            return response;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">操作日志</param>
        /// <returns>是否成功</returns>
        public ResponseBase CreateSysOperationLog(SysOperationLogDto model)
        {
            return new ResponseBase();
            //return SystemRepo.CreateSysOperationLog(model);
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        public SearchResponseBase<SysOperationLogDto> GetSysOperationLog(Guid logID)
        {
            return new SearchResponseBase<SysOperationLogDto>();
            //return SystemRepo.GetSysOperationLog(logID);
        }
        #endregion

        #region 字典管理


        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="search">数据字典查询参数</param>
        /// <returns>数据字典列表</returns>
        public SearchResponseBase<List<SysDictionaryDto>> SearchSysDictionaryList(SysDictionarySearchDto search)
        {
            int count = 0;
            var response = new SearchResponseBase<List<SysDictionaryDto>>();
            response.Rows = search.Rows;
            response.Page = search.Page;
            response.Result = SystemRepo.SearchSysDictionaryList(search, out count);
            response.TotalRecordCount = count;
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 根据字典父级编码获取数据字典子列表
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="category">作用类别</param>
        /// <returns>数据字典列表</returns>
        public SearchResponseBase<List<SysDictionaryDto>> GetSysDicChildrenByParent(DictionaryParentCode dictionaryParent)
        {
            var response = new SearchResponseBase<List<SysDictionaryDto>>();
            response.IsSuccess = true;
            response.Result = SystemRepo.GetSysDicChildrenByParent(dictionaryParent);
            return response;
        }


        /// <summary>
        /// 根据字典编码获取数据字典
        /// </summary>
        /// <param name="dictionaryCode">字典编码</param>
        /// <returns>数据字典列表</returns>
        public ResponseBase<SysDictionaryDto> GetSysDictionaryByCode(string dictionaryCode)
        {
            var response = new ResponseBase<SysDictionaryDto>();
            response.Result = SystemRepo.GetSysDictionaryByCode(dictionaryCode);
            response.IsSuccess = response.Result != null;
            return response;
        }

        /// <summary>
        /// 编辑数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        public ResponseBase EditSysDictionary(SysDictionaryDto model)
        {
            var response = ValidationDictionary(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            try
            {
                response.IsSuccess = SystemRepo.EditSysDictionary(model) > 0;
                CacheManager.Remove(CacheKeyConstantVariable.Dictionary);
            }
            catch (Exception ex)
            {
                WriteLogError(ex.ToString());
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
            }

            return response;
        }

        /// <summary>
        /// 添加数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        public ResponseBase CreateSysDictionary(SysDictionaryDto model)
        {
            var response = ValidationDictionary(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            try
            {
                response.IsSuccess = SystemRepo.CreateSysDictionary(model) > 0;
                CacheManager.Remove(CacheKeyConstantVariable.Dictionary);
            }
            catch (Exception ex)
            {
                WriteLogError(ex.ToString());
                Logger.Log(Core.Infrastructure.Logging.LogLevel.Error, ex, string.Empty);
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
            }

            return response;
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="ids">数据字典ID集合</param>
        /// <returns>是否成功</returns>
        public ResponseBase DeleteSysDictionary(IList<Guid> ids)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                response.IsSuccess = SystemRepo.DeleteSysDictionary(ids) > 0;
                CacheManager.Remove(CacheKeyConstantVariable.Dictionary);
            }
            catch (Exception ex)
            {
                WriteLogError(ex.ToString());
                response.IsSuccess = false;
                response.OperationDesc = ex.ToString();
            }

            response.OperationDesc = "删除数据字典" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 验证字典
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private ResponseBase ValidationDictionary(SysDictionaryDto dictionary)
        {
            var response = new ResponseBase();
            response.IsSuccess = false;
            if (string.IsNullOrEmpty(dictionary.DictionaryCode))
            {
                response.OperationDesc = "字典编码不能为空";
            }

            if (string.IsNullOrEmpty(dictionary.DictionaryName))
            {
                response.OperationDesc = "字典名称不能为空";
            }

            if (SystemRepo.IsExisDictionary(dictionary))
            {
                response.OperationDesc = "字典编码已存在";
            }
            else
            {
                response.IsSuccess = true;
            }

            return response;
        }

        #endregion
    }
}

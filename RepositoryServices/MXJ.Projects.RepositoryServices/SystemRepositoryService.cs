using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Context;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Domain.Models;
using MXJ.Projects.IRepositoryServices;
using MXJ.Projects.RepositoryServices;
using MXJ.Core.ValueInjector;
using MXJ.Core;

namespace MXJ.Projects.RepositoryServices
{
    /// <summary>
    /// 系统管理持久化层
    /// </summary>
    public class SystemRepositoryService : BaseRepositoryService, ISystemRepositoryService
    {
        public SystemRepositoryService(IDomainRepositoryContext domainRepositoryContext,
    IDomainRepository<SysUser, Guid> sysUserRepo,
    IDomainRepository<SysMenu, Guid> sysMenuRepo,
    IDomainRepository<SysRole, Guid> sysRoleRepo,
    IDomainRepository<SysAction, Guid> sysActionRepo,
      IDomainRepository<UserRoleRef, Guid> userRoleRefRepo,
    IDomainRepository<SysDictionary, Guid> sysDictionaryRepo)
        {
            this.DomainRepositoryContext = domainRepositoryContext;
            this.SysUserRepo = sysUserRepo;
            this.SysMenuRepo = sysMenuRepo;
            this.SysRoleRepo = sysRoleRepo;
            this.UserRoleRefRepo = userRoleRefRepo;
            this.SysDictionaryRepo = sysDictionaryRepo;
            this.SysActionRepo = sysActionRepo;
        }

        protected IDomainRepositoryContext DomainRepositoryContext { get; }

        protected IDomainRepository<SysUser, Guid> SysUserRepo { get; set; }

        protected IDomainRepository<SysMenu, Guid> SysMenuRepo { get; set; }

        protected IDomainRepository<SysRole, Guid> SysRoleRepo { get; set; }
        protected IDomainRepository<UserRoleRef, Guid> UserRoleRefRepo { get; set; }

        protected IDomainRepository<SysDictionary, Guid> SysDictionaryRepo { get; set; }
        protected IDomainRepository<SysAction, Guid> SysActionRepo { get; set; }

        #region 用户

        /// <summary>
        /// 查询菜单
        /// </summary> 
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        public List<SysUserDto> GetSystemUsers(int? pageNumber, int? pageIndex)
        {
            var sql = BuildSqlGetSystemUsers(string.Empty, string.Empty, null, pageNumber, pageIndex);
            return ReadSystemUser(sql);
        }

        /// <summary>
        /// 数据查询及对象映射
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<SysUserDto> ReadSystemUser(Tuple<string, MySqlParameter[]> sql)
        {
            // 执行数据库查询
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql.Item1, sql.Item2);
            var list = new List<SysUserDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new SysUserDto
                    {
                        UserStatus = (StatusCode)reader.GetInt32("UserStatus"),
                        UserName = reader.GetString("UserName"),
                        RealName = reader.GetString("RealName"),
                        SysUserId = reader.GetGuid("SysUserId"),
                        CreatedTime = reader.GetDateTime("CreatedTime"),
                        UpdatedTime = reader.GetValueOrDefault<DateTime?>("UpdatedTime", null)
                    });
                }
            }

            return list;
        }

        /// <summary>
        ///  构造查询sql
        /// </summary>
        /// <param name="userName">登录账户</param>
        /// <param name="userPassword">密码</param>
        /// <param name="userStatus">状态</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        private Tuple<string, MySqlParameter[]> BuildSqlGetSystemUsers(string userName, string userPassword, int? userStatus, int? pageNumber, int? pageIndex)
        {
            var parameters = new List<MySqlParameter>();
            pageIndex = pageIndex ?? 0;
            pageNumber = pageNumber ?? int.MaxValue;
            string sql = "select SysUserId,UserName,UserStatus,RoleIDs,RoleNames,RealName,CreatedTime,UpdatedTime from sysuser where IsDeleted<> true ";
            if (!string.IsNullOrEmpty(userName))
            {
                sql += " and UserName = @userName";
                parameters.Add(new MySqlParameter("@userName", userName));
            }

            if (!string.IsNullOrEmpty(userPassword))
            {
                sql += " and UserPassword = @userPassword";
                parameters.Add(new MySqlParameter("@userPassword", userPassword));
            }

            if (userStatus.HasValue && userStatus.Value != 0)
            {
                sql += " and UserStatus = @userStatus";
                parameters.Add(new MySqlParameter("@userStatus", userStatus.Value));
            }

            sql += " order by MenuOrder ";
            sql += " limit " + pageIndex * pageNumber + "," + pageNumber;

            return Tuple.Create(sql, parameters.ToArray());
        }

        /// <summary>
        /// 根据账号和密码获取登录信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public List<SysUserDto> ValidateSystemUserForLogin(string userName, string userPassword)
        {
            var sql = BuildSqlGetSystemUsers(userName, userPassword, (int)StatusCode.正常, 10, 1);
            return ReadSystemUser(sql);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public int UpdatePassword(Guid sysUserId, string newPassword)
        {
            string sql = @"update sysuser set UserPassword = @password where SysUserId = @sysUserId";
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql,
                new MySqlParameter("@password", newPassword),
                new MySqlParameter("@sysUserId", sysUserId));
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public SysUserDto GetUserByName(string userName)
        {
            var sysUser = this.SysUserRepo.Entities.FirstOrDefault(p => p.UserName == userName);
            if (sysUser != null)
            {
                SysUserDto sysUserDto = new SysUserDto();
                sysUserDto.Inject(sysUser);
                var roleList = GetRoleListByUserID(sysUserDto.SysUserId);
                if (roleList != null && roleList.Count > 0)
                {
                    sysUserDto.RoleIDs = roleList.Select(p => p.SysRoleId).ToList();
                    sysUserDto.RoleNames = roleList.Select(p => p.RoleName).ToList();
                }
                var menuList = new List<string>();
                foreach (var item in roleList)
                {
                    menuList.AddRange(item.SysMenuIDs.Split(';').ToList());
                }
                sysUserDto.MenuIds = menuList.Distinct().ToList();
                sysUserDto.Actions = GetActionListByRoleIds(roleList);
                return sysUserDto;
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// 更加角色ID获取用户列表
        ///// </summary>
        ///// <param name="search">角色ID</param>
        ///// <returns>用户信息列表</returns>
        //public SearchResponseBase<List<SysUserDTO>> SearchSysUserListByRoleID(Guid sysRoleID)
        //{
        //    SearchResponseBase<List<SysUserDTO>> response = new SearchResponseBase<List<SysUserDTO>>();
        //    var sysUserList = (from sysUser in SysUserRepo.Entities
        //                       where !string.IsNullOrEmpty(sysUser.RoleIDs)
        //                       orderby sysUser.CreatedTime descending
        //                       select sysUser).ToList();
        //    sysUserList = sysUserList.Where(u => u.RoleIDs.Split(';').Contains(sysRoleID.ToString())).ToList();
        //    response.IsSuccess = true;
        //    response.Result = sysUserList.Select(c => new SysUserDTO
        //    {
        //        SysUserID = c.SysUserID,
        //        UserName = c.UserName,
        //        NickName = c.NickName,
        //        RealName = c.RealName,
        //        Mobile = c.Mobile,
        //        UserStatus = c.UserStatus,
        //        CreatedTime = c.CreatedTime
        //    }).ToList();

        //    return response;
        //}

        ///// <summary>
        ///// 获取用户列表
        ///// </summary>
        ///// <param name="search">用户查询参数</param>
        ///// <returns>用户列表</returns>
        //public SearchResponseBase<List<SysUserDTO>> SearchSysUserList(SysUserSearchDTO search)
        //{
        //    SearchResponseBase<List<SysUserDTO>> response = new SearchResponseBase<List<SysUserDTO>>();
        //    var sysUserList = from sysUser in SysUserRepo.Entities
        //                      where ((string.IsNullOrEmpty(search.UserName) || sysUser.UserName.Contains(search.UserName)) &&
        //                      (string.IsNullOrEmpty(search.RealName) || sysUser.UserName.Contains(search.RealName)))
        //                      orderby sysUser.CreatedTime descending
        //                      select sysUser;
        //    if (Enum.IsDefined(typeof(StatusCode), search.UserStatus))
        //        sysUserList = sysUserList.Where(s => s.UserStatus == search.UserStatus).OrderByDescending(s => s.CreatedTime);
        //    response.TotalRecordCount = sysUserList.Count();
        //    response.Rows = search.Rows;
        //    response.Page = search.Page;
        //    response.IsSuccess = true;
        //    response.Result = sysUserList.Skip((response.Page - 1) * response.Rows).Take(response.Rows)
        //                 .ToList().Select(c => new SysUserDTO
        //                 {
        //                     SysUserID = c.SysUserID,
        //                     UserName = c.UserName,
        //                     NickName = c.NickName,
        //                     RealName = c.RealName,
        //                     Mobile = c.Mobile,
        //                     UserStatus = c.UserStatus,
        //                     UserType = c.UserType
        //                 }).ToList();
        //    return response;
        //}

        ///// <summary>
        ///// 获取所有用户
        ///// </summary>
        ///// <returns></returns>
        //public SearchResponseBase<List<ZTreeDTO>> GetAllSysUserList()
        //{
        //    SearchResponseBase<List<ZTreeDTO>> response = new SearchResponseBase<List<ZTreeDTO>>();
        //    var sysUserList = SysUserRepo.Entities.OrderBy(p => p.Level).Select(d => new ZTreeDTO
        //    {
        //        id = d.SysUserID.ToString(),
        //        name = d.RealName
        //    });
        //    response.Result = sysUserList.ToList();
        //    response.IsSuccess = true;
        //    return response;
        //}

        ///// <summary>
        ///// 根据用户ID获取用户信息
        ///// </summary>
        ///// <param name="id">用户ID</param>
        ///// <returns>用户信息</returns>
        //public ResponseBase<SysUserDTO> GetSysUserByID(Guid id)
        //{
        //    var response = new ResponseBase<SysUserDTO>();
        //    var model = SysUserRepo.GetByKey(id);
        //    if (model != null)
        //    {
        //        SysUserDTO sysUserDTO = new SysUserDTO();
        //        sysUserDTO.Inject(model);
        //        sysUserDTO.RoleIDs = string.Join(";", model.Roles.Select(m => m.SysRoleID).ToList());
        //        sysUserDTO.RoleNames = string.Join(",", model.Roles.Select(m => m.RoleName).ToList());
        //        response.IsSuccess = true;
        //        response.Result = sysUserDTO;
        //        return response;
        //    }
        //    response.OperationDesc = response.IsSuccess ? "获取用户成功" : "获取用户失败";
        //    return response;
        //}

        ///// <summary>
        ///// 编辑用户
        ///// </summary>
        ///// <param name="model">用户信息</param>
        ///// <returns>是否成功</returns>
        //public ResponseBase EditSysUser(SysUserDTO model)
        //{
        //    var response = new ResponseBase();
        //    var sysUser = SysUserRepo.GetByKey(model.SysUserID);
        //    if (sysUser == null)
        //    {
        //        response.IsSuccess = false;
        //        response.OperationDesc = "用户未找到";
        //        return response;
        //    }
        //    sysUser.UserName = model.UserName;
        //    sysUser.NickName = model.NickName;
        //    sysUser.RealName = model.RealName;
        //    sysUser.Mobile = model.Mobile;
        //    sysUser.Remark = model.Remark;

        //    if (!string.IsNullOrEmpty(model.RoleIDs))
        //    {
        //        sysUser.Roles.Clear();
        //        //设置用户角色
        //        IList<Guid> roleIDs = model.RoleIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll<Guid>(i => Guid.Parse(i));
        //        var roleList = from role in SysRoleRepo.Entities
        //                       from roleID in roleIDs
        //                       where role.SysRoleID == roleID
        //                       select role;
        //        sysUser.Roles = roleList.ToList();
        //        sysUser.RoleIDs = model.RoleIDs;
        //        sysUser.RoleNames = model.RoleNames;
        //        ///清除当前用户的缓存
        //        CacheManager.Remove(sysUser.SysUserID.ToString() + "_CurrentUser");
        //        CacheManager.Remove(sysUser.SysUserID.ToString() + "_CurrentUserMenu");
        //    }
        //    BusinessRepositoryContext.RegisterUpdate<SysUser, Guid>(sysUser);
        //    try
        //    {
        //        BusinessRepositoryContext.Commit();
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        BusinessRepositoryContext.Rollback();
        //        WriteLogException(ex);
        //        response.IsSuccess = false;
        //    }
        //    response.OperationDesc = "编辑用户" + (response.IsSuccess ? "成功" : "失败");
        //    return response;
        //}

        ///// <summary>
        ///// 编辑用户数据控制权限
        ///// </summary>
        ///// <param name="model">用户信息</param>
        ///// <returns>是否成功</returns>
        //public ResponseBase EditSysUserPerm(SysUserDTO dto)
        //{
        //    ResponseBase response = new ResponseBase();

        //    //SysUser model = SysUserRepo.GetByKey(dto.SysUserID);
        //    //if (model != null)
        //    //{

        //    //    var items = model.SysDataPerms.ToList();
        //    //    if (items != null && items.Count() > 0)
        //    //    {
        //    //        foreach (var v in items)
        //    //        {
        //    //            BusinessRepositoryContext.RegisterPhysicalDelete<SysDataPerm, Guid>(v);
        //    //        }
        //    //    }
        //    //    if (dto.SysDataPerms != null && dto.SysDataPerms.Count() > 0)
        //    //    {
        //    //        foreach (var item in dto.SysDataPerms)
        //    //        {
        //    //            SysDataPerm dataPermItem = new SysDataPerm();
        //    //            dataPermItem.ID = Guid.NewGuid();
        //    //            dataPermItem.UserID = model.SysUserID;
        //    //            dataPermItem.ControlPath = item.ControlPath;
        //    //            BusinessRepositoryContext.RegisterCreate<SysDataPerm, Guid>(dataPermItem);
        //    //        }
        //    //    }

        //    //    var SysAssetFeeDataPerms = model.SysAssetFeeDataPerms.ToList();
        //    //    if (SysAssetFeeDataPerms != null && SysAssetFeeDataPerms.Count() > 0)
        //    //    {
        //    //        foreach (var v in SysAssetFeeDataPerms)
        //    //        {
        //    //            BusinessRepositoryContext.RegisterPhysicalDelete<SysAssetFeeDataPerm, Guid>(v);
        //    //        }
        //    //    }
        //    //    if (dto.SysAssetFeeDataPerms != null && dto.SysAssetFeeDataPerms.Count() > 0)
        //    //    {
        //    //        foreach (var item in dto.SysAssetFeeDataPerms)
        //    //        {
        //    //            SysAssetFeeDataPerm dataPermItem = new SysAssetFeeDataPerm();
        //    //            dataPermItem.ID = Guid.NewGuid();
        //    //            dataPermItem.UserID = model.SysUserID;
        //    //            dataPermItem.ProjectID = item.ProjectID;
        //    //            dataPermItem.ProjectName = item.ProjectName;
        //    //            BusinessRepositoryContext.RegisterCreate<SysAssetFeeDataPerm, Guid>(dataPermItem);
        //    //        }
        //    //    }

        //    //    try
        //    //    {
        //    //        response.IsSuccess = BusinessRepositoryContext.Commit() > 0;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        response.IsSuccess = false;
        //    //        response.OperationDesc = ex.Message;
        //    //        this.WriteLogException(ex);
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    response.IsSuccess = false;
        //    //    response.OperationDesc = "未找到用户";
        //    //}
        //    return response;
        //}

        ///// <summary>
        ///// 添加用户
        ///// </summary>
        ///// <param name="model">用户信息</param>
        ///// <returns>是否成功</returns>
        //public ResponseBase CreateSysUser(SysUserDTO model)
        //{
        //    var response = new ResponseBase();
        //    SysUser sysUser = new SysUser();
        //    sysUser.Inject(model);
        //    sysUser.SysUserID = Guid.NewGuid();

        //    sysUser.IsLoginError = false;//上次密码是否错误，用于登陆时判断是否需要验证码
        //    sysUser.CreatedTime = DateTime.Now;
        //    //密码加密
        //    sysUser.UserPassword = MD5Encryption.Encrypt(sysUser.UserPassword);
        //    sysUser.UserStatus = StatusCode.正常;
        //    sysUser.UserType = UserTypeCode.管理员;
        //    if (!string.IsNullOrEmpty(model.RoleIDs))
        //    {
        //        //设置用户角色
        //        IList<Guid> roleIDs = model.RoleIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll<Guid>(i => Guid.Parse(i));
        //        var roleList = from role in SysRoleRepo.Entities
        //                       from roleID in roleIDs
        //                       where role.SysRoleID == roleID
        //                       select role;
        //        sysUser.Roles = roleList.ToList();
        //    }
        //    BusinessRepositoryContext.RegisterCreate<SysUser, Guid>(sysUser);
        //    try
        //    {
        //        BusinessRepositoryContext.Commit();
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        BusinessRepositoryContext.Rollback();
        //        WriteLogException(ex);
        //        response.IsSuccess = false;
        //    }
        //    response.OperationDesc = "添加用户" + (response.IsSuccess ? "成功" : "失败");
        //    return response;
        //}

        ///// <summary>
        ///// 管理员重置用户密码
        ///// </summary>
        ///// <param name="sysUserPasswordDTO">用户信息</param>
        ///// <returns>是否修改成功</returns>
        //public ResponseBase ResetUserPwd(SysUserPasswordDTO sysUserPasswordDTO)
        //{
        //    ResponseBase response = new ResponseBase();
        //    SysUser sysUser = SysUserRepo.GetByKey(sysUserPasswordDTO.SysUserID);
        //    sysUser.UserPassword = MD5Encryption.Encrypt(sysUserPasswordDTO.NewPassword);
        //    try
        //    {
        //        response.IsSuccess = SysUserRepo.Update(sysUser) >= 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogException(ex);
        //    }
        //    response.OperationDesc = response.IsSuccess ? "修改密码成功" : "修改密码失败";
        //    return response;
        //}

        ///// <summary>
        ///// 批量禁用/启用
        ///// </summary>
        ///// <param name="id">用户ID</param>
        ///// <returns>是否成功</returns>
        //public ResponseBase DisableOrEnableUser(Guid id)
        //{
        //    ResponseBase response = new ResponseBase();
        //    string str = string.Empty;
        //    //判断传入id是否为空
        //    if (id != Guid.Empty)
        //    {
        //        var sysUser = SysUserRepo.GetByKey(id);
        //        //判断该角色是否包含超级管理员
        //        if (sysUser != null)
        //        {
        //            var s = sysUser.Roles.FirstOrDefault(r => r.RoleName == RoleCodeConstant.SuperAdmin);
        //            if (s != null)
        //            {
        //                response.IsSuccess = false;
        //                response.OperationDesc = "此禁用包含超级管理员，不能禁用！";
        //            }
        //            else
        //            {
        //                if (sysUser != null)
        //                {
        //                    if (sysUser.UserStatus == StatusCode.正常)
        //                    {
        //                        sysUser.UserStatus = StatusCode.锁定;
        //                        try
        //                        {
        //                            var rows = SysUserRepo.Update(sysUser);
        //                            response.IsSuccess = rows > 0;
        //                            response.OperationDesc = "锁定成功！";
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            WriteLogException(ex);
        //                            response.IsSuccess = false;
        //                            response.OperationDesc = "锁定失败！";
        //                        }
        //                    }
        //                    else if (sysUser.UserStatus == StatusCode.锁定)
        //                    {
        //                        sysUser.UserStatus = StatusCode.正常;
        //                        try
        //                        {
        //                            var rows = SysUserRepo.Update(sysUser);
        //                            response.IsSuccess = rows > 0;
        //                            response.OperationDesc = "启用成功！";
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            WriteLogException(ex);
        //                            response.IsSuccess = false;
        //                            response.OperationDesc = "启用失败！";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// 根据用户名获取用户信息
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <returns>用户信息</returns>
        //public ResponseBase<SysUserDTO> GetSysUserByName(string userName)
        //{
        //    var response = new ResponseBase<SysUserDTO>();
        //    var model = SysUserRepo.Entities.FirstOrDefault(a => a.UserName == userName);
        //    if (model != null)
        //    {
        //        SysUserDTO sysUserDTO = new SysUserDTO();
        //        sysUserDTO.Inject(model);
        //        response.IsSuccess = true;
        //        response.Result = sysUserDTO;
        //        return response;
        //    }
        //    response.OperationDesc = response.IsSuccess ? "获取用户成功" : "获取用户失败";
        //    return response;
        //}

        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBase ChangePassword(Guid userId, string oldPass, string newPass)
        //{
        //    ResponseBase response = new ResponseBase();
        //    var user = SysUserRepo.Entities.Where(a => a.SysUserID == userId).FirstOrDefault();
        //    if (user == null)
        //    {
        //        response.OperationDesc = "用户不存在";
        //        response.IsSuccess = false;
        //        return response;
        //    }
        //    if (user.UserPassword == MD5Encryption.Encrypt(oldPass))
        //        user.UserPassword = MD5Encryption.Encrypt(newPass);
        //    else
        //    {
        //        response.OperationDesc = "当前密码不正确";
        //        response.IsSuccess = false;
        //        return response;
        //    }
        //    try
        //    {
        //        var row = SysUserRepo.Update(user);
        //        response.IsSuccess = row > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogException(ex);
        //        response.OperationDesc = "修改出错";
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// 重置密码
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBase ChangePassword(Guid userId, string newPass)
        //{
        //    ResponseBase response = new ResponseBase();
        //    var user = SysUserRepo.Entities.Where(a => a.SysUserID == userId).FirstOrDefault();
        //    if (user == null)
        //    {
        //        response.OperationDesc = "用户不存在";
        //        response.IsSuccess = false;
        //        return response;
        //    }
        //    try
        //    {
        //        user.UserPassword = MD5Encryption.Encrypt(newPass);
        //        var row = SysUserRepo.Update(user);
        //        response.IsSuccess = row > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLogException(ex);
        //        response.OperationDesc = "修改出错:" + ex.Message;
        //    }
        //    return response;
        //}

        //public bool IsExisUser(SysUserDTO user)
        //{
        //    return SysUserRepo.Entities.Any(u => u.SysUserID != user.SysUserID && u.UserName == user.UserName);
        //} 

        #endregion

        #region 菜单

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        public List<SysMenu> GetMenus(Guid? parentId, int? pageNumber, int? pageIndex)
        {
            var sql = BuildSqlGeMenus(parentId, pageNumber, pageIndex);
            // 执行数据库查询
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql.Item1, sql.Item2);
            var list = new List<SysMenu>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new SysMenu
                    {
                        MenuOrder = reader.GetInt32("MenuOrder"),
                        MenuName = reader.GetString("MenuName"),
                        MenuUrl = reader.GetString("MenuUrl"),
                        SysMenuId = reader.GetGuid("SysMenuId"),
                        ParentId = reader.GetValueOrDefault<Guid?>("ParentId", null),
                    });
                }
            }

            return list;
        }

        private Tuple<string, MySqlParameter[]> BuildSqlGeMenus(Guid? parentId, int? pageNumber, int? pageIndex)
        {
            var parameters = new List<MySqlParameter>();
            pageIndex = pageIndex ?? 0;
            pageNumber = pageNumber ?? int.MaxValue;
            string sql = "select SysMenuId,MenuName,MenuOrder,MenuUrl,ParentId from wlyc_mall.sysmenu where IsDeleted<> true ";
            if (parentId.HasValue)
            {
                sql += " and ParentId = @ParentId";
                parameters.Add(new MySqlParameter("@ParentId", parentId.Value));
            }

            sql += " order by MenuOrder ";
            sql += " limit " + pageIndex * pageNumber + "," + pageNumber;

            return Tuple.Create(sql, parameters.ToArray());
        }


        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        public int CreateSysMenu(SysMenuDto model)
        {
            var sysMenu = new SysMenu();
            sysMenu.Inject(model); 
            return SysMenuRepo.Create(sysMenu);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>是否成功</returns>
        public int DeleteSysMenu(IList<Guid> ids)
        {
            try
            {
                var list = SysMenuRepo.Entities.Where(m => ids.Any(id => id == m.SysMenuId)).ToList();
                foreach (var item in list)
                {
                    BusinessRepositoryContext.RegisterDelete<SysMenu, Guid>(item);
                }
                return BusinessRepositoryContext.Commit();
            }
            catch (Exception ex)
            {
                BusinessRepositoryContext.Rollback();
                throw ex;
            }

        }


        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        public int EditSysMenu(SysMenuDto model)
        {
            var response = new ResponseBase();
            SysMenu sysMenu = SysMenuRepo.GetByKey(model.SysMenuId);
            sysMenu.MenuName = model.MenuName;
            sysMenu.MenuOrder = model.MenuOrder;
            sysMenu.MenuStyleName = model.MenuStyleName;
            sysMenu.MenuUrl = model.MenuUrl;
            try
            {
                return SysMenuRepo.Update(sysMenu);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单信息</returns>
        public SysMenuDto GetSysMenuByID(Guid id)
        {
            var model = SysMenuRepo.GetByKey(id);
            if (model != null)
            {
                var dto = new SysMenuDto();
                dto.Inject(model);
                if (model.ParentId.HasValue)
                {
                    var menu = SysMenuRepo.GetByKey(model.ParentId.Value);
                    dto.ParentMenuName = menu.MenuName;
                }
                return dto;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="search">菜单查询参数</param>
        /// <returns>菜单列表</returns>
        public List<SysMenuDto> SearchSysMenuList(SysMenuSearchDto search)
        {
            var sysMenus = SysMenuRepo.Entities;
            var sysMenuList = from sysMenu in sysMenus
                              join parentMenu in sysMenus
                              on sysMenu.ParentId equals parentMenu.SysMenuId
                              into menuJoined
                              from parentMenu in menuJoined.DefaultIfEmpty()
                              where (string.IsNullOrEmpty(search.MenuName) || sysMenu.MenuName.Contains(search.MenuName))
                              where (string.IsNullOrEmpty(search.ParentMenuName) || parentMenu.MenuName.Contains(search.ParentMenuName))
                              orderby sysMenu.MenuOrder, sysMenu.CreatedTime descending
                              select new { sysMenu, parentMenu };
            return sysMenuList.Skip((search.Page - 1) * search.Rows).Take(search.Rows)
                         .ToList().Select(c => new SysMenuDto
                         {
                             SysMenuId = c.sysMenu.SysMenuId,
                             MenuName = c.sysMenu.MenuName,
                             MenuStyleName = c.sysMenu.MenuStyleName,
                             MenuOrder = c.sysMenu.MenuOrder,
                             MenuUrl = c.sysMenu.MenuUrl,
                             ParentMenuName = c.parentMenu == null ? string.Empty : c.parentMenu.MenuName
                         }).ToList();
        }

        /// <summary>
        /// 根据菜单ID获取菜单树
        /// </summary>
        /// <returns>用户菜单列表</returns>
        public List<SysMenuDto> GetUserMenuByIdList(IList<string> idList)
        {
            var menuIdList = new List<Guid>();
            foreach (var item in idList)
            {
                menuIdList.Add(Guid.Parse(item));
            }
            var model = new List<SysMenuDto>();
            var sysMenus = SysMenuRepo.Entities;
            var menuList = (from m in sysMenus
                            join p in sysMenus
                            on m.ParentId equals p.SysMenuId
                            from c in menuIdList
                            where m.SysMenuId == c 
                            orderby p.MenuOrder ascending
                            select new SysMenuDto
                            {
                                id = m.SysMenuId.ToString(),
                                pId = m.ParentId == null ? "" : m.ParentId.Value.ToString(),
                                name = m.MenuName,
                                icon = string.Empty,
                                SysMenuId = m.SysMenuId,
                                MenuName = m.MenuName,
                                MenuUrl = m.MenuUrl,
                                MenuStyleName = m.MenuStyleName,
                                MenuOrder = m.MenuOrder,
                                ParentId = m.ParentId,
                                ParentMenuName = p.MenuName,
                                ParentMenuOrder = p.MenuOrder
                            }).ToList();
            var group = menuList.OrderBy(h => h.ParentMenuOrder).GroupBy(p => p.ParentId).ToList();
            foreach (var item in group)
            {
                var m = new SysMenuDto();
                var parentMenu = SysMenuRepo.Entities.FirstOrDefault(s => s.SysMenuId == item.Key && s.ParentId == null);
                m.Inject(parentMenu);
                m.id = parentMenu.SysMenuId.ToString();
                m.name = parentMenu.MenuName;
                m.icon = string.Empty;
                m.ChildMenuList = item.Select(p => p).OrderBy(o => o.MenuOrder).ToList();
                model.Add(m);
            }
            return model;
        }
        /// <summary>
        /// 菜单是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsExisMenu(SysMenuDto model)
        {
            return SysMenuRepo.Entities.Any(m =>  m.SysMenuId != model.SysMenuId && m.MenuName == model.MenuName);
        }

        #endregion

        #region 角色
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="search">角色查询参数</param>
        /// <returns>角色列表</returns>
        public List<SysRoleDto> SearchSysRoleList(SysRoleSearchDto search)
        {
            var sysRoleList = from sysRole in SysRoleRepo.Entities
                              where (string.IsNullOrEmpty(search.RoleName) || sysRole.RoleName.Contains(search.RoleName))
                              orderby sysRole.CreatedTime descending
                              select sysRole;


            return sysRoleList.Skip((search.Page - 1) * search.Rows).Take(search.Rows)
                         .ToList().Select(c => new SysRoleDto
                         {
                             SysRoleId = c.SysRoleId,
                             IsSystemRole = c.IsSystemRole,
                             RoleDesc = c.RoleDesc,
                             RoleName = c.RoleName
                         }).ToList();
        }

        /// <summary>
        /// 根据角色ID获取角色信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色信息</returns>
        public SysRoleDto GetSysRoleByID(Guid id)
        {
            var response = new ResponseBase<SysRoleDto>();
            var model = SysRoleRepo.GetByKey(id);
            if (model != null)
            {
                SysRoleDto sysRoleDTO = new SysRoleDto();
                sysRoleDTO.Inject(model);
                return sysRoleDTO;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns>是否成功</returns>
        public int EditSysRole(SysRoleDto dto)
        {
            if (dto != null)
            {
                SysRole role = SysRoleRepo.Entities.FirstOrDefault(r => r.SysRoleId == dto.SysRoleId);
                var users = SysUserRepo.Entities;
                //Business 中去移除缓存
                //if (users != null && users.Count() > 0)
                //{
                //    foreach (var user in users)
                //    {
                //        CacheManager.Remove(user.SysUserId.ToString() + "_CurrentUser");
                //        CacheManager.Remove(user.SysUserId.ToString() + "_CurrentUserMenu");
                //    }
                //}
                try
                {
                    if (null != role)
                    {
                        role.Inject(dto);
                        //更新角色关联的用户信息
                        if (!string.IsNullOrEmpty(dto.SysUserIDs))
                        {
                            IList<Guid> userIDs = dto.SysUserIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll<Guid>(i => Guid.Parse(i));
                            var currentUserRoleRefList = this.UserRoleRefRepo.Entities.Where(p => p.SysRoleId == role.SysRoleId).ToList();
                            foreach (var item in currentUserRoleRefList)
                            {
                                this.BusinessRepositoryContext.RegisterPhysicalDelete<UserRoleRef, Guid>(item);
                            }
                            foreach (var item in userIDs)
                            {
                                UserRoleRef refModel = new UserRoleRef();
                                refModel.SysRoleId = role.SysRoleId;
                                refModel.SysUserId = item;
                                refModel.RefId = Guid.Empty;
                                this.BusinessRepositoryContext.RegisterCreate<UserRoleRef, Guid>(refModel);
                            }
                        }
                        this.BusinessRepositoryContext.RegisterCreate<SysRole, Guid>(role);
                        return this.BusinessRepositoryContext.Commit();

                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    this.BusinessRepositoryContext.Rollback();
                    throw ex;
                }

            }
            return 0;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns>是否成功</returns>
        public int CreateSysRole(SysRoleDto dto)
        {
            var response = new ResponseBase();
            if (dto != null)
            {
                SysRole role = new SysRole();
                role.Inject(dto);
                role.SysRoleId = Guid.NewGuid();

                if (!string.IsNullOrEmpty(dto.SysUserIDs))
                {
                    IList<Guid> userIDs = dto.SysUserIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll<Guid>(i => Guid.Parse(i));
                    var currentUserRoleRefList = this.UserRoleRefRepo.Entities.Where(p => p.SysRoleId == role.SysRoleId).ToList();
                    foreach (var item in currentUserRoleRefList)
                    {
                        this.BusinessRepositoryContext.RegisterPhysicalDelete<UserRoleRef, Guid>(item);
                    }
                    foreach (var item in userIDs)
                    {
                        UserRoleRef refModel = new UserRoleRef();
                        refModel.SysRoleId = role.SysRoleId;
                        refModel.SysUserId = item;
                        refModel.RefId = Guid.Empty;
                        this.BusinessRepositoryContext.RegisterCreate<UserRoleRef, Guid>(refModel);
                    }
                }

                role.IsSystemRole = false;
                this.BusinessRepositoryContext.RegisterCreate<SysRole, Guid>(role);
                try
                {
                    return this.BusinessRepositoryContext.Commit();
                }
                catch (Exception ex)
                {
                    this.BusinessRepositoryContext.Rollback();
                    throw ex;
                }
            }
            return 0;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids">角色ID集合</param>
        /// <returns>是否成功</returns>
        public int DeleteSysRole(IList<Guid> ids)
        {
            //缓存清楚放到Business
            //var users = SysUserRepo.Entities;
            //if (users != null && users.Count() > 0)
            //{
            //    foreach (var user in users)
            //    {
            //        CacheManager.Remove(user.SysUserId.ToString() + "_CurrentUser");
            //        CacheManager.Remove(user.SysUserId.ToString() + "_CurrentUserMenu");
            //    }
            //}
            try
            {
                var list = SysRoleRepo.Entities.Where(m => ids.Any(id => id == m.SysRoleId)).ToList();

                //foreach (var item in list)
                //{
                //    int sysUser = SysUserRepo.Entities.Where(a => a.RoleIDs.Contains(item.SysRoleId.ToString())).Count();
                //    if (sysUser <= 0 && item.IsSystemRole == false)
                //    {
                //        BusinessRepositoryContext.RegisterPhysicalDelete<SysRole, Guid>(item);
                //        var rows = BusinessRepositoryContext.Commit();
                //        //对于mysql需要重新写sql
                //        //UpdateRelateUser();
                //        response.IsSuccess = rows > 0;
                //    }
                //    else
                //    {
                //        response.IsSuccess = false;
                //        SynchroDesc = "该角色下有用户或是内置管理员！";
                //    }
                //}
                return 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool IsExisRole(SysRoleDto role)
        {
            return SysRoleRepo.Entities.Any(u => u.SysRoleId != role.SysRoleId && u.RoleName == role.RoleName);
        }
        #endregion

        #region 字典管理

        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="search">数据字典查询参数</param>
        /// <returns>数据字典列表</returns>
        public List<SysDictionaryDto> SearchSysDictionaryList(SysDictionarySearchDto search, out int totalRecordCount)
        {
            var result = new List<SysDictionaryDto>();
            var dictionary = from dic in SysDictionaryRepo.Entities
                             orderby dic.CreatedTime
                             select dic;
            if (!string.IsNullOrEmpty(search.DictionaryName))
            {
                dictionary = dictionary.Where(c => c.DictionaryName.Contains(search.DictionaryName)).OrderBy(c => c.DictionaryCode);
            }

            if (search.DictionaryParent != 0)
            {
                dictionary = dictionary.Where(c => c.DictionaryParent == search.DictionaryParent).OrderBy(c => c.DictionaryCode);
            }

            totalRecordCount = dictionary.Count();

            result = dictionary.Skip((search.Page - 1) * search.Rows).Take(search.Rows).ToList().Select(
                d => new SysDictionaryDto
                {
                    DictionaryId = d.DictionaryId,
                    DictionaryName = d.DictionaryName,
                    DictionaryCode = d.DictionaryCode,
                    Canceled = d.Canceled,
                    DictionaryParent = d.DictionaryParent
                }).ToList();

            return result;
        }

        /// <summary>
        /// 根据字典父级编码获取数据字典子列表
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="category">作用类别</param>
        /// <returns>数据字典列表</returns>
        public List<SysDictionaryDto> GetSysDicChildrenByParent(DictionaryParentCode dictionaryParent)
        {
            var result = new List<SysDictionaryDto>();
            var dictionary = (from dic in SysDictionaryRepo.Entities
                              where dic.DictionaryParent == dictionaryParent &&
                              dic.Canceled == false
                              orderby dic.DictionaryCode ascending, dic.CreatedTime ascending
                              select dic);
            result = dictionary.ToList().Select(
               d => new SysDictionaryDto
               {
                   DictionaryId = d.DictionaryId,
                   DictionaryName = d.DictionaryName,
                   DictionaryCode = d.DictionaryCode
               }).ToList();

            return result;
        }


        /// <summary>
        /// 根据字典编码获取数据字典
        /// </summary>
        /// <param name="dictionaryCode">字典编码</param>
        /// <returns>数据字典列表</returns>
        public SysDictionaryDto GetSysDictionaryByCode(string dictionaryCode)
        {
            var result = new SysDictionaryDto();
            var model = SysDictionaryRepo.Entities.FirstOrDefault(s => s.DictionaryCode == dictionaryCode);
            if (model != null)
            {
                SysDictionaryDto dto = new SysDictionaryDto();
                dto.Inject(model);
                return dto;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 编辑数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        public int EditSysDictionary(SysDictionaryDto model)
        {
            SysDictionary sysDictionary = SysDictionaryRepo.GetByKey(model.DictionaryId);
            try
            {
                if (null != sysDictionary)
                {
                    sysDictionary.Inject(model);
                    return SysDictionaryRepo.Update(sysDictionary);

                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        public int CreateSysDictionary(SysDictionaryDto model)
        {
            SysDictionary sysDictionary = new SysDictionary();
            sysDictionary.Inject(model);
            try
            {
                return SysDictionaryRepo.Create(sysDictionary);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="ids">数据字典ID集合</param>
        /// <returns>是否成功</returns>
        public int DeleteSysDictionary(IList<Guid> ids)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var list = SysDictionaryRepo.Entities.Where(m => ids.Any(id => id == m.DictionaryId)).ToList();
                foreach (var item in list)
                {
                    BusinessRepositoryContext.RegisterDelete<SysDictionary, Guid>(item);
                }

                return BusinessRepositoryContext.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public bool IsExisDictionary(SysDictionaryDto dictionary)
        {
            return SysDictionaryRepo.Entities.Any(u => u.DictionaryId != dictionary.DictionaryId &&
                                                u.DictionaryCode == dictionary.DictionaryCode &&
                                                u.DictionaryParent == dictionary.DictionaryParent);
        }

        #endregion

        #region 用户角色关系
        /// <summary>
        /// 根据角色ID 获取用户列表
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        private List<SysUserDto> GetUserListByRoleID(Guid RoleID)
        {
            List<SysUserDto> userList = new List<SysUserDto>();
            var refList = this.UserRoleRefRepo.Entities.Where(p => p.SysRoleId == RoleID).ToList();
            foreach (var item in refList)
            {
                var userItem = this.SysUserRepo.GetByKey(item.SysUserId);
                if (userItem != null)
                {
                    userList.Add(new SysUserDto().Inject(userItem));
                }
            }
            return userList;
        }
        /// <summary>
        /// 根据用户ID 获取角色列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private List<SysRoleDto> GetRoleListByUserID(Guid UserID)
        {
            List<SysRoleDto> roleList = new List<SysRoleDto>();
            var refList = this.UserRoleRefRepo.Entities.Where(p => p.SysUserId == UserID).ToList();
            foreach (var item in refList)
            {
                var roleItem = this.SysRoleRepo.GetByKey(item.SysRoleId);
                if (roleItem != null)
                {
                    roleList.Add(new SysRoleDto().Inject(roleItem));
                }
            }
            return roleList;
        }
        #endregion

        #region Action
        public List<SysActionDto> GetActionListByRoleIds(List<SysRoleDto> roleList)
        {
            List<SysActionDto> actionList = new List<SysActionDto>();
            foreach (var item in roleList)
            {
                if (!string.IsNullOrEmpty(item.SysActionIDs))
                {
                    var actionIDs = item.SysActionIDs.Split(';').ToList();
                    foreach (var actionItem in actionIDs)
                    {
                        var actionModel = this.SysActionRepo.GetByKey(Guid.Parse(actionItem));
                        if (actionModel != null)
                        {
                            actionList.Add(new SysActionDto().Inject(actionModel));
                        }
                    }
                }
            }
            return actionList;
        }
        #endregion

    }
}

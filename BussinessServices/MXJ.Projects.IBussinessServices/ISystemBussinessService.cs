using System;
using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Models.DataTransferObjects;

namespace MXJ.Projects.IBussinessServices
{
    /// <summary>
    ///     系统管理业务层
    /// </summary>
    public interface ISystemBussinessService : IBaseBussinessService
    {
        #region 菜单


        /// <summary>
        ///     创建一个新的菜单
        /// </summary>
        /// <param name="sysMenuDto"></param>
        /// <returns></returns>
        ResponseBase CreateSysMenu(SysMenuDto sysMenuDto);

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        ResponseBase<List<SysMenuDto>> GetMenus(Guid? parentId, int? pageNumber, int? pageIndex);

        /// <summary>
        /// 获取当前用户菜单树
        /// </summary>
        /// <returns>用户菜单列表</returns>
        ResponseBase<List<SysMenuDto>> GetCurrentUserMenu(CurrentUserDto currentUserDto);

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        SearchResponseBase<List<SysMenuDto>> SearchSysMenuList(SysMenuSearchDto search);
    

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>是否成功</returns>
        ResponseBase DeleteSysMenu(IList<Guid> ids);


        /// <summary>
        /// 编辑菜单序号
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        ResponseBase EditSysMenuOrder(SysMenuDto model);
 

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        ResponseBase EditSysMenu(SysMenuDto model);
 

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单信息</returns>
        ResponseBase<SysMenuDto> GetSysMenuByID(Guid id);
      

        #endregion

        #region 登录/登出
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">登录条件</param>
        /// <returns>是否成功</returns>
        ResponseBase<CurrentUserDto> Login(LoginDto model);
      
                        

        #endregion

        /// <summary>
        /// 获取操作日志列表
        /// </summary>
        /// <param name="search">操作日志查询参数</param>
        /// <returns>操作日志列表</returns>
        SearchResponseBase<List<SysOperationLogDto>> SearchSysOperationLogList(SysOperationLogSearchDTO search);

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">操作日志</param>
        /// <returns>是否成功</returns>
        ResponseBase CreateSysOperationLog(SysOperationLogDto model);

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        SearchResponseBase<SysOperationLogDto> GetSysOperationLog(Guid logID);

        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="search">数据字典查询参数</param>
        /// <returns>数据字典列表</returns>
        SearchResponseBase<List<SysDictionaryDto>> SearchSysDictionaryList(SysDictionarySearchDto search);

        /// <summary>
        /// 根据字典父级编码获取数据字典子列表
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="category">作用类别</param>
        /// <returns>数据字典列表</returns>
        SearchResponseBase<List<SysDictionaryDto>> GetSysDicChildrenByParent(DictionaryParentCode dictionaryParent);

        /// <summary>
        /// 根据字典编码获取数据字典
        /// </summary>
        /// <param name="dictionaryCode">字典编码</param>
        /// <returns>数据字典列表</returns>
        ResponseBase<SysDictionaryDto> GetSysDictionaryByCode(string dictionaryCode);


        /// <summary>
        /// 编辑数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        ResponseBase EditSysDictionary(SysDictionaryDto model);

        /// <summary>
        /// 添加数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        ResponseBase CreateSysDictionary(SysDictionaryDto model);

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="ids">数据字典ID集合</param>
        /// <returns>是否成功</returns>
        ResponseBase DeleteSysDictionary(IList<Guid> ids);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.IRepositoryServices
{
    /// <summary>
    /// 系统管理持久化层
    /// </summary>
    public interface ISystemRepositoryService : IBaseRepositoryService
    {
        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="parentId">父类Id</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        List<SysMenu> GetMenus(Guid? parentId, int? pageNumber, int? pageIndex);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        int UpdatePassword(Guid sysUserId, string newPassword);

        /// <summary>
        /// 根据账号和密码获取登录信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        List<SysUserDto> ValidateSystemUserForLogin(string userName, string userPassword);

        /// <summary>
        /// 查询菜单
        /// </summary> 
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        List<SysUserDto> GetSystemUsers(int? pageNumber, int? pageIndex);

        #region 菜单

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        int CreateSysMenu(SysMenuDto model);


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>是否成功</returns>
        int DeleteSysMenu(IList<Guid> ids);

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model">菜单信息</param>
        /// <returns>是否成功</returns>
        int EditSysMenu(SysMenuDto model);


        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单信息</returns>
        SysMenuDto GetSysMenuByID(Guid id);


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="search">菜单查询参数</param>
        /// <returns>菜单列表</returns>
        List<SysMenuDto> SearchSysMenuList(SysMenuSearchDto search);

        /// <summary>
        /// 根据菜单ID获取菜单树
        /// </summary>
        /// <returns>用户菜单列表</returns>
        List<SysMenuDto> GetUserMenuByIdList(IList<string> menuidList);

        /// <summary>
        /// 菜单是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool IsExisMenu(SysMenuDto model);

        #endregion

        #region 角色
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="search">角色查询参数</param>
        /// <returns>角色列表</returns>
        List<SysRoleDto> SearchSysRoleList(SysRoleSearchDto search);
    

        /// <summary>
        /// 根据角色ID获取角色信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色信息</returns>
        SysRoleDto GetSysRoleByID(Guid id);
    

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns>是否成功</returns>
        int EditSysRole(SysRoleDto dto);
    

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns>是否成功</returns>
        int CreateSysRole(SysRoleDto dto);
      

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids">角色ID集合</param>
        /// <returns>是否成功</returns>
        int DeleteSysRole(IList<Guid> ids);
       

        bool IsExisRole(SysRoleDto role);

        #endregion

        #region 用户
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        SysUserDto GetUserByName(string userName);
       
        #endregion

        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="search">数据字典查询参数</param>
        /// <returns>数据字典列表</returns>
        List<SysDictionaryDto> SearchSysDictionaryList(SysDictionarySearchDto search, out int totalRecordCount);

        /// <summary>
        /// 根据字典父级编码获取数据字典子列表
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="category">作用类别</param>
        /// <returns>数据字典列表</returns>
        List<SysDictionaryDto> GetSysDicChildrenByParent(DictionaryParentCode dictionaryParent);

        /// <summary>
        /// 根据字典编码获取数据字典
        /// </summary>
        /// <param name="dictionaryCode">字典编码</param>
        /// <returns>数据字典列表</returns>
        SysDictionaryDto GetSysDictionaryByCode(string dictionaryCode);

        /// <summary>
        /// 编辑数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        int EditSysDictionary(SysDictionaryDto model);

        /// <summary>
        /// 添加数据字典
        /// </summary>
        /// <param name="model">数据字典信息</param>
        /// <returns>是否成功</returns>
        int CreateSysDictionary(SysDictionaryDto model);

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="ids">数据字典ID集合</param>
        /// <returns>是否成功</returns>
        int DeleteSysDictionary(IList<Guid> ids);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        bool IsExisDictionary(SysDictionaryDto dictionary);
    }
}

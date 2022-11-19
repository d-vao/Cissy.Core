using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Cissy.Database;


namespace Cissy.Security
{
    public interface IAuthorityService
    {
        /// <summary>
        /// 获取用户的权限列表，没有合并权限的数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        IEnumerable<IAuthority> GetUserAuthorities(string UserId);
        /// <summary>
        /// 获取范围内的所有角色
        /// </summary>
        /// <param name="Scope"></param>
        /// <returns></returns>
        IEnumerable<IRole> GetAllRolesInScope(string Scope);
        /// <summary>
        /// 获取用户的所有角色信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        IEnumerable<IRole> GetUserRoles(string UserId);
        /// <summary>
        /// 根据多个角色Id，获取角色信息
        /// </summary>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        IEnumerable<IRole> GetRolesInIds(IEnumerable<long> RoleIds);
    }
}

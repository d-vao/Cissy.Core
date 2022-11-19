using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Security
{
    public interface IRole
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 权限范围
        /// </summary>
        string Scope { get; set; }
    }
}

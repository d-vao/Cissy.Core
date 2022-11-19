using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Security
{
    public interface IAuthority : IModel
    {
        /// <summary>
        /// 权限点
        /// </summary>
        string Droit { get; set; }
        /// <summary>
        /// 权限类型Id
        /// </summary>
        int PermitId { get; set; }
        /// <summary>
        /// 权限范围
        /// </summary>
        string Scope { get; set; }
        /// <summary>
        ///合并的权限值
        /// </summary>
        int Power { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Security
{
    public class CissyAuthority : IAuthority
    {
        /// <summary>
        /// 权限点
        /// </summary>
        public string Droit { get; set; }
        /// <summary>
        /// 权限类型Id
        /// </summary>
        public int PermitId { get; set; }
        /// <summary>
        /// 权限范围
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        ///合并的权限值
        /// </summary>
        public int Power { get; set; }
    }
}

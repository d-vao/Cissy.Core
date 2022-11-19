using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Cissy.Security
{
    [global::System.AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    [Serializable]
    [ComVisible(true)]
    public class PermitAttribute : EnumStringsAttribute
    {
        public string PermitGroup { get; set; }      
        public int Id { get; set; } = 0;
        public PermitAttribute(Type enumType, params string[] strings)
            : base(enumType, strings)
        {
            this.PermitGroup = "系统权限";
        }
    }
}

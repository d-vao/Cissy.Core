using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Authentication
{
    /// <summary>
    /// 护照
    /// </summary>
    public interface IPassport
    {
        string UserId { get; }
        string UserName { get; }
    }
}

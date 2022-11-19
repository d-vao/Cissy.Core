using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.PCApp
{
    /// <summary>
    /// PC应用的基础接口
    /// </summary>
    public interface IPCApp
    {
        Dictionary<string, IPCPlugin> Plugins { get; }
    }
}

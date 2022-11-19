using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Authentication;

namespace Cissy.PCApp
{
    /// <summary>
    /// 用户配置信息
    /// </summary>
    public class PCAppUserConfig : IModel
    {
        public CissyPassport Passport { get; set; }
        public PluginConfig[] Plugins { get; set; }
    }
}

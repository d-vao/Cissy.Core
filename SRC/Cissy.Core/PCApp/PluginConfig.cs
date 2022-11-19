using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.PCApp
{
    /// <summary>
    /// 插件配置信息
    /// </summary>
    public class PluginConfig : IModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public float Version { get; set; }
        /// <summary>
        /// 插件地址
        /// </summary>
        public string PluginUrl { get; set; }
        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Ext { get; set; }
    }
}

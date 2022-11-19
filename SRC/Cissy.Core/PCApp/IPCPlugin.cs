using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.PCApp
{
    /// <summary>
    /// PC应用插件
    /// </summary>
    public interface IPCPlugin
    {
        bool Load();
        void Run();
        void Stop();
        bool UnLoad();
    }
    /// <summary>
    /// 插件信息
    /// </summary>
    public class PCPluginInfo
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 插件昵称
        /// </summary>
        public string PluginNick { get; set; }
        /// <summary>
        /// 插件地址
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public float Version { get; set; }
    }
}

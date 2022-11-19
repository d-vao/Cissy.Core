using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Http;
using Cissy.Serialization;

namespace Cissy.PCApp
{
    /// <summary>
    /// 应用管理器
    /// </summary>
    public class ApplicationManager
    {
        static ApplicationManager _instance;
        public static ApplicationManager Instance
        {
            get
            {
                if (_instance.IsNull())
                    _instance = new ApplicationManager();
                return _instance;
            }
        }
        public Dictionary<string, IPCPlugin> Plugins { get; protected set; } = new Dictionary<string, IPCPlugin>();
        public string BootApiUri { get; set; }
        ApplicationManager()
        {

        }
        public void Init()
        {
        }
        //无身份加载云端基本信息
        public async Task<T> BootFromCloud<T>() where T : PCAppConfig
        {
            JsonBase<T> result = default;
            await Actor.Public.GetAsync(this.BootApiUri, msg =>
            {
                result = msg.JsonToModel<JsonBase<T>>();
            });
            return result?.data;
        }
        //有身份加载云端基本信息
        public async Task<T> StartupFromCloud<T>(string url, string token) where T : PCAppUserConfig
        {
            JsonBase<T> result = default;
            await Actor.Public.GetAsync($"{url}?token={token}", msg =>
            {
                result = msg.JsonToModel<JsonBase<T>>();
            });
            return result?.data;
        }
        public void StartPlugins(PCAppUserConfig config)
        {
            foreach (PluginConfig pc in config.Plugins)
            {
                RunPlugin(pc);
            }
        }
        void RunPlugin(PluginConfig config)
        {
            try
            {
                //1,寻找本地目录，是否有相应版本的插件文件
                //2，没有本地插件则下载
                //2，启动插件
            }
            catch { }
        }
    }
}

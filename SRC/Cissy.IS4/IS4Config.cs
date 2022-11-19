using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Cissy.Configuration;

namespace Cissy.IS4
{

    /// <summary>
    /// IS4配置
    /// </summary>
    public class IS4Config : IConfigModel
    {
        public string ConfigName { get { return "is4"; } }
        public List<ApiServiceConfig> ApiServices = new List<ApiServiceConfig>();
        public List<CorsConfig> Cors = new List<CorsConfig>();
        public List<ApiClientConfig> ApiClients = new List<ApiClientConfig>();
        public void InitConfig(IConfigurationSection section)
        {
            ApiServiceConfig t = new ApiServiceConfig();
            IConfigurationSection apiservices = section.GetSection(t.ConfigName);
            if (apiservices.IsNotNull())
            {
                foreach (IConfigurationSection apiservicesection in apiservices.GetChildren())
                {
                    ApiServiceConfig ApiServiceConfig = new ApiServiceConfig();
                    ApiServiceConfig.InitConfig(apiservicesection);
                    ApiServices.Add(ApiServiceConfig);
                }
            }
            CorsConfig t2 = new CorsConfig();
            IConfigurationSection cors = section.GetSection(t2.ConfigName);
            if (cors.IsNotNull())
            {
                foreach (IConfigurationSection coresection in cors.GetChildren())
                {
                    CorsConfig CorsConfig = new CorsConfig();
                    CorsConfig.InitConfig(coresection);
                    Cors.Add(CorsConfig);
                }
            }
            ApiClientConfig t3 = new ApiClientConfig();
            IConfigurationSection apiclients = section.GetSection(t3.ConfigName);
            if (apiclients.IsNotNull())
            {
                foreach (IConfigurationSection apiclientsection in apiclients.GetChildren())
                {
                    ApiClientConfig ApiClientConfig = new ApiClientConfig();
                    ApiClientConfig.InitConfig(apiclientsection);
                    ApiClients.Add(ApiClientConfig);
                }
            }
        }
    }
    /// <summary>
    /// Api服务配置
    /// </summary>
    public class ApiServiceConfig : IConfigModel
    {
        public string ConfigName { get { return "apiservices"; } }
        public string Scheme;
        public string Authority;
        public string Audience;
        public void InitConfig(IConfigurationSection section)
        {
            Scheme = section["scheme"];
            Authority = section["authority"];
            Audience = section["audience"];
        }
    }
    public class CorsConfig : IConfigModel
    {
        public string ConfigName { get { return "cors"; } }
        public string Name;
        public List<string> Origins = new List<string>();
        public void InitConfig(IConfigurationSection section)
        {
            Name = section["name"];
            IConfigurationSection OriginsSection = section.GetSection("origins");
            if (OriginsSection.IsNotNull())
            {
                Origins.AddRange(OriginsSection.GetChildren().Select(p => p.Value));
            }
        }
    }
    /// <summary>
    /// Api客户端配置
    /// </summary>
    public class ApiClientConfig : IConfigModel
    {
        public string ConfigName { get { return "apiclients"; } }
        public string Scheme;
        public string SignInScheme;
        public string Authority;
        public string ClientId;
        public string ClientSecret;
        public string ResponseType;
        public List<string> Scopes = new List<string>();
        public void InitConfig(IConfigurationSection section)
        {
            Scheme = section["scheme"];
            SignInScheme = section["signinscheme"];
            Authority = section["authority"];
            ClientId = section["clientid"];
            ClientSecret = section["clientsecret"];
            ResponseType = section["responsetype"];
            IConfigurationSection ScopesSection = section.GetSection("scopes");
            if (ScopesSection.IsNotNull())
            {
                Scopes.AddRange(ScopesSection.GetChildren().Select(p => p.Value));
            }
        }
    }
}

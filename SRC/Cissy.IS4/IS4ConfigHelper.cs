using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Cissy.Configuration;

namespace Cissy.IS4
{
    public static class IS4ConfigHelper
    {
        /// <summary>
        /// 注入IS4服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="WeiXinAuthHandler"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddIS4Config(this CissyConfigBuilder cissyConfigBuilder, Func<CissyConfigBuilder, bool> WeiXinAuthHandler = null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                bool ok = true;
                if (WeiXinAuthHandler != default(Func<CissyConfigBuilder, bool>))
                {
                    ok = WeiXinAuthHandler(cissyConfigBuilder);
                }
                IS4Config is4Config = cissyConfig.GetConfig<IS4Config>();
                if (is4Config.IsNotNull())
                {                   
                    if (ok)
                    {
                        var apiBuilder = cissyConfigBuilder.ServiceCollection.AddAuthentication("Bearer");
                        is4Config.ApiServices.ForEach(m =>
                        {
                            apiBuilder.AddJwtBearer(m.Scheme, options =>
                            {
                                options.Authority = m.Authority;
                                options.RequireHttpsMetadata = false;
                                options.Audience = m.Audience;
                            });
                        });
                    }
                    is4Config.Cors.ForEach(m =>
                    {
                        cissyConfigBuilder.ServiceCollection.AddCors(options =>
                        {
                            options.AddPolicy(m.Name, policy =>
                            {
                                policy.WithOrigins(m.Origins.ToArray())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                            });
                        });
                    });

                    if (is4Config.ApiClients.IsNotNullAndEmpty())
                    {
                        var builder = cissyConfigBuilder.ServiceCollection.AddAuthentication(options =>
                         {
                             options.DefaultScheme = "Cookies";
                             options.DefaultChallengeScheme = "oidc";
                         });
                        builder.AddCookie("Cookies");
                        is4Config.ApiClients.ForEach(m =>
                        {
                            builder.AddOpenIdConnect(m.Scheme, options =>
                            {
                                options.SignInScheme = m.SignInScheme;
                                options.Authority = m.Authority;
                                options.RequireHttpsMetadata = false;
                                options.ClientId = m.ClientId;
                                options.ClientSecret = m.ClientSecret;
                                options.ResponseType = m.ResponseType;
                                options.SaveTokens = true;
                                options.GetClaimsFromUserInfoEndpoint = true;
                                foreach (string scope in m.Scopes)
                                    options.Scope.Add(scope);
                                options.Scope.Add("offline_access");
                            });
                        });
                    }
                }
            }
            return cissyConfigBuilder;
        }
    }
}

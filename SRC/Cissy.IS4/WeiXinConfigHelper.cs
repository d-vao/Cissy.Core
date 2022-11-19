using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cissy.Configuration;
using Cissy.WeiXin;

namespace Cissy.IS4
{
    public static class WeiXinConfigHelper
    {
        enum WeiXinAppType
        {
            Web,
            Mp,
            WXA
        }
        /// <summary>
        /// 注入微信小程序Jwt认证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="AuthenticationScheme"></param>
        /// <returns></returns>
        public static bool UseWeiXinAppJwt<T>(this CissyConfigBuilder cissyConfigBuilder, string AuthenticationScheme) where T : WeiXinAppAuthBaseController
        {
            cissyConfigBuilder.UseWeiXin<T>(AuthenticationScheme, true, WeiXinAppType.WXA);
            return false;
        }
        /// <summary>
        /// 注入微信公众号Jwt认证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="AuthenticationScheme"></param>
        /// <returns></returns>
        public static bool UseWeiXinMpJwt<T>(this CissyConfigBuilder cissyConfigBuilder) where T : WeiXinMpJwtAuthBaseController
        {
            cissyConfigBuilder.UseWeiXin<T>(BaseAuthController.DefaultSchemeName, true, WeiXinAppType.Mp);
            return false;
        }
        /// <summary>
        /// 注入微信公众号Cookie认证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="AuthenticationScheme"></param>
        /// <returns></returns>
        public static bool UseWeiXinMpCookie<T>(this CissyConfigBuilder cissyConfigBuilder, string AuthenticationScheme) where T : WeiXinMpCookieAuthBaseController
        {
            cissyConfigBuilder.UseWeiXin<T>(AuthenticationScheme, false, WeiXinAppType.Mp);
            return true;
        }
        /// <summary>
        /// 注入微信WebJwt认证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="AuthenticationScheme"></param>
        /// <returns></returns>
        public static bool UseWeiXinWebJwt<T>(this CissyConfigBuilder cissyConfigBuilder) where T : WeiXinWebJwtAuthBaseController
        {
            cissyConfigBuilder.UseWeiXin<T>(BaseAuthController.DefaultSchemeName, true, WeiXinAppType.Web);
            return false;
        }
        /// <summary>
        /// 注入微信WebCookie认证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="AuthenticationScheme"></param>
        /// <returns></returns>
        public static bool UseWeiXinWebCookie<T>(this CissyConfigBuilder cissyConfigBuilder, string AuthenticationScheme) where T : WeiXinWebCookieAuthBaseController
        {
            cissyConfigBuilder.UseWeiXin<T>(AuthenticationScheme, false, WeiXinAppType.Web);
            return true;
        }
        static void UseWeiXin<T>(this CissyConfigBuilder cissyConfigBuilder, string AuthenticationScheme, bool isJwt, WeiXinAppType appType) where T : IWeiXinAuthController
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                WeiXinMpConfig wxConfig = cissyConfig.GetConfig<WeiXinMpConfig>();
                if (wxConfig.IsNotNull())
                {
                    string controllerName = typeof(T).Name.ToLower();
                    string c = "controller";
                    if (controllerName.EndsWith(c))
                        controllerName = controllerName.Substring(0, controllerName.Length - c.Length);
                    cissyConfigBuilder.ServiceCollection.AddAuthentication(AuthenticationScheme)
                        .AddCookie(AuthenticationScheme, options =>
                        {
                            if (appType == WeiXinAppType.WXA)
                            {
                                options.AccessDeniedPath = Actor.Public.GetWXALoginPath(controllerName, AuthenticationScheme);
                                options.LoginPath = Actor.Public.GetWXALoginPath(controllerName, AuthenticationScheme);
                            }
                            if (appType == WeiXinAppType.Mp)
                            {
                                options.AccessDeniedPath = Actor.Public.GetMpLoginPath(controllerName, AuthenticationScheme);
                                options.LoginPath = Actor.Public.GetMpLoginPath(controllerName, AuthenticationScheme);
                            }
                            else if (appType == WeiXinAppType.Web)
                            {
                                options.AccessDeniedPath = Actor.Public.GetWebLoginPath(controllerName, AuthenticationScheme);
                                options.LoginPath = Actor.Public.GetWebLoginPath(controllerName, AuthenticationScheme);

                            }
                            options.Cookie.HttpOnly = true;
                        });
                    if (isJwt)
                    {
                        cissyConfigBuilder.ServiceCollection.AddAuthentication(options =>
                        {
                            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                            //options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                            //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(x =>
                        {
                            x.RequireHttpsMetadata = false;
                            x.SaveToken = true;
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(wxConfig.JwtSecret)),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                        });
                    }
                }
            }
        }
    }
}

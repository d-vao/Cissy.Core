using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Cissy.Configuration;
using Cissy.Caching;
using Cissy.Authentication;
using Cissy.Payment;
using Cissy.Redis;
using Cissy.Elasticsearch;
using Cissy.Kafka;
using Cissy.AOS;
using Cissy.Database;
using Cissy.WeiXin;
using Cissy.Caching.Redis;
using Cissy.RateLimit;
using Cissy.Http;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;

namespace Cissy
{
    public abstract class BlazorServerStartup
    {
        public enum CissyConfigSourceKind
        {
            /// <summary>
            /// 本地Cissy.Json配置文件
            /// </summary>
            Local,
            /// <summary>
            /// 远程配置
            /// </summary>
            Remoting
        }
        public class CissyConfigSource
        {
            /// <summary>
            /// 配置源类型
            /// </summary>
            public CissyConfigSourceKind ConfigSource { get; set; } = CissyConfigSourceKind.Local;
            /// <summary>
            /// 配置源服务器地址
            /// </summary>
            public string CissyConfigServer { get; set; }
            /// <summary>
            /// 配置应用名称
            /// </summary>
            public string AppName { get; set; }
            /// <summary>
            /// 远程配置密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 版本号
            /// </summary>
            public string Version { get; set; } = "1.0";
            /// <summary>
            /// 启用MemoryCache
            /// </summary>
            public bool StartMemoryCache { get; set; } = true;
            /// <summary>
            /// 启用弱引用缓存
            /// </summary>
            public bool StartWeakCache { get; set; }
            /// <summary>
            /// 启用支付服务
            /// </summary>
            public bool StartPayment { get; set; }
            /// <summary>
            /// 启用Elastic Search
            /// </summary>
            public bool StartElasticsearch { get; set; }
            /// <summary>
            /// 启用Redist
            /// </summary>
            public bool StartRedis { get; set; }
            /// <summary>
            /// 启用Swagger
            /// </summary>
            public bool StartSwagger { get; set; }
            /// <summary>
            /// 启用Kafka
            /// </summary>
            public bool StartKafka { get; set; }
            /// <summary>
            /// 启用阿里OSS服务
            /// </summary>
            public bool StartAliOSS { get; set; }
            /// <summary>
            /// 启用Http代理服务
            /// </summary>
            public bool StartHttpProxy { get; set; }
            /// <summary>
            /// 启用微信Api并提供微信接口调用缓存接口
            /// </summary>
            public Func<CissyConfigBuilder, ICache> StartWeixinApi { get; set; }
            /// <summary>
            /// 启用微信限流机制并提供规则导入配置
            /// </summary>
            public Func<CissyConfigBuilder, IRateLimitLoader> StartRateLimit { get; set; }
            /// <summary>
            /// 启用Cissy认证并提供认证选项
            /// </summary>
            public Func<CissyAuthenticationOption> StartAuthentication { get; set; }

            /// <summary>
            /// 启用Https跳转
            /// </summary>
            public bool HttpsRedirection { get; set; } = true;
            /// <summary>
            /// 启用静态文件
            /// </summary>
            public bool StartStaticFiles { get; set; } = true;
            /// <summary>
            /// 异常处理路径
            /// </summary>
            public string ExceptionHandlerPath { get; set; }

        }
        static object _program { get; set; }
        public CissyConfigSource _cissyConfigSource { get; set; } = new CissyConfigSource();
        public IConfiguration _configuration { get; }
        public IWebHostEnvironment _hostingEnvironment { get; }
        public BlazorServerStartup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public static void Start<T>(string[] args) where T : BlazorServerStartup
        {
            var pg = new Cissy.BlazorServerProgram<T>();
            pg.Start(args);
            _program = pg;
        }
        public abstract void _InitCissyConfig(CissyConfigSource CissyConfigSource);
        public abstract void _ConfigureCissyDatabaseServices(CissyConfigBuilder CissyConfigBuilder);
        public abstract void _ConfigureAspServices(CissyConfigBuilder CissyConfigBuilder);
        public abstract void _ConfigureAsp(IApplicationBuilder app, IWebHostEnvironment env);
        public abstract void _RegisterDTOMap(IMapperConfigurationExpression register);
        public abstract void _ConfigureServices(IServiceCollection services);

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(sp.GetService<NavigationManager>().BaseUri)
            });
            _ConfigureServices(services);
            _InitCissyConfig(_cissyConfigSource);
            var CissyConfigBuilder = BuildCissyConfig(services, _cissyConfigSource);
            _ConfigureCissyDatabaseServices(CissyConfigBuilder);
            ConfigCissyServices(CissyConfigBuilder, _cissyConfigSource);
            _ConfigureAspServices(CissyConfigBuilder);
            CissyConfigBuilder.Do();
        }
        CissyConfigBuilder BuildCissyConfig(IServiceCollection services, CissyConfigSource CissyConfigSource)
        {
            if (CissyConfigSource.ConfigSource == CissyConfigSourceKind.Local)
                return services.AddCissyConfig(_hostingEnvironment.IsDevelopment() ? AppRunStatus.Delelopment : AppRunStatus.PreProduction);
            else
                return services.AddCissyRemoteConfig(new RemoteConfigSource()
                {
                    RunStatus = _hostingEnvironment.IsDevelopment() ? AppRunStatus.Delelopment : AppRunStatus.PreProduction,
                    ConfigServer = CissyConfigSource.CissyConfigServer,
                    AppName = CissyConfigSource.AppName,
                    Password = CissyConfigSource.Password,
                    Version = CissyConfigSource.Version
                });
        }
        void ConfigCissyServices(CissyConfigBuilder CissyConfigBuilder, CissyConfigSource CissyConfigSource)
        {
            if (CissyConfigSource.StartPayment)
                CissyConfigBuilder.AddPaymentConfig();
            if (CissyConfigSource.StartMemoryCache)
                CissyConfigBuilder.AddLocalMemoryCache();
            if (CissyConfigSource.StartWeakCache)
                CissyConfigBuilder.AddWeakCache();
            if (CissyConfigSource.StartRedis)
                CissyConfigBuilder.AddRedisConfig();
            if (CissyConfigSource.StartElasticsearch)
                CissyConfigBuilder.AddElasticsearchConfig();
            if (CissyConfigSource.StartKafka)
                CissyConfigBuilder.AddKafkaConfig();
            if (CissyConfigSource.StartAliOSS)
                CissyConfigBuilder.AddAliOSSConfig();
            if (CissyConfigSource.StartHttpProxy)
                CissyConfigBuilder.AddHttpProxyConfig();
            if (CissyConfigSource.StartWeixinApi.IsNotNull())
            {
                CissyConfigBuilder.AddWeiXinMpApiConfig(builder =>
                {
                    return CissyConfigSource.StartWeixinApi(CissyConfigBuilder);
                });
            }
            if (CissyConfigSource.StartRateLimit.IsNotNull())
            {
                var loaderOption = CissyConfigSource.StartRateLimit(CissyConfigBuilder);
                if (loaderOption.IsNotNull())
                {
                    var cache = loaderOption.GetRedisCache();
                    if (cache.IsNull())
                    {
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
                    }
                    else
                    {
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IIpPolicyStore, RedisCacheIpPolicyStore>();
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IRateLimitCounterStore, RedisCacheRateLimitCounterStore>();
                        CissyConfigBuilder.ServiceCollection.AddSingleton<IClientPolicyStore, RedisCacheClientPolicyStore>();
                    }
                    CissyConfigBuilder.ServiceCollection.AddSingleton<IRateLimitLoader>(loaderOption);
                    CissyConfigBuilder.ServiceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    CissyConfigBuilder.ServiceCollection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                }
            }
            if (CissyConfigSource.StartAuthentication.IsNotNull())
            {
                var option = CissyConfigSource.StartAuthentication();
                if (option.IsNotNull())
                    CissyConfigBuilder.AddCissyAuthentication(x =>
                    {
                        x.Scheme = option.Scheme;
                        x.AuthenticationApply = option.AuthenticationApply;
                        x.AuthenticationType = option.AuthenticationType;
                    });
                else
                    CissyConfigBuilder.AddCissyAuthentication();
            }
            CissyConfigBuilder.RegisterMapper(reg =>
            {
                _RegisterDTOMap(reg);
            });
        }
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                if (this._cissyConfigSource.ExceptionHandlerPath.IsNotNullAndEmpty())
                {
                    app.UseExceptionHandler(this._cissyConfigSource.ExceptionHandlerPath);
                    app.UseHsts();
                }
            }
            if (this._cissyConfigSource.StartRateLimit.IsNotNull())
            {
                app.UseIpRateLimiting();
                app.UseClientRateLimiting();
            }
            if (_cissyConfigSource.StartAuthentication.IsNotNull())
            {
                app.UseCissyAuthentication();
            }
            if (_cissyConfigSource.HttpsRedirection)
                app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            if (_cissyConfigSource.StartStaticFiles)
                app.UseStaticFiles();

            //app.UseCookiePolicy();

            app.UseRouting();
            _ConfigureAsp(app, env);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                _UseEndpoints(endpoints);
            });

        }
        public virtual void _UseEndpoints(IEndpointRouteBuilder endpoints)
        {
            //endpoints.MapRazorPages();
            //endpoints.MapControllers();
            endpoints.MapFallbackToPage("/_Host");
        }
    }
}

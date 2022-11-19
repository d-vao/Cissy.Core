using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;

namespace Cissy.Configuration
{
    [EnumStrings(typeof(AppRunStatus), "开发状态", "试产状态", "生产状态", EnumName = "软件运行状态")]
    [Flags]
    public enum AppRunStatus
    {
        Delelopment = 1,
        PreProduction = 2,
        Production = 4
    }
    public class CissyConfigBuilder
    {
        public IServiceCollection ServiceCollection { get; set; }
        public ICissyConfig CissyConfig { get; set; }
        public T GetService<T>()
        {
            if (ServiceCollection.IsNotNullAndEmpty())
            {
                return ServiceCollection.BuildServiceProvider().GetService<T>();
            }
            return default(T);
        }
        public AppRunStatus AppRunState()
        {
            return CissyConfigHelper.appRunStatus;
        }
    }
    public static class CissyConfigHelper
    {
        internal static AppRunStatus appRunStatus;
        static IServiceCollection ServiceCollection;
        static IMapper mapper;
        static MapperConfiguration mapperConfiguration;
        static List<Action<IMapperConfigurationExpression>> MapperConfigurationExpressionList = new List<Action<IMapperConfigurationExpression>>();
        public static T GetService<T>(this Public Public)
        {
            if (ServiceCollection.IsNotNullAndEmpty())
            {
                return ServiceCollection.BuildServiceProvider().GetService<T>();
            }
            return default(T);
        }
        public static T Map<T>(this Public Public, object obj)
        {
            if (mapper.IsNull())
                return default(T);
            return mapper.Map<T>(obj);
        }
        public static AppRunStatus AppRunState(this Public Public)
        {
            return appRunStatus;
        }
        public static void RunDebug(this Public Public, Action action)
        {
            if (appRunStatus != AppRunStatus.Production)
            {
                action();
            }
        }
        /// <summary>
        /// 注入Cissy配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddCissyConfig(this IServiceCollection services, AppRunStatus runState)
        {
            appRunStatus = runState;
            ServiceCollection = services;
            CissyConfig config = GetCissyConfig();
            if (config.IsNotNull())
            {
                services.AddSingleton(typeof(ICissyConfig), config);
            }
            return new CissyConfigBuilder() { ServiceCollection = services, CissyConfig = config };
        }
        public static CissyConfigBuilder AddCissyRemoteConfig(this IServiceCollection services, RemoteConfigSource remoteConfigSource)
        {
            appRunStatus = remoteConfigSource.RunStatus;
            ServiceCollection = services;
            var ConfigUrl = $"{remoteConfigSource.ConfigServer}/api/{remoteConfigSource.Version}/appconfig/index?appname={remoteConfigSource.AppName}&configpwd={remoteConfigSource.Password}&r={new Random().Next (1000,9999)}";
            CissyConfig config = GetCissyRemoteConfig(ConfigUrl);
            if (config.IsNotNull())
            {
                services.AddSingleton(typeof(ICissyConfig), config);
            }
            return new CissyConfigBuilder() { ServiceCollection = services, CissyConfig = config };
        }
        static CissyConfig GetCissyConfig()
        {
            Type type = typeof(CissyConfigHelper);
            var builder = new ConfigurationBuilder();
            //string s = type.Assembly.Location;
            if (appRunStatus == AppRunStatus.Delelopment)
                builder.AddJsonFile("cissy.dev.json");
            else
                builder.AddJsonFile("cissy.json");
            var root = builder.Build();
            CissyConfig config = new CissyConfig();
            IConfigurationSection section = root.GetSection(config.ConfigName);
            config.InitConfig(section);
            return config;
        }
        static CissyConfig GetCissyRemoteConfig(string url)
        {
            var configUrl = url + $"&r={new Random().Next(100000)}";
            var devconfigUrl = configUrl + $"&dev=1";
            Type type = typeof(CissyConfigHelper);
            var builder = new ConfigurationBuilder();
            DatabaseFileProvider efp = new DatabaseFileProvider();
            if (appRunStatus == AppRunStatus.Delelopment)
            {
                //builder.AddJsonFile("cissy.dev.json");
                builder.AddJsonFile(efp, devconfigUrl, true, true);
            }
            else
            {
                builder.AddJsonFile(efp, configUrl, true, true);
            }
            var root = builder.Build();
            CissyConfig config = new CissyConfig();
            IConfigurationSection section = root.GetSection(config.ConfigName);
            config.InitConfig(section);
            return config;
        }
        public static CissyConfigBuilder RegisterMapper(this CissyConfigBuilder cissyConfigBuilder, Action<IMapperConfigurationExpression> register)
        {
            MapperConfigurationExpressionList.Add(register);
            return cissyConfigBuilder;
        }
        public static void Do(this CissyConfigBuilder cissyConfigBuilder)
        {
            mapperConfiguration = new MapperConfiguration(cfg =>
           {
               MapperConfigurationExpressionList.ForEach(m =>
               {
                   m(cfg);
               });
           });
            if (appRunStatus == AppRunStatus.Delelopment)
                mapperConfiguration.AssertConfigurationIsValid();
            mapper = mapperConfiguration.CreateMapper();
        }
    }
    public interface IConfigModel
    {
        string ConfigName { get; }
        void InitConfig(IConfigurationSection section);
    }
    public interface ICissyConfig : IConfigModel
    {
        T GetConfig<T>() where T : IConfigModel, new();
    }
    /// <summary>
    /// IdentityServer4配置
    /// </summary>
    public class CissyConfig : ICissyConfig
    {
        public string ConfigName { get { return "cissy"; } }
        IConfigurationSection _section;
        public void InitConfig(IConfigurationSection section)
        {
            _section = section;

        }
        public T GetConfig<T>() where T : IConfigModel, new()
        {
            T instance = new T();
            IConfigurationSection subSection = _section.GetSection(instance.ConfigName);
            if (subSection.IsNull())
                return default(T);
            instance.InitConfig(subSection);
            return instance;
        }
    }
    public class RemoteConfigSource
    {
        public AppRunStatus RunStatus;
        public string ConfigServer;
        public string AppName;
        public string Password;
        public string Version = "1.0";
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Cissy.Dapper.Sql;
using Cissy.Configuration;

namespace Cissy.Database
{
    /// <summary>
    /// 数据库配置帮助
    /// </summary>
    public static class DatabaseConfigHelper
    {
        /// <summary>
        /// 注入数据库服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddDatabaseConfig<T, K>(this CissyConfigBuilder cissyConfigBuilder, string ConnectionName) where T : DbConnection, new() where K : SqlDialectBase, new()
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                DatabaseConfig dbConfig = cissyConfig.GetConfig<DatabaseConfig>();
                if (dbConfig.IsNotNull())
                {
                    var factory = new DefaultDatabaseFactory<T, K>();
                    factory.Config = dbConfig;
                    var dbBuilder = cissyConfigBuilder.ServiceCollection.BuildServiceProvider().GetService<IDBBuilder>();
                    if (dbBuilder.IsNull())
                    {
                        dbBuilder = new DefaultDBBuilder();
                        cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IDBBuilder), dbBuilder);
                    }
                    DefaultDBBuilder defaultDBBuilder = dbBuilder as DefaultDBBuilder;
                    if (defaultDBBuilder.IsNotNull())
                    {
                        defaultDBBuilder.RegisterDatabaseFactory(ConnectionName, factory);
                    }
                }
            }
            return cissyConfigBuilder;
        }
    }
}

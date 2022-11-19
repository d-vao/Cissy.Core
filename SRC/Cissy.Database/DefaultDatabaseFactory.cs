using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.Common;
using Cissy.Dapper;
using Cissy.Dapper.Sql;

namespace Cissy.Database
{
    internal class DefaultDBBuilder : IDBBuilder
    {
        ConcurrentDictionary<string, IDatabaseFactory> factories = new ConcurrentDictionary<string, IDatabaseFactory>();
        public IDatabase Build(string ConnectionName)
        {
            if (factories.TryGetValue(ConnectionName, out IDatabaseFactory factory))
            {
                return factory.Build(ConnectionName);
            }
            return default(IDatabase);
        }
        public void RegisterDatabaseFactory(string ConnectionName, IDatabaseFactory factory)
        {
            factories[ConnectionName] = factory;
        }
    }
    internal class DefaultDatabaseFactory<T, K> : IDatabaseFactory where T : DbConnection, new() where K : SqlDialectBase, new()
    {
        //public string ConnectionName { get; private set; }
        public DatabaseConfig Config { get; internal set; }
        public IDatabase Build(string ConnectionName)
        {
            //this.ConnectionName = ConnectionName;
            DbConnectionConfig dbconfig = Config.Connections.FirstOrDefault(m => m.Name.ToLower() == ConnectionName.ToLower());
            if (dbconfig.IsNotNull())
            {
                var config = new CissyDapperConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new K());
                var sqlGenerator = new SqlGeneratorImpl(config);
                T dbconnection = new T();
                dbconnection.ConnectionString = dbconfig.GetConnectionString();
                return new DB(dbconnection, sqlGenerator);
            }
            return default(IDatabase);
        }
    }
}

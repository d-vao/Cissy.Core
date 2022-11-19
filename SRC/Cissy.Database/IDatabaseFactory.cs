using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Cissy.Dapper;
using Cissy.Dapper.Sql;

namespace Cissy.Database
{
    internal interface IDatabaseFactory
    {
        //string ConnectionName { get; }
        DatabaseConfig Config { get; }
        IDatabase Build(string ConnectionName);
    }
    public interface IDBBuilder
    {
        IDatabase Build(string ConnectionName);
    }
}

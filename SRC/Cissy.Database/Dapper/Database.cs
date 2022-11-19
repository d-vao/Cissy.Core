using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Cissy.Dapper;
using Cissy.Dapper.Sql;
using System.Data.Common;

namespace Cissy.Dapper
{
    public interface IDatabase : IDisposable
    {
        bool HasActiveTransaction { get; }
        DbConnection Connection { get; }
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        void RunInTransaction(Action action);
        T RunInTransaction<T>(Func<T> func) where T : Cissy.Database.IEntity;
        T Get<T>(dynamic id, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        T Get<T>(dynamic id, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        void Insert<T>(IEnumerable<T> entities, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        dynamic Insert<T>(T entity, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        dynamic Insert<T>(T entity, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool Update<T>(T entity, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool Update<T>(T entity, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool LockUpdate<T>(T entity, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IDataVersion;
        bool LockUpdate<T>(T entity, int? commandTimeout = null) where T : Cissy.Database.IDataVersion;
        bool Delete<T>(T entity, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool Delete<T>(object predicate, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        bool Delete<T>(object predicate, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryInIds<T, K>(Expression<Func<T, object>> expression, IEnumerable<K> Ids, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        IEnumerable<T> Query<T>(object predicate, IList<ISort> sort, DbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryColInIds<T, K>(Action<ColSelect<T>> ColSelector, Expression<Func<T, object>> expression, IEnumerable<K> Ids, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryCol<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, DbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> Query<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryCol<T>(Action<ColSelect<T>> ColSelector, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, DbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryColPage<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int page, int resultsPerPage, DbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryColPage<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout = null, bool buffered = true) where T : Cissy.Database.IEntity;
        IEnumerable<T> QuerySet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryColSet<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int firstResult, int maxResults, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        IEnumerable<T> QuerySet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        IEnumerable<T> QueryColSet<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity;
        int Count<T>(object predicate, DbTransaction transaction, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        int Count<T>(object predicate, int? commandTimeout = null) where T : Cissy.Database.IEntity;
        IMultipleResultReader QueryMultiple(GetMultiplePredicate predicate, DbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader QueryMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);
        void ClearCache();
        Guid GetNextGuid();
        IClassMapper GetMap<T>() where T : Cissy.Database.IEntity;
        int ExecuteSql(string Sql, DbTransaction transaction, int? commandTimeout = null);
        int ExecuteSql( string Sql, int? commandTimeout = null);
    }

    public class DB : IDatabase
    {
        private readonly IDapperImplementor _dapper;

        private DbTransaction _transaction;

        public DB(DbConnection connection, ISqlGenerator sqlGenerator)
        {
            _dapper = new DapperImplementor(sqlGenerator);
            Connection = connection;

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public bool HasActiveTransaction
        {
            get
            {
                return _transaction != null;
            }
        }

        public DbConnection Connection { get; private set; }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }

                Connection.Close();
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public void RunInTransaction(Action action)
        {
            BeginTransaction();
            try
            {
                action();
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        public T RunInTransaction<T>(Func<T> func) where T : Cissy.Database.IEntity
        {
            BeginTransaction();
            try
            {
                T result = func();
                Commit();
                return result;
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        public T Get<T>(dynamic id, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return (T)_dapper.Get<T>(Connection, id, transaction, commandTimeout);
        }

        public T Get<T>(dynamic id, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return (T)_dapper.Get<T>(Connection, id, _transaction, commandTimeout);
        }

        public void Insert<T>(IEnumerable<T> entities, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            _dapper.Insert<T>(Connection, entities, transaction, commandTimeout);
        }

        public void Insert<T>(IEnumerable<T> entities, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            _dapper.Insert<T>(Connection, entities, _transaction, commandTimeout);
        }

        public dynamic Insert<T>(T entity, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Insert<T>(Connection, entity, transaction, commandTimeout);
        }

        public dynamic Insert<T>(T entity, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Insert<T>(Connection, entity, _transaction, commandTimeout);
        }

        public bool Update<T>(T entity, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Update<T>(Connection, entity, transaction, commandTimeout);
        }

        public bool Update<T>(T entity, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Update<T>(Connection, entity, _transaction, commandTimeout);
        }
        public bool LockUpdate<T>(T entity, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IDataVersion
        {
            return _dapper.LockUpdate<T>(Connection, entity, transaction, commandTimeout);
        }

        public bool LockUpdate<T>(T entity, int? commandTimeout) where T : Cissy.Database.IDataVersion
        {
            return _dapper.LockUpdate<T>(Connection, entity, _transaction, commandTimeout);
        }
        public bool Delete<T>(T entity, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Delete(Connection, entity, transaction, commandTimeout);
        }

        public bool Delete<T>(T entity, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Delete(Connection, entity, _transaction, commandTimeout);
        }

        public bool Delete<T>(object predicate, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Delete<T>(Connection, predicate, transaction, commandTimeout);
        }

        public bool Delete<T>(object predicate, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Delete<T>(Connection, predicate, _transaction, commandTimeout);
        }
        public IEnumerable<T> QueryInIds<T, K>(Expression<Func<T, object>> expression, IEnumerable<K> Ids, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            string Name = propertyInfo.Name;
            return _dapper.GetListInIds<T, K>(Connection, Name, Ids, sort, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> Query<T>(object predicate, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetList<T>(Connection, predicate, sort, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryColInIds<T, K>(Action<ColSelect<T>> ColSelector, Expression<Func<T, object>> expression, IEnumerable<K> Ids, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            string Name = propertyInfo.Name;
            return _dapper.GetListColInIds<T, K>(Connection, ColSelector, Name, Ids, sort, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryCol<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetListCol<T>(Connection, ColSelector, predicate, sort, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> Query<T>(object predicate, IList<ISort> sort, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetList<T>(Connection, predicate, sort, _transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryCol<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetListCol<T>(Connection, ColSelector, predicate, sort, _transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryColPage<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int page, int resultsPerPage, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetPageCol<T>(Connection, ColSelector, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

        public IEnumerable<T> QueryPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryColPage<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetPageCol<T>(Connection, ColSelector, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QuerySet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryColSet<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int firstResult, int maxResults, DbTransaction transaction, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetSetCol<T>(Connection, ColSelector, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QuerySet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, buffered);
        }
        public IEnumerable<T> QueryColSet<T>(Action<ColSelect<T>> ColSelector, object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : Cissy.Database.IEntity
        {
            return _dapper.GetSetCol<T>(Connection, ColSelector, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, buffered);
        }
        public int Count<T>(object predicate, DbTransaction transaction, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Count<T>(Connection, predicate, transaction, commandTimeout);
        }

        public int Count<T>(object predicate, int? commandTimeout) where T : Cissy.Database.IEntity
        {
            return _dapper.Count<T>(Connection, predicate, _transaction, commandTimeout);
        }

        public IMultipleResultReader QueryMultiple(GetMultiplePredicate predicate, DbTransaction transaction, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, transaction, commandTimeout);
        }

        public IMultipleResultReader QueryMultiple(GetMultiplePredicate predicate, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, _transaction, commandTimeout);
        }

        public void ClearCache()
        {
            _dapper.SqlGenerator.Configuration.ClearCache();
        }

        public Guid GetNextGuid()
        {
            return _dapper.SqlGenerator.Configuration.GetNextGuid();
        }

        public IClassMapper GetMap<T>() where T : Cissy.Database.IEntity
        {
            return _dapper.SqlGenerator.Configuration.GetMap<T>();
        }
        public int ExecuteSql( string Sql, DbTransaction transaction, int? commandTimeout)
        {
            return _dapper.ExecuteSql(Connection, Sql, transaction, commandTimeout);
        }
        public int ExecuteSql(string Sql, int? commandTimeout)
        {
            return _dapper.ExecuteSql(Connection, Sql, _transaction, commandTimeout);
        }
    }
}
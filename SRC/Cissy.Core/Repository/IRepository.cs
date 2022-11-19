using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Cissy.Database;

namespace Cissy
{
    public interface IRepository<T> where T : IEntity
    {
        T GetById(long id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}

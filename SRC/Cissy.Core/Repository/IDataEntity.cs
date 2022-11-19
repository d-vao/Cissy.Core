using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Database;

namespace Cissy
{
    public interface IDataEntity<T> : IEntity
    {
        T Id { get; }
    }
}

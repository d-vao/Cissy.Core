using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Database
{
    public static class DataVersionMeta
    {
        public const string DataVersionColumName = "DataVersion";
    }
    public interface IDataVersion : IEntity
    {
        int DataVersion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Tools.Templates
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class Table
    {
        public string NameSpaces { get; set; }
        public string TableName { get; set; }

        public string CreateTable { get; set; }

        public Dictionary<string, Column> Columns { get; set; } = new Dictionary<string, Column>();

        public Dictionary<string, Index> Indexes { get; set; } = new Dictionary<string, Index>();
    }
}

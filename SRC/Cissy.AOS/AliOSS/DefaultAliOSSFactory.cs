using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Cissy.AOS
{
    public sealed class DefaultAliOSSFactory : IAliOSSFactory
    {
        public AliOSSConfig Config { get; set; }
        internal DefaultAliOSSFactory()
        {
        }
    }
}

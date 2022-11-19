using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;

namespace Cissy.Tools.Templates
{
    public static class OrmExtensions
    {
        public static ORMGenerator GenerateOrm(this TextTransformation transformation, ITextTemplatingEngineHost host, string SectionName, string ConnectionName, string SchemaConnectionString)
        {
            return new ORMGenerator(host, SectionName, ConnectionName, SchemaConnectionString);
        }
    }
}

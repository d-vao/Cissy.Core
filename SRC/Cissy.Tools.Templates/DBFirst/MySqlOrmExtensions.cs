using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;

namespace Cissy.Tools.Templates
{
    public static class MySqlOrmExtensions
    {
        public static MySqlORMGenerator GenerateModelsFromMySql(this TextTransformation transformation, ITextTemplatingEngineHost host, string nameSpaces, string SchemaConnectionString)
        {
            return new MySqlORMGenerator(host, nameSpaces, SchemaConnectionString);
        }
        public static MySqlORMGenerator GenerateModelsFromRemoteMySql(this TextTransformation transformation, ITextTemplatingEngineHost host, string nameSpaces, string ConfigServer, string db, string AppName, string Password, string servername = null, string Version = "1.0")
        {
            var ConfigUrl = $"{ConfigServer}/api/{Version}/appconfig/DbScheme?appname={AppName}&configpwd={Password}&db={db}&servername={servername}";
            return new MySqlORMGenerator(host, nameSpaces, new Uri(ConfigUrl));
        }
    }
}

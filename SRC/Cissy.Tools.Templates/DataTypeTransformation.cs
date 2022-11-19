using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cissy.Tools.Templates
{
    public interface IDataTypeTransformation
    {
        /// <summary>
        /// 获取数据列类型名对应的模型字段类型名
        /// </summary>
        /// <param name="ColumnTypeName"></param>
        /// <returns></returns>
        string Map(string ColumnTypeName);
    }

    public class MySqlDataTypeTransformation : IDataTypeTransformation
    {
        static Dictionary<string, string> names = new Dictionary<string, string>();
        static MySqlDataTypeTransformation()
        {

            RegisterType("bit", "bool");
            RegisterType("tinyint", "int");
            RegisterType("smallint", "int");
            RegisterType("mediumint", "int");
            RegisterType("int", "int");
            RegisterType("integer", "int");
            RegisterType("bigint", "long");
            RegisterType("decimal", "decimal");
            RegisterType("dec", "decimal");
            RegisterType("numeric", "decimal");
            RegisterType("fixed", "decimal");
            RegisterType("float", "float");
            RegisterType("double", "double");
            RegisterType("precision", "double");
            RegisterType("real", "double");
            RegisterType("bool", "int");
            RegisterType("boolean", "int");

            RegisterType("char", "string");
            RegisterType("varchar", "string");
            RegisterType("tinytext", "string");
            RegisterType("text", "string");
            RegisterType("mediumtext", "string");
            RegisterType("longtext", "string");
            RegisterType("binary", "byte[]");
            RegisterType("varbinary", "byte[]");

            RegisterType("date", "DateTime");
            RegisterType("datetime", "DateTime");
            RegisterType("timestamp", "uint");
            RegisterType("time", "DateTime");
            RegisterType("year", "int");
            
            RegisterType("tinyblob", "byte[]");
            RegisterType("blob", "byte[]");
            RegisterType("mediumblob", "byte[]");
            RegisterType("longblob", "byte[]");
            RegisterType("longtext", "byte[]");
        }
        static void RegisterType(string DataTypeName, string FieldTypeName)
        {
            names[DataTypeName] = FieldTypeName;
        }
        /// <summary>
        /// 获取数据列类型名对应的模型字段类型名
        /// </summary>
        /// <param name="ColumnTypeName"></param>
        /// <returns></returns>
        public string Map(string ColumnTypeName)
        {
            ColumnTypeName = ColumnTypeName.Trim();
            ColumnTypeName = ColumnTypeName.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var FieldTypeName = string.Empty;
            names.TryGetValue(ColumnTypeName, out FieldTypeName);
            return FieldTypeName;
        }
    }
}

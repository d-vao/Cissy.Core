using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cissy.Tools.Templates
{
    public class MySqlDataModelTemplate : Template
    {
        Table _table;
        IDataTypeTransformation dtt;
        public MySqlDataModelTemplate(Table table)
        {
            dtt = new MySqlDataTypeTransformation();
            this._table = table;
        }
        public override void TransformText()
        {
            this.WriteLine("/*");
            this.WriteLine("有任何问题,请联系:  bzure@outlook.com");
            this.WriteLine("此代码文件由系统自动生成，请不要做任何修改，因为T4模板随时可能重新生成覆盖你的文件");
            this.WriteLine("如果你需要扩展实体类的映射文件功能以实现自定义的查询或者更新操作");
            this.WriteLine(string.Format("请使用partial关键字定义类{0}在另外的代码文件中,不过请注意扩展类的命名空间必须跟原类一致", this._table.TableName));
            this.WriteLine("*/");
            this.WriteLine("using System;");
            this.WriteLine("using System.Collections.Generic;");
            this.WriteLine("using System.Linq;");
            this.WriteLine("using System.Text;");
            this.WriteLine("using Cissy.Database;");
            //this.WriteLine(string.Format("using {0};", this.EntityType.Namespace));
            this.WriteLine($"namespace {this._table.NameSpaces}");
            this.WriteLine("{ ");
            //类开始
            string baseType = IsDataVersion(this._table.Columns.Values) ? "IDataVersion" : "IEntity";
            this.WriteLine($"    public partial class  {this._table.TableName} :{baseType}");
            this.WriteLine("    { ");
            string key = string.Empty;
            if (this._table.Indexes.TryGetValue("PRIMARY", out Index PrimaryIndex))
            {
                key = PrimaryIndex.Columns.FirstOrDefault();
            }
            foreach (Column col in this._table.Columns.Values)
            {
                if (!string.IsNullOrEmpty(col.Comment))
                {
                    //this.WriteLine($"        /*{col.Comment}*/");
                    this.WriteLine($"        /// <summary>");
                    this.WriteLine($"        ///{col.Comment.TrimOther()}");
                    this.WriteLine($"        /// </summary>");
                }
                if (!string.IsNullOrEmpty(key) && col.Name == key)
                {
                    this.WriteLine("        [Key]");
                }
                var typeName = this.dtt.Map(col.Type);
                if (!string.IsNullOrEmpty(typeName))
                    this.WriteLine($"        public {typeName} {col.Name} " + "{get;set;}");
            }
            this.WriteLine("    }");
            //类结束
            this.WriteLine("}");
        }
        bool IsDataVersion(IEnumerable<Column> cols)
        {
            foreach (Column col in cols)
            {
                if (this.dtt.Map(col.Type) == "int" && col.Name == "DataVersion")
                    return true;
            }
            return false;
        }
    }
    public static class TemplateHelper
    {
        public static string TrimOther(this string str)
        {
            return str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
        }
    }
}

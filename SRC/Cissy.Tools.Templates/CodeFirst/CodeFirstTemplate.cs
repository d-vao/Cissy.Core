using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cissy.Tools.Templates
{

    public class EntityProviderTemplate : Template
    {
        Dictionary<Type, ORM> ORMs;
        string SectionName;
        string ConnectionName;
        public EntityProviderTemplate(string SectionName, string ConnectionName, Dictionary<Type, ORM> ORMs)
        {
            this.SectionName = SectionName;
            this.ConnectionName = ConnectionName;
            this.ORMs = ORMs;
        }

        public override void TransformText()
        {
            this.WriteLine("/*");
            this.WriteLine("此代码文件由系统自动生成，请不要做任何修改，因为T4模板随时可能重新生成覆盖你的文件");
            this.WriteLine("*/");
            this.WriteLine("using System;");
            this.WriteLine("using System.Collections.Generic;");
            this.WriteLine("using System.Linq;");
            this.WriteLine("using System.Text;");
            this.WriteLine("using System.Data.SqlClient;");
            this.WriteLine("using ESF.Common.Data;");
            this.WriteLine("using ESF.Common.Core;");
            this.WriteLine("using ESF.Common.Transactions;");
            this.WriteLine("using Autofac;");



            List<string> namesapces = new List<string>();
            foreach (ORM m in this.ORMs.Values)
            {
                if (!namesapces.Contains(m.EntityType.Namespace))
                    namesapces.Add(m.EntityType.Namespace);
            }
            namesapces.ForEach(m =>
            {
                this.WriteLine(string.Format("using {0};", m));
            });
            this.WriteLine(string.Format("namespace ESF.Data.Templates.{0}.Orm", this.SectionName));
            this.WriteLine("{ ");

            this.WriteLine(string.Format(" [Injection(\"{0}EntityStarter\")]", this.SectionName));
            this.WriteLine("public class SecurityAppStarter : IApplicationStart");
            this.WriteLine("{");
            this.WriteLine(" public float ContractVersion { get { return 1.0F; } }");
            this.WriteLine("public void RegisterContract(ContainerBuilder ContainerBuilder)");
            this.WriteLine("{");
            this.WriteLine(string.Format("ContainerBuilder.RegisterType<{0}_EntityRequestProcessorBuider>().Named<IContractFactory<IEntityRequestProcess>>(\"{0}\");", SectionName));
            this.WriteLine(string.Format("ContainerBuilder.RegisterType<{0}_EntityPostProcessorBuider>().Named<IContractFactory<IEntityPostProcess>>(\"{0}\");", SectionName));
            this.WriteLine("  }");
            this.WriteLine("public void PreStart(IApplicationStartContext StartContext)");
            this.WriteLine("{");
            this.WriteLine("}");
            this.WriteLine("public void Start(IApplicationStartContext StartContext)");
            this.WriteLine("{");
            this.WriteLine("}");
            this.WriteLine("}");

            this.WriteLine(string.Format("[Injection(\"{0}\")]", SectionName));
            this.WriteLine(string.Format(" public class {0}_EntityRequestProcessorBuider : IContractFactory<IEntityRequestProcess>", SectionName));
            this.WriteLine(" {");
            this.WriteLine("public float ContractVersion { get { return 1.0F; } }");
            this.WriteLine(" public IEntityRequestProcess Build()");
            this.WriteLine("  {");
            this.WriteLine(string.Format("   return new {0}_EntityRequestProcessor();", SectionName));
            this.WriteLine(" }");
            this.WriteLine("  }");

            this.WriteLine(string.Format("[Injection(\"{0}\")]", SectionName));
            this.WriteLine(string.Format(" public class {0}_EntityPostProcessorBuider : IContractFactory<IEntityPostProcess>", SectionName));
            this.WriteLine(" {");
            this.WriteLine("public float ContractVersion { get { return 1.0F; } }");
            this.WriteLine(" public IEntityPostProcess Build()");
            this.WriteLine("  {");
            this.WriteLine(string.Format("   return new {0}_EntityRequestProcessor();", SectionName));
            this.WriteLine(" }");
            this.WriteLine("  }");


            this.WriteLine(string.Format("  public class {0}_EntityRequestProcessor : ESF.Common.Data.EntityRequestProcessBase", SectionName));
            this.WriteLine("{");
            this.WriteLine("public override System.Data.Common.DbConnection CreateConnection()");
            this.WriteLine("{");
            this.WriteLine(string.Format("return {0}_ConnectionFactory.GetConnection();", SectionName));
            this.WriteLine(" }");
            this.WriteLine("public override void InitEntityMap(Type TargetType)");
            this.WriteLine("{");


            foreach (ORM m in this.ORMs.Values)
            {
                this.WriteLine(string.Format("AppendEntityHandler<{0}>(TargetType, () => new {1}(this));", m.EntityType.Name, BuildClassName(m.EntityType)));
            }

            this.WriteLine("}");
            this.WriteLine("}");

            this.WriteLine(string.Format("public static class {0}_ConnectionFactory", SectionName));
            this.WriteLine("{");
            this.WriteLine(string.Format("private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"{0}\"].ConnectionString;", ConnectionName));
            this.WriteLine("public static System.Data.Common.DbConnection GetConnection()");
            this.WriteLine(" {");
            this.WriteLine("   return new SqlConnection(_connectionString);");
            this.WriteLine(" }");
            this.WriteLine("}");


            this.WriteLine(string.Format("    public abstract partial class {0}_RepositoryBase :ESF.Common.Data.TemplateRepository", SectionName));
            this.WriteLine("  {");
            this.WriteLine(string.Format(" public  {0}_RepositoryBase(ITransactionSupport contractProvider)", SectionName));
            this.WriteLine("   : base(contractProvider)");
            this.WriteLine(" {");
            this.WriteLine("  }     ");
            this.WriteLine("}");
            this.WriteLine("}");

        }
        public string BuildClassName(Type EntityType)
        {
            return string.Format("{0}_Repository", EntityType.Name);
        }
    }
    public class EntityMapTemplate : Template
    {
        public Type EntityType { get; private set; }
        string SectionName;
        string Key;
        Dictionary<string, ColumnMap> columns = default(Dictionary<string, ColumnMap>);
        string tableName = string.Empty;
        public EntityMapTemplate(Type entityType, string SectionName, string TableName, string Key, Dictionary<string, ColumnMap> Columns)
        {
            this.EntityType = entityType;
            this.SectionName = SectionName;
            this.tableName = TableName;
            this.Key = Key;
            this.columns = Columns;
        }
        public override void TransformText()
        {
            this.WriteLine("/*");
            this.WriteLine("有任何问题,请联系:  bzure@outlook.com");
            this.WriteLine("此代码文件由系统自动生成，请不要做任何修改，因为T4模板随时可能重新生成覆盖你的文件");
            this.WriteLine("如果你需要扩展实体类的映射文件功能以实现自定义的查询或者更新操作");
            this.WriteLine(string.Format("请使用partial关键字定义类{0}在另外的地代码文件中,不过请注意扩展类的命名空间必须跟原类一致", BuildClassName()));
            this.WriteLine("*/");
            this.WriteLine("using System;");
            this.WriteLine("using System.Collections.Generic;");
            this.WriteLine("using System.Linq;");
            this.WriteLine("using System.Text;");
            this.WriteLine("using ESF.Common.Data;");
            this.WriteLine("using ESF.Common.Core;");
            this.WriteLine(string.Format("using {0};", this.EntityType.Namespace));
            this.WriteLine(string.Format("namespace ESF.Data.Templates.{0}.Orm", this.SectionName));
            this.WriteLine("{ ");
            this.WriteLine(string.Format("public partial class  {0}:{1}_RepositoryBase", BuildClassName(), SectionName));
            this.WriteLine("{ ");
            //表名
            this.WriteLine("public override string TableName");
            this.WriteLine("{ get {");
            this.WriteLine(string.Format("     return \"{0}\";", this.tableName));
            this.WriteLine(" }}");
            //静态构造方法
            this.WriteLine(string.Format("static {0}()", BuildClassName()));
            this.WriteLine("{");
            this.WriteLine(string.Format(" {0} entity = new {0}();", this.EntityType.Name));
            this.WriteLine("  Type type = entity.GetType();");
            foreach (ColumnMap map in this.columns.Values)
            {
                this.WriteLine(BuildUpdateMap(map));
            }
            this.WriteLine(string.Format(" ResultMapDelegate<{0}> queryMap = {0}_Map;", this.EntityType.Name));
            this.WriteLine("QueryMapContainer[entity.GetType()] = queryMap;");
            this.WriteLine("}");
            //实例构造方法
            this.WriteLine(string.Format("public {0}(ESF.Common.Transactions.ITransactionSupport contractProvider)", BuildClassName()));
            this.WriteLine("  : base(contractProvider) { }");
            //实体映射
            this.WriteLine(string.Format("public static {0} {0}_Map(DataReader reader,string[] Colums)", this.EntityType.Name));
            this.WriteLine("   {");
            this.WriteLine(string.Format(" {0} {0}_Instance= new {0}();", this.EntityType.Name));
            foreach (ColumnMap map in this.columns.Values)
            {
                this.WriteLine(string.Format("if ( Colums.IsNullOrEmpty ()||Colums.Contains(\"{0}\"))", map.ColumnName));
                this.WriteLine("{");
                this.WriteLine(BuildQueryMap(this.EntityType.Name + "_Instance", map));
                this.WriteLine("}");
            }
            this.WriteLine(string.Format("return {0}_Instance;", this.EntityType.Name));
            this.WriteLine("}");
            //获取实体
            this.WriteLine(" public override EntityQueryRequest<T> ToGetRequest<T>(object KeyValue)");
            this.WriteLine("{");
            this.WriteLine(string.Format("  {0} factor = new {0}()", this.EntityType.Name));
            this.WriteLine("{");
            this.WriteLine(string.Format("{0} = ({1})KeyValue", this.Key, GetFieldTypeName(this.Key)));
            this.WriteLine("};");
            this.WriteLine(string.Format("   EntityQueryRequest<{0}> query = new EntityQueryRequest<{0}>();", this.EntityType.Name));
            this.WriteLine(string.Format("  query.AppendConditionMap(factor.{0}, () => factor.{0});", this.Key));
            this.WriteLine("  return query as EntityQueryRequest<T>;");
            this.WriteLine("}");
            //条件查询
            this.WriteLine("   public override EntityQueryRequest<T> ToFilterRequest<T>(Action<object,EntityQueryRequest<T>> action) ");
            this.WriteLine("{");
            this.WriteLine(string.Format("  {0} factor = new {0}();", this.EntityType.Name));
            this.WriteLine(string.Format(" EntityQueryRequest<{0}> query = new EntityQueryRequest<{0}>();", this.EntityType.Name));
            this.WriteLine("    EntityQueryRequest<T> q = query as EntityQueryRequest<T>;");
            this.WriteLine("    action(factor, q);");
            this.WriteLine("    return q;");
            this.WriteLine(" }");
            //多条件分页查询
            this.WriteLine("  public override EntityPageQueryRequest<T> ToFilterPageRequest<T>(Action<object, EntityPageQueryRequest<T>> action, int PageIndex, int PageSize)  ");
            this.WriteLine("{");
            this.WriteLine(string.Format("  {0} factor = new {0}();", this.EntityType.Name));
            this.WriteLine(string.Format(" EntityPageQueryRequest<{0}> query = new EntityPageQueryRequest<{0}>()", this.EntityType.Name));
            this.WriteLine("{");
            this.WriteLine("      PageSize = PageSize,");
            this.WriteLine("   PageIndex = PageIndex");
            this.WriteLine(" };");
            this.WriteLine(string.Format("query.SetOrderByMember(factor.{0}, () => factor.{0});", this.Key));
            this.WriteLine("   EntityPageQueryRequest<T> q = query as EntityPageQueryRequest<T>;");
            this.WriteLine(" action(factor, q);");
            this.WriteLine(" return q;");
            this.WriteLine(" }");
            //删除ID请求
            this.WriteLine("    public override   EntityPostRequest ToDeleteIDRequest<T>(object KeyValue)");
            this.WriteLine("{");

            this.WriteLine(string.Format("  {0} factor = new {0}()", this.EntityType.Name));
            this.WriteLine("{");
            this.WriteLine(string.Format("{0} = ({1})KeyValue", this.Key, GetFieldTypeName(this.Key)));
            this.WriteLine("};");
            this.WriteLine("   EntityPostRequest request = new EntityPostRequest(EntityPostRequest.EntityPostMethod.Delete)");
            this.WriteLine("{");
            this.WriteLine("Entity = factor");
            this.WriteLine(" };");
            this.WriteLine(string.Format("request.AppendConditionMap(factor.{0}, () => factor.{0});", this.Key));
            this.WriteLine("return request;");
            this.WriteLine("}");
            //删除请求
            this.WriteLine("  public override EntityPostRequest ToDeleteRequest(object entity)");
            this.WriteLine("{");
            this.WriteLine(string.Format("  {0} nt = entity as {0};", this.EntityType.Name));
            this.WriteLine(" EntityPostRequest request = new EntityPostRequest(EntityPostRequest.EntityPostMethod.Delete)");
            this.WriteLine("{");
            this.WriteLine("  Entity = nt");
            this.WriteLine("};");
            this.WriteLine(string.Format("  request.AppendConditionMap(nt.{0}, () => nt.{0});", this.Key));
            this.WriteLine(" return request;");
            this.WriteLine("}");
            //更新请求
            this.WriteLine("   public override EntityPostRequest ToUpdateRequest(object entity)");
            this.WriteLine("{");
            this.WriteLine(string.Format("  {0} nt = entity as {0};", this.EntityType.Name));
            this.WriteLine(" if (nt is ILastUpdate)");
            this.WriteLine("   {");
            this.WriteLine("    ILastUpdate lud = nt as ILastUpdate;");
            this.WriteLine("    lud.LastTime = Actor.Public.BeijingNow();");
            this.WriteLine("}");
            this.WriteLine("  EntityPostRequest Request;");
            this.WriteLine("  if (nt.IsPersisted())");
            this.WriteLine("{");
            this.WriteLine("    Request = new EntityPostRequest(EntityPostRequest.EntityPostMethod.Update)");
            this.WriteLine("  {");
            this.WriteLine("     Entity = nt");
            this.WriteLine("  };");
            this.WriteLine(string.Format("  Request.AppendConditionMap(nt.{0}, () => nt.{0});", this.Key));
            this.WriteLine("}");
            this.WriteLine(" else");
            this.WriteLine(" {");
            this.WriteLine("    Request = new EntityPostRequest(EntityPostRequest.EntityPostMethod.Create)");
            this.WriteLine("    {");
            this.WriteLine("   Entity = nt");
            this.WriteLine("   };");
            this.WriteLine(string.Format("   Request.AppendFieldMap(nt.{0}, () => nt.{0});", this.Key));
            this.WriteLine("  }");
            foreach (ColumnMap map in this.columns.Values)
            {
                if (map.FieldName != this.Key)
                    this.WriteLine(string.Format("   Request.AppendFieldMap(nt.{0} , () => nt.{0} );", map.FieldName));
            }

            this.WriteLine("  return Request;");
            this.WriteLine("}");
            //类结束
            this.WriteLine("}");
            this.WriteLine("}");
        }
        public string BuildClassName()
        {
            return string.Format("{0}_Repository", this.EntityType.Name);
        }
        public string BuildUpdateMap(ColumnMap map)
        {
            return string.Format("AppendDataMap(type, \"{0}\", () => entity.{1});", map.ColumnName, map.FieldName);
        }
        public string GetFieldTypeName(string FieldName)
        {
            ColumnMap cm = default(ColumnMap);
            if (this.columns.TryGetValue(FieldName, out cm))
            {
                return cm.MemberExpression.Type.FullName;
            }
            return string.Empty;
        }
        public string BuildQueryMap(string InstanceName, ColumnMap map)
        {
            string method = string.Empty;
            Type type = map.MemberExpression.Type;
            if (type == typeof(int))
            {
                method = "GetInt";
            }
            else if (type == typeof(int?))
            {
                method = "GetIntNullable";
            }
            else if (type == typeof(long))
            {
                method = "GetLong";
            }
            else if (type == typeof(long?))
            {
                method = "GetLongNullable";
            }
            else if (type == typeof(Decimal))
            {
                method = "GetDecimal";
            }
            else if (type == typeof(Decimal?))
            {
                method = "GetDecimalNullable";
            }
            else if (type == typeof(DateTime))
            {
                method = "GetDateTime";
            }
            else if (type == typeof(DateTime?))
            {
                method = "GetDateTimeNullable";
            }
            else if (type == typeof(bool))
            {
                method = "GetBoolean";
            }
            else if (type == typeof(bool?))
            {
                method = "GetBooleanNullable";
            }
            else if (type == typeof(Guid))
            {
                method = "GetGuid";
            }
            else if (type == typeof(Guid?))
            {
                method = "GetGuidNullable";
            }
            else if (type == typeof(string))
            {
                method = "GetStringNullable";
            }
            StringBuilder sb = new StringBuilder();
            string s = string.Empty;
            if (!string.IsNullOrEmpty(method))
            {
                s = string.Format("{3}.{0}=reader.{1}(\"{2}\");", map.FieldName, method, map.ColumnName, InstanceName);
            }
            else
            {
                s = string.Format("{3}.{0}=reader.GetValue<{1}>(\"{2}\");", map.FieldName, map.MemberExpression.Type.Name, map.ColumnName, InstanceName);
            }
            sb.Append("try{");
            sb.Append(s);
            sb.Append("}");

            sb.Append("catch{");
            sb.Append("}");
            return sb.ToString();
        }
    }
}

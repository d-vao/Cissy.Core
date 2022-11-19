using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Cissy.Tools.Templates
{
    /// <summary>
    /// 模板实体
    /// </summary>
    [Serializable]
    public class TemplateEntity
    {
        //public event EntityUpdateHandler EntityUpdating;
        //public event EntityUpdateHandler EntityUpdated;
        //public event EntityDeleteHandler EntityDeleting;
        //public event EntityDeleteHandler EntityDeleted;
    }
    public class ORM
    {
        public Type EntityType { get; set; }
        public string TableName { get; set; }
        public string KeyField { get; set; }
        public bool IsView { get; set; }
        public Dictionary<string, ColumnMap> Columns { get; set; }
    }
    public class ColumnMap
    {
        public string FieldName { get; set; }
        public string ColumnName { get; set; }
        public Expression MemberExpression { get; set; }
        public string DbType { get; set; }
    }
    public interface IMapBuilder
    {

        ORM[] Build();
    }
    public abstract class MapBuilderBase : IMapBuilder
    {
        Dictionary<Type, ORM> orms = new Dictionary<Type, ORM>();
        public ORM[] Build()
        {
            Register();
            return orms.Values.ToArray();
        }
        public void RegisterEntity<T>(Action<ORMBuilder<T>> BuildAction) where T : TemplateEntity
        {
            ORMBuilder<T> map = new ORMBuilder<T>();
            BuildAction(map);
            ORM m = map.Build();
            orms[m.EntityType] = m;
        }
        public abstract void Register();
    }
    public class ORMBuilder<T> where T : TemplateEntity
    {
        public ORMBuilder()
        {
            this.TableName = string.Empty;
            this.KeyField = string.Empty;
            this.Columns = new Dictionary<string, ColumnMap>();
        }
        bool IsView { get; set; }
        string TableName { get; set; }
        string KeyField { get; set; }
        Dictionary<string, ColumnMap> Columns;
        public ORM Build()
        {
            return new ORM()
            {
                EntityType = typeof(T),
                Columns = this.Columns,
                KeyField = this.KeyField,
                TableName = this.TableName,
                IsView = this.IsView
            };
        }
        public ORMBuilder<T> MapColumn(Expression<Func<T, object>> expression, string ColumnName, string DbType = "")
        {
            ColumnMap ColumnMap = expression.GetColumnMap(ColumnName);
            ColumnMap.DbType = DbType;
            if (ColumnMap != null)
            {
                this.Columns[ColumnMap.FieldName] = ColumnMap;
            }
            return this;
        }
        public ORMBuilder<T> MapTable(string TableName, bool IsView = false)
        {
            this.TableName = TableName;
            this.IsView = IsView;
            return this;
        }
        public ORMBuilder<T> MapKey(Expression<Func<T, object>> expression)
        {
            this.KeyField = expression.GetMemberFromExpression().Name;
            return this;
        }
    }
    public static class TemplateExtensions
    {
        internal static MemberInfo GetMemberFromExpression<T>(this Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                return memberExpression.Member;
            }
            else
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression.Body;

                if (unaryExpression.Operand is MemberExpression)
                {
                    MemberExpression memberExpression = (MemberExpression)unaryExpression.Operand;
                    return memberExpression.Member;
                }
            }
            return null;
        }
        internal static ColumnMap GetColumnMap<T>(this Expression<Func<T, object>> expression, string ColumnName)
        {
            if (expression.Body is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                return new ColumnMap()
                {
                    FieldName = memberExpression.Member.Name,
                    ColumnName = ColumnName,
                    MemberExpression = memberExpression
                };
            }
            else
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression.Body;

                if (unaryExpression.Operand is MemberExpression)
                {
                    MemberExpression memberExpression = (MemberExpression)unaryExpression.Operand;
                    return new ColumnMap()
                    {
                        FieldName = memberExpression.Member.Name,
                        ColumnName = ColumnName,
                        MemberExpression = memberExpression
                    };
                }
            }
            return default(ColumnMap);

            if (expression.Body is Expression)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                return new ColumnMap()
                {
                    FieldName = expression.GetMemberFromExpression().Name,
                    ColumnName = ColumnName,
                    MemberExpression = memberExpression
                };
            }
            return default(ColumnMap);
        }

    }
}

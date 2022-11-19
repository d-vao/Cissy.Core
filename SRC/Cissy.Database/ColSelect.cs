using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Cissy.Database;

namespace Cissy.Dapper
{
    public sealed class ColSelect<T> where T : IEntity
    {
        List<string> _list = new List<string>();
        public IEnumerable<string> Cols { get { return _list.AsEnumerable(); } }
        public ColSelect<T> Col(Expression<Func<T, object>> expression)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            string Name = propertyInfo.Name;
            if (!_list.Contains(Name))
                _list.Add(Name);
            return this;
        }
    }
    public sealed class FieldSelect<T> where T : IEntity
    {
        Dictionary<string, object> list = new Dictionary<string, object>();
        Dictionary<string, Type> typelist = new Dictionary<string, Type>();
        public IEnumerable<KeyValuePair<string, object>> Cols { get { return list.AsEnumerable(); } }
        public IEnumerable<KeyValuePair<string, Type>> Types { get { return typelist.AsEnumerable(); } }
        public FieldSelect<T> Col(Expression<Func<T, object>> expression, object v)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            string Name = propertyInfo.Name;
            Type type = propertyInfo.PropertyType;
            if (!list.Keys.Contains(Name))
            {
                list[Name] = v;
                typelist[Name] = type;
            }
            return this;
        }
    }
    public class FieldDescriptor
    {
        public FieldDescriptor(string fieldName, Type fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }
        public string FieldName { get; }
        public Type FieldType { get; }
    }
    public static class QueryHelper
    {
        public static object Condition<T>(Action<FieldSelect<T>> FieldSelector) where T : IEntity
        {
            if (FieldSelector.IsNotNull())
            {
                string TypeName = typeof(T).FullName + "_Q";
                FieldSelect<T> FieldSelect = new FieldSelect<T>();
                FieldSelector(FieldSelect);
                if (FieldSelect.Cols.Any())
                {
                    TypeName += string.Join("_", FieldSelect.Cols.Select(m => m.Value));
                    var myTypeInfo = CompileResultTypeInfo(TypeName, FieldSelect.Types);
                    var myType = myTypeInfo.AsType();
                    var myObject = Activator.CreateInstance(myType);
                    foreach (var c in FieldSelect.Cols)
                    {
                        PropertyInfo pi = myType.GetProperty(c.Key);
                        if (pi != default(PropertyInfo))
                        {
                            pi.SetValue(myObject, c.Value);
                        }
                    }
                    return myObject;
                }
            }
            return default;
        }

        public static TypeInfo CompileResultTypeInfo(string TypeName, IEnumerable<KeyValuePair<string, Type>> types)
        {
            TypeBuilder tb = GetTypeBuilder(TypeName);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var yourListOfFields = new List<FieldDescriptor>();
            foreach (var type in types)
            {
                yourListOfFields.Add(new FieldDescriptor(type.Key, type.Value));
            }
            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            TypeInfo objectTypeInfo = tb.CreateTypeInfo();
            return objectTypeInfo;
        }

        private static TypeBuilder GetTypeBuilder(string TypeName)
        {
            var typeSignature = TypeName;
            var an = new AssemblyName(typeSignature);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}

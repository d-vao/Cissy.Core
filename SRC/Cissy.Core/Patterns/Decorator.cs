
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy.Patterns
{
    public interface IDecorator<T> where T : class, new()
    {
        T Instance { get; set; }
    }
    public interface IDecorate<T> where T : class
    {
        T Decorator { get; }
        void InitDecorator(T obj);
    }
    public interface IDecorateContext<T> : IDecorate<T>
      where T : class
    {
        System.Collections.Generic.IDictionary<string, object> Context { get; }
        void InitContext(IDictionary<string, object> context);
    }
    public class Decorate<T> : IDecorateContext<T> where T : class
    {
        public void InitContext(IDictionary<string, object> context)
        {
            this.Context = context;
        }
        public void InitDecorator(T obj)
        {
            this.Decorator = obj;
        }
        public T Decorator { get; private set; }
        public IDictionary<string, object> Context { get; private set; }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DecoratorAttribute : Attribute
    {
        public DecoratorAttribute(params Type[] types)
        {
            _typeList = types;
        }
        private Type[] _typeList;
        public Type[] TypeList
        {
            get { return _typeList; }
        }
    }

    public sealed class DecorateFactory<T> where T : class
    {
        private IDictionary<string, Type> _types;
        private T _sourceObject;
        public DecorateFactory(T sourceObject)
        {
            _types = new Dictionary<string, Type>();
            _sourceObject = sourceObject;
            foreach (Attribute attr in sourceObject.GetType().GetCustomAttributes(true))
            {
                if (attr is DecoratorAttribute)
                {
                    DecoratorAttribute DecoratorAttribute = attr as DecoratorAttribute;
                    this.AddType(DecoratorAttribute.TypeList);
                }
            }
        }
        private void AddType(Type type)
        {
            _types[type.FullName] = type;
        }
        private void AddType(Type[] typeList)
        {
            foreach (Type type in typeList)
                this.AddType(type);
        }

        private T BuildInstance()
        {
            T result = _sourceObject;
            IDictionary<string, object> context = new Dictionary<string, object>();
            foreach (KeyValuePair<string, Type> pair in _types)
            {
                object obj = Activator.CreateInstance(pair.Value);
                if (obj is T)
                {
                    if (obj is IDecorateContext<T>)
                    {
                        IDecorateContext<T> ctxt = obj as IDecorateContext<T>;
                        ctxt.InitDecorator(result);
                        ctxt.InitContext(context);
                    }
                    result = (T)obj;
                }
            }
            return result;
        }
        public static T CreateInstance(T SourceObject)
        {
            return new DecorateFactory<T>(SourceObject).BuildInstance();
        }
        public static T CreateInstance(Type type, params object[] args)
        {
#if COMPACT_FRAMEWORK
            object obj = Activator.CreateInstance(type);
#else
            object obj = Activator.CreateInstance(type, args);
#endif
            if (obj is T)
            {
                T tObj = obj as T;
                return new DecorateFactory<T>(tObj).BuildInstance();
            }
            else
                throw new InvalidCastException("invalid instance type");
        }
    }
}

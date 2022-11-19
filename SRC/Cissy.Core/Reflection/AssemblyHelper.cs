using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Cissy.Reflection
{
    public static class AssemblyHelper
    {
        static Dictionary<string, Assembly> Assemblies;
        static Dictionary<string, Type> Types = new Dictionary<string, Type>();
        public static Type SearchType(this Public Public, string TypeFullName)
        {
            Type type = Type.GetType(TypeFullName);
            if (type.IsNotNull())
                return type;
            if (Types.TryGetValue(TypeFullName, out Type distType))
            {
                return distType;
            }
            if (Assemblies.IsNullOrEmpty())
                Assemblies = AppDomain.CurrentDomain.GetAssemblies().DistinctBy(n => n.FullName).ToDictionary(m => m.FullName);
            foreach (Assembly ab in Assemblies.Values)
            {
                foreach (Type tp in ab.GetTypes())
                {
                    Types[tp.FullName] = tp;
                    if (tp.FullName == TypeFullName)
                    {
                        return tp;
                    }
                }
            }
            return default;
        }
        public static void LoadAssambly(this Public Public, string AssamblyFileName)
        {
            var ab = Assembly.LoadFrom(AssamblyFileName);
            if (ab.IsNotNull())
            {
                Assemblies[ab.FullName] = ab;
                foreach (Type tp in ab.GetTypes())
                {
                    Types[tp.FullName] = tp;
                }
            }
        }
    }
}

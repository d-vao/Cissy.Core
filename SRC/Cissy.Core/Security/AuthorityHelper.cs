using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Linq;
using Cissy.Database;

namespace Cissy.Security
{

    public static class AuthorityHelper
    {
        static ConcurrentDictionary<string, Dictionary<string, IEnumerable<string>>> Permits = new ConcurrentDictionary<string, Dictionary<string, IEnumerable<string>>>();
        static ConcurrentDictionary<int, Type> PermitTypes = new ConcurrentDictionary<int, Type>();
        public static Tuple<int, string> RegisterPermit<T>(this Public Public) where T : struct
        {
            var droit = typeof(T).FullName;
            var permitId = AuthorityHelper.PermitId<T>();
            if (PermitTypes.TryGetValue(permitId, out Type type))
            {
                if (type != typeof(T))
                    throw new CissyException($"权限点类型ID冲突,请重新定义Id：{type.FullName}跟{typeof(T).FullName}之间");
            }
            else
            {
                PermitTypes[permitId] = typeof(T);
            }
            var enumname = EnumHelper.EnumName<T>();
            var authorityname = $"{enumname}:{permitId}";
            var groupname = AuthorityHelper.PermitGroup<T>();
            var rootname = $"{groupname}";
            if (!Permits.TryGetValue(rootname, out Dictionary<string, IEnumerable<string>> dict))
            {
                dict = new Dictionary<string, IEnumerable<string>>();
                Permits[rootname] = dict;
            }

            if (!dict.ContainsKey(authorityname))
            {
                List<string> list = new List<string>();
                foreach (T t in EnumHelper.All<T>())
                {
                    list.Add($"{t.StringValue()}:{t.Value()}");
                }
                dict[authorityname] = list;
            }
            return new Tuple<int, string>(permitId, droit);
        }
        public static ConcurrentDictionary<string, Dictionary<string, IEnumerable<string>>> GetPermits(this Public Public)
        {
            return Permits;
        }
        public static CissyAuthority[] MergeAuthority(this IEnumerable<IAuthority> authorities)
        {
            List<CissyAuthority> list = new List<CissyAuthority>();
            foreach (var scope in authorities.GroupBy(n => n.Scope))
            {
                foreach (var group in scope.GroupBy(m => m.PermitId))
                {
                    CissyAuthority da = new CissyAuthority();
                    da.Scope = scope.Key;
                    da.PermitId = group.Key;
                    int k = 0;
                    foreach (var v in group)
                    {
                        da.Droit = v.Droit;
                        k = k | v.Power;
                    }
                    da.Power = k;
                    list.Add(da);
                }
            }
            return list.ToArray();
        }
        public static string PermitGroup(Type type)
        {
            Attribute[] attributes = type.GetTypeInfo().GetCustomAttributes(typeof(PermitAttribute), false) as Attribute[];
            PermitAttribute PermitAttribute = attributes[0] as PermitAttribute;
            string name = PermitAttribute.PermitGroup;
            return name.IsNullOrEmpty() ? string.Empty : name;
        }
        public static string PermitGroup<EnumType>() where EnumType : struct
        {
            Attribute[] attributes = typeof(EnumType).GetTypeInfo().GetCustomAttributes(typeof(PermitAttribute), false) as Attribute[];
            PermitAttribute PermitAttribute = attributes[0] as PermitAttribute;
            string name = PermitAttribute.PermitGroup;
            return name.IsNullOrEmpty() ? string.Empty : name;
        }      
        public static int PermitId(Type type)
        {
            Attribute[] attributes = type.GetTypeInfo().GetCustomAttributes(typeof(PermitAttribute), false) as Attribute[];
            PermitAttribute PermitAttribute = attributes[0] as PermitAttribute;
            return PermitAttribute.Id;
        }
        public static int PermitId<EnumType>() where EnumType : struct
        {
            Attribute[] attributes = typeof(EnumType).GetTypeInfo().GetCustomAttributes(typeof(PermitAttribute), false) as Attribute[];
            PermitAttribute PermitAttribute = attributes[0] as PermitAttribute;
            return PermitAttribute.Id;
        }
    }
}

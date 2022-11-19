using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Cissy
{
    /// <summary>
    /// 枚举辅助器
    /// </summary>
    public static class EnumHelper
    {
        public static int Value<EnumType>(this EnumType enu) where EnumType : struct
        {
            return Convert.ToInt32(enu);
        }
        /// <summary>
        /// 拆分枚举
        /// </summary>
        /// <typeparam name="EnumType">枚举类型</typeparam>
        /// <param name="enu">枚举值</param>
        /// <returns>枚举子集</returns>
        public static EnumType[] Parse<EnumType>(this EnumType enu) where EnumType : struct
        {
            List<EnumType> list = new List<EnumType>();
            EnumType[] ets = _Parse(enu);
            foreach (EnumType et in ets)
            {
                if (_Parse(et).Length == 1)
                    list.Add(et);
            }
            return list.ToArray();

        }
        static EnumType[] _Parse<EnumType>(EnumType enu) where EnumType : struct
        {
            string[] names = Enum.GetNames(typeof(EnumType));
            List<EnumType> list = new List<EnumType>();
            foreach (string name in names)
            {
                EnumType type = (EnumType)Enum.Parse(typeof(EnumType), name);
                if ((Convert.ToInt32(type) | Convert.ToInt32(enu)) == Convert.ToInt32(enu))
                    list.Add(type);
            }
            return list.ToArray();

        }
        public static int[] ParseValues(Type type, int value)
        {
            string[] names = Enum.GetNames(type);
            List<int> list = new List<int>();
            foreach (string name in names)
            {
                object oj = Enum.Parse(type, name);
                int v = Convert.ToInt32(oj);
                if ((v | value) == value)
                    list.Add(v);
            }
            return list.ToArray();

        }
        /// <summary>
        /// 转换值
        /// </summary>
        /// <typeparam name="EnumType">枚举类型</typeparam>
        /// <param name="MergeValue">枚举整型值</param>
        /// <returns>枚举子集</returns>
        public static EnumType[] Parse<EnumType>(this int MergeValue) where EnumType : struct
        {
            EnumType hrt = (EnumType)Enum.Parse(typeof(EnumType), MergeValue.ToString());
            return Parse<EnumType>(hrt);
        }
        /// <summary>
        /// 合并转换值
        /// </summary>
        /// <typeparam name="EnumType">枚举类型</typeparam>
        /// <param name="MergeValue">枚举整型值</param>
        /// <returns>枚举子集</returns>
        public static EnumType MergeParse<EnumType>(this int MergeValue) where EnumType : struct
        {
            EnumType hrt = (EnumType)Enum.Parse(typeof(EnumType), MergeValue.ToString());
            return hrt;
        }
        public static EnumType Total<EnumType>() where EnumType : struct
        {
            string[] names = Enum.GetNames(typeof(EnumType));
            List<EnumType> list = new List<EnumType>();
            int x = 0;
            foreach (string name in names)
            {
                EnumType type = (EnumType)Enum.Parse(typeof(EnumType), name);
                x = x | Convert.ToInt32(type);
            }
            return x.MergeParse<EnumType>();
        }
        public static EnumType[] All<EnumType>() where EnumType : struct
        {
            return EnumHelper.Total<EnumType>().Parse();
        }
        /// <summary>
        /// 枚举子集检测 
        /// </summary>
        /// <typeparam name="EnumType">枚举类型</typeparam>
        /// <param name="enu">母枚举值</param>
        /// <param name="enu2">子枚举值</param>
        /// <returns>是否包含</returns>
        public static bool Contains<EnumType>(this EnumType enu, EnumType enu2) where EnumType : struct
        {
            return Convert.ToInt32(enu.Intersect(enu2)) == Convert.ToInt32(enu2);
        }
        public static bool Contains(this int enu, int enu2)
        {
            return enu.Intersect(enu2) == enu2;
        }
        /// <summary>
        /// 两个枚举的交集
        /// </summary>
        /// <typeparam name="EnumType"></typeparam>
        /// <param name="enu"></param>
        /// <param name="enu2"></param>
        /// <returns>交集</returns>
        public static EnumType Intersect<EnumType>(this EnumType enu, EnumType enu2) where EnumType : struct
        {
            return (Convert.ToInt32(enu) & Convert.ToInt32(enu2)).MergeParse<EnumType>();
        }
        public static int Intersect(this int enu, int enu2)
        {
            return enu & enu2;
        }
        /// <summary>
        /// 获取枚举最大的枚举值名
        /// </summary>
        /// <typeparam name="EnumType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String StringMaxValue<EnumType>(this EnumType value) where EnumType : struct
        {
            Attribute[] attributes = typeof(EnumType).GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            if (!ReferenceEquals(attributes, null) && attributes.Length > 0)
            {
                Dictionary<int, string> StringsLookup = (attributes[0] as EnumStringsAttribute).StringsLookup;
                int[] enums = Parse(value) as int[];
                enums = enums.OrderBy(va => va).ToArray();
                string ret = "";
                if (StringsLookup.TryGetValue(enums[enums.Length - 1], out ret))
                {
                    return ret;
                }
            }
            return value.ToString();
        }
        public static string EnumName(Type type)
        {
            Attribute[] attributes = type.GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            EnumStringsAttribute EnumStringsAttribute = attributes[0] as EnumStringsAttribute;
            string name = EnumStringsAttribute.EnumName;
            return name.IsNullOrEmpty() ? string.Empty : name;
        }
        public static string EnumName<EnumType>() where EnumType : struct
        {
            Attribute[] attributes = typeof(EnumType).GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            EnumStringsAttribute EnumStringsAttribute = attributes[0] as EnumStringsAttribute;
            string name = EnumStringsAttribute.EnumName;
            return name.IsNullOrEmpty() ? string.Empty : name;
        }
        /// <summary>
        /// 获取枚举值名称列表
        /// </summary>
        /// <typeparam name="EnumType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String StringValue<EnumType>(this EnumType value) where EnumType : struct
        {
            Attribute[] attributes = typeof(EnumType).GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            if (!ReferenceEquals(attributes, null) && attributes.Length > 0)
            {
                Dictionary<int, string> StringsLookup = (attributes[0] as EnumStringsAttribute).StringsLookup;
                return GetStringValue<EnumType>(value, StringsLookup);
            }
            return value.ToString();
        }
        public static ICollection<KeyValuePair<int, string>> AllStringValue(Type type)
        {
            Attribute[] attributes = type.GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            if (!ReferenceEquals(attributes, null) && attributes.Length > 0)
            {
                Dictionary<int, string> StringsLookup = (attributes[0] as EnumStringsAttribute).StringsLookup;
                return StringsLookup;
            }
            return new Dictionary<int, string>();
        }
        public static ICollection<KeyValuePair<int, string>> StringValue(Type type, int value)
        {
            Attribute[] attributes = type.GetTypeInfo().GetCustomAttributes(typeof(EnumStringsAttribute), false) as Attribute[];
            if (!ReferenceEquals(attributes, null) && attributes.Length > 0)
            {
                Dictionary<int, string> StringsLookup = (attributes[0] as EnumStringsAttribute).StringsLookup;
                return GetStringValue(type, value, StringsLookup);
            }
            return new Dictionary<int, string>();
        }
        static ICollection<KeyValuePair<int, string>> GetStringValue(Type type, int value, Dictionary<int, string> StringsLookup)
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            string oret;
            if (StringsLookup.TryGetValue(value, out oret))
            {
                ret.Add(value, oret);
                return ret;
            }
            int[] enums = value.ToFlags();
            foreach (int et in enums)
            {
                foreach (KeyValuePair<int, string> pair in GetStringValue(type, et, StringsLookup))
                {
                    ret.Add(pair.Key, pair.Value);
                }
            }
            return ret;
        }

        static string GetStringValue<EnumType>(EnumType value, Dictionary<int, string> StringsLookup) where EnumType : struct
        {
            int enumValue = (value as IConvertible).ToInt32(CultureInfo.InvariantCulture);
            string ret = string.Empty;
            if (StringsLookup.TryGetValue(enumValue, out ret))
            {
                return ret;
            }
            EnumType[] enums = Parse(value);
            List<string> ls = new List<string>();
            foreach (EnumType et in enums)
            {
                ls.Add(GetStringValue(et, StringsLookup));
            }
            return string.Join(",", ls.ToArray());
        }
    }
}

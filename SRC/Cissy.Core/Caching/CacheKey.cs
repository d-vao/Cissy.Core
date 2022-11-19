using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Cissy.Caching
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public class CacheKey : IModel
    {
        public const string STAR = "*";
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module;
        /// <summary>
        /// 模型名称
        /// </summary>
        public string Model;
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName;
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue;
        /// <summary>
        /// 存储的目标名称
        /// </summary>
        public string TargetName;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version;
        public static string GetModule(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[0];
        }
        public static string GetModel(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[1];
        }
        public static string GetPropertyName(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[2];
        }
        public static string GetPropertyValue(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[3];
        }
        public static string GetTargetName(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[4];
        }
        public static string GetVersion(string KeyString)
        {
            return KeyString.Split(new string[] { ":" }, StringSplitOptions.None)[5];
        }
        public override string ToString()
        {
            return $"{Module}:{Model}:{PropertyName}:{PropertyValue}:{TargetName}:{Version}";
        }
        public string ToPathPattern(Action<ICollection<string>, CacheKey> action = null)
        {
            if (action.IsNull())
                return STAR;
            List<string> list = new List<string>();
            action(list, this);
            return string.Join(":", list) + STAR;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy
{
    public interface ITemplateExpression
    {

        /// <summary>
        /// 配置模板表达式
        /// </summary>
        string Expression { get; set; }
        /// <summary>
        /// 配置模板值
        /// </summary>
        string Value { get; set; }
    }
    public static class TemplateExpressionExtensions
    {
        const string SPLITESPACER = "<$#!>";
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this ITemplateExpression expression)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] exs = expression.Expression.Split(new string[] { SPLITESPACER }, StringSplitOptions.RemoveEmptyEntries);
            string[] vas = expression.Value.Split(new string[] { SPLITESPACER }, StringSplitOptions.None);
            if (exs.Length > 0)
            {
                if (exs.Length != vas.Length)
                    throw new Exception("表达式跟值不匹配，请确保他们的元素一样多");

                for (int i = 0; i < exs.Length; i++)
                {
                    dic[exs[i]] = vas[i];
                }
            }
            return dic;
        }
        public static void FromKeys(this ITemplateExpression expression, IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                Validation(key);

            }
            expression.Expression = string.Join(SPLITESPACER, keys.ToArray());
        }
        public static void FromValues(this ITemplateExpression expression, IEnumerable<string> values)
        {
            foreach (string value in values)
            {
                Validation(value);
            }
            expression.Value = string.Join(SPLITESPACER, values.ToArray());
        }
        public static void AppendConfig(this ITemplateExpression expression, string Key, string Value)
        {
            if (string.IsNullOrEmpty(Key))
                throw new Exception("配置键不能为空");
            expression.Validation(Key);
            expression.Validation(Value);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in expression.ToKeyValuePairs())
            {
                dic.Add(pair.Key, pair.Value);
            }
            dic.Add(Key, Value);
            expression.FromKeys(dic.Keys);
            expression.FromValues(dic.Values);
        }

        public static bool Validation(string s)
        {
            return !s.Contains(SPLITESPACER);

        }
        public static void Validation(this ITemplateExpression expression, string s)
        {
            if (!Validation(s))
                throw new Exception(string.Format("配置键和配置值均不能包含{0}字符", SPLITESPACER));
        }
    }
}

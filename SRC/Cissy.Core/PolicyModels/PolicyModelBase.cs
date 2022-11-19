using System;
using System.Collections.Generic;
//using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Reflection;
using Cissy;

namespace Cissy.PolicyModels
{
    public abstract class PolicyModelBase<T> : EntityModel<T>, IPolicyModel<T> where T : class, IPolicyModule
    {
        /// <summary>
        ///  策略模型键
        /// </summary>
        public string PolicyModelKey { get; private set; }
        Dictionary<MemberInfo, PolicyVariantAttribute> mvs = new Dictionary<MemberInfo, PolicyVariantAttribute>();
        /// <summary>
        /// 变体成员名集
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> VariantMembers
        {
            get
            {
                foreach (KeyValuePair<MemberInfo, PolicyVariantAttribute> pair in this.mvs)
                {
                    yield return new KeyValuePair<string, string>(pair.Key.Name, pair.Value.VariantName);
                }
            }
        }

        public PolicyModelBase(string PolicyModelKey, T policy)
            : base(policy)
        {
            this.PolicyModelKey = PolicyModelKey;
            LoadVariant();
        }
        void LoadVariant()
        {
            this.mvs.Clear();
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                PolicyVariantAttribute[] attrs = pi.GetCustomAttributes(typeof(PolicyVariantAttribute), true) as PolicyVariantAttribute[];
                if (!attrs.IsNullOrEmpty())
                {
                    mvs[pi] = attrs[0];
                }
            }
            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                PolicyVariantAttribute[] attrs = fi.GetCustomAttributes(typeof(PolicyVariantAttribute), true) as PolicyVariantAttribute[];
                if (!attrs.IsNullOrEmpty())
                {
                    mvs[fi] = attrs[0];
                }
            }
        }
        /// <summary>
        /// 填充策略表达式，不填充值
        /// </summary>
        public virtual void FillPolicyExpression()
        {
            List<string> values;
            this.Entity.FromKeys(MergeItems(out values));
            this.Entity.PolicyModelKey = this.PolicyModelKey;
        }
        /// <summary>
        /// 填充值，不填充策略表达式
        /// </summary>
        public virtual void FillPolicyValue()
        {
            List<string> values;
            MergeItems(out values);
            this.Entity.FromValues(values);
        }
        /// <summary>
        /// 填充模型变体值
        /// </summary>
        public virtual void FillPolicyModelVariant()
        {
            Dictionary<string, string> configs = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> pair in this.Entity.ToKeyValuePairs())
            {
                configs[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<MemberInfo, PolicyVariantAttribute> pair in this.mvs)
            {
                if (pair.Key is PropertyInfo)
                {
                    PropertyInfo pi = pair.Key as PropertyInfo;
                    string val;
                    if (configs.TryGetValue(pair.Value.VariantName, out val))
                    {
                        pi.SetValue(this, Convert.ChangeType(val, pi.PropertyType), null);
                    }
                }
                if (pair.Key is FieldInfo)
                {
                    FieldInfo fi = pair.Key as FieldInfo;
                    string val;
                    if (configs.TryGetValue(pair.Value.VariantName, out val))
                    {
                        fi.SetValue(this, Convert.ChangeType(val, fi.FieldType));
                    }
                }
            }

        }
        List<string> MergeItems(out List<string> values)
        {
            List<string> ls = new List<string>();
            List<string> list = new List<string>();
            foreach (KeyValuePair<MemberInfo, PolicyVariantAttribute> pair in this.mvs)
            {
                list.Add(pair.Value.VariantName);

                if (pair.Key is PropertyInfo)
                {
                    PropertyInfo pi = pair.Key as PropertyInfo;
                    object obj = pi.GetValue(this, null);
                    ls.Add(obj.IsNull() ? string.Empty : obj.ToString());
                }
                if (pair.Key is FieldInfo)
                {
                    FieldInfo fi = pair.Key as FieldInfo;
                    object obj = fi.GetValue(this);
                    ls.Add(obj.IsNull() ? string.Empty : obj.ToString());
                }
            }
            values = ls;
            return list;
        }
        /// <summary>
        /// 获取变体值
        /// </summary>
        /// <param name="MemberName">变体成员名</param>
        /// <returns></returns>
        public object GetVariantValue(string MemberName)
        {
            MemberInfo member = this.mvs.Keys.First(m => m.Name.Equals(MemberName, StringComparison.OrdinalIgnoreCase));
            if (member.IsNotNull())
            {
                if (member is PropertyInfo)
                {
                    PropertyInfo pi = member as PropertyInfo;
                    return pi.GetValue(this, null);
                }
                else
                    if (member is FieldInfo)
                {
                    FieldInfo fi = member as FieldInfo;
                    return fi.GetValue(this);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取变体值
        /// </summary>
        /// <param name="VariantName">变体名</param>
        /// <returns></returns>
        public object GetVariantValueByAlias(string VariantName)
        {
            MemberInfo member = this.mvs.FirstOrDefault(m => m.Value.VariantName.Equals(VariantName, StringComparison.OrdinalIgnoreCase)).Key;
            if (member.IsNotNull())
            {
                if (member is PropertyInfo)
                {
                    PropertyInfo pi = member as PropertyInfo;
                    return pi.GetValue(this, null);
                }
                else
                    if (member is FieldInfo)
                {
                    FieldInfo fi = member as FieldInfo;
                    return fi.GetValue(this);
                }
            }
            return null;
        }
        /// <summary>
        /// 设置变体值
        /// </summary>
        /// <param name="MemberName">变体成员名</param>
        /// <param name="value">变体值</param>
        public void SetVariantValue(string MemberName, object value)
        {
            MemberInfo member = this.mvs.Keys.First(m => m.Name.Equals(MemberName, StringComparison.OrdinalIgnoreCase));
            if (member.IsNotNull())
            {
                if (member is PropertyInfo)
                {
                    PropertyInfo pi = member as PropertyInfo;
                    pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType), null);
                }
                else
                    if (member is FieldInfo)
                {
                    FieldInfo fi = member as FieldInfo;
                    fi.SetValue(this, Convert.ChangeType(value, fi.FieldType));
                }
            }
        }
        /// <summary>
        /// 设置变体值
        /// </summary>
        /// <param name="VariantName">变体名</param>
        /// <param name="value">变体值</param>
        public void SetVariantValueByAlias(string VariantName, object value)
        {
            MemberInfo member = this.mvs.FirstOrDefault(m => m.Value.VariantName.Equals(VariantName, StringComparison.OrdinalIgnoreCase)).Key;
            if (member.IsNotNull())
            {
                if (member is PropertyInfo)
                {
                    PropertyInfo pi = member as PropertyInfo;
                    pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType), null);
                }
                else
                    if (member is FieldInfo)
                {
                    FieldInfo fi = member as FieldInfo;
                    fi.SetValue(this, Convert.ChangeType(value, fi.FieldType));
                }
            }
        }
    }
}

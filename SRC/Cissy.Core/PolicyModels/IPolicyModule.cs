using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cissy.Database;

namespace Cissy.PolicyModels
{
    /// <summary>
    /// 策略模块信息
    /// </summary>
    public interface IPolicyModule : IEntity, ITemplateExpression
    {
        string ID { get; }
        #region 系统关联
        /// <summary>
        ///  策略模型键
        /// </summary>
        string PolicyModelKey { get; set; }

        #endregion
    }
    /// <summary>
    /// 策略模型
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IPolicyModel<Entity> : IEntityModel<Entity> where Entity : class, IPolicyModule
    {
        /// <summary>
        ///  策略模型键
        /// </summary>
        string PolicyModelKey { get; }
        /// <summary>
        /// 填充模型变体值
        /// </summary>
        void FillPolicyModelVariant();
        /// <summary>
        /// 填充策略表达式，不填充值
        /// </summary>
        void FillPolicyExpression();
        /// <summary>
        /// 填充值，不填充策略表达式
        /// </summary>
        void FillPolicyValue();
        /// <summary>
        /// 变体成员名集
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> VariantMembers { get; }

        /// <summary>
        /// 获取变体值
        /// </summary>
        /// <param name="MemberName">变体成员名</param>
        /// <returns></returns>
        object GetVariantValue(string MemberName);
        /// <summary>
        /// 获取变体值
        /// </summary>
        /// <param name="VariantName">变体名</param>
        /// <returns></returns>
        object GetVariantValueByAlias(string VariantName);
        /// <summary>
        /// 设置变体值
        /// </summary>
        /// <param name="MemberName">变体成员名</param>
        /// <param name="value">变体值</param>
        void SetVariantValue(string MemberName, object value);
        /// <summary>
        /// 设置变体值
        /// </summary>
        /// <param name="VariantName">变体名</param>
        /// <param name="value">变体值</param>
        void SetVariantValueByAlias(string VariantName, object value);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy.PolicyModels
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public class PolicyVariantAttribute : Attribute
    {

        /// <summary>
        /// 变体名称
        /// </summary>
        public string VariantName { get; private set; }
        public PolicyVariantAttribute(string variantName)
        {
            TemplateExpressionExtensions.Validation(variantName);
            this.VariantName = variantName;
        }
    }
}

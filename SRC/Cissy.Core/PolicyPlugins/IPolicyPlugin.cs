using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Plugins;

namespace Cissy.PolicyPlugins
{
    public interface IPolicyPlugin<Data> : IPlugin where Data : IPolicyData
    {
        /// <summary>
        /// 策略名称，唯一
        /// </summary>
        string PolicyName { get; }
        /// <summary>
        /// 策略说明
        /// </summary>
        string Description { get; }
        Data PolicyData { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="jsonData"></param>
        void Init(string jsonData);
    }
}

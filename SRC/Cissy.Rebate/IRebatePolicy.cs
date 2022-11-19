using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Rebate
{
    /// <summary>
    /// 返利策略
    /// </summary>
    public interface IRebatePolicy
    {
        string PolicyName { get; }
        /// <summary>
        /// 执行推荐流程
        /// </summary>
        /// <param name="Recommendation"></param>
        /// <returns></returns>
        int Execute(IRebateBill Recommendation);
        /// <summary>
        /// 解析json数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        IRebateBill ParseBill(string json);

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Rebate
{
    /// <summary>
    /// 人际关系链接请求
    /// </summary>
    public interface ILinkRequest : IModel
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        string AppName { get; }
        /// <summary>
        /// 消费者用户Id
        /// </summary>
        string ConsumerId { get; }
        /// <summary>
        /// 父级用户Id
        /// </summary>
        string ParentId { get; }
        /// <summary>
        /// 商户ID
        /// </summary>
        long MerchantId { get; }
        /// <summary>
        /// 主体ID
        /// </summary>
        long SubjectID { get; }
    }
}

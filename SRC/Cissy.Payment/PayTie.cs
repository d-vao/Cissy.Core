using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Payment
{

    [EnumStrings(typeof(PayTie), "微信支付", "支付宝", "银联", "其他支付", EnumName = "支付体系")]
    [Flags]
    public enum PayTie
    {
        Ten = 1,
        Ali = 2,
        UnionPay = 3,
        Other = 4
    }
}

using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    /// <summary>
    /// 身份证识别返回结果
    /// </summary>
    public class IDCardOCRResult : WxJsonResult
    {
        /// <summary>
        /// 正面：Front,反面：Back
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 有效期，方面时有效
        /// </summary>
        public string valid_date { get; set; }
        /// <summary>
        /// 姓名，正面时有效
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 身份证号码，正面时有效
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 银行卡识别返回接口
    /// </summary>
    public class BankCardOCRResult : WxJsonResult
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string number { get; set; }
    }
    /// <summary>
    /// 行驶证识别返回接口
    /// </summary>
    public class DrivingOCRResult : WxJsonResult
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string plate_num { get; set; }
        /// <summary>
        /// 车型，如：小型普通客⻋
        /// </summary>
        public string vehicle_type { get; set; }
        /// <summary>
        /// 车辆所有者
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// 营运类型，如： 非营运
        /// </summary>
        public string use_character { get; set; }
        public string model { get; set; }
        public string vin { get; set; }
        public string engine_num { get; set; }
        public string register_date { get; set; }
        public string issue_date { get; set; }
        public string plate_num_b { get; set; }
        public string record { get; set; }
        public string passengers_num { get; set; }
        public string total_quality { get; set; }
        public string prepare_quality { get; set; }
    }
}

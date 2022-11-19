using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinMpOCR
    {
        /// <summary>
        /// 扫描身份证信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        Task<IDCardOCRResult> ScanIDCardAsync(string imgUrl);
        /// <summary>
        /// 扫描银行卡信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        Task<BankCardOCRResult> ScanBankCardAsync(string imgUrl);
        /// <summary>
        /// 扫描行驶证信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        Task<DrivingOCRResult> ScanDrivingAsync(string imgUrl);
    }
}

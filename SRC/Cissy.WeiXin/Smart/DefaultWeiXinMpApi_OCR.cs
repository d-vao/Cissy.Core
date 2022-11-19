using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Caching;
using Cissy.Configuration;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi
    {
        /// <summary>
        /// 扫描身份证信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public async Task<IDCardOCRResult> ScanIDCardAsync(string imgUrl)
        {
            var urlFormat = WeiXinApiHelper.BaseUrlssl + $"/cv/ocr/idcard?type=photo&img_url={imgUrl}" + "&access_token={0}";
            return await CommonJsonSend.SendAsync<IDCardOCRResult, StringRef>(await this.GetAccessTokenAsync(), urlFormat, null, CommonJsonSendType.POST);

        }
        /// <summary>
        /// 扫描银行卡信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public async Task<BankCardOCRResult> ScanBankCardAsync(string imgUrl)
        {
            var urlFormat = WeiXinApiHelper.BaseUrlssl + $"/cv/ocr/bankcard?img_url={imgUrl}" + "&access_token={0}";
            return await CommonJsonSend.SendAsync<BankCardOCRResult, StringRef>(await this.GetAccessTokenAsync(), urlFormat, null, CommonJsonSendType.POST);
        }
        /// <summary>
        /// 扫描行驶证信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public async Task<DrivingOCRResult> ScanDrivingAsync(string imgUrl)
        {
            var urlFormat = WeiXinApiHelper.BaseUrlssl + $"/cv/ocr/driving?img_url={imgUrl}" + "&access_token={0}";
            return await CommonJsonSend.SendAsync<DrivingOCRResult, StringRef>(await this.GetAccessTokenAsync(), urlFormat, null, CommonJsonSendType.POST);
        }

    }
}

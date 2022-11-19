using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cissy.Tools.Templates
{
    public static class HttpHelper
    {
        #region 异步
        public static async Task<bool> GetAsync(string Url, Action<string> action)
        {
            var apiClient = new HttpClient();

            var response = await apiClient.GetAsync(Url);
            if (!response.IsSuccessStatusCode)
            {
                action(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                action(content);
                return true;
            }
        }
        public static async Task<bool> PostAsync(string Url, HttpContent content, Action<string> action)
        {
            var apiClient = new HttpClient();

            var response = await apiClient.PostAsync(Url, content);
            if (!response.IsSuccessStatusCode)
            {
                action(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                action(result);
                return true;
            }
        }
        public static async Task<bool> GetJsonAsync<T>(string Url, Action<T> action, Action<string> FailedAction = null) 
        {
            var apiClient = new HttpClient();

            var response = await apiClient.GetAsync(Url);
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(content);
                action(result);
                return true;
            }
        }
        public static async Task<bool> PostJsonAsync<T>(string Url, HttpContent content, Action<T> action, Action<string> FailedAction = null) 
        {
            var apiClient = new HttpClient();

            var response = await apiClient.PostAsync(Url, content);
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var str = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(str);
                action(result);
                return true;
            }
        }
        public static async Task<Stream> DownloadAsync(string url)
        {
            var apiClient = new HttpClient();
            var response = await apiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                return default(Stream);
            }
        }
        #endregion
        #region 同步
        public static bool Get(string Url, Action<string> action)
        {
            var apiClient = new HttpClient();

            var response = apiClient.GetAsync(Url).Result;
            if (!response.IsSuccessStatusCode)
            {
                action(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                action(content);
                return true;
            }
        }
        public static bool Post(string Url, HttpContent content, Action<string> action)
        {
            var apiClient = new HttpClient();

            var response = apiClient.PostAsync(Url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                action(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                action(result);
                return true;
            }
        }
        public static bool GetJson<T>(string Url, Action<T> action, Action<string> FailedAction = null) 
        {
            var apiClient = new HttpClient();

            var response = apiClient.GetAsync(Url).Result;
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                T result = JsonConvert.DeserializeObject<T>(content);
                action(result);
                return true;
            }
        }
        public static bool PostJson<T>(string Url, HttpContent content, Action<T> action, Action<string> FailedAction = null) 
        {
            var apiClient = new HttpClient();

            var response = apiClient.PostAsync(Url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var str = response.Content.ReadAsStringAsync().Result;
                T result = JsonConvert.DeserializeObject<T>(str);
                action(result);
                return true;
            }
        }

        /// <summary>
        /// 从Url下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public static Stream Download(string url)
        {
            var apiClient = new HttpClient();
            var response = apiClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStreamAsync().Result;
            }
            else
            {
                return default(Stream);
            }
        }
        #endregion
    }
}

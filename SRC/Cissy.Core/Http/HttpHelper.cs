using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Cissy.Serialization.Json;
using Cissy.Configuration;

namespace Cissy.Http
{
    public static class HttpHelper
    {
        public static bool UseProxy { get; internal set; }
        #region 异步
        public static async Task<bool> GetAsync(this Public Public, string Url, Action<string> action, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);
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
        public static async Task<bool> PostAsync(this Public Public, string Url, HttpContent content, Action<string> action, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);

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
        public static async Task<bool> GetJsonAsync<T>(this Public Public, string Url, Action<T> action, Action<string> FailedAction = null, HttpClientHandler handler = null) where T : IModel
        {
            var apiClient = Public.CreateHttpClient(handler);

            var response = await apiClient.GetAsync(Url);
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                T result = SerializerHelper.GetObject<T>(content);
                action(result);
                return true;
            }
        }
        public static async Task<bool> PostJsonAsync<T>(this Public Public, string Url, HttpContent content, Action<T> action, Action<string> FailedAction = null, HttpClientHandler handler = null) where T : IModel
        {
            var apiClient = Public.CreateHttpClient(handler);

            var response = await apiClient.PostAsync(Url, content);
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var str = await response.Content.ReadAsStringAsync();
                T result = SerializerHelper.GetObject<T>(str);
                action(result);
                return true;
            }
        }
        public static async Task<Stream> DownloadAsync(this Public Public, string url, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);
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
        public static bool Get(this Public Public, string Url, Action<string> action, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);

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
        public static bool Post(this Public Public, string Url, HttpContent content, Action<string> action, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);

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
        public static bool GetJson<T>(this Public Public, string Url, Action<T> action, Action<string> FailedAction = null, HttpClientHandler handler = null) where T : IModel
        {
            var apiClient = Public.CreateHttpClient(handler);

            var response = apiClient.GetAsync(Url).Result;
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                T result = SerializerHelper.GetObject<T>(content);
                action(result);
                return true;
            }
        }
        public static bool PostJson<T>(this Public Public, string Url, HttpContent content, Action<T> action, Action<string> FailedAction = null, HttpClientHandler handler = null) where T : IModel
        {
            var apiClient = Public.CreateHttpClient(handler);

            var response = apiClient.PostAsync(Url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                FailedAction(response.StatusCode.ToString());
                return false;
            }
            else
            {
                var str = response.Content.ReadAsStringAsync().Result;
                T result = SerializerHelper.GetObject<T>(str);
                action(result);
                return true;
            }
        }

        /// <summary>
        /// 从Url下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public static Stream Download(this Public Public, string url, HttpClientHandler handler = null)
        {
            var apiClient = Public.CreateHttpClient(handler);
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
        public static HttpClient CreateHttpClient(this Public Public, HttpClientHandler handler = null)
        {
            if (UseProxy)
            {
                var proxy = Actor.Public.GetService<IHttpProxyFactory>().BuildProxy();
                if (handler.IsNull())
                    handler = new HttpClientHandler();
                handler.Proxy = proxy;
                return new HttpClient(handler);
            }
            else
            {
                return handler.IsNotNull() ? new HttpClient(handler) : new HttpClient();
            }
        }
        #endregion
    }
}

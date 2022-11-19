using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Cissy;
using IdentityModel.Client;
using System.Threading.Tasks;


namespace Cissy.IS4
{
    public static class AccessApiHelper
    {
        public static async Task<bool> DoRequestApi(this Public Public, string Url, string AccessToken, Action<string> action)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(AccessToken);

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
    }
}

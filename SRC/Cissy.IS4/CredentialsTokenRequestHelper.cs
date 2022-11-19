using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Cissy;
using IdentityModel.Client;
using System.Threading.Tasks;
namespace Cissy.IS4
{
    public static class CredentialsTokenRequestHelper
    {
        //public static async Task<bool> DoGetCredentialsToken(this Public Public, string Url, string ClientId, string ClientSecret, string Scope, Action<TokenResponse> action)
        //{
        //    DiscoveryClient dc = new DiscoveryClient(Url)
        //    {
        //        Policy = { RequireHttps = false }
        //    };
        //    var disco = await dc.GetAsync();
        //    if (disco.IsError)
        //    {
        //        return false;
        //    }

        //    // request token
        //    var client = new HttpClient();
        //    var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    {
        //        Address = disco.TokenEndpoint,
        //        ClientId = ClientId,
        //        ClientSecret = ClientSecret,
        //        Scope = Scope
        //    });

        //    if (token.IsError)
        //    {
        //        return false;
        //    }
        //    action(token);
        //    return true;
        //}
    }
}

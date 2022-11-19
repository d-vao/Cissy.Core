using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cissy.Serialization;
using System.Security.Cryptography;
using System.Security.Claims;
using Cissy.Authentication.JWT;


namespace Cissy.Rebate
{
    public class LinkAppToken
    {
        public string Token;
        public string AppName;
    }
    public class LinkArg
    {
        public const string LinkClaimType = "linkjson";
        public string BillJson { get; set; }
        public string AppName { get; set; }
    }
    public static class LinkArgHelper
    {
        public static LinkAppToken ToLinkAppToken(this LinkArg arg, string secret)
        {
            List<Claim> list = new List<Claim>();
            Claim orderid = new Claim(LinkArg.LinkClaimType, arg.BillJson);
            list.Add(orderid);
            DefaultJwtPrincipalBuilder jwtBuilder = new DefaultJwtPrincipalBuilder(secret);
            var token = jwtBuilder.BuildToken(list);

            LinkAppToken pac = new LinkAppToken()
            {
                Token = token,
                AppName = arg.AppName
            };
            return pac;
        }
        public static LinkArg FromLinkAppToken(this LinkAppToken apptoken, Func<string, string> GetSecret)
        {
            var secret = GetSecret(apptoken.AppName);
            if (secret.IsNullOrEmpty())
            {
                return default;
            }
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(secret);
            var claimsPrincipal = builder.GetPrincipal("linktoken", apptoken.Token);
            if (claimsPrincipal.IsNull())
            {
                return default;
            }
            LinkArg arg = new LinkArg();
            arg.BillJson = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == LinkArg.LinkClaimType).Value;
            arg.AppName = apptoken.AppName;
            return arg;
        }
    }
}

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
    public class RebateAppToken
    {
        public string Token;
        public string AppName;
    }
    public class RebateArg
    {
        public const string RebateClaimType = "rebatebilljson";
        public string BillJson { get; set; }
        public string AppName { get; set; }
    }
    public static class RebateArgHelper
    {
        public static RebateAppToken ToRebateAppToken(this RebateArg arg, string secret)
        {
            List<Claim> list = new List<Claim>();
            Claim orderid = new Claim(RebateArg.RebateClaimType, arg.BillJson);
            list.Add(orderid);
            DefaultJwtPrincipalBuilder jwtBuilder = new DefaultJwtPrincipalBuilder(secret);
            var token = jwtBuilder.BuildToken(list);

            RebateAppToken pac = new RebateAppToken()
            {
                Token = token,
                AppName = arg.AppName
            };
            return pac;
        }
        public static RebateArg FromRebateAppToken(this RebateAppToken apptoken, Func<string, string> GetSecret)
        {
            var secret = GetSecret(apptoken.AppName);
            if (secret.IsNullOrEmpty())
            {
                return default;
            }
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(secret);
            var claimsPrincipal = builder.GetPrincipal("rebatetoken", apptoken.Token);
            if (claimsPrincipal.IsNull())
            {
                return default;
            }
            RebateArg arg = new RebateArg();
            arg.BillJson = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == RebateArg.RebateClaimType).Value;
            arg.AppName = apptoken.AppName;
            return arg;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cissy.Serialization;
using System.Security.Cryptography;
using System.Security.Claims;
using Cissy.Authentication.JWT;

namespace Cissy.Payment
{
    public class BalanceAppToken
    {
        public string Token;
        public string AppName;
    }
    public class BalanceArg
    {
        public const string BalanceClaimType = "balancesjson";
        public string BillJson { get; set; }
        public string AppName { get; set; }
    }
    public static class BalanceArgHelper
    {
        public static BalanceAppToken ToBalanceAppToken(this BalanceArg arg, string secret)
        {
            List<Claim> list = new List<Claim>();
            Claim orderid = new Claim(BalanceArg.BalanceClaimType, arg.BillJson);
            list.Add(orderid);
            DefaultJwtPrincipalBuilder jwtBuilder = new DefaultJwtPrincipalBuilder(secret);
            var token = jwtBuilder.BuildToken(list);

            BalanceAppToken pac = new BalanceAppToken()
            {
                Token = token,
                AppName = arg.AppName
            };
            return pac;
        }
        public static BalanceArg FromBalanceAppToken(this BalanceAppToken apptoken, Func<string, string> GetSecret)
        {
            var secret = GetSecret(apptoken.AppName);
            if (secret.IsNullOrEmpty())
            {
                return default;
            }
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(secret);
            var claimsPrincipal = builder.GetPrincipal("balancetoken", apptoken.Token);
            if (claimsPrincipal.IsNull())
            {
                return default;
            }
            BalanceArg arg = new BalanceArg();
            arg.BillJson = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == BalanceArg.BalanceClaimType).Value;
            arg.AppName = apptoken.AppName;
            return arg;
        }
    }
}

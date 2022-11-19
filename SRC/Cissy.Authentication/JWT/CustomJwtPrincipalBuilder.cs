using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using Cissy.Authentication;
using JWT.Builder;
using JWT.Algorithms;

namespace Cissy.Authentication.JWT
{
    public class CustomJwtPrincipalBuilder : IJwtPrincipalBuilder
    {
        public string Secret { get; private set; }
        public CustomJwtPrincipalBuilder(string secret)
        {
            this.Secret = secret;
        }
        public string BuildToken(IEnumerable<Claim> claims, int ExpireMinutes = 20)
        {
            var jb = new JwtBuilder()
                       .WithAlgorithm(new HMACSHA256Algorithm())
                       .WithSecret(Secret);

            foreach (var claim in claims)
            {
                jb.AddClaim(claim.Type, claim.Value);
            }
            var token = jb.Build();
            return token;
        }
        public ClaimsPrincipal GetPrincipal(string AuthenticationType, string token)
        {
            var json = new JwtBuilder()
                  .WithSecret(this.Secret)
                  .MustVerifySignature()
              .Decode<IDictionary<string, string>>(token);
            var claimsIdentity = new ClaimsIdentity(json.Select(m => new Claim(m.Key, m.Value)), AuthenticationType);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}

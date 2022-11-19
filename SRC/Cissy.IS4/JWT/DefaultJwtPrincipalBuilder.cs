using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Cissy.Authentication;

namespace Cissy.IS4.JWT
{
    /// <summary>
    ///var hmac = new System.Security.Cryptography. HMACSHA256();
    ///var key = Convert.ToBase64String(hmac.Key);
    /// </summary>
    public class DefaultJwtPrincipalBuilder : IJwtPrincipalBuilder
    {
        public string Secret { get; private set; }
        public DefaultJwtPrincipalBuilder(string secret)
        {
            this.Secret = secret;
        }
        public string BuildToken(IEnumerable<Claim> claims, int ExpireMinutes = 60 * 24 * 30)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = now.AddMinutes(Convert.ToInt32(ExpireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }
        public ClaimsPrincipal GetPrincipal(string AuthenticationType, string token)
        {
            //try
            //{
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var symmetricKey = Convert.FromBase64String(Secret);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                AuthenticationType = AuthenticationType
                //AuthenticationType = JwtBearerDefaults.AuthenticationScheme
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            return principal;
            //}

            //catch (Exception)
            //{
            //    return null;
            //}
        }
    }
}

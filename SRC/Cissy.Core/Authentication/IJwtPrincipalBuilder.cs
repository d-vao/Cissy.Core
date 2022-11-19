using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace Cissy.Authentication
{
    public interface IJwtPrincipalBuilder
    {
        string Secret { get; }
        string BuildToken(IEnumerable<Claim> claims, int ExpireMinutes = 20);
        ClaimsPrincipal GetPrincipal(string AuthenticationType, string token);
    }
}

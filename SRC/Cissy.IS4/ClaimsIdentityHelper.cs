using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Security;
using Cissy.Authentication;
using System.Security.Claims;

namespace Cissy.IS4
{

    public static class ClaimsIdentityHelper
    {
        public static void BuildPermits(this ClaimsIdentity identity, IEnumerable<IAuthority> authorities)
        {
            if (authorities.IsNotNullAndEmpty())
            {
                List<string> list = new List<string>();
                foreach (var authority in authorities.MergeAuthority())
                {
                    string s = $"{authority.Scope}{AuthoritySeparators.FirstLevel}{authority.PermitId}{AuthoritySeparators.FirstLevel}{authority.Power}";
                    list.Add(s);
                }
                identity.AddClaim(new Claim(CissyClaimTypes.Permits, string.Join(AuthoritySeparators.Top, list)));
            }
        }
        public static void BuildRoles(this ClaimsIdentity identity, IEnumerable<IRole> roles)
        {
            if (roles.IsNotNullAndEmpty())
            {
                foreach (IRole role in roles)
                {
                    identity.AddClaim(new Claim(CissyClaimTypes.Role, role.Id.ToString()));
                }
            }
        }
    }
}

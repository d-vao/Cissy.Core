using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Cissy.Security;
using System.Security.Claims;

namespace Cissy.Authentication
{
    public class AuthorityContext
    {
        public const string AskScopeKey = "_askscope";
        public const string AuthorityContextKey = "_cissy_authority_context";
        public bool OK { get; internal set; } = false;
        public ClaimsPrincipal User { get; internal set; }
        public Dictionary<string, CissyAuthority> Inspect { get; internal set; } = new Dictionary<string, CissyAuthority>();
        public string AskScope { get; internal set; }
        public Dictionary<string, Dictionary<int, int>> AuthorityScopes { get; internal set; } = new Dictionary<string, Dictionary<int, int>>();
        public IEnumerable<string> Scopes { get { return this.AuthorityScopes.Keys; } }
        public Dictionary<int, int> AskScopeAuthority
        {
            get
            {
                Dictionary<int, int> dic = default;
                if (AskScope.IsNullOrEmpty())
                    return dic;
                AuthorityScopes.TryGetValue(AskScope, out dic);
                return dic;
            }
        }

    }
    public static class AuthorityContextHelper
    {
        public static AuthorityContext GetAuthorityContext(this HttpContext httpContext)
        {
            AuthorityContext context = default;
            if (httpContext.Items.TryGetValue(AuthorityContext.AuthorityContextKey, out object ac))
            {
                context = ac as AuthorityContext;
            }
            return context;
        }
    }
}

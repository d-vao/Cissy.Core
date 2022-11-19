using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Cissy.Security;
using Cissy.IS4;

namespace Cissy.IS4
{
    public abstract class ScopePermitAttribute : CissyPermitAttribute
    {
        public override void OnScopeExecute(ActionExecutingContext context, string AskScope, Dictionary<string, Dictionary<int, int>> AuthorityScopes)
        {
            if (context.Controller is ScopeAuthorityApiController mapictr)
            {
                mapictr.AskScope = AskScope;
                mapictr.AuthorityScopes = AuthorityScopes;
            }
        }
    }
}

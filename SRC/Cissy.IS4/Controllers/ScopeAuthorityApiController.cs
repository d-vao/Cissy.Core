using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Cissy.Database;
using Cissy.Configuration;
using Cissy.IS4;
using Cissy.Security;

namespace Cissy.IS4
{
    public abstract class ScopeAuthorityApiController : ApiController
    {
        public Dictionary<string, Dictionary<int, int>> AuthorityScopes { get; set; } = new Dictionary<string, Dictionary<int, int>>();
        public string AskScope { get; set; } = AuthoritySeparators.DefaultScope;
    }
}

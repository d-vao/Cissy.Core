using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Cissy.Database;
using Cissy.Configuration;
using Cissy.Authentication;
using Cissy.Security;

namespace Cissy.Authentication
{
    public abstract class ScopeAuthorityApiController : ApiController
    {
        public AuthorityContext AuthorityContext
        {
            get
            {
                return this.HttpContext.GetAuthorityContext();
            }
        }
    }
}

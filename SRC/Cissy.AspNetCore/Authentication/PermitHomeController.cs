using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cissy.Database;
using Cissy.Configuration;
using Cissy.Security;
using Cissy.Reflection;
using System.Reflection;

namespace Cissy.Authentication
{
    public abstract class PermitHomeController : ScopeAuthorityApiController
    {

        [HttpGet]
        [CissyAuthorize]
        public IActionResult MyPermits()
        {
            var userId = this.User.CissyUserId();
            var service = Actor.Public.GetService<IAuthorityService>();
            var authorities = service.GetUserAuthorities(userId);
            List<object> l = new List<object>();
            if (authorities.IsNotNullAndEmpty())
            {
                foreach (var authority in authorities.MergeAuthority())
                {
                    Type type = Actor.Public.SearchType(authority.Droit);
                    if (type.IsNotNull())
                    {
                        var s = string.Join(",", EnumHelper.ParseValues(type, authority.Power));
                        var a = new { PermitId = authority.PermitId, Droit = authority.Droit, Scope = authority.Scope, Power = s };
                        l.Add(a);
                    }
                }
            }
            return Success(l);
        }

        [HttpGet]
        public IActionResult AllPermits()
        {
            return Success(Actor.Public.GetPermits());
        }
    }
}

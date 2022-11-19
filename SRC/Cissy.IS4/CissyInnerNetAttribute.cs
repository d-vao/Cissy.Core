using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;

namespace Cissy.IS4
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CissyInnerNetAttribute : Attribute, IAsyncAuthorizationFilter
    {
        int statusCode;
        public CissyInnerNetAttribute(int StatusCode = 403)
        {
            statusCode = StatusCode;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.IsInnerNet())
            {
                context.Result = new StatusCodeResult(statusCode);
            }
            await Task.CompletedTask;
        }
    }

}

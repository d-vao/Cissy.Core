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
using Cissy.Authentication;

namespace Cissy.IS4
{
    /// <summary>
    /// 多个权限申明叠加，只要满足其中一个即授权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CissyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter, IAsyncActionFilter
    {
        public Claim[] Claims { get; set; }
        //string TokenName = "Bearer";
        public CissyAuthorizeAttribute()
        {
        }
        public CissyAuthorizeAttribute(string claimType, string claimValue)
        {
            Claims = new Claim[] { new Claim(claimType, claimValue) };
        }
        public CissyAuthorizeAttribute(string claimType, params string[] claimValues)
        {
            List<Claim> cs = new List<Claim>();
            foreach (var v in claimValues)
            {
                cs.Add(Claim(claimType, v));
            }
            Claims = cs.ToArray();
        }
        public Claim Claim(string claimType, string claimValue)
        {
            return new Claim(claimType, claimValue);
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //var token = await context.HttpContext.GetTokenAsync(TokenName);

            //var authorization = context.HttpContext.Authentication;
            //if (authorization == null || authorization.Scheme != "Bearer")
            //    return;
            if (context.HttpContext.User.IsNull() || context.HttpContext.User.Claims.IsNullOrEmpty())
                context.Result = new StatusCodeResult(401);
            if (Claims.IsNotNull())
            {
                var hasClaim = context.HttpContext.User.Claims.Any(c => Claims.Any(m => c.Type == m.Type && c.Value == m.Value));
                if (!hasClaim)
                {
                    var hc = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                    if (!hc)
                        context.Result = new StatusCodeResult(401);
                    else
                        context.Result = new StatusCodeResult(403);
                }
            }
            else
            {
                var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                if (!hasClaim)
                {
                    context.Result = new StatusCodeResult(401);
                }
            }

            await Task.CompletedTask;
        }
        void _buildPrincipal(ActionExecutingContext context)
        {

        }
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            _buildPrincipal(context);
        }
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
        }

        //
        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }
            OnActionExecuting(context);
            if (context.Result == null)
            {
                OnActionExecuted(await next());
            }
        }
    }

}

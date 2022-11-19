using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authentication;
using Cissy.Authentication;
using Cissy.Security;

namespace Cissy.IS4
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //[ComVisible(true)]
    public abstract class CissyPermitAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter, IAsyncActionFilter
    {
        public const string AskScope = "_askcope";
        CissyAuthority CissyAuthority = default(CissyAuthority);
        protected Dictionary<string, Dictionary<int, int>> Scopes { get; private set; } = new Dictionary<string, Dictionary<int, int>>();
        protected void SetPower<T>(T requiredRight) where T : struct
        {
            var tp = Actor.Public.RegisterPermit<T>();
            CissyAuthority = new CissyAuthority()
            {
                PermitId = tp.Item1,
                Droit = tp.Item2,
                Power = requiredRight.Value()
            };
        }
        void _buildPrincipal(ActionExecutingContext context)
        {

        }
        //
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            _buildPrincipal(context);
            var scope = _getAuthorityScope(context);
            scope = scope.IsNullOrEmpty() ? AuthoritySeparators.DefaultScope : scope.Trim();
            OnScopeExecute(context, scope, this.Scopes);
            if (scope != AuthoritySeparators.DefaultScope)
            {
                if (!this.Scopes.ContainsKey(scope))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
        public abstract void OnScopeExecute(ActionExecutingContext context, string AskScope, Dictionary<string, Dictionary<int, int>> AuthorityScopes);
        string _getAuthorityScope(ActionExecutingContext context)
        {
            StringValues sv;
            if (context.HttpContext.Request.Query.TryGetValue(AskScope, out sv))
            {
                return sv.FirstOrDefault();
            }
            if (context.HttpContext.Request.Form.TryGetValue(AskScope, out sv))
            {
                return sv.FirstOrDefault();
            }
            if (context.HttpContext.Request.Headers.TryGetValue(AskScope, out sv))
            {
                return sv.FirstOrDefault();
            }
            if (context.HttpContext.Request.Cookies.TryGetValue(AskScope, out string str))
            {
                return str;
            }
            return default;
        }
        //
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
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsNull() || context.HttpContext.User.Claims.IsNullOrEmpty())
                context.Result = new StatusCodeResult(401);
            var cl = context.HttpContext.User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.Permits);
            if (cl.IsNull())
            {
                var hc = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                if (!hc)
                    context.Result = new StatusCodeResult(401);
                else
                    context.Result = new StatusCodeResult(403);
            }
            else
            {
                string[] vs = cl.Value.Split(AuthoritySeparators.Top, StringSplitOptions.RemoveEmptyEntries);
                bool find = false;
                foreach (string v in vs)
                {
                    string[] ms = v.Split(AuthoritySeparators.FirstLevel);
                    if (!this.Scopes.TryGetValue(ms[0], out Dictionary<int, int> dic))
                    {
                        dic = new Dictionary<int, int>();
                        this.Scopes[ms[0]] = dic;
                    }
                    dic[int.Parse(ms[1])] = int.Parse(ms[2]);

                    ///权限ID权限值匹配
                    if (ms[1] == this.CissyAuthority.PermitId.ToString())
                    {
                        if (this.CissyAuthority.IsNull())
                        {
                            find = true;
                            break;
                        }
                        else
                        {
                            if (int.Parse(ms[2]).Contains(this.CissyAuthority.Power) || this.CissyAuthority.Power == 0)
                            {
                                find = true;
                                break;
                            }
                        }
                    }
                }
                if (!find)
                {
                    var hc = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                    if (!hc)
                        context.Result = new StatusCodeResult(401);
                    else
                        context.Result = new StatusCodeResult(403);
                }
            }

            await Task.CompletedTask;
        }
    }

}

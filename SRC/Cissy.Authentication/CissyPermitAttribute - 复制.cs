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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Cissy.Authentication;
using Cissy.Configuration;
using Cissy.Security;
using Cissy.WeiXin;

namespace Cissy.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //[ComVisible(true)]
    public abstract class CissyPermitAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter, IAsyncActionFilter
    {
        public const string AskScope = "_askcope";
        CissyAuthority CissyAuthority = default;
        AppConfig Config = default;
        CissyAuthenticationOption Option = default;
        protected Dictionary<string, Dictionary<int, int>> Scopes { get; private set; } = new Dictionary<string, Dictionary<int, int>>();
        public CissyPermitAttribute()
        {
            Option = Actor.Public.GetService<IAuthenticationOptionBuilder>().Build();
            Config = Actor.Public.GetService<ICissyConfig>().GetConfig<AppConfig>();
        }
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

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            var scope = _getAuthorityScope(context);
            scope = scope.IsNullOrEmpty() ? AuthoritySeparators.DefaultScope : scope.Trim();
            OnScopeExecute(context, scope, this.Scopes);
            if (scope != AuthoritySeparators.DefaultScope)
            {
                if (!this.Scopes.ContainsKey(scope))
                {
                    //context.Result = new StatusCodeResult(403);
                    _on401or403(context, 403);
                }
            }
        }
        public abstract void OnScopeExecute(ActionExecutingContext context, string AskScope, Dictionary<string, Dictionary<int, int>> AuthorityScopes);
        string _getAuthorityScope(ActionExecutingContext context)
        {
            StringValues sv;
            try
            {
                if (context.HttpContext.Request.Query.TryGetValue(AskScope, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.HttpContext.Request.Form.TryGetValue(AskScope, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.HttpContext.Request.Headers.TryGetValue(AskScope, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.HttpContext.Request.Cookies.TryGetValue(AskScope, out string str))
                {
                    return str;
                }
            }
            catch { }

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
        void _on401or403(AuthorizationFilterContext context, int StatusCode)
        {
            context.Result = _401or403(context.HttpContext, StatusCode);
        }
        void _on401or403(ActionExecutingContext context, int StatusCode)
        {
            context.Result = _401or403(context.HttpContext, StatusCode);
        }
        IActionResult _401or403(HttpContext context, int StatusCode)
        {
            string url = default;
            if (this.Option.AuthenticationType == AuthenticationTypes.Cookie)
            {
                switch (this.Option.AuthenticationApply)
                {
                    case AuthenticationApplies.Web:
                        //cookie+web,跳转
                        url = Actor.Public.GetWxWebAuthUrl(this.Config, context.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}");
                        return new RedirectResult(url);
                    case AuthenticationApplies.WxQR:
                        //cookie+web,跳转
                        url = Actor.Public.GetWxQRAuthUrl(this.Config, context.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}");
                        return new RedirectResult(url);
                    case AuthenticationApplies.WxMp:
                        //cookie+WxMp,跳转
                        url = Actor.Public.GetWxMpAuthUrl(this.Config, context.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}");
                        //return new ContentResult() { Content = url };
                        return new RedirectResult(url);
                    case AuthenticationApplies.WxApp:
                        //cookie+WxApp,不存在，抛异常
                        throw new CissyException("微信小程序不允许采用cookie认证");
                        //break;
                }
            }
            else if (this.Option.AuthenticationType == AuthenticationTypes.Token)
            {
                switch (this.Option.AuthenticationApply)
                {
                    case AuthenticationApplies.Web:
                        //token+web,不跳转
                        return new StatusCodeResult(StatusCode);
                    case AuthenticationApplies.WxQR:
                        //token+web,不跳转
                        return new StatusCodeResult(StatusCode);
                    case AuthenticationApplies.WxMp:
                        //token+WxMp,不跳转
                        return new StatusCodeResult(StatusCode);
                    case AuthenticationApplies.WxApp:
                        //token+WxApp,不跳转
                        return new StatusCodeResult(StatusCode);
                }
            }
            return new StatusCodeResult(500);
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
                    //context.Result = new StatusCodeResult(401);
                    _on401or403(context, 401);
                else
                    //context.Result = new StatusCodeResult(403);
                    _on401or403(context, 403);
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
                        }
                        else
                        {
                            if (this.CissyAuthority.Power == 0 || int.Parse(ms[2]).Contains(this.CissyAuthority.Power))
                            {
                                find = true;
                            }
                        }
                    }
                }
                if (!find)
                {
                    var hc = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                    if (!hc)
                        //context.Result = new StatusCodeResult(401);
                        _on401or403(context, 401);
                    else
                        //context.Result = new StatusCodeResult(403);
                        _on401or403(context, 403);
                }

            }
            await Task.CompletedTask;
        }
    }

}

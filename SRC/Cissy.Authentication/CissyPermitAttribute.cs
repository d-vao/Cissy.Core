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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    //[ComVisible(true)]
    public abstract class CissyPermitAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter, IAsyncActionFilter
    {

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
            var ac = context.HttpContext.GetAuthorityContext();
            if (ac.IsNotNull() && !ac.OK)
            {
                context.Result = _401or403(context.HttpContext, 403);
            }
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
            //不管是否设置检查权限，都必须是已认证身份，否则返回401
            if (context.HttpContext.User.IsNull() || context.HttpContext.User.Claims.IsNullOrEmpty())
                context.Result = _401or403(context.HttpContext, 401);
            if (!context.HttpContext.User.IsCissyAuthenticated())
                context.Result = _401or403(context.HttpContext, 401);
            if (this.CissyAuthority.IsNotNull() && this.CissyAuthority.Power != 0)
            {
                AuthorityContext ac = context.HttpContext.GetAuthorityContext();
                if (ac.IsNotNull())
                {
                    var AskScopeAuthority = ac.AskScopeAuthority;
                    if (AskScopeAuthority.IsNotNullAndEmpty())
                    {
                        if (AskScopeAuthority.TryGetValue(this.CissyAuthority.PermitId, out int k))
                        {
                            if (k.Contains(this.CissyAuthority.Power))
                                ac.OK = true;
                        }
                    }
                }
                else
                    context.Result = _401or403(context.HttpContext, 401);
            }
            await Task.CompletedTask;
        }
    }
}

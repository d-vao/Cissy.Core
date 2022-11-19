using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Cissy.Authentication;
using Cissy.Configuration;
using Cissy.WeiXin;

namespace Cissy.Authentication
{
    /// <summary>
    /// 多个权限申明叠加，只要满足其中一个即授权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CissyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter, IAsyncActionFilter
    {
        public Claim[] Claims { get; set; }
        AppConfig Config = default;
        CissyAuthenticationOption Option = default;
        public CissyAuthorizeAttribute()
        {
            Option = Actor.Public.GetService<IAuthenticationOptionBuilder>().Build();
            Config = Actor.Public.GetService<ICissyConfig>().GetConfig<AppConfig>();
        }
        public CissyAuthorizeAttribute(string claimType, string claimValue) : this()
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
            //不管是否设置检查权限，都必须是已认证身份，否则返回401
            if (context.HttpContext.User.IsNull() || context.HttpContext.User.Claims.IsNullOrEmpty())
                context.Result = _401or403(context.HttpContext, 401);
            if (!context.HttpContext.User.IsCissyAuthenticated())
                context.Result = _401or403(context.HttpContext, 401);

            if (Claims.IsNotNull())
            {
                var hasClaim = context.HttpContext.User.Claims.Any(c => Claims.Any(m => c.Type == m.Type && c.Value == m.Value));
                if (!hasClaim)
                {
                    var hc = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                    if (!hc)
                        _on401or403(context, 401);
                    else
                        _on401or403(context, 403);
                }
            }
            else
            {
                var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == CissyClaimTypes.UserId);
                if (!hasClaim)
                {
                    _on401or403(context, 401);
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

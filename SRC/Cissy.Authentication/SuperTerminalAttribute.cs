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
    public enum SuperTerminal
    {
        WeiXin,
        Ali
    }
    public static class SuperTerminalHelper
    {
        public static bool IsAliBrowser(this HttpRequest request)
        {
            string useragent = request.Headers["User-Agent"];
            return useragent.IsNotNullAndEmpty() && useragent.ToLower().Contains("alipay");
        }
        public static bool IsWXBrowser(this HttpRequest request)
        {
            string useragent = request.Headers["User-Agent"];
            return useragent.IsNotNullAndEmpty() && useragent.ToLower().Contains("micromessenger");
        }
    }
    /// <summary>
    /// 终端过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SuperTerminalAttribute : Attribute, IActionFilter, IAsyncActionFilter
    {
        public SuperTerminal Terminal { get; set; }
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            bool ok = false;
            foreach (SuperTerminal t in this.Terminal.Parse())
            {
                if (t == SuperTerminal.Ali && context.HttpContext.Request.IsAliBrowser())
                {
                    ok = true;
                    break;
                }
                if (t == SuperTerminal.WeiXin && context.HttpContext.Request.IsWXBrowser())
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
                context.Result = new StatusCodeResult(404);
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

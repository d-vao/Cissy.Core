using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Cissy.Authentication.JWT;
using Cissy.Configuration;
using Microsoft.Extensions.Primitives;

namespace Cissy.Authentication
{
    public sealed class CissyAuthMiddlerware
    {

        private readonly RequestDelegate _next;
        private readonly CissyAuthenticationOption Option;
        private ICissyConfig CissyConfig;
        private AppConfig AppConfig;
        public CissyAuthMiddlerware(RequestDelegate next)
        {
            _next = next;

            this.CissyConfig = Actor.Public.GetService<ICissyConfig>();
            this.AppConfig = this.CissyConfig.GetConfig<AppConfig>();
            var builder = Actor.Public.GetService<IAuthenticationOptionBuilder>();
            Option = builder.Build();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string token = string.Empty;
            if (Option.AuthenticationType == AuthenticationTypes.Cookie)
            {
                token = context.Request.Cookies[AuthenticationHelper.BuildCookieName(Option.Scheme)];
            }
            else if (Option.AuthenticationType == AuthenticationTypes.Token)
            {
                string tokenAuthentication = context.Request.Headers["Authorization"];
                if (tokenAuthentication != null && tokenAuthentication.Contains(Option.Scheme))
                {
                    token = tokenAuthentication.Trim().Split(" ")[1];
                }
            }
            if (token.IsNotNullAndEmpty())
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.AppConfig.AuthSecret);
                var claimsPrincipal = builder.GetPrincipal(Option.Scheme, token);
                if (claimsPrincipal.IsNotNull())
                {
                    context.User = claimsPrincipal;
                    AuthorityContext ac = new AuthorityContext();
                    ac.User = claimsPrincipal;
                    var cl = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.Permits);
                    if (cl.IsNotNull())//开始构建权限上下文
                    {

                        //step1,收集当前身份具有的权限集
                        string[] vs = cl.Value.Split(AuthoritySeparators.Top, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string v in vs)
                        {
                            string[] ms = v.Split(AuthoritySeparators.FirstLevel);
                            if (!ac.AuthorityScopes.TryGetValue(ms[0], out Dictionary<int, int> dic))
                            {
                                dic = new Dictionary<int, int>();
                                ac.AuthorityScopes[ms[0]] = dic;
                            }
                            dic[int.Parse(ms[1])] = int.Parse(ms[2]);
                        }
                        //step2,搜索权限请求范围
                        var scope = _getAuthorityScope(context);
                        scope = scope.IsNullOrEmpty() ? AuthoritySeparators.DefaultScope : scope.Trim();
                        ac.AskScope = scope;
                    }
                    context.Items[AuthorityContext.AuthorityContextKey] = ac;
                }
            }
            await _next.Invoke(context);
        }
        string _getAuthorityScope(HttpContext context)
        {
            StringValues sv;
            try
            {
                if (context.Request.Query.TryGetValue(AuthorityContext.AskScopeKey, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.Request.Form.TryGetValue(AuthorityContext.AskScopeKey, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.Request.Headers.TryGetValue(AuthorityContext.AskScopeKey, out sv))
                {
                    return sv.FirstOrDefault();
                }
            }
            catch { }
            try
            {
                if (context.Request.Cookies.TryGetValue(AuthorityContext.AskScopeKey, out string str))
                {
                    return str;
                }
            }
            catch { }

            return default;

        }
    }
}

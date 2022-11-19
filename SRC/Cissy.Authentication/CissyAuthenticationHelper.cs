using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Builder;
using System.IdentityModel.Tokens.Jwt;
using Cissy.Configuration;

namespace Cissy.Authentication
{
    //public class CissyAuthenticationHandle : IAuthenticationHandler
    //{
    //    private HttpContext _context;
    //    private AuthenticationScheme _authenticationScheme;
    //    //private string _cookieName = "user";

    //    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    //    {
    //        _context = context;
    //        _authenticationScheme = scheme;
    //        return Task.CompletedTask;
    //    }

    //    public Task<AuthenticateResult> AuthenticateAsync()
    //    {
    //        var cookie = _context.Request.Cookies[BaseAuthController.DefaultSchemeName];
    //        if (string.IsNullOrEmpty(cookie))
    //        {
    //            return Task.FromResult(AuthenticateResult.NoResult());
    //        }
    //        var identity = new ClaimsIdentity(_authenticationScheme.Name);
    //        identity.AddClaim(new Claim(ClaimTypes.Name, cookie));
    //        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), _authenticationScheme.Name);
    //        return Task.FromResult(AuthenticateResult.Success(ticket));
    //    }

    //    //public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    //    //{
    //    //    _context.Response.Cookies.Append(BaseAuthController.DefaultSchemeName, user.Identity.Name);
    //    //    return Task.CompletedTask;
    //    //}
    //    public async Task ChallengeAsync(AuthenticationProperties properties)
    //    {

    //        await _context.ChallengeAsync(BaseAuthController.DefaultSchemeName, properties);

    //    }

    //    public async Task ForbidAsync(AuthenticationProperties properties)
    //    {
    //        await _context.ForbidAsync(BaseAuthController.DefaultSchemeName, properties);
    //    }
    //}
    public static class CissyAuthenticationHelper
    {
        public static CissyConfigBuilder AddCissyAuthentication(this CissyConfigBuilder cissyConfigBuilder, Action<CissyAuthenticationOption> action = null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            DefaultAuthenticationOptionBuilder authOptionBuilder = new DefaultAuthenticationOptionBuilder(action);
            cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IAuthenticationOptionBuilder), authOptionBuilder);
            return cissyConfigBuilder;
        }
        public static IApplicationBuilder UseCissyAuthentication(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CissyAuthMiddlerware>();
            return builder;
        }
    }
}

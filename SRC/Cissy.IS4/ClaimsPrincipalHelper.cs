using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Cissy.Authentication;

namespace Cissy.IS4
{
    public static class ClaimsPrincipalHelper
    {
        public static WeiXinPassport GetWeiXinPassport(this ClaimsPrincipal User)
        {
            WeiXinPassport passport = new WeiXinPassport();
            passport.UserName = User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.UserName)?.Value;
            passport.UserId = User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.UserId)?.Value;
            passport.NickName = User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.NickName)?.Value;
            passport.OpenId = User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.WeiXinOpenId)?.Value;
            passport.UnionId = User.Claims.FirstOrDefault(m => m.Type == CissyClaimTypes.WeiXinUnionId)?.Value;
            return passport;
        }
        public static IEnumerable<string> Roles(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == ClaimTypes.Role)?.Select(n => n.Value);
        }
        public static IEnumerable<string> Roles(this HttpContext HttpContext)
        {
            return HttpContext.User.Roles();
        }
        public static bool ContainRole(this ClaimsPrincipal User, string RoleName)
        {
            return User.HasClaim(m => m.Value == RoleName && m.Type == ClaimTypes.Role);
        }
        public static bool ContainRole(this HttpContext HttpContext, string RoleName)
        {
            return HttpContext.User.ContainRole(RoleName);
        }

        public static IEnumerable<string> CissyRoles(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.Role)?.Select(n => n.Value);
        }
        public static IEnumerable<string> CissyRoles(this HttpContext HttpContext)
        {
            return HttpContext.User.CissyRoles();
        }
        public static bool ContainCissyRole(this ClaimsPrincipal User, string RoleName)
        {
            return User.HasClaim(m => m.Value == RoleName && m.Type == CissyClaimTypes.Role);
        }
        public static bool ContainCissyRole(this HttpContext HttpContext, string RoleName)
        {
            return HttpContext.User.ContainCissyRole(RoleName);
        }

        public static IEnumerable<string> WeiXinOpenId(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.WeiXinOpenId).Select(n => n.Value);
        }
        public static IEnumerable<string> WeiXinOpenId(this HttpContext httpContext)
        {
            return httpContext.User.WeiXinOpenId();
        }

        public static IEnumerable<string> WeiXinUnionId(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.WeiXinUnionId).Select(n => n.Value);
        }
        public static IEnumerable<string> WeiXinUnionId(this HttpContext httpContext)
        {
            return httpContext.User.WeiXinUnionId();
        }
        public static IEnumerable<string> CissyUserIds(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.UserId).Select(n => n.Value);
        }
        public static string CissyUserId(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.UserId).Select(n => n.Value)?.FirstOrDefault();
        }
        public static IEnumerable<string> CissyUserIds(this HttpContext httpContext)
        {
            return httpContext.User.CissyUserIds();
        }
        public static IEnumerable<string> CissyUserNames(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.UserName).Select(n => n.Value);
        }
        public static string CissyUserName(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.UserName).Select(n => n.Value)?.FirstOrDefault();
        }
        public static IEnumerable<string> CissyUserNames(this HttpContext httpContext)
        {
            return httpContext.User.CissyUserNames();
        }
        public static IEnumerable<string> NickNames(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.NickName).Select(n => n.Value);
        }
        public static IEnumerable<string> NickNames(this HttpContext httpContext)
        {
            return httpContext.User.NickNames();
        }
        public static IEnumerable<string> Avatars(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == CissyClaimTypes.Avatar).Select(n => n.Value);
        }
        public static IEnumerable<string> Avatars(this HttpContext httpContext)
        {
            return httpContext.User.Avatars();
        }

        public static IEnumerable<string> ClaimsSids(this ClaimsPrincipal User)
        {
            return User.Claims.Where(m => m.Type == ClaimTypes.Sid).Select(n => n.Value);
        }
        public static IEnumerable<string> ClaimsSids(this HttpContext httpContext)
        {
            return httpContext.User.ClaimsSids();
        }
        public static string UserId(this HttpContext httpContext)
        {
            return httpContext.User.UserId();
        }
        public static string UserId(this ClaimsPrincipal User)
        {
            return User.ClaimsSids()?.FirstOrDefault();
        }
        public static string UserId(this ClaimsPrincipal User, string AuthenticationScheme)
        {
            var identity = User.Identities.FirstOrDefault(m => m.AuthenticationType == AuthenticationScheme);
            if (identity.IsNull())
                return null;
            var claim = identity.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Sid);
            if (claim.IsNull())
                return null;
            return claim.Value;
        }

        public static string UserId(this HttpContext httpContext, string AuthenticationScheme)
        {
            return httpContext.User.UserId(AuthenticationScheme);
        }
        public static string UserName(this ClaimsPrincipal User)
        {
            var identity = User.Identity;
            if (identity.IsNull())
                return null;
            return identity.Name;
        }
        public static string UserName(this HttpContext httpContext)
        {
            return httpContext.User.UserName();
        }
        public static string UserName(this ClaimsPrincipal User, string AuthenticationScheme)
        {
            var identity = User.Identities.FirstOrDefault(m => m.AuthenticationType == AuthenticationScheme);
            if (identity.IsNull())
                return null;
            return identity.Name;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Cissy.RateLimit
{
    public class ClientHeaderResolveContributor : IClientResolveContributor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _headerName;

        public ClientHeaderResolveContributor(
            IHttpContextAccessor httpContextAccessor,
            string headerName)
        {
            _httpContextAccessor = httpContextAccessor;
            _headerName = headerName;
        }
        public string ResolveClient()
        {
            var clientId = "anon";
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Headers.TryGetValue(_headerName, out var values))
            {
                clientId = values.First();
            }

            return clientId;
        }
    }
}
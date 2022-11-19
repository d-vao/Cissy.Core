using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Cissy.Http
{
    public sealed class DefaultHttpProxyFactory : IHttpProxyFactory
    {
        public HttpProxyConfig Config { get; set; }
        public IWebProxy BuildProxy()
        {
            return new CissyHttpProxy(new Uri(this.Config.Uri), this.Config.UserName, this.Config.Password);
        }

    }
}

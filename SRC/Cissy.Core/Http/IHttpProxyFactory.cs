using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Cissy.Http
{
    public interface IHttpProxyFactory
    {
        HttpProxyConfig Config { get; }
        IWebProxy BuildProxy();
    }
}

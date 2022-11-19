using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cissy.Http
{
    public class CissyHttpProxy : IWebProxy
    {
        //代理的地址
        public CissyHttpProxy(Uri proxyUri, string username, string password)
        {
            //设置代理请求的票据
            credentials = new NetworkCredential(username, password);
            ProxyUri = proxyUri;
        }
        private NetworkCredential credentials;

        private Uri ProxyUri;

        public ICredentials Credentials { get => credentials; set => throw new NotImplementedException(); }

        //获取代理地址
        public Uri GetProxy(Uri destination)
        {
            return ProxyUri; // your proxy Uri
        }
        //主机host是否绕过代理服务器，设置false即可
        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}

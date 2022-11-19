using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Authentication;

namespace Cissy.IS4
{
    public class WeiXinPassport : IPassport
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string OpenId { get; set; }
        public string UnionId { get; set; }
    }
}

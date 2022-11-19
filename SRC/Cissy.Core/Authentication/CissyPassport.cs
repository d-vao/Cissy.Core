using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Authentication;

namespace Cissy.Authentication
{

    public class CissyPassport : IPassport
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string OpenId { get; set; }
        public string UnionId { get; set; }
    }
}

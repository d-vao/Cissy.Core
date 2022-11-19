using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.WeiXin
{
    public class AccessToken : IModel
    {
        public string access_token;
        public int expires_in;
        public DateTime CreateTime;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Authentication
{
    public static class AuthenticationHelper
    {
        public static string BuildCookieName(string Scheme)
        {
            return $"_cissy.{Scheme}";
        }
    }
}

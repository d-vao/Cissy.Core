using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Authentication
{
    public static class AuthoritySeparators
    {
        public const string DefaultScope = "*";
        public const string Top = "|";
        public const string FirstLevel = ":";
        public const string SencondLevel = "-";
        public const string ThirdLevel = ".";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Authentication
{
    public interface IAuthenticationOptionBuilder
    {
        CissyAuthenticationOption Build();
    }
    internal class DefaultAuthenticationOptionBuilder : IAuthenticationOptionBuilder
    {
        CissyAuthenticationOption option = default;
        public DefaultAuthenticationOptionBuilder(Action<CissyAuthenticationOption> action)
        {
            option = new CissyAuthenticationOption() { Scheme = AuthenticationScheme.Default, AuthenticationType = AuthenticationTypes.Cookie, AuthenticationApply = AuthenticationApplies.Web };
            if (action.IsNotNull())
                action(option);
        }
        public CissyAuthenticationOption Build()
        {
            return option;
        }
    }
}

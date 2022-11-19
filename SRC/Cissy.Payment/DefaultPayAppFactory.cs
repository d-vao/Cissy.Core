using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Payment
{
    public class DefaultPayAppFactory : IPayAppFactory
    {
        PaymentConfig _config;
        public DefaultPayAppFactory(PaymentConfig config)
        {
            _config = config;
        }
        public IPayApp Build()
        {
            return new DefaultPayApp(_config);
        }
    }
}

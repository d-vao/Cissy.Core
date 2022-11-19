using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Payment
{
    public interface IPayAppFactory
    {
        IPayApp Build();
    }
}

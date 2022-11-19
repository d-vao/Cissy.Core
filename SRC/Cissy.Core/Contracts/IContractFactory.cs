using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy
{
    public interface IContractFactory<Contract> : IContractVersion
    {
        Contract Build();
    }
}

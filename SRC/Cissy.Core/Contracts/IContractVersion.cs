using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy
{
    public interface IContractVersion
    {
        float ContractVersion { get; }
    }
    public interface IConfigurableContract : IContractVersion
    {
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinApp
    {
        Task<Code2SessionResult> Code2SessionAsync(string code);
    }
}

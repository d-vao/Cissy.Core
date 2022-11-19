using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Static
{
    public abstract class CissyClient 
    {
        public abstract GrantTypes GrantType { get; }
    }
    public class CissyCredentialClient : CissyClient
    {
        public override GrantTypes GrantType { get { return GrantTypes.Credential; } }
    }
    public class CissyHybridClient : CissyClient
    {
        public override GrantTypes GrantType { get { return GrantTypes.Hybrid; } }
    }
    public class CissyCodeClient : CissyClient
    {
        public override GrantTypes GrantType { get { return GrantTypes.Code; } }
    }
}

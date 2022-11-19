using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public interface IApplicationStartContext
    {
    }
    public interface IApplicationStart
    {
        void PreStart(IApplicationStartContext StartContext);
        void Start(IApplicationStartContext StartContext);
    }
    public class ApplicationStartContext : IApplicationStartContext
    {
    }
}

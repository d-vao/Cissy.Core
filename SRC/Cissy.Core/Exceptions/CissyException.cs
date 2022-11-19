using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy
{
    public class CissyException : ApplicationException
    {
        public CissyException()
        {
            //Log.Info(this.Message, this);
        }

        public CissyException(string message) : base(message)
        {
            //Log.Info(message, this);
        }

        public CissyException(string message, Exception inner) : base(message, inner)
        {
            //Log.Info(message, inner);
        }

    }
}

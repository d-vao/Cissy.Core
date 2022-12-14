using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Plugins
{
    /// <summary>
    /// 插件异常
    /// </summary>
    public class PluginException : CissyException
    {
        public PluginException()
        {
            //Log.Info(this.Message, this);
        }

        public PluginException(string message)
            : base(message)
        {
            //Log.Info(message, this);
        }

        public PluginException(string message, Exception inner)
            : base(message, inner)
        {
            //Log.Info(message, inner);
        }
    }
}

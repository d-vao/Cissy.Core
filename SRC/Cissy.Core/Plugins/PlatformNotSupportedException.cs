using System;

namespace Cissy.Plugins
{
    /// <summary>
    /// 平台不支持异常
    /// </summary>
    public class PlatformNotSupportedException : CissyException
    {
        public PlatformNotSupportedException(PlatformType platformType)
            : base("不支持" + platformType.StringValue() + "平台")
        {
            //Log.Info(this.Message, this);
        }
    }
}

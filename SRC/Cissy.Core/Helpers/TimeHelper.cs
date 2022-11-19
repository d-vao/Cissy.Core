using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace Cissy
{
    public static class TimeHelper
    {
        /// <summary>
        /// Unix起始时间
        /// </summary>
        public readonly static DateTimeOffset BaseTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public static uint ToTimestamp(this DateTimeOffset LocalTime)
        {
            return (uint)(LocalTime.ToUniversalTime() - BaseTime).TotalSeconds;
        }
        public static uint ToTimestamp(this DateTime LocalTime)
        {
            return (uint)(LocalTime.ToUniversalTime() - BaseTime).TotalSeconds;
        }
        public static DateTimeOffset ToDateTime(this uint timestamp)
        {
            return BaseTime.AddSeconds(timestamp).ToLocalTime();
        }

        public static DateTimeOffset ToDateTime(this ulong timestamp)
        {
            return BaseTime.AddSeconds(timestamp).ToLocalTime();
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static uint GetTimeStamp(this Public Public)
        {
            TimeSpan ts = SystemTime.Now.ToUniversalTime() - BaseTime;
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        public static DateTimeOffset ToBeijingTime(this DateTimeOffset DateTime)
        {
            return DateTime.ToUniversalTime().AddHours(8);
        }
        public static DateTime ToBeijingTime(this DateTime DateTime)
        {
            return DateTime.ToUniversalTime().AddHours(8);
        }
        public static DateTimeOffset BeijingNow(this Public Public)
        {
            return SystemTime.Now.ToUniversalTime().AddHours(8);
        }

    }
}

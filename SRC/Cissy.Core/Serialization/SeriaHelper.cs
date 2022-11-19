using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cissy.Serialization
{
    public static class SeriaHelper
    {
        public static T DeserializeFrom<T>(byte[] value, int offset = 0) where T : ISerializable, new()
        {
            using (MemoryStream ms = new MemoryStream(value, offset, value.Length - offset, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
            {
                return DeserializeFrom<T>(reader);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader">数据来源</param>
        /// <returns>返回反序列化后的结果</returns>
        internal static T DeserializeFrom<T>(BinaryReader reader) where T : ISerializable, new()
        {
            T record = new T();
            record.Deserialize(reader);
            return record;
        }
        public static byte[] StringToBytes(this string str)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
            {
                writer.Write(str);
                writer.Flush();
                return ms.ToArray();
            }
        }
    }
}

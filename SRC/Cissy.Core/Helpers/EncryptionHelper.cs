using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Cissy
{
    public static class EncryptionHelper
    {
        public static string Encrypt_MD5(this byte[] data)
        {
            MD5 md5Hash = MD5.Create();
            var hash = md5Hash.ComputeHash(data);
            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }
            // 返回十六进制字符串  
            return sBuilder.ToString();
        }
    }
}

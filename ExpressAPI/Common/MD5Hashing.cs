using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ExpressAPI.Common
{
    public sealed class MD5Hashing
    {
        private static MD5 md5 = MD5.Create();
        public enum MD5Type { Type16 = 16, Type32 = 32 };
        //私有化构造函数
        private MD5Hashing()
        {
        }
        /// <summary>
        /// 使用utf8编码将字符串散列
        /// </summary>
        /// <param name="sourceString">要散列的字符串</param>
        /// <returns>散列后的字符串</returns>
        public static string HashString(string sourceString, MD5Type md5Type)
        {
            switch (md5Type.ToString())
            {
                case "Type16":
                    return HashString(Encoding.UTF8, sourceString).Substring(8, 16);
                case "Type32":
                    return HashString(Encoding.UTF8, sourceString);
                default:
                    return null;

            }

        }


        /// <summary>
        /// 使用指定的编码将字符串散列
        /// </summary>
        /// <param name="encode">编码</param>
        /// <param name="sourceString">要散列的字符串</param>
        /// <returns>散列后的字符串</returns>
        public static string HashString(Encoding encode, string sourceString)
        {
            byte[] source = md5.ComputeHash(encode.GetBytes(sourceString));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            sBuilder = sBuilder.Replace("-", "");
            return sBuilder.ToString();
        }
    }
}
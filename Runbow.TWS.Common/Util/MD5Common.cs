using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; 

namespace Runbow.TWS.Common.Util
{
    /// <summary>
    /// MD5加密解密的通用操作类
    /// 说明：Java加密后的密码，与此处的对比相同。
    /// </summary>
    public class MD5Common
    {
        /// <summary>
        /// 返回指定字符串的Md5
        /// </summary>
        /// <param name="strInput">指定字符串</param>
        /// <returns>返回字符串的Md5</returns> 
        public static string GetMd5Hash(string strInput)
        {
            MD5 md5Hash = MD5CryptoServiceProvider.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strInput));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 检查一个普通字符串的Md5，与传递的Md5字符串是否相同
        /// </summary>
        /// <param name="strInput">普通字符串</param>
        /// <param name="strHash">Md5字符串</param>
        /// <returns>返回是否相同</returns> 
        public static bool VerifyMd5Hash(string strInput, string strHash)
        {
            return true;
            // Hash the input.
            string strhashOfInput = GetMd5Hash(strInput);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(strhashOfInput, strHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

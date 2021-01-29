using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace ExpressAPI.Common
{
    public class CommonHelper
    {
        /// <summary>
        /// BASE64 编码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToBASE64(string param)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(param));
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToMD5(string param)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(param));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// URL 编码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string UrlEncode(string param)
        {
            StringBuilder sb = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(param);
            foreach (byte b in data)
            {
                sb.Append(@"%" + Convert.ToString(b, 16));
            }
            return sb.ToString();
        }
        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(o.GetType());
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");//去掉命名空间，如果要加自己在字符串拼接，不然不好处理
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;//换行缩进
                    settings.CheckCharacters = false;
                    settings.NewLineChars = "\r\n";
                    settings.Encoding = encoding;
                    settings.IndentChars = " ";//指定缩进字符
                    settings.OmitXmlDeclaration = true; //去除xml声明
                    using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    {
                        serializer.Serialize(writer, o, ns);
                        writer.Close();
                    }
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }

        public static string XmlSerialize(object o)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;//去除xml声明
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                using (MemoryStream mem = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(mem, settings))
                    {
                        //去除默认命名空间xmlns:xsd和xmlns:xsi
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        XmlSerializer serializer = new XmlSerializer(o.GetType());
                        serializer.Serialize(writer, o, ns);
                    }
                    using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }

        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml) where T : class, new()
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T; ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将XML转换成实体对象异常", ex);
            }
        }

        public static string UnicodeToString(string param)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                param, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }


        /// <summary>
        /// Api Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postDataStr = "")
        {
            string resStr = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse res = null;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            if (res != null)
            {
                Stream responseStream = res.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    resStr = sr.ReadToEnd();
                    sr.Close();
                }
                responseStream.Close();
            }
            res.Close();
            req.Abort();

            return resStr;
        }
    }
}
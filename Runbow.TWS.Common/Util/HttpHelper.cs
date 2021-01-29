using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Runbow.TWS.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpGet(string url, string postData)
        {
            string result = string.Empty;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + (!string.IsNullOrEmpty(postData) ? "?" : "") + postData);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "application/json;charset=utf-8";

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
                Stream resStream = res.GetResponseStream();
                using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                resStream.Close();
                res.Close();
            }
            req.Abort();

            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData = null)
        {
            string result = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "application/json;charset=utf-8";
            req.ContentLength = 0;

            if (!string.IsNullOrEmpty(postData))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = bytes.Length;
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
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
                Stream resStream = res.GetResponseStream();
                using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                resStream.Close();
                res.Close();
            }
            req.Abort();

            return result;
        }
    }
}

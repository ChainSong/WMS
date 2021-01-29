using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP.Common
{
    public class SFTPConstants
    {
        /// <summary>
        /// 合并路径用
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        //public static readonly string AKZOCONFIGPATH = MapPath("AKZOConfig.xml");
        //public static string SavePath = System.Configuration.ConfigurationSettings.AppSettings["SaveNRFilePath"].ToString();

        public readonly static string ReceiveFilePath = GetConfigValue("ReceiveFilePath");//接收数据的文件夹
        public readonly static string SuccessFilePath = GetConfigValue("SuccessFilePath");//解析成功的文件夹
        public readonly static string FaildFilePath = GetConfigValue("FaildFilePath");//解析失败的文件夹
        public readonly static string ErrorFilePath = GetConfigValue("ErrorFilePath");//解析报错的文件夹
        public readonly static string SendFilePath = GetConfigValue("SendFilePath");//等待发送的文件夹
        public readonly static string SentFilePath = GetConfigValue("SentFilePath");//发送成功的文件夹
        public readonly static string LogFilePath = GetConfigValue("LogFilePath");//发送成功的文件夹



        public readonly static string sftpip = GetConfigValue("sftpip");//ip
        public readonly static string sftpport = GetConfigValue("sftpport");//端口
        public readonly static string sftpuser = GetConfigValue("sftpuser");//用户名
        public readonly static string sftppwd = GetConfigValue("sftppwd");//密码
        public readonly static string sftpfilepath = GetConfigValue("sftpfilepath");//lf接收文件地址

        public readonly static string IsParsingFile = GetConfigValue("IsParsingFile");//解析文件功能是否启用
        public readonly static string IsBuildFile = GetConfigValue("IsBuildFile");//生成文件功能是否启用
        public readonly static string IsSendFile = GetConfigValue("IsSendFile");//发送文件功能是否启用-

        public static string GetConfigValue(string key)
        {
            return string.IsNullOrEmpty(key) ? string.Empty : ConfigurationManager.AppSettings[key].ToString();
        }

        public static void CheckFolderExists()
        {
            //先检查需要的文件夹是否都存在 不存在就创建           
            if (!Directory.Exists(ReceiveFilePath))
            {
                Directory.CreateDirectory(ReceiveFilePath);
            }

            if (!Directory.Exists(SuccessFilePath))
            {
                Directory.CreateDirectory(SuccessFilePath);
            }
            if (!Directory.Exists(SuccessFilePath + @"\WMSALLOC"))//解析成功的出库单
            {
                Directory.CreateDirectory(SuccessFilePath + @"\WMSALLOC");
            }
            if (!Directory.Exists(FaildFilePath))
            {
                Directory.CreateDirectory(FaildFilePath);
            }
            if (!Directory.Exists(FaildFilePath + @"\WMSALLOC"))//解析成功的出库单
            {
                Directory.CreateDirectory(FaildFilePath + @"\WMSALLOC");
            }


            if (!Directory.Exists(ErrorFilePath))
            {
                Directory.CreateDirectory(ErrorFilePath);
            }

            if (!Directory.Exists(SendFilePath))
            {
                Directory.CreateDirectory(SendFilePath);
            }
            if (!Directory.Exists(SentFilePath))
            {
                Directory.CreateDirectory(SentFilePath);
            }




        }
    }
}

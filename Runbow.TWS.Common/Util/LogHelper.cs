using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Runbow.TWS.Common
{
    public class LogHelper
    {
        /// <summary>
        /// 日志文件路径 分文件夹
        /// </summary>
        private static string logPreFilePath = "";

        /// <summary>
        /// 日志文件子文件夹
        /// </summary>
        public static string SetPreFilePath
        {
            set
            {
                logPreFilePath = value;
            }
        }

        /// <summary>
        /// 日志文件名
        /// </summary>
        private static string LogFileName
        {
            get
            {
                return string.Format(@"{0}-{1}-{2}.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
        }

        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        private static string LogFilePath
        {
            get
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory + @"ApiLog\";
                if (!string.IsNullOrEmpty(logPreFilePath))
                    folder += logPreFilePath + @"\";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                return folder;
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log"></param>
        public static void Info(string log)
        {
            string logFullName = LogFilePath + LogFileName;
            FileStream fs = new FileStream(logFullName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + log);
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}

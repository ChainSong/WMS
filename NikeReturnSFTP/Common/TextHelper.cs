using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP.Common
{
    /// <summary>
    /// txt操作类
    /// </summary>
    public static class TextHelper
    {

        /// <summary>
        /// 读取文本文件转换为List 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> ReadTextFileToList(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(fs,Encoding.Unicode);
            try
            {
                //使用StreamReader类来读取文件 
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                // 从数据流中读取每一行，直到文件的最后一行
                string tmp = sr.ReadLine();
                while (tmp != null)
                {
                    list.Add(tmp);
                    tmp = sr.ReadLine();
                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
            finally
            {
                //关闭此StreamReader对象 
                sr.Close();
                fs.Close();
            }
        }

        //将List转换为TXT文件
        public static void WriteListToTextFile(List<string> list, string txtFile)
        {
            FileStream fs = new FileStream(txtFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs,Encoding.Unicode);
            try
            {
                //创建一个文件流，用以写入或者创建一个StreamWriter 
                sw.Flush();
                // 使用StreamWriter来往文件中写入内容 
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭此文件t 
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }

        /// <summary>
        /// 截取字符串去空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string TxtSubstring(this string str, int start, int len)
        {
            return str.Substring(start, len).Trim();
        }

        /// <summary>
        /// 右边自动加字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TxtPadRightstring(this string str, int len, char charstr = ' ')
        {
            return str.PadRight(len, charstr);
        }

        /// <summary>
        /// 左边自动加字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TxtPadLeftstring(this string str, int len, char charstr = ' ')
        {
            return str.PadLeft(len, charstr);
        }

 

    }
}

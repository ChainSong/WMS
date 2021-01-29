using NikeReturnSFTP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP
{
    class Program
    {
        /// <summary>
        /// nike退货仓SFTP接口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //SFTPHelper sftp1 = new SFTPHelper(SFTPConstants.sftpip,SFTPConstants.sftpport,SFTPConstants.sftpuser,SFTPConstants.sftppwd);
            //sftp1.Put("D:\\testsftp\\222.txt", "222.txt");

            //SFTPHelper sftp1 = new SFTPHelper("192.168.10.243", "22", "testrbsftpuser1", "runbow2020");
            //sftp1.Put("D:\\testsftp\\222.txt", "\\NIKEReturn\\Receive\\222.txt");
            //sftp1.Put("D:\\testsftp\\222.txt", "222.txt");

            //SFTPHelper sftp = new SFTPHelper("192.168.10.207", "22", "rbsftpuser1", "runbow2020");
            //sftp.Put("D:\\testsftp\\222.txt", "\\NIKEReturn\\Receive\\222.txt");

            //创建文件夹
            SFTPConstants.CheckFolderExists();



            #region 解析
            if (SFTPConstants.IsParsingFile == "1")
            {
                try
                {
                    //从Receive文件夹抓取LF传来的文件
                    new ParsingFile().ReadFile();
                }
                catch (Exception ex)
                {
                }
            }
            #endregion

            #region 生成反馈文件
            if (SFTPConstants.IsBuildFile == "1")
            {
                try
                {
                    //生成包装反馈文件
                    new BuildFile().BuildWMSPAC();
                }
                catch (Exception ex)
                {
                }
            }
            #endregion

            #region 发送
            if (SFTPConstants.IsSendFile == "1")
            {
                try
                {
                    //将包装信息文件发送给LF
                    new SendFile().SendFileToLF();
                }
                catch (Exception ex)
                {
                }
            }
            #endregion

            

        }
    }
}

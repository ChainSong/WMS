using NikeReturnSFTP.Common;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity.WMS.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP
{
    /// <summary>
    /// 发送文件
    /// </summary>
    public class SendFile
    {
        /// <summary>
        /// 发送文件
        /// </summary>
        public void SendFileToLF()
        {
            try
            {


                //读取send文件夹，发送到LFsftp
                //读取文件列表
                string[] sendfiles = Directory.GetFiles(SFTPConstants.SendFilePath);
                if (sendfiles.Length > 0)
                {
                    for (int i = 0; i < sendfiles.Length; i++)
                    {
                        WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                        log.SourceFileName = sendfiles[i];
                        log.Type = "SendFile";

                        try
                        {
                            FileInfo file = new FileInfo(sendfiles[i]);
                            string filename = file.Name;
                            log.Str7 = SFTPConstants.sftpip;
                            log.Str8 = SFTPConstants.sftpport;
                            log.Str9 = SFTPConstants.sftpuser;
                            log.Str10 = SFTPConstants.sftppwd;
                            SFTPHelper sftp = new SFTPHelper(SFTPConstants.sftpip, SFTPConstants.sftpport, SFTPConstants.sftpuser, SFTPConstants.sftppwd);

                            //string sftpfilepath = SFTPConstants.sftpfilepath + @"\" + filename;
                            //发送文件
                            sftp.Put(sendfiles[i], filename);
                            log.ToFileName = SFTPConstants.SentFilePath + @"\" + filename;
                            log.Flag = "Y";
                            log.ResultDesc = "发送成功";
                            FileCommon.MoveToCover(log.SourceFileName, log.ToFileName);

                        }
                        catch (Exception ex)
                        {
                            log.Flag = "N";
                            log.ResultDesc = "发送失败：" + ex.Message.ToString();
                        }
                        new LogOperationService().AddNikeReturnSFTPLog(log);
                    }


                }
            }
            catch (Exception e)
            {
                WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                log.Type = "SendFile";
                log.Flag = "N";
                log.ResultDesc = e.Message.ToString();
                new LogOperationService().AddNikeReturnSFTPLog(log);
            }
        }

    }
}

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
    /// 解析文件判断类型
    /// </summary>
    public class ParsingFile
    {

        public void ReadFile()
        {
            try
            {
                //读取文件列表
                string[] receivefiles = Directory.GetFiles(SFTPConstants.ReceiveFilePath);
                if (receivefiles.Length > 0)
                {
                    //TextHelper txthelper = new TextHelper();
                    for (int i = 0; i < receivefiles.Length; i++)
                    {
                        WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                        log.SourceFileName = receivefiles[i];

                        FileInfo file = new FileInfo(receivefiles[i]);
                        string filename = file.Name;
                        string result = "";//解析的错误提示
                        string externumber = "";
                        try
                        {
                            List<string> txtlists = TextHelper.ReadTextFileToList(receivefiles[i]);//读取成list

                            //没有数据
                            if (txtlists.Count() > 0)
                            {
                                //可以处理多个接口文件
                                switch (txtlists[0].ToString().Substring(0, 9).Trim())
                                {
                                    case "WMSSHP"://LF推给我们的出库单
                                        log.Type = "WMSALLOC";
                                        result = new WMSALLOCManage().LFOrderImportByLoadKey(txtlists, out externumber);
                                        break;
                                    default:
                                        log.Type = "";
                                        result = "未能从文件中识别出对应的接口";
                                        break;
                                }

                                if (result == "")
                                {
                                    //解析成功，移动到success文件夹
                                    log.ToFileName = SFTPConstants.SuccessFilePath + @"\" + log.Type + @"\" + filename;
                                    log.ResultDesc = "解析成功";
                                    log.Str1 = externumber;
                                    log.Flag = "Y";
                                }
                                else
                                {
                                    if (log.Type != "")
                                    {
                                        if (result.Contains("数据库插入失败"))
                                        {
                                            log.ToFileName = "";// SFTPConstants.SuccessFilePath + @"\" + log.Type + @"\" + filename;
                                            log.ResultDesc = "解析失败：" + result;
                                            log.Str1 = externumber;
                                            log.Flag = "E";
                                        }
                                        else
                                        {
                                            log.ToFileName = SFTPConstants.FaildFilePath + @"\" + log.Type + @"\" + filename;//移动到解析失败文件夹                                            
                                            log.ResultDesc = "解析失败：" + result;
                                            log.Str1 = externumber;
                                            log.Flag = "N";
                                        }
                                    }
                                    else
                                    {
                                        log.ToFileName = SFTPConstants.ErrorFilePath + @"\" + filename;
                                        log.ResultDesc = "解析失败：" + result;
                                        log.Str1 = externumber;
                                        log.Flag = "N";
                                    }
                                }
                            }
                            else
                            {
                                log.ToFileName = SFTPConstants.ErrorFilePath + @"\" + filename;
                                log.Flag = "N";
                                log.ResultDesc = "解析失败：文档中无数据";
                            }

                        }
                        catch (Exception ex)
                        {
                            //报错了放到error文件
                            log.ToFileName = SFTPConstants.ErrorFilePath + @"\" + filename;
                            log.Flag = "N";
                            log.ResultDesc = "解析报错：" + ex.Message.ToString();
                        }
                        if (log.Flag == "E")//数据库失败再解析一次
                        {

                        }
                        else
                        {
                            FileCommon.MoveToCover(log.SourceFileName, log.ToFileName);
                        }
                        new LogOperationService().AddNikeReturnSFTPLog(log);
                    }
                }

            }
            catch (Exception e)
            {
                WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                log.Flag = "N";
                log.ResultDesc = "读取文件报错：" + e.Message.ToString();
                new LogOperationService().AddNikeReturnSFTPLog(log);
            }

        }

    }


}

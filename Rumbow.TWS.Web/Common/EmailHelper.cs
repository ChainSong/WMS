using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Runbow.TWS.Web.Common
{
    public class EmailHelper
    {
        public static readonly string Email = System.Configuration.ConfigurationManager.AppSettings["SenderMail"].ToString().Trim();
        public static readonly string EmailPWD = System.Configuration.ConfigurationManager.AppSettings["SenderPWD"].ToString().Trim();
        public static readonly string Host = System.Configuration.ConfigurationManager.AppSettings["Host"].ToString().Trim();
        public static readonly string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString().Trim();
        //public static readonly string MailToConfig = System.Configuration.ConfigurationManager.AppSettings["MailToConfig"].ToString().Trim();
        //public static readonly string CCEmailConfig = System.Configuration.ConfigurationManager.AppSettings["CCEmailConfig"].ToString().Trim();
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="EmailTo">收件人</param>
        /// <param name="CCEmail">抄送人</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">内容</param>
        /// <param name="FilePath">附件</param>
        /// <returns></returns>
        public static string Send(string Subject, string Body, List<string> FilePath, string MailToConfig, string CCEmailConfig)
        {
            List<string> EmailToList = new List<string>();
            List<string> CCEmailList = new List<string>();
            var MailTos = MailToConfig.Split('|');
            var CCEmails = CCEmailConfig.Split('|');
            for (int i = 0; i < MailTos.Length; i++)
            {
                EmailToList.Add(MailTos[i]);
            }
            for (int i = 0; i < CCEmails.Length; i++)
            {
                CCEmailList.Add(CCEmails[i]);
            }
            string result = "";
            SmtpClient mail = new SmtpClient();
            //发送方式
            mail.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp服务器
            mail.Host = Host;
            mail.Port = 587;
            mail.UseDefaultCredentials = false;
            mail.EnableSsl = true;
            //用户名凭证          
            mail.Credentials = new System.Net.NetworkCredential(UserName, EmailPWD);
            //邮件信息
            MailMessage message = new MailMessage();
            try
            {
                //收件人
                foreach (string ReceiverEmail in EmailToList)
                {
                    if (ReceiverEmail.ToString().Trim() != "")
                    {
                        if (!message.To.Contains(new MailAddress(ReceiverEmail)))
                        {
                            message.To.Add(ReceiverEmail.ToString());
                        }
                    }
                }

            }
            catch { }

            try
            {
                //抄送人
                foreach (string CCEmailItem in CCEmailList)
                {
                    if (CCEmailItem.ToString().Trim() != "")
                    {
                        if (!message.CC.Contains(new MailAddress(CCEmailItem)))
                        {
                            message.CC.Add(CCEmailItem.ToString());
                        }
                    }
                }

            }
            catch { }
            //添加抄送人

            //发件人
            message.From = new MailAddress(Email);

            //主题
            message.Subject = Subject;
            //内容
            message.Body = Body + "<br/>===============================<br/>此邮件为系统自动发送，请不要回复。<br/>";
            //正文编码
            message.BodyEncoding = System.Text.Encoding.UTF8;
            //设置为HTML格式
            message.IsBodyHtml = true;
            //优先级
            message.Priority = MailPriority.Normal;

            try
            {
                foreach (string FileNamePath in FilePath)
                {
                    message.Attachments.Add(new Attachment(FileNamePath));
                }
            }
            catch
            {
            }

            try
            {
                mail.Send(message);
                message.Attachments.Dispose();
                return result;
            }
            catch (Exception e)
            {
                message.Attachments.Dispose();
                result = e.ToString();
            }
            return result;
        }
    }
}
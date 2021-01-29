using D.Net.EmailClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace Runbow.TWS.Dao
{
    public class getMail 
    {
        public void mail(string PATH)
        {
            POP3_Wrapper pop = new POP3_Wrapper();
            pop.Connect("pop.qiye.163.com", "tforecast@runbow.com.cn", "abc123", 995, true);
            int s = pop.GetMessagesCount();
            int cc = s - 50;
            pop.LoadMessages();
            if (!Directory.Exists(PATH))
                Directory.CreateDirectory(PATH);

        for (int i = 0; i < pop.Messages.Count; i++)
        {
            string c = pop.Messages[i].From[0].ToString();

            for (int j = 0; j < pop.Messages[i].Attachments.Count; j++)
            {
                int cs = 0;
                string id = pop.Messages[i].UID;
                cs = ForecastWarehouseAccessor.count(id);
                if (cs == 0)
                {
                    if (pop.Messages[i].From[0].Equals("sz_transportation@adidas.com"))
                    {
                        int x = ForecastWarehouseAccessor.count2(id);

                        //FileStream fs = new FileStream("D:\\Storage\\UI\\Email\\" + pop.Messages[i].Attachments[j].Text, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        //MemoryStream stream = new MemoryStream(b);

                        //FileStream fs = new FileStream("D:\\Storage\\UI\\Email\\" + pop.Messages[i].Attachments[j].Text, FileMode.CreateNew);

                        //fs.Write(pop.Messages[i].Attachments[j].Body, 0, pop.Messages[i].Attachments[j].Body.Length);
                        //fs.Close();

                        System.IO.File.WriteAllBytes(PATH + pop.Messages[i].Subject.Substring(20, 12).ToString() + ".xls", pop.Messages[i].Attachments[j].Body);
                       // System.IO.File.WriteAllBytes("E:\\Storage\\Storage\\UI\\Email\\" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+i + ".xls", pop.Messages[i].Attachments[j].Body);
                    }

                }

            }
        }
      //  ComFun.LogTxtAdd("测试2", "进入方法2");
   // LogTxtAdd("测试2", "进入方法2");

        ForecastWarehouseAccessor.bianli(PATH);
    }
        public static string GetLogPath()
        {
            return "E:\\CANDA\\TWS\\Runbow.TWS\\WindowsService1\\bin\\Debug\\Log";
        }

        public static void LogTxtAdd(string message, string type)
        {
            string LogPath = GetLogPath();
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string filePath = LogPath + "\\" + "Log" + DateTime.Now.ToString("yyyyMM") + ".txt";
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "   ");
            sb.Append(type + "   ");
            sb.Append(message);
            try
            {
                File.AppendAllText(filePath, sb.ToString() + "\r\n");
            }
            catch (Exception)
            {

            }
        }



       
    }
}

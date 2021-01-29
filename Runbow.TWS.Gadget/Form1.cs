using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Runbow.TWS.Common;
using Runbow.TWS.Gadget.Model;

namespace Runbow.TWS.Gadget
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.FileName;
            string filePath = textBox1.Text;
            //filePath = "F:\\朱学刚\\AKZO\\412E\\412E20171031.csv";
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            int a = 0;
            foreach (DataRow item in dt.Rows)
            {
                a++;   
                MBGMCR02 MBGMCR02 = new MBGMCR02();
                MBGMCR02.IDOC = new IDOC();
                MBGMCR02.IDOC.BEGIN = "1";
                MBGMCR02.IDOC.EDI_DC40 = new EDI_DC40();
                MBGMCR02.IDOC.EDI_DC40.SEGMENT = "1";
                MBGMCR02.IDOC.EDI_DC40.DOCNUM = "K97D" + DateTime.Now.ToString("yyyyMMddhhmmss") + a.ToString();
                MBGMCR02.IDOC.EDI_DC40.IDOCTYP = "MBGMCR02";
                MBGMCR02.IDOC.EDI_DC40.MESTYP = "MBGMCR";
                MBGMCR02.IDOC.EDI_DC40.SNDPOR = "ONEH_L_PRI";
                MBGMCR02.IDOC.EDI_DC40.SNDPRT = "LS";
                MBGMCR02.IDOC.EDI_DC40.SNDPRN = "ONEHUBK97D";
                MBGMCR02.IDOC.EDI_DC40.RCVPOR = "SAPCDE";
                MBGMCR02.IDOC.EDI_DC40.RCVPRT = "LS";
                MBGMCR02.IDOC.EDI_DC40.RCVPRN = "CDE100";

                MBGMCR02.IDOC.E1BP2017_GM_HEAD_01 = new E1BP2017_GM_HEAD_01();
                MBGMCR02.IDOC.E1BP2017_GM_HEAD_01.SEGMENT = "1";
                MBGMCR02.IDOC.E1BP2017_GM_HEAD_01.PSTNG_DATE = DateTime.Now.ToString("yyyyMMdd");
                MBGMCR02.IDOC.E1BP2017_GM_HEAD_01.DOC_DATE = DateTime.Now.ToString("yyyyMMdd");
                MBGMCR02.IDOC.E1BP2017_GM_HEAD_01.EXT_WMS = "2";

                MBGMCR02.IDOC.E1BP2017_GM_CODE = new E1BP2017_GM_CODE();
                MBGMCR02.IDOC.E1BP2017_GM_CODE.SEGMENT = "1";
                MBGMCR02.IDOC.E1BP2017_GM_CODE.GM_CODE = "06";

                MBGMCR02.IDOC.E1BP2017_GM_ITEM_CREATE = new List<E1BP2017_GM_ITEM_CREATE>();
                E1BP2017_GM_ITEM_CREATE create = new E1BP2017_GM_ITEM_CREATE();
                create.SEGMENT = "1";
                create.MATERIAL = "00000000000" + item["Material"].ToString();
                create.PLANT = "K97D";
                create.STGE_LOC = "0090";
                create.MOVE_TYPE = "412";
                create.SPEC_STOCK = "E";
                create.ENTRY_QNT = item["Qty"].ToString();
                create.MOVE_STLOC = "0090";
                create.VAL_SALES_ORD = item["Document"].ToString();

                create.VAL_S_ORD_ITEM = item["Item"].ToString();
                create.NO_PST_CHGNT = "X";
                create.NO_TRANSFER_REQ = "X";
                MBGMCR02.IDOC.E1BP2017_GM_ITEM_CREATE.Add(create);
                //生成备份文件
                XmlSerializerHelper<MBGMCR02> confrimReceipt = new XmlSerializerHelper<MBGMCR02>();
                confrimReceipt.Value = MBGMCR02;
                //已当前年月日为文件名称，写死*******
                string savefilepath = MBGMCR02.IDOC.EDI_DC40.DOCNUM;
                confrimReceipt.CreateTo(@"F:\朱学刚\AKZO\412E\result" + @"\" + savefilepath + ".xml");
            }
        }
    }
}

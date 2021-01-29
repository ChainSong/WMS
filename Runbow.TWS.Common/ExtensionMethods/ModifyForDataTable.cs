using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Runbow.TWS.Common
{
    public class ModifyForDataTable
    {
        public DataSet AddSerialNumberForNFS(DataSet dt)
        {
            DataSet d = new DataSet();
            DataTable d0 = dt.Tables[0];
            DataTable d1 = dt.Tables[1];

            d0.DefaultView.Sort = "外部单号 ASC";//排序
            d0 = d0.DefaultView.ToTable();
            string PreviousExternNumber = "";//上一个外部单号
            Random ran = new Random(GetRandomSeed());
            string SerialNumber = ran.Next().ToString().Substring(0,3);//序列号 DateTime.Now.ToString("HHmmss")
            for (int i = 0; i < d0.Rows.Count; i++)
            {
                if (i==0)
                {
                    PreviousExternNumber = d0.Rows[i]["外部单号"].ToString();
                    d0.Rows[i]["外部单号"] = d0.Rows[i]["外部单号"] + "-" + SerialNumber;
                    for (int p = 0; p < d1.Rows.Count; p++)
                    {
                        string s = d1.Rows[p]["外部单号"].ToString() ;
                        if (PreviousExternNumber==s)
                        {
                            d1.Rows[p]["外部单号"] = d1.Rows[p]["外部单号"] + "-" + SerialNumber;
                        }
                    }
                }
                else
                {
                    if (PreviousExternNumber == d0.Rows[i]["外部单号"].ToString())
                    {
                        d0.Rows[i]["外部单号"] = d0.Rows[i - 1]["外部单号"];
                        for (int k = 0; k < d1.Rows.Count; k++)
                        {
                            string s = d1.Rows[k]["外部单号"].ToString();
                            if (PreviousExternNumber == s)
                            {
                                d1.Rows[k]["外部单号"] = d1.Rows[k]["外部单号"] + "-" + SerialNumber;
                            }
                        }
                    }
                    else
                    {
                        PreviousExternNumber = d0.Rows[i]["外部单号"].ToString();
                        SerialNumber = ran.Next().ToString().Substring(0, 3);
                        d0.Rows[i]["外部单号"] = d0.Rows[i]["外部单号"] + "-" + SerialNumber;
                        for (int m = 0; m < d1.Rows.Count; m++)
                        {
                            string s = d1.Rows[m]["外部单号"].ToString();
                            if (PreviousExternNumber == s)
                            {
                                d1.Rows[m]["外部单号"] = d1.Rows[m]["外部单号"] + "-" + SerialNumber;
                            }
                        }
                    }
                }
            }
            d.Tables.Add(d0.Copy());
            d.Tables.Add(d1.Copy());
            return d;
        }

        //获取随机种子
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
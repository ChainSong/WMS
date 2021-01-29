using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.InventoryApi;
using Runbow.TWS.MessageContracts.Inventory;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao.Inventory
{
    public class DaoSampleStorage : BaseAccessor
    {
       public DataTable ds = new DataTable();
       private string connStr = ConfigurationManager.ConnectionStrings["Sample"].ConnectionString.ToString();
       public SampleStorageResponses GetStorage(SampleStorageCondition WhereStorage)
       {
           SampleStorageResponses Sample = new SampleStorageResponses();
           using (SqlConnection conn = new SqlConnection(connStr))
           {
               try
               {
                   SqlCommand cmd = new SqlCommand("pro_wms_StorageSelect", conn);
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.AddWithValue("@Category", WhereStorage.Category);
                   cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[0].Size = 100;

                   cmd.Parameters.AddWithValue("@SKU", WhereStorage.SKU);
                   cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[1].Size = 4000;

                   cmd.Parameters.AddWithValue("@PE", WhereStorage.PE);
                   cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[2].Size = 100;

                   cmd.Parameters.AddWithValue("@RcvBeginDate", WhereStorage.ReceiveBeginDate);
                   cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[3].Size = 100;

                   cmd.Parameters.AddWithValue("@RcvEndDate", WhereStorage.ReceiveEndDate);
                   cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[4].Size = 100;
                   

                   cmd.Parameters.AddWithValue("@PageIndex", WhereStorage.PageIndex);
                   cmd.Parameters[5].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@PageSize", WhereStorage.PageSize);
                   cmd.Parameters[6].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@RowCount",0);
                   cmd.Parameters[7].Direction = ParameterDirection.Output;
                   cmd.Parameters[7].SqlDbType = SqlDbType.Int;
                   

                   conn.Open();
                   SqlDataAdapter sda = new SqlDataAdapter();
                   sda.SelectCommand = cmd;
                   sda.Fill(ds);
                   Sample.RowCount = (int)cmd.Parameters[7].Value;
                   Sample.SampleSto = ds.ConvertToEntityCollection<SampleStorageSelect>();
                   conn.Close();
               }
               catch (Exception)
               {

                   throw;
               }

           }
           return Sample;
       }
       public SampleThelibraryResponses GetThelibrary(SampleThelibraryCondition WhereThelibrary)
       {
           SampleThelibraryResponses Sample = new SampleThelibraryResponses();
           using (SqlConnection conn = new SqlConnection(connStr))
           {
               try
               {
                   SqlCommand cmd = new SqlCommand("pro_wms_SampleThelibrarySelect", conn);
                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.Parameters.AddWithValue("@Category",WhereThelibrary.Category);
                   cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[0].Size = 100;

                   cmd.Parameters.AddWithValue("@SKU", WhereThelibrary.SKU);
                   cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[1].Size = 4000;

                   cmd.Parameters.AddWithValue("@PE", WhereThelibrary.PE);
                   cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[2].Size = 100;

                   cmd.Parameters.AddWithValue("@Requester", WhereThelibrary.Requester);
                   cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[3].Size = 4000;

                   cmd.Parameters.AddWithValue("@OrderBeginTime", WhereThelibrary.OrderBeginTime);
                   cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[4].Size = 100;

                   cmd.Parameters.AddWithValue("@OrderEndTime", WhereThelibrary.OrderEndTime);
                   cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[5].Size = 100;

                   cmd.Parameters.AddWithValue("@DeliverBeginTime", WhereThelibrary.DeliverBeginTime);
                   cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[6].Size = 100;

                   cmd.Parameters.AddWithValue("@DeliverEndTime", WhereThelibrary.DeliverEndTime);
                   cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[7].Size = 100;

                   cmd.Parameters.AddWithValue("@Type", WhereThelibrary.Type);
                   cmd.Parameters[8].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[8].Size = 10;

                   cmd.Parameters.AddWithValue("@PageIndex", WhereThelibrary.PageIndex);
                   cmd.Parameters[9].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@PageSize", WhereThelibrary.PageSize);
                   cmd.Parameters[10].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@RowCount", 0);
                   cmd.Parameters[11].Direction = ParameterDirection.Output;
                   cmd.Parameters[11].SqlDbType = SqlDbType.Int;

                   conn.Open();
                   SqlDataAdapter sda = new SqlDataAdapter();
                   sda.SelectCommand = cmd;
                   sda.Fill(ds);
                   //Type=0才分页
                   if (WhereThelibrary.Type == "0")
                   {
                       Sample.RowCount = (int)cmd.Parameters[11].Value;
                   }
                   Sample.SampleTb = ds.ConvertToEntityCollection<SampleThelibrarySelect>();
                   conn.Close();
               }
               catch (Exception)
               {

                   throw;
               }

           }
           return Sample;
       }
       public SampleStockResponses GetSampleStock(SampleStockCondition WhereStock)
       {
           string sqlWhere = "";
           if (WhereStock != null)
           {
               sqlWhere = StockCondition(WhereStock);
           }
           SampleStockResponses Sample = new SampleStockResponses();
           using (SqlConnection conn = new SqlConnection(connStr))
           {
               try
               {
                   SqlCommand cmd = new SqlCommand("[pro_wms_SampleStockSelect]", conn);
                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.Parameters.AddWithValue("@Where", sqlWhere);
                   cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                   cmd.Parameters[0].Size = 4000;

                   cmd.Parameters.AddWithValue("@PageIndex", WhereStock.PageIndex);
                   cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@PageSize", WhereStock.PageSize);
                   cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                   cmd.Parameters.AddWithValue("@RowCount", 0);
                   cmd.Parameters[3].Direction = ParameterDirection.Output;
                   cmd.Parameters[3].SqlDbType = SqlDbType.Int;

                   //cmd.Parameters.AddWithValue("@Size", WhereStock.Size);
                   //cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[0].Size = 4000;
                   //cmd.Parameters.AddWithValue("@Gender", WhereStock.Gender);
                   //cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[1].Size = 4000;
                   //cmd.Parameters.AddWithValue("@Season", WhereStock.Season);
                   //cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[2].Size = 100;
                   //cmd.Parameters.AddWithValue("@SKU", WhereStock.SKU);
                   //cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[3].Size = 8000;
                   //cmd.Parameters.AddWithValue("@PE", WhereStock.PE);
                   //cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[4].Size = 100;
                   //cmd.Parameters.AddWithValue("@ModelName", WhereStock.ModelName);
                   //cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[5].Size = 100;
                   //cmd.Parameters.AddWithValue("@FOBBegin", WhereStock.FOBBegin);
                   //cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[6].Size = 100;
                   //cmd.Parameters.AddWithValue("@FOBEnd", WhereStock.FOBEnd);
                   //cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[7].Size = 100;
                   //cmd.Parameters.AddWithValue("@RetailPriceBegin", WhereStock.RetailPriceBegin);
                   //cmd.Parameters[8].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[8].Size = 100;
                   //cmd.Parameters.AddWithValue("@RetailPriceEnd", WhereStock.RetailPriceEnd);
                   //cmd.Parameters[9].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[9].Size = 100;
                   //cmd.Parameters.AddWithValue("@Silhouette", WhereStock.Silhouette);
                   //cmd.Parameters[10].SqlDbType = SqlDbType.VarChar;
                   //cmd.Parameters[10].Size = 100;

                   //cmd.Parameters.AddWithValue("@PageIndex", WhereStock.PageIndex);
                   //cmd.Parameters[11].SqlDbType = SqlDbType.Int;

                   //cmd.Parameters.AddWithValue("@PageSize", WhereStock.PageSize);
                   //cmd.Parameters[12].SqlDbType = SqlDbType.Int;

                   //cmd.Parameters.AddWithValue("@RowCount", 0);
                   //cmd.Parameters[13].Direction = ParameterDirection.Output;
                   //cmd.Parameters[13].SqlDbType = SqlDbType.Int;
                   conn.Open();
                   SqlDataAdapter sda = new SqlDataAdapter();
                   sda.SelectCommand = cmd;
                   sda.Fill(ds);
                   Sample.RowCount = (int)cmd.Parameters[3].Value;
                   Sample.SampleSS = ds.ConvertToEntityCollection<SampleStockSelect>();
                   conn.Close();
               }
               catch (Exception)
               {

                   throw;
               }

           }
           return Sample;
       }
       public SampleJobResponses GetSampleJob(SampleJobCondition WhereJob)
        {
            SampleJobResponses Sample = new SampleJobResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_SampleJobSelect", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BeginTime", WhereJob.BeginTime);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 100;

                    cmd.Parameters.AddWithValue("@EndTime", WhereJob.EndTime);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 100;

                    cmd.Parameters.AddWithValue("@PageIndex", WhereJob.PageIndex);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", WhereJob.PageSize);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].SqlDbType = SqlDbType.Int;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    Sample.RowCount = (int)cmd.Parameters[4].Value;
                    Sample.SampleJob = ds.ConvertToEntityCollection<SampleJobSelect>();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return Sample;
        }
       public string GetImport(DataTable dt)
       {
           string result = string.Empty;
           using (SqlConnection conn = new SqlConnection(connStr))
           {
               try
               {
                   SqlCommand cmd = new SqlCommand("pro_wms_SampleImport", conn);
                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.Parameters.AddWithValue("@SampleTB", dt);
                   cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                   SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                   para.Direction=ParameterDirection.InputOutput;
                   para.Value="";
                   cmd.Parameters.Add(para);
                   conn.Open();
                   cmd.ExecuteNonQuery();
                   result = cmd.Parameters[1].Value.ToString();
                   conn.Close();
               }
               catch (Exception)
               {
                   throw;
               }
           }
           return result;
       }
       public DataTable GetDropDownList()
       {
           string result = string.Empty;
           DataTable dt = new DataTable();
           using (SqlConnection conn = new SqlConnection(connStr))
           {
               try
               {
                   SqlCommand cmd = new SqlCommand(" SELECT TOP 17 SKUCategory03 FROM tbl_wms_SKU WHERE SKUCategory03 NOT IN ('BA4645-055SP2014','NIKESPORTSWEAR','FOOTBALL, BASEBALL, AT')  GROUP BY SKUCategory03 ORDER BY SKUCategory03 DESC", conn);
                   cmd.CommandType = CommandType.Text;
                   conn.Open();
                   new SqlDataAdapter()
                   {
                       SelectCommand = cmd
                   }.Fill(dt);
                   conn.Close();
               }
               catch (Exception)
               {

                   throw;
               }
           }
           return dt;
       }
       private string StockCondition(SampleStockCondition Stock)
       {
           StringBuilder sb = new StringBuilder();
           sb.Append(" 1=1");
           if (Stock.SKU != null && Stock.SKU != "")
           {
               sb.Append(" AND substring(sk.SKUCategory10,1,len(sk.SKUCategory10)-6) in(" + Stock.SKU + ")");
           }
           if (Stock.PE != null && Stock.PE != "")
           {
               sb.Append(" AND SKUCategory07 ='" + Stock.PE + "'");
           }
           if (Stock.Gender != null && Stock.Gender != "")
           {
               sb.Append(" AND SKUCategory01 in(" + Stock.Gender + ")");
           }
           if (Stock.Size != null && Stock.Size != "")
           {
               sb.Append(" AND SKUCategory09 in(" + Stock.Size + ")");
           }
           if (Stock.ModelName != null && Stock.ModelName != "")
           {
               sb.Append(" AND sk.SKUCategory03 ='" + Stock.ModelName + "'");
           }

           
           if (Stock.Silhouette != null && Stock.Silhouette != "")
           {
               if (Stock.Silhouette.IndexOf(',') > 0)
               {
                   sb.Append(" AND (1=1");
                   string[] str = Stock.Silhouette.Split(',');
                   for (int i = 0; i < str.Length; i++)
                   {
                       sb.Append(" OR sk.AllocationRule LIKE '%" + str[i].ToString() + "%'");
                   }
                   sb.Append(") ");
               }
               else
                   sb.Append(" AND sk.AllocationRule LIKE '%" + Stock.Silhouette + "%'");
           }
           if (Stock.Season != null && Stock.Season != "")
           {
               if (Stock.Season.IndexOf(',') > 0)
               {
                   sb.Append(" AND (1=1");
                   string[] str = Stock.Season.Split(',');

                   for (int i = 0; i < str.Length; i++)
                   {
                       sb.Append(" OR lc.LotCategory02 LIKE '%" + str[i].ToString() + "%'");
                   }
                   sb.Append(") ");
               }
               else
                   sb.Append(" AND lc.LotCategory02 LIKE '%" + Stock.Season + "%'");
              
           }
           //if (Stock.ModelName != null && Stock.ModelName != "")
           //{
           //    if (Stock.ModelName.IndexOf(',') > 0)
           //    {
           //        sb.Append(" AND (1=1");
           //        string[] str = Stock.ModelName.Split(',');

           //        for (int i = 0; i < str.Length; i++)
           //        {
           //            sb.Append(" OR sk.AttentionToAddressLine2 LIKE '%" + str[i].ToString() + "%'");
           //        }
           //        sb.Append(") ");
           //    }
           //    else
           //        sb.Append(" AND sk.AttentionToAddressLine2 LIKE '%" + Stock.ModelName + "%'");
               
           //}
           if (Stock.RetailPriceBegin != null && Stock.RetailPriceBegin != "")
           {
               sb.Append(" AND sk.AttentionToAddressLine4 >=" + Stock.RetailPriceBegin + "");
           }
           if (Stock.RetailPriceEnd != null && Stock.RetailPriceEnd != "")
           {
               sb.Append(" AND sk.AttentionToAddressLine4 <=" + Stock.RetailPriceEnd + "");
           }
           if (Stock.FOBBegin != null && Stock.FOBBegin != "")
           {
               sb.Append(" AND sk.AttentionToAddressLine3 >=" + Stock.FOBBegin + "");
           }
           if (Stock.FOBEnd != null && Stock.FOBEnd != "")
           {
               sb.Append(" AND sk.AttentionToAddressLine3 <=" + Stock.FOBEnd + "");
           }
           
           return sb.ToString();
       }
    }
}

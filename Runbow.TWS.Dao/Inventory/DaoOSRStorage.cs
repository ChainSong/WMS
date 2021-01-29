using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.Inventory;
using Runbow.TWS.Entity.InventoryApi;
using Runbow.TWS.Entity;
namespace Runbow.TWS.Dao.Inventory
{
    public class DaoOSRStorage 
    {
        DataTable dt = new DataTable();
        private string connStr = ConfigurationManager.ConnectionStrings["OSR"].ConnectionString.ToString();
        private string connStr_GZ = ConfigurationManager.ConnectionStrings["OSR_GZ"].ConnectionString.ToString();
        private string connStrNFS_BJ = ConfigurationManager.ConnectionStrings["BJ_NFS"].ConnectionString.ToString();
        private string connStrNFS_GZ = ConfigurationManager.ConnectionStrings["GZ_NFS"].ConnectionString.ToString();
        public OSRStorageResponses GetOSRStorage(OSRStorageCondition WhereStorage)
        {
            DataSet ds = new DataSet();
            OSRStorageResponses or = new OSRStorageResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRStorageSelect", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PE", WhereStorage.PE);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 100;

                    cmd.Parameters.AddWithValue("@Category", WhereStorage.Category);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 100;

                    cmd.Parameters.AddWithValue("@TransOrderNO", WhereStorage.TransOrderNO);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 2000;

                    cmd.Parameters.AddWithValue("@Status", WhereStorage.Status);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[3].Size = 100;

                    cmd.Parameters.AddWithValue("@BeginTime", WhereStorage.BeginTime);
                    cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[4].Size = 50;

                    cmd.Parameters.AddWithValue("@EndTime", WhereStorage.EndTime);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 50;

                    cmd.Parameters.AddWithValue("@ShiptoCode", WhereStorage.ShiptoCode);
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 10;

                    cmd.Parameters.AddWithValue("@Type", WhereStorage.Type);
                    cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[7].Size = 10;

                    cmd.Parameters.AddWithValue("@PageIndex", WhereStorage.PageIndex);
                    cmd.Parameters[8].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", WhereStorage.PageSize);
                    cmd.Parameters[9].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[10].Direction = ParameterDirection.Output;
                    cmd.Parameters[10].SqlDbType = SqlDbType.Int;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    or.RowCount = (int)cmd.Parameters[10].Value;
                    or.Header = ds.Tables[0].ConvertToEntityCollection<OSRStorageHeader>();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return or;
        }
        public OSRStorageResponses GetOSRReceivingDetailed(OSRStorageCondition WhereStorage)
        {
            DataSet ds = new DataSet();
            OSRStorageResponses or = new OSRStorageResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    if (WhereStorage.Identification == "Export")
                    {
                        cmd.CommandText = "pro_wms_OSRReceivingExport";

                    }
                    else
                    {
                        cmd.CommandText = "pro_wms_OSRReceivingDetailed";
                    }
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PE", WhereStorage.PE);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 100;

                    cmd.Parameters.AddWithValue("@Category", WhereStorage.Category);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 100;

                    cmd.Parameters.AddWithValue("@TransOrderNO", WhereStorage.TransOrderNO);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 2000;

                    cmd.Parameters.AddWithValue("@Status", WhereStorage.Status);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[3].Size = 100;

                    cmd.Parameters.AddWithValue("@BeginTime", WhereStorage.BeginTime);
                    cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[4].Size = 50;

                    cmd.Parameters.AddWithValue("@EndTime", WhereStorage.EndTime);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 50;

                    cmd.Parameters.AddWithValue("@ShiptoCode", WhereStorage.ShiptoCode);
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 10;

                    cmd.Parameters.AddWithValue("@Type", WhereStorage.Type);
                    cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[7].Size = 10;

                    cmd.Parameters.AddWithValue("@ReceiptNumber", WhereStorage.ReceiptNumber);
                    cmd.Parameters[8].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[8].Size = 100;
                    cmd.CommandTimeout = 180;
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    if (WhereStorage.Identification == "Export")
                    {
                        or.Export = ds.Tables[0].ConvertToEntityCollection<OSRStorageSelect>();
                    }
                    else
                    {
                        or.Header = ds.Tables[0].ConvertToEntityCollection<OSRStorageHeader>();
                        or.Detailed = ds.Tables[1].ConvertToEntityCollection<OSRStorageDetailed>();
                        conn.Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return or;
        }
        public OSRThelibraryResponses GetOSRThelibrary(OSRThelibraryCondition WhereStorage)
        {
            DataSet ds = new DataSet();
            OSRThelibraryResponses or = new OSRThelibraryResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRThelibrarySelect", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", WhereStorage.Category);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 100;
                    cmd.Parameters.AddWithValue("@TransOrderNO", WhereStorage.TransOrderNO);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 4000;
                    cmd.Parameters.AddWithValue("@PE", WhereStorage.PE);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@DeliveryStore", WhereStorage.DeliveryStore);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[3].Size = 100;
                    cmd.Parameters.AddWithValue("@ShiptoCode", WhereStorage.ShiptoCode);
                    cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[4].Size = 100;
                    
                    cmd.Parameters.AddWithValue("@BeginTime", WhereStorage.BeginTime);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 50;
                    cmd.Parameters.AddWithValue("@EndTime", WhereStorage.EndTime);
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 50;
                    cmd.Parameters.AddWithValue("@SKU", WhereStorage.SKU);
                    cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[7].Size = 2000;
                    cmd.Parameters.AddWithValue("@UPC", WhereStorage.UPC);
                    cmd.Parameters[8].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[8].Size = 2000;
                    cmd.Parameters.AddWithValue("@Season", WhereStorage.Season);
                    cmd.Parameters[9].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[9].Size = 200;
                    cmd.Parameters.AddWithValue("@Article", WhereStorage.Article);
                    cmd.Parameters[10].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[10].Size = 200;
                    cmd.Parameters.AddWithValue("@Type", WhereStorage.Type);
                    cmd.Parameters[11].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[11].Size = 10;
                    cmd.Parameters.AddWithValue("@PageIndex", WhereStorage.PageIndex);
                    cmd.Parameters[12].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@PageSize", WhereStorage.PageSize);
                    cmd.Parameters[13].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[14].Direction = ParameterDirection.Output;
                    cmd.Parameters[14].SqlDbType = SqlDbType.Int;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    or.RowCount = (int)cmd.Parameters[14].Value;
                    or.Header = ds.Tables[0].ConvertToEntityCollection<OSRThelibraryHeader>();

                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return or;
        }
        public OSRThelibraryResponses GetOSROutboundDetailed(OSRThelibraryCondition WhereStorage)
        {
            DataSet ds = new DataSet();
            OSRThelibraryResponses or = new OSRThelibraryResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    if (WhereStorage.Identification == "Export")
                    {
                        cmd.CommandText = "pro_wms_OSROutboundExport";

                    }
                    else
                    {
                        cmd.CommandText = "pro_wms_OSROutboundDetailed";
                    }
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", WhereStorage.Category);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 100;
                    cmd.Parameters.AddWithValue("@TransOrderNO", WhereStorage.TransOrderNO);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 4000;
                    cmd.Parameters.AddWithValue("@PE", WhereStorage.PE);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 100;
                    cmd.Parameters.AddWithValue("@DeliveryStore", WhereStorage.DeliveryStore);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[3].Size = 100;
                    cmd.Parameters.AddWithValue("@ShiptoCode", WhereStorage.ShiptoCode);
                    cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[4].Size = 100;
                   
                    cmd.Parameters.AddWithValue("@BeginTime", WhereStorage.BeginTime);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 50;
                    cmd.Parameters.AddWithValue("@EndTime", WhereStorage.EndTime);
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 50;
                    cmd.Parameters.AddWithValue("@SKU", WhereStorage.SKU);
                    cmd.Parameters[7].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[7].Size = 2000;
                    cmd.Parameters.AddWithValue("@UPC", WhereStorage.UPC);
                    cmd.Parameters[8].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[8].Size = 2000;
                    cmd.Parameters.AddWithValue("@Season", WhereStorage.Season);
                    cmd.Parameters[9].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[9].Size = 200;
                    cmd.Parameters.AddWithValue("@Article", WhereStorage.Article);
                    cmd.Parameters[10].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[10].Size = 200;
                    cmd.Parameters.AddWithValue("@Type", WhereStorage.Type);
                    cmd.Parameters[11].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[11].Size = 10;
                    cmd.Parameters.AddWithValue("@OrderKey", WhereStorage.OrderKey);
                    cmd.Parameters[12].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[12].Size = 100;
                    cmd.CommandTimeout = 180;
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    if (WhereStorage.Identification == "Export")
                    {
                        or.Export = ds.Tables[0].ConvertToEntityCollection<OSRThelibrarySelect>();
                    }
                    else
                    {
                        or.Header = ds.Tables[1].ConvertToEntityCollection<OSRThelibraryHeader>();
                        or.Detailed = ds.Tables[0].ConvertToEntityCollection<OSRThelibraryDetailed>();
                    }
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return or;
        }
        public OSRStockResponses GetOSRStock(OSRStockCondition WhereStock)
        {
            OSRStockResponses or = new OSRStockResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRStockSelect", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SKU", WhereStock.SKU);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 4000;

                    cmd.Parameters.AddWithValue("@Season", WhereStock.Season);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 2000;

                    cmd.Parameters.AddWithValue("@Category", WhereStock.Category);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 2000;

                    cmd.Parameters.AddWithValue("@Article", WhereStock.Article);
                    cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[3].Size = 2000;

                    cmd.Parameters.AddWithValue("@UPC", WhereStock.UPC);
                    cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[4].Size = 2000;

                    cmd.Parameters.AddWithValue("@PE", WhereStock.PE);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 100;

                    cmd.Parameters.AddWithValue("@PageIndex", WhereStock.PageIndex);
                    cmd.Parameters[6].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", WhereStock.PageSize);
                    cmd.Parameters[7].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[8].Direction = ParameterDirection.Output;
                    cmd.Parameters[8].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@Total", 0);
                    cmd.Parameters[9].Direction = ParameterDirection.Output;
                    cmd.Parameters[9].SqlDbType = SqlDbType.Int;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    or.RowCount = (int)cmd.Parameters[8].Value;
                    or.Total = (int)cmd.Parameters[9].Value;
                    or.OSRsk = dt.ConvertToEntityCollection<OSRStockSelect>();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return or;
        }
        public OSRJobResponses GetOSRJob(OSRJobCondition WhereJob)
        {
            OSRJobResponses OJ = new OSRJobResponses();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRJobSelect", conn);
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
                    sda.Fill(dt);
                    OJ.RowCount = (int)cmd.Parameters[4].Value;
                    OJ.OSRJob = dt.ConvertToEntityCollection<OSRJobSelect>();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return OJ;
        }
        public string GetImport(DataTable ds)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRImport", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OSRTB", ds);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 2000);
                    para.Direction = ParameterDirection.InputOutput;
                    para.Value = "";
                    cmd.CommandTimeout = 180;
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
        public string GetOSRLineNumber(DataTable ds)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRLineNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OSRLineNumber", ds);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 2000);
                    para.Direction = ParameterDirection.InputOutput;
                    para.Value = "";
                    cmd.CommandTimeout = 180;
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
        public string GetOSRLineNumberGZ(DataTable ds)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr_GZ))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_OSRLineNumberGZ", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OSRLineNumber", ds);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 2000);
                    para.Direction = ParameterDirection.InputOutput;
                    para.Value = "";
                    cmd.CommandTimeout = 180;
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
        public string GetNFSLineNumberGZ(DataTable ds)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStrNFS_GZ))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_NFSPackListNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NFSLineNumber", ds);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 2000);
                    para.Direction = ParameterDirection.InputOutput;
                    para.Value = "";
                    cmd.CommandTimeout = 180;
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
        public string GetNFSLineNumberBJ(DataTable ds)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStrNFS_BJ))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_NFSPackListNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NFSLineNumber", ds);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlParameter para = new SqlParameter("@Result", SqlDbType.VarChar, 2000);
                    para.Direction = ParameterDirection.InputOutput;
                    para.Value = "";
                    cmd.CommandTimeout = 180;
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
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT CategoryMeaning,CategoryCode FROM dbo.tbl_wms_OsrAdditionalData ORDER BY CategoryMeaning,CategoryCode ", conn);
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

        //private string StorageCondition(OSRStorageCondition Storage)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (Storage.PE != null && Storage.PE != "")
        //    {
        //        sb.Append(" AND lc.LocationCategory ='" + Storage.PE + "'");
        //    }
        //    if (Storage.Category != null && Storage.Category != "")
        //    {
        //        sb.Append(" AND ad.CategoryCode ='" + Storage.Category + "'");
        //    }
        //    if (Storage.Status != null && Storage.Status != "")
        //    {
        //        sb.Append(" AND rh.Status ='" + Storage.Status + "'");
        //    }
        //    //if (Storage.TransOrderNO != null && Storage.TransOrderNO != "")
        //    //{
        //    //    sb.Append(" AND SKUCategory01 in(" + Storage.TransOrderNO + ")");
        //    //}
        //    return sb.ToString();
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WebApi;
using Runbow.TWS.MessageContracts.WebApi;

namespace Runbow.TWS.Dao.WebApi
{
    public class WXPODServiceAccessor
    {
        private string connStr = ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString();
        public DataSet GetWXPOD(WXPODRequest wx)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
               
                try
                {
                    SqlCommand cmd = new SqlCommand("proc_Pod_Select", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerName", wx.Customer);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.Parameters.AddWithValue("@PODTypeName", wx.PodType);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@ShipperTypeName", wx.ShipperType);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 50;
                    cmd.Parameters.AddWithValue("@StartCityName", wx.txtStart);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 50;
                    cmd.Parameters.AddWithValue("@EndCityName", wx.txtEnd);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Size = 50;
                    cmd.Parameters.AddWithValue("@ActualDeliveryDate", wx.datetimes);
                    cmd.Parameters[5].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[5].Size = 100;
                    cmd.Parameters.AddWithValue("@CustomerOrderNumber", wx.st);
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 200;                  
                    conn.Open();                  
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);                
                    conn.Close();
                   
                }
                catch
                {  }
            }
            return ds;
        }

        public DataSet GetType(string type)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    if (type == "Pod")
                    {
                        SqlConnection cnn = new SqlConnection();//实例化一个连接
                        cnn.ConnectionString = connStr;
                        cnn.Open();//打开数据库连接
                        SqlDataAdapter da = new SqlDataAdapter();//实例化sqldataadpter
                        string sql = "select Name from Config where IdentifyType='PODType'";
                        SqlCommand cmd1 = new SqlCommand(sql, cnn);
                        da.SelectCommand = cmd1;//设置为已实例化SqlDataAdapter的查询命令    
                        da.Fill(ds);//把数据填充到dataset
                        cnn.Close();
                    }

                    if (type == "Shipper")
                    {

                        SqlConnection cnn = new SqlConnection();//实例化一个连接
                        cnn.ConnectionString = connStr;
                        cnn.Open();//打开数据库连接

                        SqlDataAdapter da = new SqlDataAdapter();//实例化sqldataadpter
                        string sql = "select Name from Config where IdentifyType='ShipperType'";
                        SqlCommand cmd1 = new SqlCommand(sql, cnn);
                        da.SelectCommand = cmd1;//设置为已实例化SqlDataAdapter的查询命令    
                        da.Fill(ds);//把数据填充到dataset
                        cnn.Close();
 
                    }
                    if (type == "Customer")
                    {
                        SqlConnection cnn = new SqlConnection();//实例化一个连接
                        cnn.ConnectionString = connStr;
                        cnn.Open();//打开数据库连接

                        SqlDataAdapter da = new SqlDataAdapter();//实例化sqldataadpter
                        string sql = "select Code from Customer";
                        SqlCommand cmd1 = new SqlCommand(sql, cnn);
                        da.SelectCommand = cmd1;//设置为已实例化SqlDataAdapter的查询命令    
                        da.Fill(ds);//把数据填充到dataset
                        cnn.Close();
 
                    }
                }
                catch
                { }
            }
            return ds;
        }
    }
}
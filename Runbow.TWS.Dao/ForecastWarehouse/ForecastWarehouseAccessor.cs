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
    public class ForecastWarehouseAccessor : BaseAccessor
    {
        public IEnumerable<ForecastOrders> GetCRMInfo(ForecastOrders Info, int PageIndex, int PageSize, out int RowCount)
        {
            string strSQL = this.GetSqlWhere(Info);
            string strSQL2 = this.GetSqlWhere2(Info);
            DbParam[] dbParams = {
                           new DbParam("@where",DbType.String,strSQL,ParameterDirection.Input),
                            new DbParam("@zhi",DbType.String,strSQL2,ParameterDirection.Input),
                           new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                           new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                           new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            IEnumerable<ForecastOrders> crmlist = base.ExecuteDataTable("Proc_GetWave", dbParams).ConvertToEntityCollection<ForecastOrders>();
            RowCount = (int)dbParams[4].Value;
            return crmlist;
        }
        public IEnumerable<ForecastOrders> GetCRMInfo2( int PageIndex, int PageSize, out int RowCount)
        {
            
            DbParam[] dbParams = {
                          
                           new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                           new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                           new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            IEnumerable<ForecastOrders> crmlist = base.ExecuteDataTable("Proc_GetWave2", dbParams).ConvertToEntityCollection<ForecastOrders>();
            RowCount = (int)dbParams[2].Value;
            return crmlist;
        }
        public string GetSqlWhere2(ForecastOrders info) {
            StringBuilder sb = new StringBuilder();
            if (info.zhi2 != "请选择")
            {
                if (info.zhi2 == "desc")
                {
                    sb.Append(" desc");

                }
                else
                {

                    sb.Append(" asc");
                }

            }
            return sb.ToString();
        }
        public string GetSqlWhere(ForecastOrders info)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" 1=1 ");
            sb.Append("AND Mail.ThreePL='RBOW'");
            if (!string.IsNullOrEmpty(info.waveId))
            {
                sb.Append(" AND Wave.waveId like '%"+info.waveId.Trim()+"%'");
            }
            if (!string.IsNullOrEmpty(info.WaveReleaseTime))
            {
                sb.Append(" AND CONVERT(varchar, Wave.WaveReleaseTime,112)>=CONVERT(varchar, dateadd(day,0,'" + info.WaveReleaseTime + "'),112)");
            }
            if (info.State != "全部")
            {
                if (info.State == "待发货/正在发货中")
                {
                    sb.Append(" AND State in('正在发货中' , '待发货')");

                }
                else
                {
                    sb.Append(" AND State='" + info.State + "'");
                }
            }
           
            if (!string.IsNullOrEmpty(info.WaveReleaseTime2))
            {
                sb.Append("AND CONVERT(varchar, Wave.WaveReleaseTime,112)<=CONVERT(varchar, dateadd(day,0,'" + info.WaveReleaseTime2 + "'),112)");
            }
            if (!string.IsNullOrEmpty(info.PickTime))
            {
                sb.Append("AND CONVERT(varchar, Wave.PickTime,112)>=CONVERT(varchar, dateadd(day,0,'" + info.PickTime + "'),112)");
            }
            if (!string.IsNullOrEmpty(info.PickTime2))
            {
                sb.Append("AND CONVERT(varchar, Wave.PickTime,112)<=CONVERT(varchar, dateadd(day,0,'" + info.PickTime2 + "'),112)");
            }
            if (!string.IsNullOrEmpty(info.DeliverTime))
            {
                sb.Append("AND CONVERT(varchar,Wave. shipments,112)>=CONVERT(varchar, dateadd(day,0,'" + info.DeliverTime + "'),112)");
            }
            if (!string.IsNullOrEmpty(info.DeliverTime2))
            {
                sb.Append("AND CONVERT(varchar, Wave.shipments,112)<=CONVERT(varchar, dateadd(day,0,'" + info.DeliverTime2 + "'),112)");
            }

            return sb.ToString();
        }
        public string deblocking(string type)
        {

            string sql = string.Format("select State from dbo.Wave where WID={0}", type);

            //comm.CommandType = CommandType.StoredProcedure;

            return base.ExecuteScalar(sql, null).ToString();

        }
        public int deblocking2(string type)
        {

            //DbParam[] dbParams = {
            //           new DbParam("@WID",DbType.String,type,ParameterDirection.Input)};
            //int i = (int)base.ExecuteScalar("", dbParams);
            //return i;
            int C = Convert.ToInt32(type);
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = "update dbo.Wave  set State='正在发货中' where WID=" + type;
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        public int confirmation(string type)
        {

            //DbParam[] dbParams = {
            //           new DbParam("@WID",DbType.String,type,ParameterDirection.Input)};
            //int i = (int)base.ExecuteScalar("", dbParams);
            //return i;
            int C = Convert.ToInt32(type);
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DateTime dt = DateTime.Now;
                string str = dt.ToString("yyyy-MM-dd ");
                string d = dt.ToString("HH:mm");
                string sql = "update dbo.Wave  set State='已发货',shipments='" + str + d + "' where WID=" + type;
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);
              

                conn.Open();
             
                try
                {
                    return cmd.ExecuteNonQuery();
                 
                }
                catch
                {
                    throw;
                }
            }
        }

        public int cancellation(string type)
        {

            //DbParam[] dbParams = {
            //           new DbParam("@WID",DbType.String,type,ParameterDirection.Input)};
            //int i = (int)base.ExecuteScalar("", dbParams);
            //return i;
            int C = Convert.ToInt32(type);
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = "update dbo.Wave  set State='已作废' where WID=" + type;
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        public int appointed(string type, string id)
        {
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd ");
            string d = dt.ToString("HH:mm");
            type = type +" "+ d;
            ForecastOrders info = new ForecastOrders();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = "update dbo.Wave  set DeliverTime= '" + type + "' where wid=" + id; ;
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        public int require(string type, string id)
        {
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd ");
            string d = dt.ToString("HH:mm");
            type = type +" "+ d;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = "update dbo.Wave  set PickTime= CONVERT(varchar,'" + type + "',112) where wid=" + id; ;
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        public int mail_Select(ForecastOrders info)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = string.Format(@"select count(*) from Mail where Waveid='{0}' and WaveType='{1}'and WaveReleaseTime='{2} '
                                            and Shipment='{3}' and ThreePL='{4}'and ShipToSity='{5}'  and ShipToCode='{6}' and 
                                            ShipToName='{7}' and Pieces='{8}'and Cartons='{9 }'and Sorterlane='{10}' "
                                           , info.waveId, info.WaveType, info.WaveReleaseTime, info.Shipment, info.pl, info.Shiptocity, info.Shiptocode, info.Shiptoname, info.Pieces, info.Cartons, info.Sorterlane);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        public static object ExecuteScalar(string sql)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteScalar();
                }
                catch
                {
                    throw;
                }
            }
        }
        public static object ExecuteScalar2(string sql)
        {
            ///Data Source=192.168.18.248;Initial Catalog=TWS;uid=sysdb;Password=SYSdb

          //  using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=TWS;Integrated Security=True"))
            using (SqlConnection conn = new SqlConnection("Data Source=192.168.18.248;Initial Catalog=TWS;uid=sysdb;Password=SYSdb"))
            {

                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteScalar();
                }
                catch(Exception e)
                {
                  
                    LogTxtAdd("错误",  "" );
                    throw;
                }
            }
        }
        public IEnumerable<ForecastOrders> waveList(string id)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format("select * from dbo.Mail where Waveid='{0}'  order by WaveReleaseTime", id);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);
                DataTable dt = new DataTable();
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                foreach (DataRow dr in dt.Rows)
                {


                    returnCrms.Add(
                        new ForecastOrders()
                        {
                            // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            waveId = dr["waveId"].ToString(),
                            WaveType = dr["WaveType"].ToString(),
                            WaveReleaseTime = dr["WaveReleaseTime"].ToString(),
                            Shipment = dr["Shipment"].ToString(),
                            ThreePL = dr["ThreePL"].ToString(),
                            ShipToSity = dr["ShipToSity"].ToString(),
                            Shiptocode = dr["ShipToCode"].ToString(),
                            Shiptoname = dr["ShipToName"].ToString(),
                            Pieces = dr["Pieces"].ToString(),
                            Cartons = dr["Cartons"].ToString(),
                            Sorterlane = dr["Sorterlane"].ToString(),
                            MailID = dr["MailID"].ToString()

                        });
                }
            }
            try
            {

                return returnCrms;
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<ForecastOrders> carriers(string id, string sb)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format(@"select SUM(Convert(int,Cartons)) as Cartons, 
                                        CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 )
                                        as WaveReleaseTime ,ShipToSity from Mail ,Wave where Mail.waveId=Wave.waveId and ThreePL='{0}' and 1=1  {1}
                                        group by ShipToSity,CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 ),State,ThreePL  order by WaveReleaseTime", id, sb);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredPsrocedure;
                //cmd.Parameters.AddWithValue("@WID",C);
                DataTable dt = new DataTable();
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                foreach (DataRow dr in dt.Rows)
                {


                    returnCrms.Add(
                        new ForecastOrders()
                        {
                            // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Cartons = dr["Cartons"].ToString(),
                            WaveReleaseTime = dr["WaveReleaseTime"].ToString(),
                            ShipToSity = dr["ShipToSity"].ToString()
                        });
                }
            }
            try
            {

                return returnCrms;
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<ForecastOrders> export(string sb)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format(@"select Shipment,ThreePL,ShipToCode ,ShipToName ,Pieces ,ShipToSity ,Cartons ,isNULL(shipments, CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 ))as shipments from dbo.Wave,Mail  where Mail.waveId=Wave.waveId {0}", sb);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredPsrocedure;
                //cmd.Parameters.AddWithValue("@WID",C);
                DataTable dt = new DataTable();
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                foreach (DataRow dr in dt.Rows)
                {


                    returnCrms.Add(
                        new ForecastOrders()
                        {
                            // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                          Shipment=dr["Shipment"].ToString(),
                        ThreePL=dr["ThreePL"].ToString(),
                    Shiptocode=dr["ShipToCode"].ToString(),
                    Shiptoname=dr["ShipToName"].ToString(),
                    Pieces=dr["Pieces"].ToString(),
                    ShipToSity=dr["ShipToSity"].ToString(),
                   Cartons = dr["Cartons"].ToString(),
                   shipments=dr["shipments"].ToString()
                        });
                }
            }
            try
            {

                return returnCrms;
            }
            catch
            {
                throw;
            }
        }

        public DataTable export2(string sb)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format(@"select Shipment,ThreePL,ShipToCode ,ShipToName ,Pieces ,ShipToSity ,Cartons ,isNULL(Wave.shipments, CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 ))as shipments from dbo.Wave,Mail  where Mail.waveId=Wave.waveId {0}", sb);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredPsrocedure;
                //cmd.Parameters.AddWithValue("@WID",C);
             
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                
                
            }
            try
            {

                return dt;
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<ForecastOrders> carrierslist(string id, string cs, string WaveReleaseTime, string sb)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format(@"select SUM(Convert(int,Cartons)) as Cartons,ShipToName, 
                                       ShipToSity from Mail ,Wave where Mail.waveId=Wave.waveId and 1=1 {3}  and  CONVERT(varchar(10),dateadd(day,0,cast(Mail.WaveReleaseTime as datetime)) , 120 )= CONVERT(varchar(10),dateadd(day,-1,cast('{2}' as datetime)) , 120 ) 
                                and ShipToSity='{1}'  and ThreePL='{0}'  
                           group by ShipToSity,ShipToName,
                        CONVERT(varchar(10),Mail.WaveReleaseTime,120 )
                            order by CONVERT(varchar(10),Mail.WaveReleaseTime,120 ) ", id, cs, WaveReleaseTime, sb);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);
                DataTable dt = new DataTable();
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                foreach (DataRow dr in dt.Rows)
                {


                    returnCrms.Add(
                        new ForecastOrders()
                        {
                            // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Cartons = dr["Cartons"].ToString(),

                            ShipToSity = dr["ShipToSity"].ToString(),
                            Shiptoname = dr["ShipToName"].ToString()
                        });
                }
            }
            try
            {

                return returnCrms;
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<ForecastOrders> xiangxi(string id)
        {
            IList<ForecastOrders> returnCrms = new List<ForecastOrders>();


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                string sql = string.Format(@"select SUM(Convert(int,Cartons)) as Cartons, ShipToName,
                                        CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 )
                                        as WaveReleaseTime ,ShipToSity from Mail ,Wave where Mail.waveId=Wave.waveId and (State='正在发货中'OR State='已锁定')  and ThreePL='{0}' 
                                        group by ShipToSity,ShipToName,CONVERT(varchar(10),dateadd(day,1,cast(Mail.WaveReleaseTime as datetime)) , 120 ),State,ThreePL  order by WaveReleaseTime", id);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);
                DataTable dt = new DataTable();
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
                //Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane, MailID
                foreach (DataRow dr in dt.Rows)
                {


                    returnCrms.Add(
                        new ForecastOrders()
                        {
                            // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Cartons = dr["Cartons"].ToString(),
                            WaveReleaseTime = dr["WaveReleaseTime"].ToString(),
                            ShipToSity = dr["ShipToSity"].ToString(),
                            Shiptoname = dr["ShipToName"].ToString()
                        });
                }
            }
            try
            {

                return returnCrms;
            }
            catch
            {
                throw;
            }
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
        public static void bianli(string savePath)
        {
         // LogTxtAdd("测试3", "进入方法3");
           // string savePath = "E:\\Storage\\Storage\\UI\\Email\\";
            DirectoryInfo dir = new DirectoryInfo(savePath);
            FileInfo[] files = dir.GetFiles();
            // string Username = Session["Username"].ToString();
            int c = 0;
            int i = 0;
           // LogTxtAdd("测试4", "进入方法5");
            foreach (FileInfo fi in files)
            {
               // LogTxtAdd("测试5", "进入方法4");
                string name = fi.DirectoryName + "\\" + fi.Name;

                 DataTable dt=new DataTable();
               // LogTxtAdd(" 1111111111", name);
                try
                {
                    NewExcelHelper n = new NewExcelHelper(name);
                   // LogTxtAdd(" 222222222222", name);
                   dt = n.GetAllDataFromAllSheets().Tables[0];
                   // LogTxtAdd("测试6", name);
                    n.Dispose();
                }
                catch (Exception e)
                {
                   
                   // LogTxtAdd("错误1", e.Message);
                    //LogTxtAdd("错误2", e.Source);
                    //LogTxtAdd("错误3", e.InnerException.ToString());
                    throw;
                }
              
                // DataTable dt = n.GetAllDataFromAllSheets().Tables[0];
                DataTable dt1 = new DataTable();

                string Wave = "";
                string WaveType = "";
                string WaveReleaseTime = "";
                string Shipment = "";
                string pl = "";
                string Shiptocity = "";
                string Shiptocode = "";
                string Shiptoname = "";
                string Pieces = "";
                string Cartons = "";
                string Sorterlane = "";
                string DeliverTime = "";
                string shipments = "";

                if (dt != null && dt.Rows.Count > 0)
                {
                   // LogTxtAdd("测试8", "进入方法9");
                    foreach (DataRow dr in dt.Rows)
                    {
                        // Wave = dr["Wave#"].ToString();
                       // LogTxtAdd("测试10", "进入方法11");
                        Wave = dr[0].ToString();
                        Wave = Wave.Replace("'", "");
                        //  WaveType = dr["Wave Type"].ToString();
                        WaveType = dr[1].ToString();
                        //WaveReleaseTime = dr["Wave Release Time"].ToString();
                        WaveType = WaveType.Replace("'", ""); 

                        WaveReleaseTime = dr[2].ToString();
                        WaveReleaseTime = Convert.ToDateTime(WaveReleaseTime).ToString();
                        WaveReleaseTime = WaveReleaseTime.Replace("'", "");
                        //    WaveReleaseTime = Convert.ToDateTime(WaveReleaseTime).ToString();
                        //Shipment = dr["Shipment"].ToString();
                        Shipment = dr[3].ToString();
                        Shipment = Shipment.Replace("'", "");
                        DateTime da = Convert.ToDateTime(WaveReleaseTime);
                        shipments=da.AddDays(1).ToString();
                        shipments=shipments.Replace("'", "");
                        DeliverTime = da.AddDays(1).ToString();
                        
                        //     DateTime da = Convert.ToDateTime(WaveReleaseTime);
                        //        DeliverTime=da.AddDays(1).ToString();
                        // pl = dr["3pl"].ToString();
                        pl = dr[4].ToString();
                        pl=pl.Replace("'", "");
                        //  Shiptocity = dr["Ship-to-city"].ToString();

                        Shiptocity = dr[5].ToString();
                        Shiptocity = Shiptocity.Replace("'", "");
                        //Shiptocode = dr["Ship-to-code"].ToString();
                        Shiptocode = dr[6].ToString();
                        Shiptocode = Shiptocode.Replace("'", "");
                        //Shiptoname = dr["Ship-to-name"].ToString();
                        Shiptoname = dr[7].ToString();
                        Shiptoname = Shiptoname.Replace("'","");
                        // Pieces = dr["Pieces"].ToString();
                        Pieces = dr[8].ToString();
                        Pieces = Pieces.Replace("'", "");
                        // Cartons = dr["Cartons"].ToString();

                        Cartons = dr[9].ToString();
                        Cartons = Cartons.Replace("'", "");
                        //  Sorterlane = dr["Sorter lane"].ToString();

                        Sorterlane = dr[10].ToString();

                        Sorterlane = Sorterlane.Replace("'", "");







                        string sql = string.Format(@"select count(*) from Mail where Waveid='{0}' and WaveType='{1}'and WaveReleaseTime='{2} 'and Shipment='{3}' and ThreePL='{4}'and ShipToSity='{5}'  and ShipToCode='{6}' and ShipToName='{7}' and Pieces='{8}'and Cartons='{9 }'and Sorterlane='{10}' ", Wave, WaveType, WaveReleaseTime, Shipment, pl, Shiptocity, Shiptocode, Shiptoname, Pieces, Cartons, Sorterlane);
                        int s = Convert.ToInt32(ExecuteScalar2(sql));
                        if (s == 0)
                        {
                           // LogTxtAdd("插入Mail", "进入Mail");

                                string sql1 = string.Format(@"insert into dbo.Mail(Waveid, WaveType, WaveReleaseTime, Shipment, ThreePL, ShipToSity, ShipToCode, ShipToName, Pieces, Cartons, Sorterlane)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')"
                                  , Wave, WaveType, WaveReleaseTime, Shipment, pl, Shiptocity, Shiptocode, Shiptoname, Pieces, Cartons, Sorterlane);
                                i = Convert.ToInt32(ExecuteScalar2(sql1));

                            
                            
                           
                        }
                    }

                    string sql3 = string.Format(@"select count(*) from Wave where Waveid='{0}' and WaveReleaseTime='{1}'and DeliverTime='{2} ' ", Wave, WaveReleaseTime, DeliverTime);
                    int y = Convert.ToInt32(ExecuteScalar2(sql3));

                    if (y == 0)
                    {
                     //   LogTxtAdd("插入Wave", "进入Wave");

                            string sql2 = string.Format(@"insert into dbo.Wave ( waveId, WaveReleaseTime, DeliverTime) values ('{0}','{1}','{2}')", Wave, WaveReleaseTime, DeliverTime);
                            c = Convert.ToInt32(ExecuteScalar2(sql2));



                        
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        // Wave = dr["Wave#"].ToString();

                       Wave = dr[0].ToString();
                       Wave = Wave.Replace("'", "");
                        WaveReleaseTime = dr[2].ToString();
                        WaveReleaseTime = Convert.ToDateTime(WaveReleaseTime).ToString();
                        WaveReleaseTime = WaveReleaseTime.Replace("'", "");
                        //    WaveReleaseTime = Convert.ToDateTime(WaveReleaseTime).ToString();
                        //Shipment = dr["Shipment"].ToString();
                        Shipment = dr[3].ToString();
                        DateTime da = Convert.ToDateTime(WaveReleaseTime);
                        shipments = da.AddDays(1).ToString();
                        shipments = shipments.Replace("'", "");
                        //     DateTime da = Convert.ToDateTime(WaveReleaseTime);
                        //        DeliverTime=da.AddDays(1).ToString();
                        // pl = dr["3pl"].ToString();
                        pl = dr[4].ToString();
                        //  Shiptocity = dr["Ship-to-city"].ToString();
                        pl = pl.Replace("'", "");
                        if (pl != "RBOW")
                        {
                         //   LogTxtAdd("修改Wave", "xiugWave");
                            string sql2 = string.Format(@"update  dbo.Wave set shipments='{0}' where waveId='{1}' ", shipments,Wave);
                            c = Convert.ToInt32(ExecuteScalar2(sql2));
                   }


                    }
                   

                }
            }
            
                 string strpath="E:\\Storage\\Storage\\UI\\Email\\";
                 bool b = DeleteDir(strpath);
 
                    
        }
        public static int count(string id)
        {
            string sql = string.Format("select count(*) from Mail_ID where uid='{0}'", id);
            return (int)ExecuteScalar2(sql);

        }
        public static int count2(string id)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=TWS;Integrated Security=True"))
            {
                string sql = string.Format("insert into Mail_ID  values('{0}')", id);

                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@WID",C);



                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }


            }

        }
        #region 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// <summary>
        /// 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        /// <returns>执行结果</returns>
        public static bool DeleteDir(string strPath)
        {
            try
            {
                // 清除空格
                strPath = @strPath.Trim().ToString();
                // 判断文件夹是否存在
                if (System.IO.Directory.Exists(strPath))
                {
                    // 获得文件夹数组
                    string[] strDirs = System.IO.Directory.GetDirectories(strPath);
                    // 获得文件数组
                    string[] strFiles = System.IO.Directory.GetFiles(strPath);
                    // 遍历所有子文件夹
                    foreach (string strFile in strFiles)
                    {
                        // 删除文件夹
                        System.IO.File.Delete(strFile);
                    }
                    // 遍历所有文件
                    foreach (string strdir in strDirs)
                    {
                        // 删除文件
                        System.IO.Directory.Delete(strdir, true);
                    }
                }
                // 成功
                return true;
            }
            catch (Exception Exp) // 异常处理
            {
                // 异常信息
                System.Diagnostics.Debug.Write(Exp.Message.ToString());
                // 失败
                return false;
            }
        }
        #endregion
    }

}

    


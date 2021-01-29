using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.POD.Hilti;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao
{
    public class HiltiAccessor : BaseAccessor
    {
        public IEnumerable<PodAll> GetPodAndPodReplyDocumentByCondition(IEnumerable<string> CustomerOrderNumbers, long? ShipperID, DateTime? StartActualArrivalDate, DateTime? EndActualArrivalDate, int MinPodState)
        { 
            IList<PodAll> returnPodAlls = new List<PodAll>();
            StringBuilder sb = new StringBuilder();
            sb.Append("WITH Temp AS(SELECT a.ID AS PodID, a.SystemNumber,a.CustomerOrderNumber,a.ActualDeliveryDate,a.StartCityName, a.EndCityName,a.DateTime2,b.ID AS ReplyDocumentID, b.Replier,b.ReplyTime,b.Remark,b.AttachmentGroupID,b.Str1,b.Str2,b.DateTime1,b.Str3,c.DateTime1 AS TrackDate,c.Str1 AS Location, c.Str2 AS TrackState,c.Str3 AS CausesOFDelays,c.CreateTime AS TrackCreateTime,c.ID AS TrackID,c.Str4 as CsusesOfDelaysType FROM dbo.POD a LEFT JOIN dbo.PodReplyDocument b ON a.ID = b.PodID LEFT JOIN dbo.PodTrack c ON a.ID=c.PodID")
                .Append(" Where a.ProjectID=1 and a.CustomerID=2 ").Append(" and a.PodStateID between 2 and 7 ");
            //sb.Append("SELECT a.ID AS PodID, a.SystemNumber,a.CustomerOrderNumber,a.ActualDeliveryDate,a.StartCityName, a.EndCityName,a.DateTime2,b.ID AS ReplyDocumentID, b.Replier,b.ReplyTime,b.Remark,b.AttachmentGroupID,b.Str1,b.Str2,b.DateTime1,b.Str3 FROM dbo.POD a LEFT JOIN dbo.PodReplyDocument b ON a.ID = b.PodID")
            //    .Append(" Where a.ProjectID=1 and a.CustomerID=2 ").Append(" and a.PodStateID between 2 and 5 ");
            if (CustomerOrderNumbers != null && CustomerOrderNumbers.Any())
            {
                sb.Append(" and (a.CustomerOrderNumber in (");
                CustomerOrderNumbers.Each((i, p) =>
                {
                    sb.Append("'").Append(p).Append("'").Append(",");
                });

                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");


                sb.Append(" or a.Str1 in (");
                CustomerOrderNumbers.Each((i, p) =>
                {
                    sb.Append("'").Append(p).Append("'").Append(",");
                });

                sb.Remove(sb.Length - 1, 1);
                sb.Append(") )");
            }

            if (ShipperID.HasValue)
            {
                sb.Append(" and a.ShipperID=").Append(ShipperID.Value).Append(" ");
            }

            sb.Append(" and a.PodStateID > ").Append(MinPodState);

            if (StartActualArrivalDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate >= '").Append(StartActualArrivalDate.Value.ToString("yyyy-MM-dd")).Append(" 00:00' ");
            }

            if (EndActualArrivalDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate <= '").Append(EndActualArrivalDate.Value.ToString("yyyy-MM-dd")).Append(" 23:59' ");
            }

            sb.Append(" ) SELECT * FROM Temp,(SELECT PodID,MAX(TrackCreateTime) AS timer1 FROM Temp GROUP BY PodID) AS tr  WHERE Temp.PodID = tr.PodID AND (Temp.TrackCreateTime = tr.timer1 OR  Temp.TrackCreateTime IS NULL) ORDER BY Temp.ActualDeliveryDate,Temp.CustomerOrderNumber");
            DataTable dt;

            try
            {
                dt = base.ExecuteDataTableBySqlString(sb.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Rows.Each((i, r) =>
                    {
                        PodAll podAll = new PodAll();
                        podAll.Pod = new Pod();
                        podAll.Pod.ID = r[0].ObjectToInt64();
                        podAll.Pod.SystemNumber = r[1].ToString();
                        podAll.Pod.CustomerOrderNumber = r[2].ToString();
                        podAll.Pod.ActualDeliveryDate = r[3].ObjectToNullableDateTime();
                        podAll.Pod.StartCityName = r[4].ToString();
                        podAll.Pod.EndCityName = r[5].ToString();
                        podAll.Pod.DateTime2 = r[6].ObjectToNullableDateTime();
                        if (r[7].ToString() != "")
                        {
                            podAll.PodReplyDocument = new PodReplyDocument();
                            podAll.PodReplyDocument.ID = r[7].ObjectToInt64();
                            podAll.PodReplyDocument.Replier = r[8].ToString();
                            podAll.PodReplyDocument.ReplyTime = r[9].ObjectToNullableDateTime();
                            podAll.PodReplyDocument.Remark = r[10].ToString();
                            podAll.PodReplyDocument.AttachmentGroupID = r[11].ToString();
                            podAll.PodReplyDocument.Str1 = r[12].ToString();
                            podAll.PodReplyDocument.Str2 = r[13].ToString();
                            podAll.PodReplyDocument.DateTime1 = r[14].ObjectToNullableDateTime();
                            podAll.PodReplyDocument.Str3 = r[15].ToString();
                        }

                        if (r[16].ToString() != "")
                        {
                            PodTrack podTrack = new PodTrack();
                            podTrack.DateTime1 = r[16].ObjectToNullableDateTime();
                            podTrack.Str1 = r[17].ToString();
                            podTrack.Str2 = r[18].ToString();
                            podTrack.Str3 = r[19].ToString();
                            podTrack.Str4 = r["CsusesOfDelaysType"].ToString();
                            podAll.PodTracks = new PodTrack[] { podTrack };
                        }

                        returnPodAlls.Add(podAll);
                    });
                }

                return returnPodAlls;
            }
            catch
            {
                throw;
            }
        }

        public ServicePeriod GetServicePeriodByCondition(long projectID, long customerID, long startCityID, long endCityID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, customerID, ParameterDirection.Input),
                new DbParam("@StartCityID", DbType.Int64, startCityID, ParameterDirection.Input),
                new DbParam("@EndCityID", DbType.Int64, endCityID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetServicePeriodByCondition", dbParams).ConvertToEntity<ServicePeriod>();
        }

        public IEnumerable<ServicePeriod> GetServicePeriod()
        {
            return base.ExecuteDataTableBySqlString("SELECT * FROM dbo.ServicePeriod").ConvertToEntityCollection<ServicePeriod>();
        }
        /// <summary>
        /// 获取xld数据
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public DataTable GetPodTrackReport(string SqlWhere, int PageIndex, int PageSize,string ReportName, out int RowCount,out double  SumGrossWeight,out double  SumNetWeight) 
        {
            
            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output),
                            new DbParam("@SumGrossWeight",DbType.Double,0,ParameterDirection.Output),
                            new DbParam("@SumNetWeight",DbType.Double,0,ParameterDirection.Output),
                            new DbParam("@RepotrName",DbType.String,ReportName,ParameterDirection.Input),
                            
                          };

            DataTable table = base.ExecuteDataTable("Proc_XLDTrackReport", dbParams);
            RowCount = (int)dbParams[3].Value;
            SumGrossWeight = (double)dbParams[4].Value;
            SumNetWeight = (double)dbParams[5].Value;

            return table;
           
        }


        /// <summary>
        /// 获取xld数据
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public DataTable GetPodTrackReportExport(string SqlWhere,string ReportName)
        {
            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@RepotrName",DbType.String,ReportName,ParameterDirection.Input)
                          };
           return  base.ExecuteDataTable("Proc_XLDTrackReport_Export", dbParams);
        }



        public UpdatePodInfoUpdateRequest PodInfoUpdateByTable(UpdatePodInfoUpdateRequest Info)
        {

            
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                //var PodInfo = new List<PodInfoUpdateToDb>();
                //PodInfo.Add(new PodInfoUpdateToDb(Info.PodInfoUpdate));
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_Hilti_UpdateNotDeliverGoods", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NotDeliverGoodsSOURCE", Info.NotDeliverGoods);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ERRORSOURCEVALUE","");
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 2000;
                cmd.Parameters.AddWithValue("@ISORSUCCESS",0);
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.Bit;

                cmd.Parameters.AddWithValue("@ActualDeliveryDate",Info.ActualDeliveryDate);
                cmd.Parameters[3].Direction = ParameterDirection.Input;
                cmd.Parameters[3].SqlDbType = SqlDbType.DateTime;
                cmd.CommandTimeout = 180;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                Info.ERRORSOURCEVALUE = cmd.Parameters[1].Value.ToString();
                Info.ISORSUCCESS = cmd.Parameters[2].Value.ObjectToBoolean();
                Info.DeliverGoods = dtable;

                return Info;
            }

        }


        public UpdateOrderNoInfoUpdateRequest OrderNoInfoUpdateByTable(UpdateOrderNoInfoUpdateRequest Info)
        {


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
               
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_Hilti_Update103OrderNo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNoPODSOURCE", Info.OrderNoinfo);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ERRORSOURCEVALUE", "");
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 2000;
                cmd.Parameters.AddWithValue("@ISORSUCCESS", 0);
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.Bit;
                conn.Open();

                cmd.ExecuteNonQuery();

                Info.ERRORSOURCEVALUE = cmd.Parameters[1].Value.ToString();
                Info.ISORSUCCESS = cmd.Parameters[2].Value.ObjectToBoolean();
               

                return Info;
            }

        }


        public DataTable GetPrintHandoverDetailedListDetail(string SqlWhere)
        {
            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input)
                            
                          };
            return base.ExecuteDataTable("Proc_GetPrintHandoverDetailedListDetailBySqlWhere", dbParams);
        }





        public bool PODColumnsInfoUpdateByTable(DataTable table)
        {

            UpdatePodColumnInfoRequest result = new UpdatePodColumnInfoRequest();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_UpdateColumnInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODColumnsInfoSOURCE", table);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;

                cmd.Parameters.AddWithValue("@RESULT", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.Bit;
                conn.Open();

                cmd.ExecuteNonQuery();


                result.result = cmd.Parameters[1].Value.ObjectToBoolean();


                return result.result;
            }
        }



        /// <summary>
        /// 更新或添加时效
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public AddOrUpdateServicePeriodRequest AddOrUpdateServicePeriod(AddOrUpdateServicePeriodRequest Info)
        {


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
         
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_Hilti_AddOrUpdateServicePeriod", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EndCity", Info.EndCity);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                cmd.Parameters.AddWithValue("@EndCityID", Info.EndCityID);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@Period", Info.Period);
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@ErrorValue", "");
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[3].Size = 2000;
               
                conn.Open();

                cmd.ExecuteNonQuery();

                Info.ErrorValue = cmd.Parameters[3].Value.ToString();
                

                return Info;
            }

        }




        public DataTable CheckShipperDistributionDate(string ShipperName,DateTime? BegindateTime,DateTime? EndDateTime)
        {
            DbParam[] dbParams = {
                            new DbParam("@ShipperName",DbType.String,ShipperName,ParameterDirection.Input),
                             new DbParam("@BeginDateTime",DbType.String,BegindateTime,ParameterDirection.Input),
                              new DbParam("@EndDateTime",DbType.String,EndDateTime,ParameterDirection.Input)
                            
                          };
            return base.ExecuteDataTable("Proc_Hilti_CheckHandoverDate", dbParams);
        }




        /// <summary>
        ///
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public int AddOrUpdateSellInfo(string SellName, string SellPhone)
        {

            
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Prco_Hilti_AddOrUpdateSellInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SellName", SellName);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                cmd.Parameters.AddWithValue("@SellPhone", SellPhone);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;

               

                conn.Open();

               return  cmd.ExecuteNonQuery();

            }

        }



        public int GetServicePeriodInfo(string EndCity)
        {


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {


                SqlCommand cmd = new SqlCommand("Proc_Hilti_GetServicePeriodINfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EndCity", EndCity);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

               


                conn.Open();
                object value = cmd.ExecuteScalar();
                int Period = 0;
                if (value != null) 
                {
                    Period = Convert.ToInt32(value);
                }

                return Period;

            }

        }




        public string  GetHiltiDriverPhone(string Name)
        {


            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                
                SqlCommand cmd = new SqlCommand("Proc_Hilti_GetHiltiDriverPhone", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;




                conn.Open();
                object value = cmd.ExecuteScalar();
                string  Pohone = "";
                if (value != null)
                {
                    Pohone = value.ToString();
                }

                return Pohone;

            }

        }

        /// <summary>
        /// 批量插入值到HiltiCustomerCreditMapping表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool BulkCopy(DataTable dt)
        {
            return BulkCopy(BaseAccessor._dataBase.ConnectionString, "HiltiCustomerCreditMapping", dt);
        }

        /// <summary>
        /// 把HiltiCustomerCreditMapping表清空
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllPod()
        {
            try
            {
                this.ExecuteNoQueryBySqlString("DELETE FROM HiltiCustomerCreditMapping");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 更新POD和HiltiCustomerCreditMapping表对应值
        /// </summary>
        /// <returns></returns>
        public bool UpdateCode()
        {
            try
            {
                this.ExecuteNoQuery(@"Proc_Hilti_PodUpdate", null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

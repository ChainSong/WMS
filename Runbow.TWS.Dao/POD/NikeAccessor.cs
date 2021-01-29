using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity.POD.Nike;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao.POD
{
    public class NikeAccessor : BaseAccessor
    {
        /// <summary>
        /// 获取Nike导出报表
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public DataTable GetNikeReportExprot(string SqlWhere, string ReportName)
        {

            DbParam[] dbParams = {
                            new DbParam("@Where",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@ReportName",DbType.String,ReportName,ParameterDirection.Input)
                          };

            DataTable NikeExport = base.ExecuteDataTable("Proc_Nike_ExportReport", dbParams);

            return NikeExport;
        }



        public DataTable GetNikeReportQuery(string SqlWhere, string ReportName, int? PageIndex, int PageSize, out int RowCount)
        {

            DbParam[] dbParams = {
                            new DbParam("@Where",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@ReportName",DbType.String,ReportName,ParameterDirection.Input),
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            DataTable NikeExport = base.ExecuteDataTable("Proc_Nike_QueryReport", dbParams);
            RowCount = (int)dbParams[4].Value;
            return NikeExport;
        }

        public IEnumerable<PodAll> GetNikeExportPodAllByCondition(string Condition)
        {
            DbParam[] dbParams = {
                            new DbParam("@Where",DbType.String,Condition,ParameterDirection.Input)
                          };

            DataSet NikeExport = base.ExecuteDataSet("Proc_Nike_GetPodAllByCondition", dbParams);

            IList<PodAll> podAllList = new List<PodAll>();
            if (NikeExport != null && NikeExport.Tables[0] != null && NikeExport.Tables[0].Rows.Count > 0)
            {
                IEnumerable<Pod> podCollection = NikeExport.Tables[0].ConvertToEntityCollection<Pod>();

                IEnumerable<PodTrack> podTrackCollection = Enumerable.Empty<PodTrack>();

                if (NikeExport.Tables[1] != null && NikeExport.Tables[1].Rows.Count > 0)
                {
                    podTrackCollection = NikeExport.Tables[1].ConvertToEntityCollection<PodTrack>();
                }

                podCollection.Each((i, p) =>
                {
                    PodAll podAll = new PodAll();
                    podAll.Pod = p;
                    podAll.PodTracks = podTrackCollection.Where(t => t.PodID == p.ID);
                    podAllList.Add(podAll);
                });
            }

            return podAllList;
        }

        public UpdateNikePodAndGetTheDifferenceResponse UpdateNikePodAndGetTheDifference(IEnumerable<UpdateNikePod> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateNikePodAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", pods.Select(i => new UdtUpdateNikePodToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateNikePodAndGetTheDifferenceResponse response = new UpdateNikePodAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPods = ds.Tables[0].ConvertToEntityCollection<UpdateNikePod>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPods = ds.Tables[1].ConvertToEntityCollection<UpdateNikePod>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        response.CityNotMatchPods = ds.Tables[2].ConvertToEntityCollection<UpdateNikePod>();
                    }

                    if (ds.Tables[3] != null)
                    {
                        response.StartCityNotMatchPods = ds.Tables[3].ConvertToEntityCollection<UpdateNikePod>();
                    }
                }

                return response;
            }
        }
        /// <summary>
        /// 箱数件数同步
        /// </summary>
        /// <param name="pods"></param>
        /// <returns></returns>
        public UpdateNikePodBGAndGetTheDifferenceResponse UpdateNikePodBGAndGetTheDifference(IEnumerable<UpdateNikePodBG> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateNikePodBGAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", pods.Select(i => new UdtUpdateNikePodBGToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateNikePodBGAndGetTheDifferenceResponse response = new UpdateNikePodBGAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPods = ds.Tables[0].ConvertToEntityCollection<UpdateNikePodBG>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPods = ds.Tables[1].ConvertToEntityCollection<UpdateNikePodBG>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        response.StateNotMatchPods = ds.Tables[2].ConvertToEntityCollection<UpdateNikePodBG>();
                    }

                    if (ds.Tables[3] != null)
                    {
                        response.RepeatPods = ds.Tables[3].ConvertToEntityCollection<UpdateNikePodBG>();
                    }
                }

                return response;
            }
        }
        public IEnumerable<NikeforBSPOD> GetNikePOD(NikePodForBSCondition Condition, int PageIndex, int PageSize, out int RowCount)
        {
            string SqlWhere = "";
            if (Condition != null)
            {
                SqlWhere = SqlWhereCondition(Condition);
            }
            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),//,
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            DataTable dt = base.ExecuteDataTable("Proc_GetNikePod", dbParams);
            var NikePOD = dt.ConvertToEntityCollection<NikeforBSPOD>();
            RowCount = (int)dbParams[3].Value;
        
            return NikePOD;
        }
        private string SqlWhereCondition(NikePodForBSCondition condition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> SystemNumber = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    SystemNumber = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    SystemNumber = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (SystemNumber != null && SystemNumber.Any())
                {
                    SystemNumber = SystemNumber.Where(c => !string.IsNullOrEmpty(c));
                }

                if (SystemNumber != null && SystemNumber.Any())
                {
                    sb.Append(" and p.SystemNumber in ( ");
                    foreach (string s in SystemNumber)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and  p.SystemNumber  like '%" + condition.SystemNumber.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(condition.Str1))
            {
                IEnumerable<string> Str1 = Enumerable.Empty<string>();
                if (condition.Str1.IndexOf("\n") > 0)
                {
                    Str1 = condition.Str1.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Str1.IndexOf(',') > 0)
                {
                    Str1 = condition.Str1.Split(',').Select(s => { return s.Trim(); });
                }

                if (Str1 != null && Str1.Any())
                {
                    Str1 = Str1.Where(c => !string.IsNullOrEmpty(c));
                }

                if (Str1 != null && Str1.Any())
                {
                    sb.Append(" and  p.Str1 in ( ");
                    foreach (string s in Str1)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and  p.Str1  like '%" + condition.Str1.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.CustomerOrderNumber))
            {
                IEnumerable<string> CustomerOrderNumber = Enumerable.Empty<string>();
                if (condition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    CustomerOrderNumber = condition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    CustomerOrderNumber = condition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (CustomerOrderNumber != null && CustomerOrderNumber.Any())
                {
                    CustomerOrderNumber = CustomerOrderNumber.Where(c => !string.IsNullOrEmpty(c));
                }

                if (CustomerOrderNumber != null && CustomerOrderNumber.Any())
                {
                    sb.Append(" and  p.CustomerOrderNumber in ( ");
                    foreach (string s in CustomerOrderNumber)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and  p.CustomerOrderNumber  like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(condition.PodStateName))
            {
                sb.Append(" and  p.PodStateName = '" + condition.PodStateName.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.ShipperName))
            {


                sb.Append(" and  p.ShipperName = '" + condition.ShipperName.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.TtlOrTplName))
            {
                sb.Append(" and  p.TtlOrTplName = '" + condition.TtlOrTplName.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.StartCityName))
            {
                sb.Append(" and  p.StartCityName = '" + condition.StartCityName.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.EndCityName))
            {
                sb.Append(" and  p.EndCityName = '" + condition.EndCityName.Trim() + "' ");
            }
            if (condition.StartDeliveryTime != null)
            {
                sb.Append(" and  p.ActualDeliveryDate >= '" + condition.StartDeliveryTime + "' ");
            }
            if (condition.EndDeliveryTime != null)
            {
                sb.Append(" and  p.ActualDeliveryDate < '" + condition.EndDeliveryTime + "  23:59:59'");
            }
            if (condition.IsConversion !=null)
            {
                if (condition.IsConversion == 1)
                {
                    sb.Append(" and  p.Str50 =1 ");
                }
                else
                {
                    sb.Append(" and  p.Str50 is NULL ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将Nike单号转为宝胜单号插入
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public string AddNikePodForBS(IEnumerable<NikeforBSPOD> Condition, string UserName, string ShipperName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_NikeforBSPODToDb", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Creator", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Shipper", ShipperName);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@message", ShipperName);
                cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[3].Size = 20;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = cmd.Parameters[3].Value.ToString();
                conn.Close();
              //  SqlDataAdapter Adp = new SqlDataAdapter(cmd);
              //  Adp.Fill(ds);
              //  UpdateNikePodAndGetTheDifferenceResponse response = new UpdateNikePodAndGetTheDifferenceResponse();
           
                return message;
            }
         
        }

        public string CancelNikePodForBS(IEnumerable<NikeforBSPOD> Condition)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("[Proc_CancelNikeforBSPODToDb]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[1].Size = 10;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = cmd.Parameters[1].Value.ToString();
                conn.Close();
                //  SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                //  Adp.Fill(ds);
                //  UpdateNikePodAndGetTheDifferenceResponse response = new UpdateNikePodAndGetTheDifferenceResponse();

                return message;
            }
        }
    }
}

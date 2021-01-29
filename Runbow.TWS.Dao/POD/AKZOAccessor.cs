using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.Entity.POD;
namespace Runbow.TWS.Dao.POD
{
    public class AKZOAccessor : BaseAccessor
    {
        /// <summary>
        /// 获取AKZO异常数据
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <param name="ReportName"></param>
        /// <returns></returns>
        public DataTable GetAbnormalPODSearch(string SqlWhere, int? PageIndex, int PageSize, out int RowCount)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            DataTable table = base.ExecuteDataTable("Proc_AKZO_GetAbnormalPODSearch", dbParams);
            RowCount = (int)dbParams[3].Value;
            return table;

        }







        public IEnumerable<Pod> GetAbnormalPODSearchToExcel(string SqlWhere)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input)
                          
                          };

            return base.ExecuteDataTable("Proc_AKZO_GetAbnormalPODSearch_ExprotRep", dbParams).ConvertToEntityCollection<Pod>();
        }



       


        public IEnumerable<long> GetAbnormalPODTrackSearchToExcel(string SqlWhere)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input)
                          
                          };

            DataTable dt = base.ExecuteDataTable("Proc_AKZO_GetAbnormalPODTrackSearch_ExprotRep", dbParams);

            IList<long> idList = new List<long>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                idList.Add(dt.Rows[i][0].ObjectToInt64());
            }

            return idList;

           

        }

        public UpdateAkzoPodAndGetTheDifferenceResponse UpdateAkzoPodAndGetTheDifference(IEnumerable<UpdateAKZOPod> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateAkzoPodAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", pods.Select(i => new UdtUpdateAkzoPodToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateAkzoPodAndGetTheDifferenceResponse response = new UpdateAkzoPodAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPods = ds.Tables[0].ConvertToEntityCollection<UpdateAKZOPod>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPods = ds.Tables[1].ConvertToEntityCollection<UpdateAKZOPod>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        response.CityNotMatchPods = ds.Tables[2].ConvertToEntityCollection<UpdateAKZOPod>();
                    }
                }

                return response;
            }

        }
        //苏州阿迪运单信息同步
        public UpdateAdidasPodAndGetTheDifferenceResponse UpdateAdidasPodAndGetTheDifference(IEnumerable<UpdateAdidasPod> podAD)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateAdidasPodAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", podAD.Select(i => new UdtUpdateAdidasPodToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateAdidasPodAndGetTheDifferenceResponse response = new UpdateAdidasPodAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPodAD = ds.Tables[0].ConvertToEntityCollection<UpdateAdidasPod>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPodAD = ds.Tables[1].ConvertToEntityCollection<UpdateAdidasPod>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        response.CityNotMatchPodAD = ds.Tables[2].ConvertToEntityCollection<UpdateAdidasPod>();
                    }
                }

                return response;
            }
        }
        //广州阿迪运单信息同步
        public UpdateAdidasPurchasePodAndGetTheDifferenceResponse UpdateAdidasPurchasePodAndGetTheDifference(IEnumerable<UpdateAdidasPurchasePod> podAD)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateAdidasPurchasePodAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", podAD.Select(i => new UpdateAdidasPurchasePodDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateAdidasPurchasePodAndGetTheDifferenceResponse response = new UpdateAdidasPurchasePodAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPodAD = ds.Tables[0].ConvertToEntityCollection<UpdateAdidasPurchasePod>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPodAD = ds.Tables[1].ConvertToEntityCollection<UpdateAdidasPurchasePod>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        response.CityNotMatchPodAD = ds.Tables[2].ConvertToEntityCollection<UpdateAdidasPurchasePod>();
                    }
                }

                return response;
            }
        }
        //车型同步
        public UpdateAkzoModelsPodAndGetTheDifferenceResponse UpdateAkzoModelsPodAndGetTheDifference(IEnumerable<UpdateAKZOModelsPod> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_UpdateAkzoModelsPodAndGetTheDifference", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PODS", pods.Select(i => new UdtUpdateAkzoModelsPodToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);
                UpdateAkzoModelsPodAndGetTheDifferenceResponse response = new UpdateAkzoModelsPodAndGetTheDifferenceResponse();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        response.UpdatedPods = ds.Tables[0].ConvertToEntityCollection<UpdateAKZOModelsPod>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        response.NotUpdatedPods = ds.Tables[1].ConvertToEntityCollection<UpdateAKZOModelsPod>();
                    }
                }

                return response;
            }
        }
    }
}

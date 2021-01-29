using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD;
using Runbow.TWS.Entity.POD.Nike;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Dao
{
    public class PodAccessor : BaseAccessor
    {
        public Pod AddPod(Pod pod)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podList = new List<PodToDb>();
                podList.Add(new PodToDb(pod));
                SqlCommand cmd = new SqlCommand("Proc_AddPod", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodData", podList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                pod.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return pod;
            }
        }

        public IEnumerable<string> CheckIfPodExistsByPodCustomerOrderNumber(long projectID, long customerID, IEnumerable<string> customerOrderNumberCollection)
        {
            StringBuilder sqlSb = new StringBuilder();
            sqlSb.Append(" where  ProjectID=").Append(projectID).Append(" and CustomerID=").Append(customerID).Append(" and CustomerOrderNumber in (");
            customerOrderNumberCollection.Each((i, s) =>
            {
                sqlSb.Append("'").Append(s).Append("',");
            });

            sqlSb.Remove(sqlSb.Length - 1, 1);
            sqlSb.Append(") ");

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlSb.ToString(), ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_QueryPodsWithNoPaging", dbParams).Rows.Select(dr => dr["CustomerOrderNumber"].ToString());
        }

        public IEnumerable<PodKey> AddPods(IEnumerable<Pod> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPods", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodData", pods.Select(p => new PodToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.CommandTimeout = 180;
                conn.Open();
                IList<PodKey> returnPods = new List<PodKey>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPods.Add(
                        new PodKey()
                        {
                            SystemNumber = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            CustomerOrderNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PODStateName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            ShipperName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            ShipperTypeName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            StartCityName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            EndCityName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            PODTypeName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            TtlOrTplName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            ActualDeliveryDate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            BoxNumber = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            GoodsNumber = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            Weight = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            Volume = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        });
                }

                return returnPods;
            }
        }

        public PodDetail AddPodDetail(PodDetail podDetail)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podDetailList = new List<PodDetailToDb>();
                podDetailList.Add(new PodDetailToDb(podDetail));
                SqlCommand cmd = new SqlCommand("Proc_AddPodDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodDetailData", podDetailList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                cmd.ExecuteNonQuery();

                podDetail.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podDetail;
            }
        }

        public IEnumerable<PodDetail> AddPodDetails(IEnumerable<PodDetail> podDetails, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodDetailData", podDetails.Select(p => new PodDetailToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodDetail> returnPodDetails = new List<PodDetail>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodDetails.Add(new PodDetail()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        GoodCode = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        GoodName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        UnitCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        UnitName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        ExpectedAmount = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        ActualAmount = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Remark = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Creator = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        CreateTime = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                        Str1 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str2 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str3 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        Str4 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        Str5 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        DateTime1 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        Str6 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str7 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                        Str8 = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                        Str9 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                        Str10 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                        Str11 = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                        Str12 = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                        Str13 = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                        Str14 = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                        Str15 = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                        Str16 = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                        Str17 = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                        Str18 = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                        Str19 = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                        Str20 = reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                        DateTime2 = reader.IsDBNull(34) ? null : (DateTime?)reader.GetDateTime(34),
                        DateTime3 = reader.IsDBNull(35) ? null : (DateTime?)reader.GetDateTime(35),
                        DateTime4 = reader.IsDBNull(36) ? null : (DateTime?)reader.GetDateTime(36),
                        DateTime5 = reader.IsDBNull(37) ? null : (DateTime?)reader.GetDateTime(37)
                    });
                }

                return returnPodDetails;
            }
        }

        public int DeletePodAndRelatedInfo(long podID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
                new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output),
            };

            base.ExecuteNoQuery("Proc_DeletePodAndRelatedInfo", dbParams);

            return dbParams[1].Value.ObjectToInt32();
        }

        public PodDetail DeletePodDetailByID(long podDetailID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podDetailID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_DeletePodDetailByID", dbParams).ConvertToEntity<PodDetail>();
        }

        public PodStatusLog AddPodStatusLog(PodStatusLog podStatusLog)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podStatusLogList = new List<PodStatusLogToDb>();
                podStatusLogList.Add(new PodStatusLogToDb(podStatusLog));
                SqlCommand cmd = new SqlCommand("Proc_AddPodStatusLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodStatusLogData", podStatusLogList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podStatusLog.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podStatusLog;
            }
        }

        public PodStatusTrack AddPodStatusTrack(PodStatusTrack podStatusTrack)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podStatusTrackList = new List<PodStatusTrackToDb>();
                podStatusTrackList.Add(new PodStatusTrackToDb(podStatusTrack));
                SqlCommand cmd = new SqlCommand("Proc_AddPodStatusTrack", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodStatusTrackData", podStatusTrackList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podStatusTrack.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podStatusTrack;
            }
        }

        public IEnumerable<PodStatusLog> AddPodStatusLogs(IEnumerable<PodStatusLog> podStatusLogs, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodStatusLogs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodStatusLogData", podStatusLogs.Select(p => new PodStatusLogToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodStatusLog> returnPodStatusLogs = new List<PodStatusLog>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodStatusLogs.Add(new PodStatusLog()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return returnPodStatusLogs;
            }
        }

        public IEnumerable<PodStatusTrack> AddPodStatusTracks(IEnumerable<PodStatusTrack> podStatusTracks, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodStatusTracks", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodStatusTrackData", podStatusTracks.Select(p => new PodStatusTrackToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodStatusTrack> returnPodStatusTracks = new List<PodStatusTrack>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodStatusTracks.Add(new PodStatusTrack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return returnPodStatusTracks;
            }
        }

        public PodStatusLog DeletePodStatusLogByID(long podStatusLogID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podStatusLogID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_DeletePodStatusLogByID", dbParams).ConvertToEntity<PodStatusLog>();
        }

        public PodStatusTrack DeletePodStatusTrackByID(long podStatusTrackID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podStatusTrackID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_DeletePodStatusTrackByID", dbParams).ConvertToEntity<PodStatusTrack>();
        }

        public PodAll GetPodAndReleatedInfo(long ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
            };

            DataSet ds = base.ExecuteDataSet("Proc_GetPodAndReleatedInfo", dbParams);

            PodAll podAll = new PodAll();

            podAll.Pod = ds.Tables[0].ConvertToEntity<Pod>();

            if (podAll.Pod == null)
            {
                podAll.Pod = new Pod();
            }

            podAll.PodReplyDocument = ds.Tables[1].ConvertToEntity<PodReplyDocument>();

            if (podAll.PodReplyDocument == null)
            {
                podAll.PodReplyDocument = new PodReplyDocument();
            }

            podAll.PodFeadBack = ds.Tables[2].ConvertToEntity<PodFeadBack>();

            if (podAll.PodFeadBack == null)
            {
                podAll.PodFeadBack = new PodFeadBack();
            }

            podAll.PodDetails = ds.Tables[3].ConvertToEntityCollection<PodDetail>();

            podAll.PodTracks = ds.Tables[4].ConvertToEntityCollection<PodTrack>();

            podAll.PodStatusLogs = ds.Tables[5].ConvertToEntityCollection<PodStatusLog>();

            podAll.PodExceptions = ds.Tables[6].ConvertToEntityCollection<PodException>();

            podAll.PodFee = ds.Tables[7].ConvertToEntity<PodFee>();

            if (podAll.PodFee == null)
            {
                podAll.PodFee = new PodFee();
            }

            podAll.PodStatusTracks = ds.Tables[8].ConvertToEntityCollection<PodStatusTrack>();

            //二维码信息
            podAll.WXPODBarCode = ds.Tables[9].ConvertToEntity<WXPODBarCode>();
            if (podAll.WXPODBarCode == null)
            {
                podAll.WXPODBarCode = new WXPODBarCode();
            }

            //主订单对应的拆单信息
            podAll.podBX = ds.Tables[10].ConvertToEntity<Pod>();
            if (podAll.podBX == null)
            {
                podAll.podBX = new Pod();
            }

            return podAll;
        }

        public IEnumerable<PodDetail> GetPodDetailsByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodDetailsByPodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<PodDetail> podDetails = new List<PodDetail>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podDetails.Add(new PodDetail()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        GoodCode = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        GoodName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        UnitCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        UnitName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        ExpectedAmount = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        ActualAmount = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Remark = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Creator = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        CreateTime = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                        Str1 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str2 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str3 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        Str4 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        Str5 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        DateTime1 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return podDetails;
            }
        }

        public IEnumerable<PodException> GetPodExceptionsByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodExceptionsByPodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<PodException> podExceptions = new List<PodException>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podExceptions.Add(new PodException()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        DateTime1 = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                        DateTime2 = reader.IsDBNull(12) ? null : (DateTime?)reader.GetDateTime(12),
                        DateTime3 = reader.IsDBNull(13) ? null : (DateTime?)reader.GetDateTime(13),
                        Str6 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str7 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        Str8 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        Str9 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        Str10 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                        Str11 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str12 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20)
                    });
                }

                return podExceptions;
            }
        }

        public PodFeadBack GetPodFeadBackByPodID(long podID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_GetPodFeadBackByPodID", dbParams).ConvertToEntity<PodFeadBack>();
        }

        public PodReplyDocument GetPodReplyDocumentByPodID(long podID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_GetPodReplyDocumentByPodID", dbParams).ConvertToEntity<PodReplyDocument>();
        }

        public PodFee GetPodFeeByPodID(long podID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_GetPodFeeByPodID", dbParams).ConvertToEntity<PodFee>();
        }

        public IEnumerable<PodStatusLog> GetPodStatusLogsByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodStatusLogsByPodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<PodStatusLog> podStatusLogs = new List<PodStatusLog>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podStatusLogs.Add(new PodStatusLog()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return podStatusLogs;
            }
        }

        public IEnumerable<PodStatusTrack> GetPodStatusTracksByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodStatusTracksByPodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<PodStatusTrack> podStatusTracks = new List<PodStatusTrack>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podStatusTracks.Add(new PodStatusTrack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return podStatusTracks;
            }
        }

        public IEnumerable<PodFeadBack> GetPodFeadBacksByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_GetPodFeadBacksByPodIDs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<PodFeadBack>();
            }
        }

        public IEnumerable<PodTrack> GetPodTracksByPodIDs(IEnumerable<long> podIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodTracksByPodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", podIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<PodTrack> podTracks = new List<PodTrack>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podTracks.Add(new PodTrack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        Str11 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str12 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                        Str13 = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                        Str14 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                        Str15 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                        DateTime4 = reader.IsDBNull(24) ? null : (DateTime?)reader.GetDateTime(24),
                        DateTime5 = reader.IsDBNull(25) ? null : (DateTime?)reader.GetDateTime(25),
                        DateTime6 = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                        DateTime7 = reader.IsDBNull(27) ? null : (DateTime?)reader.GetDateTime(27),
                        DateTime8 = reader.IsDBNull(28) ? null : (DateTime?)reader.GetDateTime(28),
                        DateTime9 = reader.IsDBNull(29) ? null : (DateTime?)reader.GetDateTime(29),
                        DateTime10 = reader.IsDBNull(30) ? null : (DateTime?)reader.GetDateTime(30)
                    });
                }

                return podTracks;
            }
        }

        public PodException AddPodException(PodException podException)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podExceptionList = new List<PodExceptionToDb>();
                podExceptionList.Add(new PodExceptionToDb(podException));
                SqlCommand cmd = new SqlCommand("Proc_AddPodException", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodExceptionData", podExceptionList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podException.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podException;
            }
        }

        public IEnumerable<PodException> AddPodExceptions(IEnumerable<PodException> podExceptions, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodExceptions", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodExceptionData", podExceptions.Select(p => new PodExceptionToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodException> returnPodExceptions = new List<PodException>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodExceptions.Add(new PodException()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        DateTime1 = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                        DateTime2 = reader.IsDBNull(12) ? null : (DateTime?)reader.GetDateTime(12),
                        DateTime3 = reader.IsDBNull(13) ? null : (DateTime?)reader.GetDateTime(13),
                        Str6 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str7 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        Str8 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        Str9 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        Str10 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                        Str11 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str12 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20)
                    });
                }

                return returnPodExceptions;
            }
        }

        public PodFeadBack AddOrUpdatePodFeadBack(PodFeadBack podFeadBack)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podFeadBackList = new List<PodFeadBackToDb>();
                podFeadBackList.Add(new PodFeadBackToDb(podFeadBack));
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdatePodFeadBack", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodFeadBackData", podFeadBackList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podFeadBack.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podFeadBack;
            }
        }

        public IEnumerable<PodFeadBack> AddPodFeadBacks(IEnumerable<PodFeadBack> podFeadBacks, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodFeadBacks", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodFeadBackData", podFeadBacks.Select(p => new PodFeadBackToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodFeadBack> returnPodFeadBacks = new List<PodFeadBack>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodFeadBacks.Add(new PodFeadBack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18)
                    });
                }

                return returnPodFeadBacks;
            }
        }

        public PodReplyDocument AddOrUpdatePodReplyDocument(PodReplyDocument podReplyDocument)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podReplyDocumentList = new List<PodReplyDocumentToDb>();
                podReplyDocumentList.Add(new PodReplyDocumentToDb(podReplyDocument));
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdatePodReplyDocument", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodReplyDocumentData", podReplyDocumentList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podReplyDocument.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podReplyDocument;
            }
        }

        public IEnumerable<PodFee> AddPodFees(IEnumerable<PodFee> podFees, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable table = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_AddPodFee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodFeeData", podFees.Select(p => new PodFeeToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(table);
                return table.ConvertToEntityCollection<PodFee>();
            }
        }

        public IEnumerable<PodReplyDocument> AddPodReplyDocuments(IEnumerable<PodReplyDocument> podReplyDocuments, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodReplyDocuments", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodReplyDocumentData", podReplyDocuments.Select(p => new PodReplyDocumentToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();

                IList<PodReplyDocument> returnPodReplyDocuments = new List<PodReplyDocument>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodReplyDocuments.Add(new PodReplyDocument()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Replier = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        ReplyTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Remark = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        AttachmentGroupID = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Creator = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        CreateTime = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9),
                        Str1 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str2 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str3 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str4 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str5 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        DateTime1 = reader.IsDBNull(15) ? null : (DateTime?)reader.GetDateTime(15),
                        DateTime2 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime3 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        IsAudit = reader.IsDBNull(18) ? false : (bool?)reader.GetBoolean(18)
                    });
                }

                return returnPodReplyDocuments;
            }
        }

        public PodException DeletePodExceptionByID(long podExceptionID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podExceptionID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_DeletePodExceptionByID", dbParams).ConvertToEntity<PodException>();
        }

        public PodTrack AddPodTrack(PodTrack podTrack)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var podTrackList = new List<PodTrackToDb>();
                podTrackList.Add(new PodTrackToDb(podTrack));
                SqlCommand cmd = new SqlCommand("Proc_AddPodTrack", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodTrackData", podTrackList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                podTrack.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return podTrack;
            }
        }

        public IEnumerable<string> CheckNikePodTrack(IEnumerable<PodTrack> podTracks)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_CheckNikePodTrack", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodTrackData", podTracks.Select(p => new PodTrackToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.CommandTimeout = 180;
                conn.Open();

                IList<string> returnPodNumbers = new List<string>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodNumbers.Add(reader.GetString(0));
                }

                return returnPodNumbers;
            }
        }

        public IEnumerable<PodTrack> AddPodTracks(IEnumerable<PodTrack> podTracks, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodTracks", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodTrackData", podTracks.Select(p => new PodTrackToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.CommandTimeout = 180;
                conn.Open();

                IList<PodTrack> returnPodTracks = new List<PodTrack>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPodTracks.Add(new PodTrack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        Str11 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str12 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                        Str13 = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                        Str14 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                        Str15 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                        DateTime4 = reader.IsDBNull(24) ? null : (DateTime?)reader.GetDateTime(24),
                        DateTime5 = reader.IsDBNull(25) ? null : (DateTime?)reader.GetDateTime(25),
                        DateTime6 = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                        DateTime7 = reader.IsDBNull(27) ? null : (DateTime?)reader.GetDateTime(27),
                        DateTime8 = reader.IsDBNull(28) ? null : (DateTime?)reader.GetDateTime(28),
                        DateTime9 = reader.IsDBNull(29) ? null : (DateTime?)reader.GetDateTime(29),
                        DateTime10 = reader.IsDBNull(30) ? null : (DateTime?)reader.GetDateTime(30)
                    });
                }

                return returnPodTracks;
            }
        }

        public PodTrack DeletePodTrackByID(long podTrackID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podTrackID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_DeletePodTrackByID", dbParams).ConvertToEntity<PodTrack>();
        }

        public IEnumerable<PodReplyDocumentWithAttachment> GetPodReplyDocumentWithAttachmentByCondition(PodReplyDocumentSearchCondition condition, long projectID)
        {
            if (condition.IsInnerUser && (condition.CustomerIDs == null || !condition.CustomerIDs.Any()))
            {
                return Enumerable.Empty<PodReplyDocumentWithAttachment>();
            }

            string sqlWhere = this.GenPodReplyDocumentWithAttachmentWhere(condition, projectID);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetPodReplyDocumentWithAttachmentByCondition", dbParams).ConvertToEntityCollection<PodReplyDocumentWithAttachment>();
        }

        public DataTable GetExportPodReplyDocumentWithAttachmentByCondition(PodReplyDocumentSearchCondition condition, long projectID)
        {
            if (condition.IsInnerUser && (condition.CustomerIDs == null || !condition.CustomerIDs.Any()))
            {
                return null;
            }

            string sqlWhere = this.GenPodReplyDocumentWithAttachmentWhere(condition, projectID);
            long customerID = condition.CustomerIDs.First();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@CustomerID",DbType.Int64,customerID,ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetExportPodReplyDocumentWithAttachmentByCondition", dbParams);
        }

        private string endCitiesId(string condition)
        {
            string endCities = string.Empty;

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EndCityIDs", condition.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                for (int i = 0; i < dtable.Rows.Count; i++)
                {
                    endCities += dtable.Rows[i][0].ToString() + ",";
                }
                endCities = endCities.Substring(0, endCities.Length - 1);
            }

            return endCities;
        }
        public IEnumerable<PodWithAttachment> QueryPod(PodSearchCondition condition, long projectID, int pageIndex, int pageSize, out int rowCount)
        {
            if (condition.UserType == 2 && !condition.CustomerIDs.Any())
            {
                rowCount = 0;
                return Enumerable.Empty<PodWithAttachment>();
            }
            string endCities = string.Empty;
            string endCitiesRuleArea = string.Empty;
            if (condition.EndCities != null && condition.EndCities.IndexOf(',') > 0)
            {
                endCities = endCitiesId(condition.EndCities);
            }
            if (!string.IsNullOrEmpty(condition.RuleArea) && condition.RuleArea.IndexOf(',') > 0)
            {
                endCitiesRuleArea = endCitiesId(condition.RuleArea);
            }

            string sqlWhere = this.GenQueryPodWhere(condition, projectID, endCities, endCitiesRuleArea);
            int tempRowCount = 0;
            //DataTable dt = new DataTable();
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand("[Proc_QueryPodsTest]", conn);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@Where", sqlWhere);
            //    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
            //    cmd.Parameters.AddWithValue("@RuleArea", condition.RuleArea.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
            //    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
            //    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            //    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
            //    cmd.Parameters.AddWithValue("@PageSize", pageSize);
            //    cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
            //    cmd.Parameters.AddWithValue("@RowCount", tempRowCount);
            //    cmd.Parameters[4].Direction = ParameterDirection.Output;
            //    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
            //    SqlDataAdapter Adp = new SqlDataAdapter(cmd);
            //    Adp.Fill(dt);
            //    rowCount = cmd.Parameters[4].Value.ObjectToInt32();
            //}
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_QueryPods", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<PodWithAttachment>();
        }
        public int CancelPODDistributionVehicle(PODDistributionVehicle req)
        {
            int str = 0;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_CancelPODDistributionVehicle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ids", req.Ids.Select(c => new IdsForInt64(c.ObjectToInt64())));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@PODType", req.PODType);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@RowCount", 0);
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                str = cmd.Parameters[2].Value.ObjectToInt32();
            }

            return str;
        }
        public int PODDistributionVehicle(PODDistributionVehicle req)
        {
            int str = 0;

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_PODDistributionVehicle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ids", req.Ids.Select(c => new IdsForInt64(c.ObjectToInt64())));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Name", req.UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@CarNo", req.CarNo);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@DriverName", req.DriverName);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@DriverPhone", req.DriverPhone);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@PODType", req.PODType);
                cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@StartTime", req.StartTime);
                cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@RowCount", 0);
                cmd.Parameters[7].Direction = ParameterDirection.Output;
                cmd.Parameters[7].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@GPSCode", req.GPSCode);
                cmd.Parameters[8].SqlDbType = SqlDbType.NVarChar;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                str = cmd.Parameters[7].Value.ObjectToInt32();
                //for (int i = 0; i < dtable.Rows.Count; i++)
                //{
                //    str += dtable.Rows[i][0].ToString() + ",";
                //}
                //str = str.Substring(0, condition.Length - 1);
            }

            return str;
        }
        public int WaybillReach(PODDistributionVehicle req)
        {
            int str = 0;

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_WaybillReach", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ids", req.Ids.Select(c => new IdsForInt64(c.ObjectToInt64())));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@PODType", req.PODType);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@EndTime", req.EndTime);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Hub", req.Hub);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@RowCount", 0);
                cmd.Parameters[4].Direction = ParameterDirection.Output;
                cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                str = cmd.Parameters[4].Value.ObjectToInt32();
                //for (int i = 0; i < dtable.Rows.Count; i++)
                //{
                //    str += dtable.Rows[i][0].ToString() + ",";
                //}
                //str = str.Substring(0, condition.Length - 1);
            }

            return str;
        }
        /// <summary>
        /// 打印宝胜运单
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="projectID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<PodWithAttachment> QueryBSPod(PodSearchCondition condition, long projectID, int pageIndex, int pageSize, out int rowCount)
        {
            if (condition.UserType == 2 && !condition.CustomerIDs.Any())
            {
                rowCount = 0;
                return Enumerable.Empty<PodWithAttachment>();
            }
            string endCities = string.Empty;

            string endCitiesRuleArea = string.Empty;


            string sqlWhere = this.GenQueryPodWhere(condition, projectID, endCities, endCitiesRuleArea);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input)//,
             //   new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_QueryBSPod]", dbParams);
            rowCount = 0;
            return dt.ConvertToEntityCollection<PodWithAttachment>();
        }

        #region 导出VF报表
        public PodAll VFTrackingReport(PodSearchCondition condition, long projectID, int PageIndex, int PageSize)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input)//,
            };
            PodAll all = new PodAll();
            //DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReport]", dbParams);
            DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReportCustomer]", dbParams);

            all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            return all;
        }
        public PodAll VFTrackingReportALL(PodSearchCondition condition, long projectID)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)
            };
            PodAll all = new PodAll();
            //DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReport]", dbParams);
            DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReportCustomerAll]", dbParams);

            all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            return all;
        }
        #endregion

        #region 导出Adidas报表
        public PodAll AdidasTrackingReport(PodSearchCondition condition, long projectID, int PageIndex, int PageSize)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input)//,
            };
            PodAll all = new PodAll();
            DataSet ds = this.ExecuteDataSet("[Proc_AdidasTrackingReportCustomer]", dbParams);

            all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            all.PodReplyDocuments = ds.Tables[2].ConvertToEntityCollection<PodReplyDocument>();
            all.PodFeadBacks = ds.Tables[3].ConvertToEntityCollection<PodFeadBack>();
            return all;
        }
        public PodAll AdidasTrackingReportALL(PodSearchCondition condition, long projectID)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)
            };
            PodAll all = new PodAll();
            //DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReport]", dbParams);
            DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReportCustomerAll]", dbParams);

            all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            return all;
        }
        #endregion

        /// <summary>
        /// 导出艺康报表
        /// </summary>
        public DataTable YKTrackingReport(PodSearchCondition condition, long projectID, int PageIndex, int PageSize)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input)//,
            };
            PodAll all = new PodAll();
            DataSet ds = this.ExecuteDataSet("[Proc_YKTrackingReportCustomer]", dbParams);

            //all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            //all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            //all.PodReplyDocuments = ds.Tables[2].ConvertToEntityCollection<PodReplyDocument>();
            //all.PodFeadBacks = ds.Tables[3].ConvertToEntityCollection<PodFeadBack>();
            return ds.Tables[0];
        }

        public SuperPodAll BSTrackingReport(PodSearchCondition condition, long projectID, int PageIndex, int PageSize)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input)//,
            };
            SuperPodAll all = new SuperPodAll();
            //DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReport]", dbParams);
            DataSet ds = this.ExecuteDataSet("[Proc_BSTrackingReportCustomer]", dbParams);

            all.PodCollections = ds.Tables[0].ConvertToEntityCollection<PodWithAttachment>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            return all;
        }
        public SuperPodAll BSTrackingReportALL(PodSearchCondition condition, long projectID)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input)
            };
            SuperPodAll all = new SuperPodAll();
            //DataSet ds = this.ExecuteDataSet("[Proc_VfTrackingReport]", dbParams);
            DataSet ds = this.ExecuteDataSet("[Proc_BSTrackingReportCustomerAll]", dbParams);

            all.PodCollections = ds.Tables[0].ConvertToEntityCollection<PodWithAttachment>();
            all.PodTracks = ds.Tables[1].ConvertToEntityCollection<PodTrack>();
            return all;
        }
        public IEnumerable<Pod> QueryPodWithNoPaging(PodSearchCondition condition, long projectID)
        {
            if (condition.UserType == 2 && !condition.CustomerIDs.Any())
            {
                return Enumerable.Empty<Pod>();
            }

            string endCities = string.Empty;

            string endCitiesRuleArea = string.Empty;
            if (condition.EndCities != null && condition.EndCities.IndexOf(',') > 0)
            {
                endCities = endCitiesId(condition.EndCities);
            }
            if (condition.RuleArea != null && condition.RuleArea.IndexOf(',') > 0)
            {
                endCitiesRuleArea = endCitiesId(condition.EndCities);
            }

            //if (condition.EndCities != null && condition.EndCities.IndexOf(',') > 0)
            //{
            //    using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //    {
            //        DataTable dtable = new DataTable();
            //        SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
            //        cmd.CommandTimeout = 180;
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@EndCityIDs", condition.EndCities.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
            //        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
            //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
            //        Adp.Fill(dtable);
            //        for (int i = 0; i < dtable.Rows.Count; i++)
            //        {
            //            endCities += dtable.Rows[i][0].ToString() + ",";
            //        }
            //        endCities = endCities.Substring(0, endCities.Length - 1);
            //    }
            //}


            string sqlWhere = this.GenQueryPodWhere(condition, projectID, endCities, endCitiesRuleArea);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_QueryPodsWithNoPaging", dbParams).ConvertToEntityCollection<Pod>();
        }
        public IEnumerable<QueryBAFPrice> QueryBAFPrice()
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, "", ParameterDirection.Input)
            };
            return this.ExecuteDataTable("ProcQueryBAFQuotedPrice", dbParams).ConvertToEntityCollection<QueryBAFPrice>();
        }
        public IEnumerable<Pod> QueryPodByPodIDs(IEnumerable<long> PodIDs)
        {
            if (!PodIDs.Any())
            {
                return Enumerable.Empty<Pod>();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(" WHERE ID IN ( ");
            foreach (long id in PodIDs)
            {
                sb.Append(id.ToString());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" ) ");

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_QueryPodsWithNoPaging", dbParams).ConvertToEntityCollection<Pod>();
        }

        public bool SetPodStatus(IEnumerable<long> ids, long podStatusID, string podStatusName, string IsSendMessage)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetPodStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", ids.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@PodStatusID", podStatusID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@PodStatusName", podStatusName);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@IsSendMessage", IsSendMessage);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public IEnumerable<GroupedPods> GetGroupedPodsByPodIDs(IEnumerable<long> ids, int settledType, int? IsID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd;
                if (IsID == 1)
                {
                    cmd = new SqlCommand("Proc_GetZCGroupedPodsByPodIDs", conn);
                }
                else
                {
                    cmd = new SqlCommand("Proc_GetGroupedPodsByPodIDs", conn);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", ids.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@SettledType", settledType);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                conn.Open();
                IList<GroupedPods> returnGroupedPods = new List<GroupedPods>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnGroupedPods.Add(new GroupedPods()
                    {
                        Target = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        TargetID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        TargetName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        ActualDeliveryDate = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        ShipperTypeID = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                        ShipperTypeName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        PODTypeID = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                        PODTypeName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        TtlOrTplID = reader.IsDBNull(8) ? 0 : reader.GetInt64(8),
                        TtlOrTplName = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        StartCityID = reader.IsDBNull(10) ? 0 : reader.GetInt64(10),
                        StartCityName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        EndCityID = reader.IsDBNull(12) ? 0 : reader.GetInt64(12),
                        EndCityName = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        BoxNumber = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                        Weight = reader.IsDBNull(15) ? 0 : reader.GetDouble(15),
                        GoodsNumber = reader.IsDBNull(16) ? 0 : reader.GetDouble(16),
                        Volume = reader.IsDBNull(17) ? 0 : reader.GetDouble(17),
                        PodNumbers = reader.IsDBNull(18) ? 0 : reader.GetInt32(18),
                        PodIDs = reader.IsDBNull(19) ? string.Empty : reader.GetString(19)
                    });
                }

                return returnGroupedPods;
            }
        }


        public IEnumerable<GroupedPods> SettledPodByAddress(IEnumerable<long> ids, int settledType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SettledPodByAddress", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", ids.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@SettledType", settledType);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                conn.Open();
                IList<GroupedPods> returnGroupedPods = new List<GroupedPods>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnGroupedPods.Add(new GroupedPods()
                    {
                        Target = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        TargetID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        TargetName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        ActualDeliveryDate = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        ShipperTypeID = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                        ShipperTypeName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        PODTypeID = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                        PODTypeName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        TtlOrTplID = reader.IsDBNull(8) ? 0 : reader.GetInt64(8),
                        TtlOrTplName = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        StartCityID = reader.IsDBNull(10) ? 0 : reader.GetInt64(10),
                        StartCityName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        EndCityID = reader.IsDBNull(12) ? 0 : reader.GetInt64(12),
                        EndCityName = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        BoxNumber = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                        Weight = reader.IsDBNull(15) ? 0 : reader.GetDouble(15),
                        GoodsNumber = reader.IsDBNull(16) ? 0 : reader.GetDouble(16),
                        Volume = reader.IsDBNull(17) ? 0 : reader.GetDouble(17),
                        PodNumbers = reader.IsDBNull(18) ? 0 : reader.GetInt32(18),
                        PodIDs = reader.IsDBNull(19) ? string.Empty : reader.GetString(19)
                    });
                }

                return returnGroupedPods;
            }
        }


        public IEnumerable<PodDescription> SetPodShipperManually(long projectID, IEnumerable<long> ids, long shipperID, string shipperName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetPodShipperManually", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", ids.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ProjectID", projectID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@ShipperName", shipperName);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                IList<PodDescription> podDescriptions = new List<PodDescription>();
                while (reader.Read())
                {
                    podDescriptions.Add(new PodDescription()
                    {
                        ID = reader.GetInt64(0),
                        Description = reader.GetSqlString(1).ToString()
                    });
                };

                return podDescriptions;
            }
        }

        public IEnumerable<PodDescription> SetPodShipper(long projectID, IEnumerable<long> ids)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetPodShipper", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", ids.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ProjectID", projectID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.CommandTimeout = 180;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IList<PodDescription> podDescriptions = new List<PodDescription>();
                while (reader.Read())
                {
                    podDescriptions.Add(new PodDescription()
                    {
                        ID = reader.GetInt64(0),
                        Description = reader.GetSqlString(1).ToString()
                    });
                };

                return podDescriptions;
            }
        }

        private string GenPodReplyDocumentWithAttachmentWhere(PodReplyDocumentSearchCondition condition, long projectID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where a.ProjectID=" + projectID + " ");

            if (condition.ShipperID.HasValue)
            {
                sb.Append(" and a.ShipperID=" + condition.ShipperID.Value + " ");
                //sb.Append(" and a.PODStateID>2 ");
            }

            if (condition.CustomerIDs != null && condition.CustomerIDs.Any())
            {
                sb.Append(" and a.CustomerID in (");
                foreach (long i in condition.CustomerIDs)
                {
                    sb.Append(i.ToString());
                    sb.Append(",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (condition.IsAudit.HasValue)
            {
                if (condition.IsAudit.Value)
                {
                    sb.Append(" and (b.IsAudit=1 or c.Remark <> '') ");
                }
                else
                {
                    sb.Append(" and (b.IsAudit=0 or b.IsAudit is null) and (c.Remark = '' or c.Remark is null) ");
                }
            }
            if (!string.IsNullOrEmpty(condition.AuditType))
            {
                if (condition.AuditType == "1")
                {
                    sb.Append(" and b.IsAudit=1 ");
                }
                else if (condition.AuditType == "2")
                {
                    sb.Append(" and (b.IsAudit=0 and c.Remark<>'') ");
                }
            }





            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and a.SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.SystemNumber like '%" + condition.SystemNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (condition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    sb.Append(" and a.CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            if (condition.ShipperTypeID.HasValue)
            {
                sb.Append(" and a.ShipperTypeID=").Append(condition.ShipperTypeID.Value).Append(" ");
            }

            if (condition.PODTypeID.HasValue)
            {
                sb.Append(" and a.PODTypeID=").Append(condition.PODTypeID.Value).Append(" ");
            }

            if (condition.CreateDate.HasValue)
            {
                sb.Append(" and c.CreateDate >= '" + condition.CreateDate.Value.DateTimeToString() + "' ");
            }

            if (condition.EndCreateDate.HasValue)
            {
                sb.Append(" and c.CreateDate <= '" + condition.EndCreateDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate >=' ").Append(condition.ActualDeliveryDate.Value.DateTimeToString()).Append("' ");
            }

            if (condition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate < '").Append(condition.EndActualDeliveryDate.Value.AddDays(1).DateTimeToString()).Append("' ");
            }

            if (condition.ShipperID.HasValue)
            {
                sb.Append(" and a.ShipperID=").Append(condition.ShipperID.Value.ToString()).Append(" ");
            }

            if (condition.HasAttachment.HasValue)
            {
                if (condition.HasAttachment.Value)
                {
                    sb.Append(" and c.ID IS NOT NULL ");
                }
                else
                {
                    sb.Append(" and c.ID IS NULL ");
                }
            }

            if (!string.IsNullOrEmpty(condition.StartWareHouse))
            {
                sb.Append(" and a.Str28 like '%").Append(condition.StartWareHouse.Trim()).Append("%' ");
            }


            if (!string.IsNullOrEmpty(condition.SalesOrderOrNoneSalesOrder))
            {
                sb.Append(" and a.Str34='").Append(condition.SalesOrderOrNoneSalesOrder).Append("' ");
            }
            if (!string.IsNullOrEmpty(condition.PodRegion))
            {
                sb.Append(" and a.Str8='").Append(condition.PodRegion.Trim()).Append("' ");
            }

            if (condition.OrderDate.HasValue)
            {
                sb.Append(" and a.DateTime1 >= '" + condition.OrderDate.Value.DateTimeToString() + "' ");
            }

            if (condition.EndOrderDate.HasValue)
            {
                sb.Append(" and a.DateTime1 <= '" + condition.EndOrderDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (!string.IsNullOrEmpty(condition.Number103s))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (condition.Number103s.IndexOf("\n") > 0)
                {
                    numbers = condition.Number103s.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Number103s.IndexOf(',') > 0)
                {
                    numbers = condition.Number103s.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and a.Str1 in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.Str1 like '%" + condition.Number103s.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(condition.CreditNumber))
            {
                sb.Append(" and a.Str8 = '" + condition.CreditNumber.Trim() + "' ");
            }
            return sb.ToString();
        }
        private string GenQueryVFPodWhere(PodSearchCondition condition, long projectID, string endCities)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where POD.ProjectID=" + projectID + " ");

            #region
            if (condition.StartCityID != null && condition.StartCityID != 0)
            {
                sb.Append(" and POD.StartCityID=" + condition.StartCityID + " ");
            }

            if (condition.EndCityID != null && condition.EndCityID != 0)
            {
                sb.Append(" and POD.EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.EndCityID + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCities))
                {
                    sb.Append(" and POD.EndCityID in (").Append(endCities).Append(") ");
                }
            }

            if (condition.UserType == 1)
            {
                sb.Append(" and POD.PODStateID>2 ");
            }

            if (condition.UserType == 2)
            {
                if (condition.CustomerID == null || condition.CustomerID == 0)
                {
                    sb.Append(" and POD.CustomerID in (");
                    foreach (long i in condition.CustomerIDs)
                    {
                        sb.Append(i.ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(") ");
                }
                else
                {
                    sb.Append(" and POD.CustomerID=" + condition.CustomerID + " ");
                }
            }
            else
            {
                if (condition.CustomerID != null && condition.CustomerID != 0)
                {
                    sb.Append(" and POD.CustomerID=" + condition.CustomerID + " ");
                }
            }

            if (condition.Types != null && condition.Types.Any())
            {
                sb.Append(" and POD.Type in (");
                foreach (int type in condition.Types)
                {
                    sb.Append(type.ToString()).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (condition.PODStateID != null && condition.PODStateID != 0)
            {
                sb.Append(" and POD.PODStateID=" + condition.PODStateID + " ");
            }

            if (condition.TtlOrTplID != null && condition.TtlOrTplID != 0)
            {
                sb.Append(" and POD.TtlOrTplID=" + condition.TtlOrTplID + " ");
            }

            if (condition.PODTypeID != null && condition.PODTypeID != 0)
            {
                sb.Append(" and POD.PODTypeID=" + condition.PODTypeID + " ");
            }

            if (condition.ShipperTypeID != null && condition.ShipperTypeID != 0)
            {
                sb.Append(" and POD.ShipperTypeID=" + condition.ShipperTypeID + " ");
            }

            if (condition.ShipperIDIsNull)
            {
                sb.Append(" and (POD.ShipperID is null or POD.ShipperID=0) ");
            }
            else
            {
                if (condition.ShipperID != null && condition.ShipperID != 0)
                {
                    sb.Append(" and POD.ShipperID=" + condition.ShipperID + " ");
                }
            }

            if (condition.BoxNumber != null && condition.BoxNumber != 0)
            {
                sb.Append(" and POD.BoxNumber=" + condition.BoxNumber + " ");
            }

            if (condition.Weight != null && condition.Weight != 0)
            {
                sb.Append(" and POD.Weight=" + condition.Weight + " ");
            }

            if (condition.GoodsNumber != null && condition.GoodsNumber != 0)
            {
                sb.Append(" and POD.GoodsNumber=" + condition.GoodsNumber + " ");
            }

            if (condition.Volume != null && condition.Volume != 0)
            {
                sb.Append(" and POD.BoxNumber=" + condition.Volume + " ");
            }

            if (!string.IsNullOrEmpty(condition.Creator))
            {
                sb.Append(" and POD.Creator='" + condition.Creator + "' ");
            }

            if (condition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and POD.ActualDeliveryDate >= '" + condition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
            }

            if (condition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and POD.ActualDeliveryDate <= '" + condition.EndActualDeliveryDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.CreateTime.HasValue)
            {
                sb.Append(" and POD.CreateTime >= '" + condition.CreateTime.Value.DateTimeToString() + "' ");
            }

            if (condition.EndCreateTime.HasValue)
            {
                sb.Append(" and POD.CreateTime <= '" + condition.EndCreateTime.Value.DateTimeToString() + " 23:59' ");
            }


            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and POD.SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.SystemNumber like '%" + condition.SystemNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (condition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {

                    sb.Append(" and POD.CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }


            if (!string.IsNullOrEmpty(condition.Str1))
            {
                sb.Append(" and POD.Str1 like '%" + condition.Str1 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str2))
            {
                sb.Append(" and POD.Str2 like '%" + condition.Str2 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str3))
            {
                sb.Append(" and POD.Str3 like '%" + condition.Str3 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str4))
            {
                sb.Append(" and POD.Str4 like '%" + condition.Str4 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str5))
            {
                sb.Append(" and POD.Str5 like '%" + condition.Str5 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str6))
            {
                sb.Append(" and POD.Str6 like '%" + condition.Str6 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str7))
            {
                sb.Append(" and POD.Str7 like '%" + condition.Str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str8))
            {
                sb.Append(" and POD.Str8 like '%" + condition.Str8 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str9))
            {
                sb.Append(" and POD.Str9 like '%" + condition.Str9 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str10))
            {
                sb.Append(" and POD.Str10 like '%" + condition.Str10 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str11))
            {
                sb.Append(" and POD.Str11 like '%" + condition.Str11 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str12))
            {
                sb.Append(" and POD.Str12 like '%" + condition.Str12 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str13))
            {
                sb.Append(" and POD.Str13 like '%" + condition.Str13 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str14))
            {
                sb.Append(" and POD.Str14 like '%" + condition.Str14 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str15))
            {
                sb.Append(" and POD.Str15 like '%" + condition.Str15 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str16))
            {
                sb.Append(" and POD.Str16 like '%" + condition.Str16 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str17))
            {
                sb.Append(" and POD.Str17 like '%" + condition.Str17 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str18))
            {
                sb.Append(" and POD.Str18 like '%" + condition.Str18 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str19))
            {
                sb.Append(" and POD.Str19 like '%" + condition.Str19 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str20))
            {
                sb.Append(" and POD.Str20 like '%" + condition.Str20 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str21))
            {
                sb.Append(" and POD.Str21 like '%" + condition.Str21 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str22))
            {
                sb.Append(" and POD.Str22 like '%" + condition.Str22 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str23))
            {
                sb.Append(" and POD.Str23 like '%" + condition.Str23 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str24))
            {
                sb.Append(" and POD.Str24 like '%" + condition.Str24 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str25))
            {
                sb.Append(" and POD.Str25 like '%" + condition.Str25 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str26))
            {
                sb.Append(" and POD.Str26 like '%" + condition.Str26 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str27))
            {
                sb.Append(" and POD.Str27 like '%" + condition.Str27 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str28))
            {
                sb.Append(" and POD.Str28 like '%" + condition.Str28 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str29))
            {
                sb.Append(" and POD.Str29 like '%" + condition.Str29 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str30))
            {
                sb.Append(" and POD.Str30 like '%" + condition.Str30 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str31))
            {
                sb.Append(" and POD.Str31 like '%" + condition.Str31 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str32))
            {
                sb.Append(" and POD.Str32 like '%" + condition.Str32 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str33))
            {
                sb.Append(" and POD.Str33 like '%" + condition.Str33 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str34))
            {
                sb.Append(" and POD.Str34 like '%" + condition.Str34 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str35))
            {
                sb.Append(" and POD.Str35 like '%" + condition.Str35 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str36))
            {
                sb.Append(" and POD.Str36 like '%" + condition.Str36 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str37))
            {
                sb.Append(" and POD.Str37 like '%" + condition.Str37 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str38))
            {
                sb.Append(" and POD.Str38 like '%" + condition.Str38 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str39))
            {
                sb.Append(" and POD.Str39 like '%" + condition.Str39 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str40))
            {
                sb.Append(" and POD.Str40 like '%" + condition.Str40 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str41))
            {
                sb.Append(" and POD.Str41 like '%" + condition.Str41 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str42))
            {
                sb.Append(" and POD.Str42 like '%" + condition.Str42 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str43))
            {
                sb.Append(" and POD.Str43 like '%" + condition.Str43 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str44))
            {
                sb.Append(" and POD.Str44 like '%" + condition.Str44 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str45))
            {
                sb.Append(" and POD.Str45 like '%" + condition.Str45 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str46))
            {
                sb.Append(" and POD.Str46 like '%" + condition.Str46 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str47))
            {
                sb.Append(" and POD.Str47 like '%" + condition.Str47 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str48))
            {
                sb.Append(" and POD.Str48 like '%" + condition.Str48 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str49))
            {
                sb.Append(" and POD.Str49 like '%" + condition.Str49 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str50))
            {
                sb.Append(" and POD.Str50 like '%" + condition.Str50 + "%' ");
            }

            if (condition.DateTime1.HasValue)
            {
                sb.Append(" and POD.DateTime1 >= '" + condition.DateTime1.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime1.HasValue)
            {
                sb.Append(" and POD.DateTime1 <= '" + condition.EndDateTime1.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime2.HasValue)
            {
                sb.Append(" and POD.DateTime2 >= '" + condition.DateTime2.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime2.HasValue)
            {
                sb.Append(" and POD.DateTime2 <= '" + condition.EndDateTime2.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime3.HasValue)
            {
                sb.Append(" and POD.DateTime3 >= '" + condition.DateTime3.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime3.HasValue)
            {
                sb.Append(" and POD.DateTime3 <= '" + condition.EndDateTime3.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime4.HasValue)
            {
                sb.Append(" and POD.DateTime4 >= '" + condition.DateTime4.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime4.HasValue)
            {
                sb.Append(" and POD.DateTime4 <= '" + condition.EndDateTime4.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime5.HasValue)
            {
                sb.Append(" and POD.DateTime5 >= '" + condition.DateTime5.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime5.HasValue)
            {
                sb.Append(" and POD.DateTime5 <= '" + condition.EndDateTime5.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime6.HasValue)
            {
                sb.Append(" and POD.DateTime6 >= '" + condition.DateTime6.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime6.HasValue)
            {
                sb.Append(" and POD.DateTime6 <= '" + condition.EndDateTime6.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime7.HasValue)
            {
                sb.Append(" and POD.DateTime7 >= '" + condition.DateTime7.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime7.HasValue)
            {
                sb.Append(" and POD.DateTime7 <= '" + condition.EndDateTime7.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime8.HasValue)
            {
                sb.Append(" and POD.DateTime8 >= '" + condition.DateTime8.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime8.HasValue)
            {
                sb.Append(" and POD.DateTime8 <= '" + condition.EndDateTime8.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime9.HasValue)
            {
                sb.Append(" and POD.DateTime9 >= '" + condition.DateTime9.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime9.HasValue)
            {
                sb.Append(" and POD.DateTime9 <= '" + condition.EndDateTime9.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime10.HasValue)
            {
                sb.Append(" and POD.DateTime10 >= '" + condition.DateTime10.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime10.HasValue)
            {
                sb.Append(" and POD.DateTime10 <= '" + condition.EndDateTime10.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime11.HasValue)
            {
                sb.Append(" and POD.DateTime11 >= '" + condition.DateTime11.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime11.HasValue)
            {
                sb.Append(" and POD.DateTime11 <= '" + condition.EndDateTime11.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime12.HasValue)
            {
                sb.Append(" and POD.DateTime12 >= '" + condition.DateTime12.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime12.HasValue)
            {
                sb.Append(" and POD.DateTime12 <= '" + condition.EndDateTime12.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime13.HasValue)
            {
                sb.Append(" and POD.DateTime13 >= '" + condition.DateTime13.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime13.HasValue)
            {
                sb.Append(" and POD.DateTime13 <= '" + condition.EndDateTime13.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime15.HasValue)
            {
                sb.Append(" and POD.DateTime15 >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime15.HasValue)
            {
                sb.Append(" and POD.DateTime15 <= '" + condition.EndDateTime14.Value.DateTimeToString() + " 23:59' ");
            }

            //for Tianjin Hub 查询 
            if (condition.DateTime14.HasValue)
            {
                sb.Append(" AND CASE POD.CustomerID WHEN 1 THEN POD.DateTime4 WHEN 8 THEN POD.DateTime6 WHEN 2 THEN POD.DateTime2 WHEN 7 THEN POD.DateTime1 END >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
                //sb.Append(" and DateTime15 >= '" + condition.DateTime15.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime14.HasValue)
            {
                sb.Append(" AND CASE POD.CustomerID WHEN 1 THEN POD.DateTime4 WHEN 8 THEN POD.DateTime6 WHEN 2 THEN POD.DateTime2 WHEN 7 THEN POD.DateTime1 END <= '" + condition.EndDateTime14.Value.DateTimeToString() + "  23:59'  ");
                //sb.Append(" and DateTime15 <= '" + condition.EndDateTime15.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.IsSettledForCustomer.HasValue)
            {
                sb.Append(" and POD.IsSettledForCustomer=" + (condition.IsSettledForCustomer.Value ? "1" : "0") + " ");
            }

            if (condition.IsSettledForShipper.HasValue)
            {
                sb.Append(" and POD.IsSettledForShipper=" + (condition.IsSettledForShipper.Value ? "1" : "0") + " ");
            }

            if (condition.HasShortDial.HasValue)
            {
                sb.Append(" and POD.HasShortDial=" + (condition.HasShortDial.Value ? "1" : "0") + " ");
            }

            if (condition.HasDistribution.HasValue)
            {
                sb.Append(" and POD.HasDistribution=" + (condition.HasDistribution.Value ? "1" : "0") + " ");
            }

            if (condition.HasExpress.HasValue)
            {
                sb.Append(" and POD.HasExpress=" + (condition.HasExpress.Value ? "1" : "0") + " ");
            }

            if (condition.PodMinStateID.HasValue)
            {
                sb.Append(" and POD.PodStateID >= " + condition.PodMinStateID.Value + " ");
            }

            if (condition.HasAllocateShipper.HasValue)
            {
                if (condition.HasAllocateShipper.Value)
                {
                    sb.Append(" and (POD.ShipperID != 0 or POD.ShipperID IS NOT NULL) ");
                }
                else
                {
                    sb.Append(" and (POD.ShipperID = 0 OR POD.ShipperID IS NULL) ");
                }
            }
            if (condition.ID != 0)
            {
                sb.Append(" and ID = " + condition.ID + " ");
            }

            #endregion

            return sb.ToString();
        }
        private string GenQueryPodWhere(PodSearchCondition condition, long projectID, string endCities, string endCitiesRuleArea)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ProjectID=" + projectID + " ");

            //百姓网订单排除快递单号
            if (condition.CustomerID == 40)
            {
                sb.Append(" and [Type]='2' ");
            }

            #region
            if (condition.StartCityID != null && condition.StartCityID != 0)
            {
                sb.Append(" and StartCityID=" + condition.StartCityID + " ");
            }

            if (condition.EndCityID != null && condition.EndCityID != 0)
            {
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.EndCityID + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCities))
                {
                    sb.Append(" and EndCityID in (").Append(endCities).Append(") ");
                }
            }

            if (condition.UserType == 1)
            {
                sb.Append(" and PODStateID>2 ");
            }

            if (condition.UserType == 2)
            {
                if (condition.CustomerID == null || condition.CustomerID == 0)
                {
                    sb.Append(" and CustomerID in (");
                    foreach (long i in condition.CustomerIDs)
                    {
                        sb.Append(i.ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(") ");
                }
                else
                {
                    sb.Append(" and CustomerID=" + condition.CustomerID + " ");
                }
            }
            else
            {
                if (condition.CustomerID != null && condition.CustomerID != 0)
                {
                    sb.Append(" and CustomerID=" + condition.CustomerID + " ");
                }
            }

            if (condition.Types != null && condition.Types.Any())
            {
                sb.Append(" and Type in (");
                foreach (int type in condition.Types)
                {
                    sb.Append(type.ToString()).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (condition.PODStateID != null && condition.PODStateID != 0)
            {
                sb.Append(" and PODStateID=" + condition.PODStateID + " ");
            }

            if (condition.TtlOrTplID != null && condition.TtlOrTplID != 0)
            {
                sb.Append(" and TtlOrTplID=" + condition.TtlOrTplID + " ");
            }

            if (condition.PODTypeID != null && condition.PODTypeID != 0)
            {
                sb.Append(" and PODTypeID=" + condition.PODTypeID + " ");
            }

            if (condition.ShipperTypeID != null && condition.ShipperTypeID != 0)
            {
                sb.Append(" and ShipperTypeID=" + condition.ShipperTypeID + " ");
            }

            if (condition.ShipperIDIsNull)
            {
                sb.Append(" and (ShipperID is null or ShipperID=0) ");
            }
            else
            {
                if (condition.ShipperID != null && condition.ShipperID != 0)
                {
                    sb.Append(" and ShipperID=" + condition.ShipperID + " ");
                }
            }

            if (condition.BoxNumber != null && condition.BoxNumber != 0)
            {
                sb.Append(" and BoxNumber=" + condition.BoxNumber + " ");
            }

            if (condition.Weight != null && condition.Weight != 0)
            {
                sb.Append(" and Weight=" + condition.Weight + " ");
            }

            if (condition.GoodsNumber != null && condition.GoodsNumber != 0)
            {
                sb.Append(" and GoodsNumber=" + condition.GoodsNumber + " ");
            }

            if (condition.Volume != null && condition.Volume != 0)
            {
                sb.Append(" and BoxNumber=" + condition.Volume + " ");
            }

            if (!string.IsNullOrEmpty(condition.Creator))
            {
                sb.Append(" and Creator='" + condition.Creator + "' ");
            }

            if (condition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate >= '" + condition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
            }

            if (condition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate <= '" + condition.EndActualDeliveryDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.CreateTime.HasValue)
            {
                sb.Append(" and CreateTime >= '" + condition.CreateTime.Value.DateTimeToString() + "' ");
            }

            if (condition.EndCreateTime.HasValue)
            {
                sb.Append(" and CreateTime <= '" + condition.EndCreateTime.Value.DateTimeToString() + " 23:59' ");
            }


            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and SystemNumber like '%" + condition.SystemNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (condition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {

                    sb.Append(" and CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }


            if (!string.IsNullOrEmpty(condition.Str1))
            {
                sb.Append(" and Str1 like '%" + condition.Str1 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str2))
            {
                sb.Append(" and Str2 like '%" + condition.Str2 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str3))
            {
                sb.Append(" and Str3 like '%" + condition.Str3 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str4))
            {
                sb.Append(" and Str4 like '%" + condition.Str4 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str5))
            {
                sb.Append(" and Str5 like '%" + condition.Str5 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str6))
            {
                sb.Append(" and Str6 like '%" + condition.Str6 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str7))
            {
                sb.Append(" and Str7 like '%" + condition.Str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str8))
            {
                sb.Append(" and Str8 like '%" + condition.Str8 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str9))
            {
                sb.Append(" and Str9 like '%" + condition.Str9 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str10))
            {
                sb.Append(" and Str10 like '%" + condition.Str10 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str11))
            {
                sb.Append(" and Str11 like '%" + condition.Str11 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str12))
            {
                sb.Append(" and Str12 like '%" + condition.Str12 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str13))
            {
                sb.Append(" and Str13 like '%" + condition.Str13 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str14))
            {
                sb.Append(" and Str14 like '%" + condition.Str14 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str15))
            {
                sb.Append(" and Str15 like '%" + condition.Str15 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str16))
            {
                sb.Append(" and Str16 like '%" + condition.Str16 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str17))
            {
                sb.Append(" and Str17 like '%" + condition.Str17 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str18))
            {
                sb.Append(" and Str18 like '%" + condition.Str18 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str19))
            {
                sb.Append(" and Str19 like '%" + condition.Str19 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str20))
            {
                sb.Append(" and Str20 like '%" + condition.Str20 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str21))
            {
                sb.Append(" and Str21 like '%" + condition.Str21 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str22))
            {
                sb.Append(" and Str22 like '%" + condition.Str22 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str23))
            {
                sb.Append(" and Str23 like '%" + condition.Str23 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str24))
            {
                sb.Append(" and Str24 like '%" + condition.Str24 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str25))
            {
                sb.Append(" and Str25 like '%" + condition.Str25 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str26))
            {
                sb.Append(" and Str26 like '%" + condition.Str26 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str27))
            {
                sb.Append(" and Str27 like '%" + condition.Str27 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str28))
            {
                sb.Append(" and Str28 like '%" + condition.Str28 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str29))
            {
                sb.Append(" and Str29 like '%" + condition.Str29 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str30))
            {
                sb.Append(" and Str30 like '%" + condition.Str30 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str31))
            {
                sb.Append(" and Str31 like '%" + condition.Str31 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str32))
            {
                sb.Append(" and Str32 like '%" + condition.Str32 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str33))
            {
                sb.Append(" and Str33 like '%" + condition.Str33 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str34))
            {
                sb.Append(" and Str34 like '%" + condition.Str34 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str35))
            {
                sb.Append(" and Str35 like '%" + condition.Str35 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str36))
            {
                sb.Append(" and Str36 like '%" + condition.Str36 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str37))
            {
                sb.Append(" and Str37 like '%" + condition.Str37 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str38))
            {
                sb.Append(" and Str38 like '%" + condition.Str38 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str39))
            {
                sb.Append(" and Str39 like '%" + condition.Str39 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str40))
            {
                sb.Append(" and Str40 like '%" + condition.Str40 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str41))
            {
                sb.Append(" and Str41 like '%" + condition.Str41 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str42))
            {
                sb.Append(" and Str42 like '%" + condition.Str42 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str43))
            {
                sb.Append(" and Str43 like '%" + condition.Str43 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str44))
            {
                sb.Append(" and Str44 like '%" + condition.Str44 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str45))
            {
                sb.Append(" and Str45 like '%" + condition.Str45 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str46))
            {
                sb.Append(" and Str46 like '%" + condition.Str46 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str47))
            {
                sb.Append(" and Str47 like '%" + condition.Str47 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str48))
            {
                sb.Append(" and Str48 like '%" + condition.Str48 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str49))
            {
                sb.Append(" and Str49 like '%" + condition.Str49 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str50))
            {
                sb.Append(" and Str50 like '%" + condition.Str50 + "%' ");
            }

            if (condition.DateTime1.HasValue)
            {
                sb.Append(" and DateTime1 >= '" + condition.DateTime1.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime1.HasValue)
            {
                sb.Append(" and DateTime1 <= '" + condition.EndDateTime1.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime2.HasValue)
            {
                sb.Append(" and DateTime2 >= '" + condition.DateTime2.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime2.HasValue)
            {
                sb.Append(" and DateTime2 <= '" + condition.EndDateTime2.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime3.HasValue)
            {
                sb.Append(" and DateTime3 >= '" + condition.DateTime3.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime3.HasValue)
            {
                sb.Append(" and DateTime3 <= '" + condition.EndDateTime3.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime4.HasValue)
            {
                sb.Append(" and DateTime4 >= '" + condition.DateTime4.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime4.HasValue)
            {
                sb.Append(" and DateTime4 <= '" + condition.EndDateTime4.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime5.HasValue)
            {
                sb.Append(" and DateTime5 >= '" + condition.DateTime5.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime5.HasValue)
            {
                sb.Append(" and DateTime5 <= '" + condition.EndDateTime5.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime6.HasValue)
            {
                sb.Append(" and DateTime6 >= '" + condition.DateTime6.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime6.HasValue)
            {
                sb.Append(" and DateTime6 <= '" + condition.EndDateTime6.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime7.HasValue)
            {
                sb.Append(" and DateTime7 >= '" + condition.DateTime7.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime7.HasValue)
            {
                sb.Append(" and DateTime7 <= '" + condition.EndDateTime7.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime8.HasValue)
            {
                sb.Append(" and DateTime8 >= '" + condition.DateTime8.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime8.HasValue)
            {
                sb.Append(" and DateTime8 <= '" + condition.EndDateTime8.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime9.HasValue)
            {
                sb.Append(" and DateTime9 >= '" + condition.DateTime9.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime9.HasValue)
            {
                sb.Append(" and DateTime9 <= '" + condition.EndDateTime9.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime10.HasValue)
            {
                sb.Append(" and DateTime10 >= '" + condition.DateTime10.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime10.HasValue)
            {
                sb.Append(" and DateTime10 <= '" + condition.EndDateTime10.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime11.HasValue)
            {
                sb.Append(" and DateTime11 >= '" + condition.DateTime11.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime11.HasValue)
            {
                sb.Append(" and DateTime11 <= '" + condition.EndDateTime11.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime12.HasValue)
            {
                sb.Append(" and DateTime12 >= '" + condition.DateTime12.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime12.HasValue)
            {
                sb.Append(" and DateTime12 <= '" + condition.EndDateTime12.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime13.HasValue)
            {
                sb.Append(" and DateTime13 >= '" + condition.DateTime13.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime13.HasValue)
            {
                sb.Append(" and DateTime13 <= '" + condition.EndDateTime13.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime15.HasValue)
            {
                sb.Append(" and DateTime15 >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime15.HasValue)
            {
                sb.Append(" and DateTime15 <= '" + condition.EndDateTime14.Value.DateTimeToString() + " 23:59' ");
            }

            //for Tianjin Hub 查询 
            if (condition.DateTime14.HasValue)
            {
                sb.Append(" AND CASE CustomerID WHEN 1 THEN DateTime4 WHEN 8 THEN DateTime6 WHEN 2 THEN DateTime2 WHEN 7 THEN DateTime1 END >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
                //sb.Append(" and DateTime15 >= '" + condition.DateTime15.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime14.HasValue)
            {
                sb.Append(" AND CASE CustomerID WHEN 1 THEN DateTime4 WHEN 8 THEN DateTime6 WHEN 2 THEN DateTime2 WHEN 7 THEN DateTime1 END <= '" + condition.EndDateTime14.Value.DateTimeToString() + "  23:59'  ");
                //sb.Append(" and DateTime15 <= '" + condition.EndDateTime15.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.IsSettledForCustomer.HasValue)
            {
                sb.Append(" and IsSettledForCustomer=" + (condition.IsSettledForCustomer.Value ? "1" : "0") + " ");
            }

            if (condition.IsSettledForShipper.HasValue)
            {
                sb.Append(" and IsSettledForShipper=" + (condition.IsSettledForShipper.Value ? "1" : "0") + " ");
            }

            if (condition.HasShortDial.HasValue)
            {
                sb.Append(" and HasShortDial=" + (condition.HasShortDial.Value ? "1" : "0") + " ");
            }

            if (condition.HasDistribution.HasValue)
            {
                sb.Append(" and HasDistribution=" + (condition.HasDistribution.Value ? "1" : "0") + " ");
            }

            if (condition.HasExpress.HasValue)
            {
                sb.Append(" and HasExpress=" + (condition.HasExpress.Value ? "1" : "0") + " ");
            }

            if (condition.PodMinStateID.HasValue)
            {
                sb.Append(" and PodStateID >= " + condition.PodMinStateID.Value + " ");
            }

            if (condition.HasAllocateShipper.HasValue)
            {
                if (condition.HasAllocateShipper.Value)
                {
                    sb.Append(" and (ShipperID != 0 or ShipperID IS NOT NULL) ");
                }
                else
                {
                    sb.Append(" and (ShipperID = 0 OR ShipperID IS NULL) ");
                }
            }
            if (condition.ID != 0)
            {
                sb.Append(" and ID = " + condition.ID + " ");
            }
            //if (!string.IsNullOrEmpty(condition.RuleArea))
            //{
            //    sb.Append(" and EndCityId in (select ID from  [Func_GetReginAndSunRegionsByRegionIDRuleArea](" + condition.RuleArea + ")) ");
            //}
            if (!string.IsNullOrEmpty(condition.RuleArea) && condition.RuleArea.Split(',').Count() >= 1 && condition.RuleArea!="0")
            {
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.RuleArea + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCitiesRuleArea))
                {
                    sb.Append(" and EndCityID in (").Append(endCitiesRuleArea).Append(") ");
                }
            }
            #endregion

            return sb.ToString();
        }

        public int GetTodaysPodCount(long projectID, string systemNumberPrefix)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                new DbParam("@SystemNumberPrefix", DbType.String, systemNumberPrefix, ParameterDirection.Input)
             };

            return this.ExecuteScalar("Proc_GetTodaysPodCount", dbParams).ObjectToInt32();
        }

        public IEnumerable<string> SplitPod(long podID, int splitNumber)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
                new DbParam("@SplitNumber", DbType.Int32, splitNumber, ParameterDirection.Input)
             };

            return this.ExecuteDataTable("Proc_SplitPod", dbParams).Rows.Select(r => r[0].ToString());
        }

        public void AuditPodReplyDocument(IEnumerable<string> SystemNumbers, string AuditUser)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AuditPodReplyDocuments", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SystemNumbers", SystemNumbers.Select(p => new StringsToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@AuditUser", AuditUser);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddPodExtensionFee(IEnumerable<long> PodIDs, IEnumerable<Pod> PodCollection, IEnumerable<SettledPod> SettledPodCollection, int Type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPodExtensionFee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodIDs", PodIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Pods", PodCollection.Select(i => new PodToDb(i)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@SettledPods", SettledPodCollection.Select(i => new SettledPodToDb(i)));
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ManualSettledPod(IEnumerable<long> PodIDs, long ShipperID, string ShipperName, IEnumerable<SettledPod> SettledPodCollection)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_ManualSettledPod", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodIDs", PodIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ShipperID", ShipperID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@ShipperName", ShipperName);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 50;
                cmd.Parameters.AddWithValue("@SettledPods", SettledPodCollection.Select(i => new SettledPodToDb(i)));
                cmd.Parameters[3].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<PodForecastInfo> GetPodForecastInfoCollection(IEnumerable<long> PodIDs, long CustomerID, long ProjectID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_GetPodForecastInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", PodIDs.Select(i => new IdsForInt64(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<PodForecastInfo>();
            }
        }

        public IEnumerable<Log56PhoneStatus> UpdateLog56PhoneStatus(IEnumerable<Log56PhoneStatus> log56PhoneStatus)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_UpdateLog56PhoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PhoneNumbers", log56PhoneStatus.Select(i => new Log56PhoneStatusToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<Log56PhoneStatus>();
            }
        }

        public void UpdateLog56PhoneStatusFromLog56(IEnumerable<Log56PhoneStatus> log56PhoneStatus)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateLog56PhoneStatusFromLog56", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PhoneNumbers", log56PhoneStatus.Select(i => new Log56PhoneStatusToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool CancelAttachmentAudit(long AttachmentID, long PodID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_CancelAttachmentAudit", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AttachmentID", AttachmentID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@PodID", PodID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                int returnVal = cmd.ExecuteScalar().ObjectToInt32();
                return returnVal == 1 ? true : false;
            }
        }

        public void SetAttachmentRemark(long ID, string Remark, string AuditUser)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetAttachmentRemark", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Remark", Remark);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@AuditUser", AuditUser);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public AddLog56TracksResult AddLog56Tracks(IEnumerable<Log56Track> usefulTracks, IEnumerable<Log56Track> uselessTracks, IEnumerable<Log56Track> allTracks)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Proc_AddPodTracksFromLog56", conn);
                var uselessTracksToDb = uselessTracks.Select(u => new Log56TrackToDb(u));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usefulTracks", usefulTracks.Select(u => new Log56TrackToDb(u)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ueslessTracks", uselessTracksToDb.Any() ? uselessTracksToDb : null);
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@allNumbers", allTracks.Select(u => new Log56TrackToDb(u)));
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(ds);

                AddLog56TracksResult result = new AddLog56TracksResult();
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        result.TracksHaveAdded = ds.Tables[0].ConvertToEntityCollection<Log56Track>();
                    }

                    if (ds.Tables[1] != null)
                    {
                        result.TracksWithIssues = ds.Tables[1].ConvertToEntityCollection<Log56Track>();
                    }

                    if (ds.Tables[2] != null)
                    {
                        result.TracksNotAdded = ds.Tables[2].ConvertToEntityCollection<Log56Track>();
                    }
                }

                return result;
            }
        }

        public IEnumerable<Attachment> PodWithAttachment(PodSearchCondition Searchcondition)
        {

            string Where = this.GetPodWithAttachment(Searchcondition);

            DbParam[] dbParams = new DbParam[]{
               new DbParam("@Where", DbType.String, Where, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_PodWithAttachment", dbParams).ConvertToEntityCollection<Attachment>();

        }

        public string GetPodWithAttachment(PodSearchCondition Searchcondition)
        {
            StringBuilder sb = new StringBuilder();

            #region 查询条件
            if (Searchcondition.StartCityID != null && Searchcondition.StartCityID != 0)
            {
                sb.Append(" and a.StartCityID=" + Searchcondition.StartCityID + " ");
            }

            if (Searchcondition.EndCityID != null && Searchcondition.EndCityID != 0)
            {
                sb.Append(" and a.EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + Searchcondition.EndCityID + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(Searchcondition.EndCities))
                {
                    sb.Append(" and a.EndCityID in (").Append(Searchcondition.EndCities).Append(") ");
                }
            }

            if (Searchcondition.UserType == 1)
            {
                sb.Append(" and a.PODStateID>2 ");
            }


            //当当前用户是内部用户的时候,customerid的一个适应范围

            if (Searchcondition.UserType == 2)
            {
                if (Searchcondition.CustomerID == null || Searchcondition.CustomerID == 0)
                {
                    sb.Append(" and a.CustomerID in (");
                    foreach (long i in Searchcondition.CustomerIDs)
                    {
                        sb.Append(i.ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(") ");
                }
                else
                {
                    sb.Append(" and a.CustomerID=" + Searchcondition.CustomerID + " ");
                }
            }
            else
            {
                if (Searchcondition.CustomerID != null && Searchcondition.CustomerID != 0)
                {
                    sb.Append(" and a.CustomerID=" + Searchcondition.CustomerID + " ");
                }
            }

            if (Searchcondition.Types != null && Searchcondition.Types.Any())
            {
                sb.Append(" and a.Type in (");
                foreach (int type in Searchcondition.Types)
                {
                    sb.Append(type.ToString()).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (Searchcondition.PODStateID != null && Searchcondition.PODStateID != 0)
            {
                sb.Append(" and a.PODStateID=" + Searchcondition.PODStateID + " ");
            }

            if (Searchcondition.TtlOrTplID != null && Searchcondition.TtlOrTplID != 0)
            {
                sb.Append(" and a.TtlOrTplID=" + Searchcondition.TtlOrTplID + " ");
            }

            if (Searchcondition.PODTypeID != null && Searchcondition.PODTypeID != 0)
            {
                sb.Append(" and a.PODTypeID=" + Searchcondition.PODTypeID + " ");
            }

            if (Searchcondition.ShipperTypeID != null && Searchcondition.ShipperTypeID != 0)
            {
                sb.Append(" and a.ShipperTypeID=" + Searchcondition.ShipperTypeID + " ");
            }

            if (Searchcondition.ShipperIDIsNull)
            {
                sb.Append(" and (a.ShipperID is null or a.ShipperID=0) ");
            }
            else
            {
                if (Searchcondition.ShipperID != null && Searchcondition.ShipperID != 0)
                {
                    sb.Append(" and a.ShipperID=" + Searchcondition.ShipperID + " ");
                }
            }

            if (Searchcondition.BoxNumber != null && Searchcondition.BoxNumber != 0)
            {
                sb.Append(" and a.BoxNumber=" + Searchcondition.BoxNumber + " ");
            }

            if (Searchcondition.Weight != null && Searchcondition.Weight != 0)
            {
                sb.Append(" and a.Weight=" + Searchcondition.Weight + " ");
            }

            if (Searchcondition.GoodsNumber != null && Searchcondition.GoodsNumber != 0)
            {
                sb.Append(" and a.GoodsNumber=" + Searchcondition.GoodsNumber + " ");
            }

            if (Searchcondition.Volume != null && Searchcondition.Volume != 0)
            {
                sb.Append(" and a.BoxNumber=" + Searchcondition.Volume + " ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Creator))
            {
                sb.Append(" and a.Creator='" + Searchcondition.Creator + "' ");
            }

            if (Searchcondition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate >= '" + Searchcondition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate <= '" + Searchcondition.EndActualDeliveryDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.CreateTime.HasValue)
            {
                sb.Append(" and a.CreateTime >= '" + Searchcondition.CreateTime.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndCreateTime.HasValue)
            {
                sb.Append(" and a.CreateTime <= '" + Searchcondition.EndCreateTime.Value.DateTimeToString() + " 23:59' ");
            }


            if (!string.IsNullOrEmpty(Searchcondition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (Searchcondition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = Searchcondition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (Searchcondition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = Searchcondition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and a.SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.SystemNumber like '%" + Searchcondition.SystemNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(Searchcondition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (Searchcondition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = Searchcondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (Searchcondition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = Searchcondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {

                    sb.Append(" and a.CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.CustomerOrderNumber like '%" + Searchcondition.CustomerOrderNumber.Trim() + "%' ");
                }
            }


            if (!string.IsNullOrEmpty(Searchcondition.Str1))
            {
                sb.Append(" and a.Str1 like '%" + Searchcondition.Str1 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str2))
            {
                sb.Append(" and a.Str2 like '%" + Searchcondition.Str2 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str3))
            {
                sb.Append(" and a.Str3 like '%" + Searchcondition.Str3 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str4))
            {
                sb.Append(" and a.Str4 like '%" + Searchcondition.Str4 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str5))
            {
                sb.Append(" and a.Str5 like '%" + Searchcondition.Str5 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str6))
            {
                sb.Append(" and a.Str6 like '%" + Searchcondition.Str6 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str7))
            {
                sb.Append(" and a.Str7 like '%" + Searchcondition.Str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str8))
            {
                sb.Append(" and a.Str8 like '%" + Searchcondition.Str8 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str9))
            {
                sb.Append(" and a.Str9 like '%" + Searchcondition.Str9 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str10))
            {
                sb.Append(" and a.Str10 like '%" + Searchcondition.Str10 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str11))
            {
                sb.Append(" and a.Str11 like '%" + Searchcondition.Str11 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str12))
            {
                sb.Append(" and a.Str12 like '%" + Searchcondition.Str12 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str13))
            {
                sb.Append(" and a.Str13 like '%" + Searchcondition.Str13 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str14))
            {
                sb.Append(" and a.Str14 like '%" + Searchcondition.Str14 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str15))
            {
                sb.Append(" and a.Str15 like '%" + Searchcondition.Str15 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str16))
            {
                sb.Append(" and a.Str16 like '%" + Searchcondition.Str16 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str17))
            {
                sb.Append(" and a.Str17 like '%" + Searchcondition.Str17 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str18))
            {
                sb.Append(" and a.Str18 like '%" + Searchcondition.Str18 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str19))
            {
                sb.Append(" and a.Str19 like '%" + Searchcondition.Str19 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str20))
            {
                sb.Append(" and a.Str20 like '%" + Searchcondition.Str20 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str21))
            {
                sb.Append(" and a.Str21 like '%" + Searchcondition.Str21 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str22))
            {
                sb.Append(" and a.Str22 like '%" + Searchcondition.Str22 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str23))
            {
                sb.Append(" and a.Str23 like '%" + Searchcondition.Str23 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str24))
            {
                sb.Append(" and a.Str24 like '%" + Searchcondition.Str24 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str25))
            {
                sb.Append(" and a.Str25 like '%" + Searchcondition.Str25 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str26))
            {
                sb.Append(" and a.Str26 like '%" + Searchcondition.Str26 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str27))
            {
                sb.Append(" and a.Str27 like '%" + Searchcondition.Str27 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str28))
            {
                sb.Append(" and a.Str28 like '%" + Searchcondition.Str28 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str29))
            {
                sb.Append(" and a.Str29 like '%" + Searchcondition.Str29 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str30))
            {
                sb.Append(" and a.Str30 like '%" + Searchcondition.Str30 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str31))
            {
                sb.Append(" and a.Str31 like '%" + Searchcondition.Str31 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str32))
            {
                sb.Append(" and a.Str32 like '%" + Searchcondition.Str32 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str33))
            {
                sb.Append(" and a.Str33 like '%" + Searchcondition.Str33 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str34))
            {
                sb.Append(" and a.Str34 like '%" + Searchcondition.Str34 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str35))
            {
                sb.Append(" and a.Str35 like '%" + Searchcondition.Str35 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str36))
            {
                sb.Append(" and a.Str36 like '%" + Searchcondition.Str36 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str37))
            {
                sb.Append(" and a.Str37 like '%" + Searchcondition.Str37 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str38))
            {
                sb.Append(" and a.Str38 like '%" + Searchcondition.Str38 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str39))
            {
                sb.Append(" and a.Str39 like '%" + Searchcondition.Str39 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str40))
            {
                sb.Append(" and a.Str40 like '%" + Searchcondition.Str40 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str41))
            {
                sb.Append(" and a.Str41 like '%" + Searchcondition.Str41 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str42))
            {
                sb.Append(" and a.Str42 like '%" + Searchcondition.Str42 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str43))
            {
                sb.Append(" and a.Str43 like '%" + Searchcondition.Str43 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str44))
            {
                sb.Append(" and a.Str44 like '%" + Searchcondition.Str44 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str45))
            {
                sb.Append(" and a.Str45 like '%" + Searchcondition.Str45 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str46))
            {
                sb.Append(" and a.Str46 like '%" + Searchcondition.Str46 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str47))
            {
                sb.Append(" and a.Str47 like '%" + Searchcondition.Str47 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str48))
            {
                sb.Append(" and a.Str48 like '%" + Searchcondition.Str48 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str49))
            {
                sb.Append(" and a.Str49 like '%" + Searchcondition.Str49 + "%' ");
            }

            if (!string.IsNullOrEmpty(Searchcondition.Str50))
            {
                sb.Append(" and a.Str50 like '%" + Searchcondition.Str50 + "%' ");
            }

            if (Searchcondition.DateTime1.HasValue)
            {
                sb.Append(" and a.DateTime1 >= '" + Searchcondition.DateTime1.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime1.HasValue)
            {
                sb.Append(" and a.DateTime1 <= '" + Searchcondition.EndDateTime1.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime2.HasValue)
            {
                sb.Append(" and a.DateTime2 >= '" + Searchcondition.DateTime2.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime2.HasValue)
            {
                sb.Append(" and a.DateTime2 <= '" + Searchcondition.EndDateTime2.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime3.HasValue)
            {
                sb.Append(" and a.DateTime3 >= '" + Searchcondition.DateTime3.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime3.HasValue)
            {
                sb.Append(" and a.DateTime3 <= '" + Searchcondition.EndDateTime3.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime4.HasValue)
            {
                sb.Append(" and a.DateTime4 >= '" + Searchcondition.DateTime4.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime4.HasValue)
            {
                sb.Append(" and a.DateTime4 <= '" + Searchcondition.EndDateTime4.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime5.HasValue)
            {
                sb.Append(" and a.DateTime5 >= '" + Searchcondition.DateTime5.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime5.HasValue)
            {
                sb.Append(" and a.DateTime5 <= '" + Searchcondition.EndDateTime5.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime6.HasValue)
            {
                sb.Append(" and a.DateTime6 >= '" + Searchcondition.DateTime6.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime6.HasValue)
            {
                sb.Append(" and a.DateTime6 <= '" + Searchcondition.EndDateTime6.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime7.HasValue)
            {
                sb.Append(" and a.DateTime7 >= '" + Searchcondition.DateTime7.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime7.HasValue)
            {
                sb.Append(" and a.DateTime7 <= '" + Searchcondition.EndDateTime7.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime8.HasValue)
            {
                sb.Append(" and a.DateTime8 >= '" + Searchcondition.DateTime8.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime8.HasValue)
            {
                sb.Append(" and a.DateTime8 <= '" + Searchcondition.EndDateTime8.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime9.HasValue)
            {
                sb.Append(" and a.DateTime9 >= '" + Searchcondition.DateTime9.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime9.HasValue)
            {
                sb.Append(" and a.DateTime9 <= '" + Searchcondition.EndDateTime9.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime10.HasValue)
            {
                sb.Append(" and a.DateTime10 >= '" + Searchcondition.DateTime10.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime10.HasValue)
            {
                sb.Append(" and a.DateTime10 <= '" + Searchcondition.EndDateTime10.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime11.HasValue)
            {
                sb.Append(" and a.DateTime11 >= '" + Searchcondition.DateTime11.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime11.HasValue)
            {
                sb.Append(" and a.DateTime11 <= '" + Searchcondition.EndDateTime11.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime12.HasValue)
            {
                sb.Append(" and a.DateTime12 >= '" + Searchcondition.DateTime12.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime12.HasValue)
            {
                sb.Append(" and a.DateTime12 <= '" + Searchcondition.EndDateTime12.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime13.HasValue)
            {
                sb.Append(" and a.DateTime13 >= '" + Searchcondition.DateTime13.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime13.HasValue)
            {
                sb.Append(" and a.DateTime13 <= '" + Searchcondition.EndDateTime13.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.DateTime15.HasValue)
            {
                sb.Append(" and a.DateTime15 >= '" + Searchcondition.DateTime14.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime15.HasValue)
            {
                sb.Append(" and a.DateTime15 <= '" + Searchcondition.EndDateTime14.Value.DateTimeToString() + " 23:59' ");
            }

            //for Tianjin Hub 查询 
            if (Searchcondition.DateTime14.HasValue)
            {
                sb.Append(" AND CASE a.CustomerID WHEN 1 THEN a.DateTime4 WHEN 8 THEN a.DateTime6 WHEN 2 THEN a.DateTime2 WHEN 7 THEN a.DateTime1 END >= '" + Searchcondition.DateTime14.Value.DateTimeToString() + "' ");
                //sb.Append(" and DateTime15 >= '" + condition.DateTime15.Value.DateTimeToString() + "' ");
            }

            if (Searchcondition.EndDateTime14.HasValue)
            {
                sb.Append(" AND CASE a.CustomerID WHEN 1 THEN a.DateTime4 WHEN 8 THEN a.DateTime6 WHEN 2 THEN a.DateTime2 WHEN 7 THEN a.DateTime1 END <= '" + Searchcondition.EndDateTime14.Value.DateTimeToString() + "  23:59'  ");
                //sb.Append(" and DateTime15 <= '" + condition.EndDateTime15.Value.DateTimeToString() + " 23:59' ");
            }

            if (Searchcondition.IsSettledForCustomer.HasValue)
            {
                sb.Append(" and a.IsSettledForCustomer=" + (Searchcondition.IsSettledForCustomer.Value ? "1" : "0") + " ");
            }

            if (Searchcondition.IsSettledForShipper.HasValue)
            {
                sb.Append(" and a.IsSettledForShipper=" + (Searchcondition.IsSettledForShipper.Value ? "1" : "0") + " ");
            }

            if (Searchcondition.HasShortDial.HasValue)
            {
                sb.Append(" and a.HasShortDial=" + (Searchcondition.HasShortDial.Value ? "1" : "0") + " ");
            }

            if (Searchcondition.HasDistribution.HasValue)
            {
                sb.Append(" and a.HasDistribution=" + (Searchcondition.HasDistribution.Value ? "1" : "0") + " ");
            }

            if (Searchcondition.HasExpress.HasValue)
            {
                sb.Append(" and a.HasExpress=" + (Searchcondition.HasExpress.Value ? "1" : "0") + " ");
            }

            if (Searchcondition.PodMinStateID.HasValue)
            {
                sb.Append(" and a.PodStateID >= " + Searchcondition.PodMinStateID.Value + " ");
            }

            if (Searchcondition.HasAllocateShipper.HasValue)
            {
                if (Searchcondition.HasAllocateShipper.Value)
                {
                    sb.Append(" and (a.ShipperID != 0 or a.ShipperID IS NOT NULL) ");
                }
                else
                {
                    sb.Append(" and (a.ShipperID = 0 OR a.ShipperID IS NULL) ");
                }
            }
            #endregion

            return sb.ToString();
        }
        public IEnumerable<NikeforBSPOD> GetPODDistributionVehicle(NikePodForBSCondition Condition, int PageIndex, int PageSize, out int RowCount)
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

            DataTable dt = base.ExecuteDataTable("Proc_GetPODDistributionVehicle", dbParams);
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
                //sb.Append(" and NOT EXISTS (select top 1 ID from PodStatusLog where Str4 = '" + condition.PodStateName.Trim() + "' and  PodID =p.ID)  ");

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
            if (!string.IsNullOrEmpty(condition.PODCarState))
            {

                switch (condition.PODCarState)
                {
                    case "已分配提货车辆":
                        sb.Append(" and   EXISTS (select top 1 ID from PodStatusLog where Str4 = '提货车辆' and  PodID =p.ID)  ");
                        break;
                    case "未分配提货车辆":
                        sb.Append(" and  NOT EXISTS (select top 1 ID from PodStatusLog where Str4 = '提货车辆' and  PodID =p.ID)  ");
                        break;
                    case "已分配干线车辆":
                        sb.Append(" and   EXISTS (select top 1 ID from PodStatusLog where Str4 = '干线车辆' and  PodID =p.ID)  ");
                        break;
                    case "未分配干线车辆":
                        sb.Append(" and  NOT EXISTS (select top 1 ID from PodStatusLog where Str4 = '干线车辆' and  PodID =p.ID)  ");
                        break;
                    case "已分配配送车辆":
                        sb.Append(" and   EXISTS (select top 1 ID from PodStatusLog where Str4 = '配送车辆' and  PodID =p.ID)  ");
                        break;
                    case "未分配配送车辆":
                        sb.Append(" and  NOT EXISTS (select top 1 ID from PodStatusLog where Str4 = '配送车辆' and  PodID =p.ID)  ");
                        break;
                    default:
                        break;
                }
            }
            //if (condition.IsConversion != null)
            //{
            //    if (condition.IsConversion == 1)
            //    {
            //        sb.Append(" and EXISTS (select top 1 ID from PodStatusLog where PodID =p.ID) ");
            //    }
            //    else
            //    {
            //        sb.Append(" and  NOT EXISTS (select top 1 ID from PodStatusLog where PodID =p.ID) ");
            //    }
            //} 
            return sb.ToString();
        }

        #region 导出百姓网订单报表
        public PodAll BaiXingTrackingReport(PodSearchCondition condition, long projectID, int PageIndex, int PageSize)
        {
            string endCities = string.Empty;
            string sqlWhere = this.GenQueryVFPodWhere(condition, projectID, endCities);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input)//,
            };
            PodAll all = new PodAll();
            DataSet ds = this.ExecuteDataSet("[Proc_BaiXingTrackingReportCustomer]", dbParams);

            all.PodPod = ds.Tables[0].ConvertToEntityCollection<Pod>();
            all.PodExceptions = ds.Tables[1].ConvertToEntityCollection<PodException>();
            //all.PodReplyDocuments = ds.Tables[2].ConvertToEntityCollection<PodReplyDocument>();
            //all.PodFeadBacks = ds.Tables[3].ConvertToEntityCollection<PodFeadBack>();
            return all;
        }
        #endregion

    }
}
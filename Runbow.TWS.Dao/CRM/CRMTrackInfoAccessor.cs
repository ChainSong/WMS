using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao
{
    public class CRMTrackInfoAccessor : BaseAccessor
    {
        public IEnumerable<CRMTrackInfo> GetCRMTrackInfo(CRMTrackInfo Info)
        {
            DbParam[] dbParams = {
                           new DbParam("@SqlWhere",DbType.String,this.GetSqlWhere(Info),ParameterDirection.Input)
                          };

            return base.ExecuteDataTable("Proc_GetCRMTrackInfo", dbParams).ConvertToEntityCollection<CRMTrackInfo>();
        }

        public CRMTrackInfo OperateCRMTrackInfo(CRMTrackInfo TrackInfo)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var CRMTrackList = new List<CRMTrackInfoToDb>();
                CRMTrackList.Add(new CRMTrackInfoToDb(TrackInfo));
                SqlCommand cmd = new SqlCommand("Proc_OperateCRMTrackinfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CRMTrackSOURCE", CRMTrackList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@outputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                cmd.ExecuteNonQuery();
                TrackInfo.ID = Convert.ToInt64(cmd.Parameters[1].Value);
                return TrackInfo;
            }
        }

        public int DeleteCRMTrackInfo(long id)
        {
            DbParam[] dbParams = {
                           new DbParam("@ID",DbType.Int64,id,ParameterDirection.Input),
                           new DbParam("@ReturnVal",DbType.Int32,0,ParameterDirection.Output)
                          };
            base.ExecuteNoQuery("Proc_DeleteCRMTrackInfo", dbParams);
            int returnval = dbParams[1].Value.ObjectToInt32();
            return returnval;
        }

        public string GetSqlWhere(CRMTrackInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("1=1 ");
            if (info.ID != 0)
            {
                sb.Append(" and ID = '" + info.ID + "'");
            }
            if(info.CRMID !=0)
            {
                sb.Append(" and CRMID = '" + info.CRMID + "'");
            }
            return sb.ToString();
        }

        public CRMTrackInfo GetCRMTrackInfoByID(long? ID)
        {
            DbParam[] dbParams = {
                           new DbParam("@ID",DbType.String,ID,ParameterDirection.Input)
                          };

            return base.ExecuteDataTable("[Proc_GetCRMTrackInfoByID]", dbParams).ConvertToEntity<CRMTrackInfo>();
        }
       

        #region NoUse

        //public CRMTrackInfo Items(SqlDataReader sdr)
        //{
        //    CRMTrackInfo info = new CRMTrackInfo();
        //    info.ID = sdr["ID"] != DBNull.Value ? Convert.ToInt64(sdr["ID"]) : Convert.ToInt64(DBNull.Value);
        //    info.CRMID = sdr["CRMID"] != DBNull.Value ? Convert.ToInt64(sdr["CRMID"]) : Convert.ToInt64(DBNull.Value);
        //    info.VisitPeople = sdr["VisitPeople"] != DBNull.Value ? sdr["VisitPeople"].ToString() : DBNull.Value.ToString();
        //    info.VisitTime = sdr["VisitTime"] != DBNull.Value ? sdr["VisitTime"].ToString() : DBNull.Value.ToString();
        //    info.VisitPlace = sdr["VisitPlace"] != DBNull.Value ? sdr["VisitPlace"].ToString() : DBNull.Value.ToString();
        //    info.VisitForm = sdr["VisitForm"] != DBNull.Value ? sdr["VisitForm"].ToString() : DBNull.Value.ToString();
        //    info.GiftsArticles = sdr["GiftsArticles"] != DBNull.Value ? sdr["GiftsArticles"].ToString() : DBNull.Value.ToString();
        //    info.VisitToCustomerEvaluation = sdr["VisitToCustomerEvaluation"] != DBNull.Value ? sdr["VisitToCustomerEvaluation"].ToString() : DBNull.Value.ToString();
        //    info.VisitingPersonnelFeedbackVisit = sdr["VisitingPersonnelFeedbackVisit"] != DBNull.Value ? sdr["VisitingPersonnelFeedbackVisit"].ToString() : DBNull.Value.ToString();
        //    info.ProjectCustomerCommunication = sdr["ProjectCustomerCommunication"] != DBNull.Value ? sdr["ProjectCustomerCommunication"].ToString() : DBNull.Value.ToString();
        //    info.CustomerSupportAndAssistance = sdr["CustomerSupportAndAssistance"] != DBNull.Value ? sdr["CustomerSupportAndAssistance"].ToString() : DBNull.Value.ToString();
        //    info.CreateTime = sdr["CreateTime"] != DBNull.Value ? sdr["CreateTime"].ToString() : DBNull.Value.ToString();
        //    info.UpdateTime = sdr["UpdateTime"] != DBNull.Value ? sdr["UpdateTime"].ToString() : DBNull.Value.ToString();
        //    info.Str1 = sdr["Str1"] != DBNull.Value ? sdr["Str1"].ToString() : DBNull.Value.ToString();
        //    info.Str2 = sdr["Str2"] != DBNull.Value ? sdr["Str2"].ToString() : DBNull.Value.ToString();
        //    info.Str3 = sdr["Str3"] != DBNull.Value ? sdr["Str3"].ToString() : DBNull.Value.ToString();
        //    info.Str4 = sdr["Str4"] != DBNull.Value ? sdr["Str4"].ToString() : DBNull.Value.ToString();
        //    info.Str5 = sdr["Str5"] != DBNull.Value ? sdr["Str5"].ToString() : DBNull.Value.ToString();
        //    info.Str6 = sdr["Str6"] != DBNull.Value ? sdr["Str6"].ToString() : DBNull.Value.ToString();
        //    info.Str7 = sdr["Str7"] != DBNull.Value ? sdr["Str7"].ToString() : DBNull.Value.ToString();
        //    info.Str8 = sdr["Str8"] != DBNull.Value ? sdr["Str8"].ToString() : DBNull.Value.ToString();
        //    return info;
        //}

        #endregion NoUse
    }
}
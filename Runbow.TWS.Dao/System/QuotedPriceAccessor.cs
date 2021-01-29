using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Dao
{
    public class QuotedPriceAccessor : BaseAccessor
    {
        public IEnumerable<SegmentDetail> GetSegmentDetailByCustomerOrShipper(long projectID, int target, long customerOrShipperID, long relatedCustomerID)
        {
            DbParam[] dbParams = {
                             new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                             new DbParam("@Target", DbType.Int32, target, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                             new DbParam("@RelatedCustomerID", DbType.Int64, relatedCustomerID, ParameterDirection.Input)
                             };

            return base.ExecuteDataTable("Proc_GetSegmentDetailByCustomerOrShipper", dbParams).ConvertToEntityCollection<SegmentDetail>();
        }

        public IEnumerable<QuotedPrice> GetQuotedPriceByCondition(long projectID, int target, long? customerOrShipperID, long? transportationLineID,
                                                                  long? shipperTypeID, long? podTypeID, long? ttlOrTplID, DateTime? effectiveStartTime, DateTime? effectiveEndTime, long? startCityID, long? endCityID, long? relatedCustomerID)
        {
            string sqlWhere = this.GenGetQuotedPriceWhere(projectID, target, customerOrShipperID, transportationLineID, shipperTypeID, podTypeID, ttlOrTplID, effectiveStartTime, effectiveEndTime, startCityID, endCityID, relatedCustomerID);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetQuotedPriceByCondition", dbParams).ConvertToEntityCollection<QuotedPrice>();
        }

        public void AddQuotedPrice(IEnumerable<QuotedPrice> quotedPrices)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetQuotedPrice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuotedPrice", quotedPrices.Select(q => new QuotedPriceToDb(q)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddQuotedPrices(IEnumerable<QuotedPrices> quotedPrices)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddQuotedPrice_New", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuotedPrices", quotedPrices.Select(q => new QuotedPricesToDb(q)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public QuotedPrice DeleteQuetedPrice(IEnumerable<long> quotedPriceIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_DeleteQuotedPrices", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QuotedPriceIds", quotedPriceIDs.Select(q => new IdsForInt64(q)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntity<QuotedPrice>();
            }
        }

        public IEnumerable<QuotedPrice> GetAllQuotedPrice()
        {
            return base.ExecuteDataTable("Proc_GetAllQuotedPrice").ConvertToEntityCollection<QuotedPrice>();
        }

        //change by cyf
        public IEnumerable<QuotedPrice> GetQuotedPrice(long projectID, int target, long targetID, long relatedCustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID",DbType.Int64,projectID,ParameterDirection.Input),
                new DbParam("@Target",DbType.Int32,target,ParameterDirection.Input),
                new DbParam("@TargetID",DbType.Int64,targetID,ParameterDirection.Input),
                new DbParam("@RelatedCustomerID",DbType.Int64,relatedCustomerID,ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetQuotedPrice", dbParams).ConvertToEntityCollection<QuotedPrice>();
        }

        private string GenGetQuotedPriceWhere(long projectID, int target, long? customerOrShipperID, long? transportationLineID,
                                                                  long? shipperTypeID, long? podTypeID, long? ttlOrTplID, DateTime? effectiveStartTime, DateTime? effectiveEndTime, long? startCityID, long? endCityID, long? relatedCustomerID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" WHERE ProjectID=" + projectID.ToString() + " AND Target=" + target.ToString());

            if (customerOrShipperID.HasValue)
            {
                sb.Append(" AND TargetID=" + customerOrShipperID.Value.ToString());
            }

            if (transportationLineID.HasValue)
            {
                sb.Append(" AND TransportationLineID=" + transportationLineID.Value.ToString());
            }

            if (shipperTypeID.HasValue)
            {
                sb.Append(" AND ShipperTypeID=" + shipperTypeID.Value.ToString());
            }

            if (podTypeID.HasValue)
            {
                sb.Append(" AND PodTypeID=" + podTypeID.Value.ToString());
            }

            if (ttlOrTplID.HasValue)
            {
                sb.Append(" AND TplOrTtlID=" + ttlOrTplID.Value.ToString());
            }

            if (effectiveStartTime.HasValue)
            {
                sb.Append(" AND EffectiveStartTime >= '" + effectiveStartTime.Value.ToString() + "'");
            }

            if (effectiveEndTime.HasValue)
            {
                sb.Append(" And (EffectiveEndTime IS NULL OR EffectiveEndTime <='" + effectiveEndTime.Value.ToString("yyyy-MM-dd") + " 23:59')");
            }

            if (startCityID.HasValue && startCityID.Value != 0)
            {
                sb.Append(" AND StartCityID=" + startCityID.Value.ToString());
            }

            if (endCityID.HasValue && endCityID.Value != 0)
            {
                //sb.Append(" AND EndCityID=" + endCityID.Value.ToString());
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + endCityID.Value.ToString() + ")) ");
            }

            if (target == 0 && relatedCustomerID.HasValue && relatedCustomerID.Value == 0)
            {
                sb.Append(" AND RelatedCustomerID=0 ");
            }

            if (target == 1 && relatedCustomerID.HasValue && relatedCustomerID.Value != 0)
            {
                sb.Append(" AND RelatedCustomerID=" + relatedCustomerID.Value.ToString());
            }

            return sb.ToString();
        }


        public int GetrelatedCustomerID(long settledType, long customerOrShipperID)
        {
            int i = 0;

            string str = " SELECT Top 1 RelatedCustomerID FROM dbo.QuotedPrice WHERE Target =  " + settledType + "AND TargetID = " + customerOrShipperID + "";

            DataTable dt = base.ExecuteDataTableBySqlString(str, null);

            return i = Convert.ToInt32(dt.Rows[0]["RelatedCustomerID"].ToString());

        }
    }
}
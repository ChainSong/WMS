using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Data;
using System.Data.SqlClient;

namespace Runbow.TWS.Dao
{
    public class SettledAccessor : BaseAccessor
    {
        public SettledPod GetSettledPodByID(long ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetSettledPodByID", dbParams).ConvertToEntity<SettledPod>();
        }

        public void DeleteSettledPod(long ID, int SettledType)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                new DbParam("@SettledType", DbType.Int32, SettledType, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_DeleteSettledPod", dbParams);
        }


        public IEnumerable<SettledPod> GetSettledPodByIDs(IEnumerable<long> IDs)
        {
            if (!IDs.Any())
            {
                return Enumerable.Empty<SettledPod>();
            }

            StringBuilder sb = new StringBuilder();
            IDs.Each((i, id) => { sb.Append(id.ToString()).Append(","); });
            sb.Remove(sb.Length - 1, 1);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetSettledPodByIDs", dbParams).ConvertToEntityCollection<SettledPod>();
        }

        public IEnumerable<SettledPod> GetSettledPodsByInvoiceID(long invoiceID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@InvoiceID", DbType.Int64, invoiceID, ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetSettledPodsByInvoiceID", dbParams).ConvertToEntityCollection<SettledPod>();
        }

        public IEnumerable<SettledPod> GetSettledPodByPodIDs(IEnumerable<long> PodIDs, int SettledType)
        {
            if (!PodIDs.Any())
            {
                return Enumerable.Empty<SettledPod>();
            }

            StringBuilder sb = new StringBuilder();
            PodIDs.Each((i,id) => {sb.Append(id.ToString()).Append(",");});
            sb.Remove(sb.Length-1, 1);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@SettledType", DbType.String, SettledType.ToString(), ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetSettledPodByPodIDs", dbParams).ConvertToEntityCollection<SettledPod>();
        }

        public IEnumerable<SettledPod> GetSettledPodByPodIDsWithNoType(IEnumerable<long> PodIDs)
        {
            if (!PodIDs.Any())
            {
                return Enumerable.Empty<SettledPod>();
            }

            StringBuilder sb = new StringBuilder();
            PodIDs.Each((i,id) => {sb.Append(id.ToString()).Append(",");});
            sb.Remove(sb.Length-1, 1);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetSettledPodByPodIDsWithOutType", dbParams).ConvertToEntityCollection<SettledPod>();
        }

        public void SettlePods(IEnumerable<SettledPod> SettledPods, int SettledType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SettlePods", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPods", SettledPods.Select(p => new SettledPodToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@SettledType", SettledType);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditSettledPod(SettledPod SettledPod, int SettledType, string Updator)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, SettledPod.ID, ParameterDirection.Input),
                new DbParam("@ShipAmt", DbType.Decimal, SettledPod.ShipAmt??0, ParameterDirection.Input),
                new DbParam("@BAFAmt", DbType.Decimal, SettledPod.BAFAmt, ParameterDirection.Input),
                new DbParam("@PointAmt", DbType.Decimal, SettledPod.PointAmt, ParameterDirection.Input),
                new DbParam("@OtherAmt", DbType.Decimal, SettledPod.OtherAmt, ParameterDirection.Input),
                new DbParam("@TotalAmt", DbType.Decimal, SettledPod.TotalAmt, ParameterDirection.Input),
                new DbParam("@Remark", DbType.String, SettledPod.Remark, ParameterDirection.Input),
                new DbParam("@SettledType", DbType.Int32, SettledType, ParameterDirection.Input),
                new DbParam("@Updator", DbType.String, Updator, ParameterDirection.Input)
            };
            base.ExecuteNoQuery("Proc_EditSettledPod", dbParams);
        }

        public IEnumerable<SettledPodAuditHistory> GetSettledHistoryBySettledPodIDs(IEnumerable<long> SettledPodIDs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in SettledPodIDs)
            {
                sb.Append(i.ToString()).Append(",");
            }
            sb.Remove(sb.Length - 1, 1);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetSettledPodAuditHistoryByCondition", dbParams).ConvertToEntityCollection<SettledPodAuditHistory>();
        }

        public IEnumerable<SettledPod> GetSettledPodsByCondition(SettledPodSearchCondition SearchCondition)
        {
            string endCities = string.Empty;
            if (SearchCondition.EndCities != null && SearchCondition.EndCities.IndexOf(',') > 0)
            {
                using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
                {
                    DataTable dtable = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EndCityIDs", SearchCondition.EndCities.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                    Adp.Fill(dtable);
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        endCities += dtable.Rows[i][0].ToString() + ",";
                    }
                    endCities = endCities.Substring(0, endCities.Length - 1);
                }
            }

            string sqlWhere = this.GenGetSettledPodsWhere(SearchCondition, endCities);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetSettledPodsByCondition", dbParams).ConvertToEntityCollection<SettledPod>();
        }

        private string GenGetSettledPodsWhere(SettledPodSearchCondition SearchCondition, string endCities)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" where ProjectID=").Append(SearchCondition.ProjectID).Append(" ");

            sb.Append(" and SettledType=").Append(SearchCondition.SettledType).Append(" ");

            if (!string.IsNullOrEmpty(SearchCondition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (SearchCondition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = SearchCondition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = SearchCondition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
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
                    sb.Append(" and SystemNumber like '%").Append(SearchCondition.SystemNumber.Trim()).Append("%' ");
                }
            }

            if (!string.IsNullOrEmpty(SearchCondition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (SearchCondition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = SearchCondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = SearchCondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
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
                    sb.Append(" and CustomerOrderNumber like '%" + SearchCondition.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            if (SearchCondition.CustomerOrShipperID != null && SearchCondition.CustomerOrShipperID != 0)
            {
                //sb.Append(" and CustomerOrShipperID=").Append(SearchCondition.CustomerOrShipperID).Append(" ");
                sb.Append(" and RelatedCustomerID=").Append(SearchCondition.CustomerOrShipperID).Append(" ");
            }

            if (SearchCondition.CustomerIDs != null && SearchCondition.CustomerIDs.Any())
            {
                sb.Append(" and RelatedCustomerID in (");
                foreach (long i in SearchCondition.CustomerIDs)
                {
                    sb.Append(i.ToString());
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }
            else
            {
                sb.Append(" and RelatedCustomerID=0 ");
            }
           

            if (SearchCondition.PODTypeID != null && SearchCondition.PODTypeID != 0)
            {
                sb.Append(" and PodTypeID=").Append(SearchCondition.PODTypeID).Append(" ");
            }

            if (SearchCondition.ShipperTypeID != null && SearchCondition.ShipperTypeID != 0)
            {
                sb.Append(" and ShipperTypeID=").Append(SearchCondition.ShipperTypeID).Append(" ");
            }

            if (SearchCondition.TtlOrTplID != null && SearchCondition.TtlOrTplID != 0)
            {
                sb.Append(" and TtlOrTplID=").Append(SearchCondition.TtlOrTplID).Append(" ");
            }

            if (SearchCondition.StartCityID != null && SearchCondition.StartCityID != 0)
            {
                sb.Append(" and StartCityID=").Append(SearchCondition.StartCityID).Append(" ");
            }

            if (SearchCondition.EndCityID != null && SearchCondition.EndCityID != 0)
            {
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + SearchCondition.EndCityID + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCities))
                {
                    sb.Append(" and EndCityID in (").Append(endCities).Append(") ");
                }
            }

            if (SearchCondition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate >= '").Append(SearchCondition.ActualDeliveryDate.Value.DateTimeToString()).Append("' ");
            }

            if (SearchCondition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate <= '").Append(SearchCondition.EndActualDeliveryDate.Value.DateTimeToString()).Append(" 23:59' ");
            }

            if (SearchCondition.InvoiceID > 0)
            {
                sb.Append(" and InvoiceID > 0 ");
            }
            else
            {
                sb.Append(" and (InvoiceID=0 or InvoiceID is NULL) ");
            }

            if (SearchCondition.IsAudit.HasValue)
            {
                sb.Append(" and IsAudit=" + (SearchCondition.IsAudit.Value ? "1" : "0") + " ");
            }

            if(!string.IsNullOrEmpty(SearchCondition.SystemNumberSufixx))
            {
                sb.Append(" and SystemNumber like '%" + SearchCondition.SystemNumberSufixx + "'");
            }

            if (SearchCondition.IsManualSettled && SearchCondition.IsForAudit)
            {
                sb.Append(" and Str3='1' ");
            }
            else if(SearchCondition.IsForAudit && !SearchCondition.IsManualSettled)
            {
                sb.Append(" and (Str3='0' OR Str3 IS NULL) ");
            }

            return sb.ToString();
        }

        public void AuditSettledPod(IEnumerable<long> SettledPodIDs, string Auditor, DateTime AuditTime, string AuditRemark, int AuditType, string AuditTypeMessage)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AuditSettledPod", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPodIDs", SettledPodIDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Auditor", Auditor);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@AuditTime", AuditTime);
                cmd.Parameters[2].SqlDbType = SqlDbType.DateTime;
                cmd.Parameters.AddWithValue("@AuditRemark", AuditRemark);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@AuditType", AuditType);
                cmd.Parameters[4].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@AuditTypeMessage", AuditTypeMessage);
                cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAllExtenFeeData(IEnumerable<long> SettledPodIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_DeleteAllExtenFeeData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPodIDs", SettledPodIDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteManualSettledFee(IEnumerable<long> SettledPodIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_DeleteManualSettledFee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPodIDs", SettledPodIDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<SettledPod> BatchUpdateSettledPodAmt(IEnumerable<SettledPod> SettledPods, int SettledType, string Updator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_BatchUpdateSettledPodAmt", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPods", SettledPods.Select(c => new SettledPodToDb(c)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@SettledType", SettledType);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@Updator", Updator);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 50;
                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<SettledPod>();
            }
        }

        public IEnumerable<SettledPod> GetSettledPodByCondition(IEnumerable<string> CustomerOrderNumbers, int SettledType, long CustomerID, long? ShipperID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Where SettledType=").Append(SettledType).Append(" ");
            if(SettledType == 0)
            {
                sb.Append(" AND CustomerOrShipperID=").Append(CustomerID).Append(" ");
            }
            else
            {
                sb.Append(" AND RelatedCustomerID=").Append(CustomerID).Append(" ");
                if(ShipperID.HasValue)
                {
                    sb.Append(" AND CustomerOrShipperID=").Append(ShipperID.Value).Append(" ");
                }
            }

            if(CustomerOrderNumbers != null && CustomerOrderNumbers.Any())
            {
                sb.Append(" AND CustomerOrderNumber IN (");
                CustomerOrderNumbers.Each((i,c)=>{
                    sb.Append("'").Append(c).Append("',");
                });
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
            }

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetSettledPodsByCondition", dbParams).ConvertToEntityCollection<SettledPod>();
        }
    }
}

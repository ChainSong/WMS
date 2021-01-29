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
    public class InvoiceAccessor : BaseAccessor
    {
        public long AddInvoice(IEnumerable<SettledPod> settledPods, Invoice invoice)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<InvoiceToDb> invoices = new List<InvoiceToDb>();
                invoices.Add(new InvoiceToDb(invoice));
                SqlCommand cmd = new SqlCommand("Proc_AddInvoice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPods", settledPods.Select(p => new SettledPodToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Invoices", invoices);
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                conn.Open();
                return cmd.ExecuteScalar().ObjectToInt64();
            }
        }

        public void AddInvoiceAndPayOrders(long SettledPodID, DateTime Date, decimal TotalAmt, string InvoiceNumber, string InvoiceSystemNumber, string ReceiveOrPayOrderNumber, string Remark, string Creator)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SettledPodID", DbType.Int64, SettledPodID, ParameterDirection.Input),
                new DbParam("@Date", DbType.DateTime, Date, ParameterDirection.Input),
                new DbParam("@TotalAmt", DbType.Decimal, TotalAmt, ParameterDirection.Input),
                new DbParam("@InvoiceNumber", DbType.String, InvoiceNumber, ParameterDirection.Input),
                new DbParam("@InvoiceSystemNumber", DbType.String, InvoiceSystemNumber, ParameterDirection.Input),
                new DbParam("@PayOrReceiveNumber", DbType.String, ReceiveOrPayOrderNumber, ParameterDirection.Input),
                new DbParam("@Remark", DbType.String, Remark, ParameterDirection.Input),
                new DbParam("@Creator", DbType.String, Creator, ParameterDirection.Input)
            };

            base.ExecuteNoQuery("Proc_AddInvoiceAndPayOrders", dbParams);
        }

        public Invoice GetInvoiceByID(long id)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetInvoiceByID", dbParams).ConvertToEntity<Invoice>();
        }

        public IEnumerable<Invoice> GetInvoicesByIDs(IEnumerable<long> IDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetInvoicesByIDs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDs", IDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<Invoice> returnInvoices = new List<Invoice>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnInvoices.Add(
                        new Invoice()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                            ProjectID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                            SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            InvoiceNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            InvoiceType = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                            InvoiceTypeName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Target = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                            Sum = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            Remain = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            EstimateDate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                            CustomerOrShipperID = reader.IsDBNull(10) ? 0 : reader.GetInt64(10),
                            CustomerOrShipperName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            TaxID = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            Address = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                            Tel = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            Bank = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            BankAccount = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                            Remark = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                            IsComplete = reader.IsDBNull(18) ? false : reader.GetBoolean(18),
                            State = reader.IsDBNull(19) ? false : reader.GetBoolean(19),
                            Creator = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                            CreateTime = reader.IsDBNull(21) ? DateTime.MinValue : reader.GetDateTime(21),
                            Str1 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                            Str2 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                            Str3 = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            Str4 = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                            Str5 = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                            DateTime1 = reader.IsDBNull(27) ? DateTime.MinValue : reader.GetDateTime(27),
                            DateTime2 = reader.IsDBNull(28) ? DateTime.MinValue : reader.GetDateTime(28),
                            Decimal1 = reader.IsDBNull(29) ? 0 : reader.GetDecimal(29),
                            Decimal2 = reader.IsDBNull(30) ? 0 : reader.GetDecimal(30),
                            Decimal3 = reader.IsDBNull(31) ? 0 : reader.GetDecimal(31)
                        });
                }

                return returnInvoices;
            }
        }

        public IEnumerable<Invoice> GetInvoicesByPodIDs(IEnumerable<long> PodIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetInvoicesByPodIDs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodIDs", PodIDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<Invoice> returnInvoices = new List<Invoice>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnInvoices.Add(
                        new Invoice()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                            ProjectID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                            SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            InvoiceNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            InvoiceType = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                            InvoiceTypeName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Target = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                            Sum = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            Remain = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            EstimateDate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                            CustomerOrShipperID = reader.IsDBNull(10) ? 0 : reader.GetInt64(10),
                            CustomerOrShipperName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            TaxID = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            Address = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                            Tel = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            Bank = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            BankAccount = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                            Remark = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                            IsComplete = reader.IsDBNull(18) ? false : reader.GetBoolean(18),
                            State = reader.IsDBNull(19) ? false : reader.GetBoolean(19),
                            Creator = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                            CreateTime = reader.IsDBNull(21) ? DateTime.MinValue : reader.GetDateTime(21),
                            Str1 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                            Str2 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                            Str3 = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            Str4 = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                            Str5 = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                            DateTime1 = reader.IsDBNull(27) ? DateTime.MinValue : reader.GetDateTime(27),
                            DateTime2 = reader.IsDBNull(28) ? DateTime.MinValue : reader.GetDateTime(28),
                            Decimal1 = reader.IsDBNull(29) ? 0 : reader.GetDecimal(29),
                            Decimal2 = reader.IsDBNull(30) ? 0 : reader.GetDecimal(30),
                            Decimal3 = reader.IsDBNull(31) ? 0 : reader.GetDecimal(31)
                        });
                }

                return returnInvoices;
            }
        }

        public IEnumerable<Invoice> GetInvoicesByCondition(InvoiceSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetInvoicesWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetInvoicesByCondition", dbParams).ConvertToEntityCollection<Invoice>();
        }

        private string GenGetInvoicesWhere(InvoiceSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" where ProjectID=").Append(SearchCondition.ProjectID).Append(" ");

            sb.Append(" and Target=").Append(SearchCondition.Target).Append(" ");

            sb.Append(" and IsComplete=").Append(SearchCondition.IsComplete ? "1" : "0").Append(" ");

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

            if (!string.IsNullOrEmpty(SearchCondition.InvoiceNumber))
            {
                IEnumerable<string> invoiceNumbers = Enumerable.Empty<string>();
                if (SearchCondition.InvoiceNumber.IndexOf("\n") > 0)
                {
                    invoiceNumbers = SearchCondition.InvoiceNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.InvoiceNumber.IndexOf(',') > 0)
                {
                    invoiceNumbers = SearchCondition.InvoiceNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (invoiceNumbers != null && invoiceNumbers.Any())
                {
                    sb.Append(" and InvoiceNumber in ( ");
                    foreach (string s in invoiceNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and InvoiceNumber like '%" + SearchCondition.InvoiceNumber.Trim() + "%' ");
                }
            }

            if (SearchCondition.CustomerOrShipperID.HasValue && SearchCondition.CustomerOrShipperID.Value != 0)
            {
                sb.Append(" and CustomerOrShipperID=").Append(SearchCondition.CustomerOrShipperID).Append(" ");
            }

            if (SearchCondition.ProjectUserCustomerIDs != null && SearchCondition.ProjectUserCustomerIDs.Any())
            {
                sb.Append(" and RelatedCustomerID in ( ");
                foreach (long id in SearchCondition.ProjectUserCustomerIDs)
                {
                    sb.Append(id).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            else
            {
                sb.Append(" and RelatedCustomerID=0 ");
            }
            

            if (SearchCondition.InvoiceType != null && SearchCondition.InvoiceType.Value != 0)
            {
                sb.Append(" and InvoiceType=").Append(SearchCondition.InvoiceType).Append(" ");
            }

            if (SearchCondition.EstimateDate.HasValue)
            {
                sb.Append(" and EstimateDate >= '").Append(SearchCondition.EstimateDate.Value.DateTimeToString()).Append("' ");
            }

            if (SearchCondition.EndEstimateDate.HasValue)
            {
                sb.Append(" and EstimateDate <= '").Append(SearchCondition.EndEstimateDate.Value.DateTimeToString()).Append(" 23:59' ");
            }

            return sb.ToString();
        }

        public void UpdateInvoiceNumber(long id, string invoiceNumber)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
                new DbParam("@InvoiceNumber", DbType.String, invoiceNumber, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_UpdateInvoiceNumber", dbParams);
        }

        public void DeleteInvoice(long ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_DeleteInvoice", dbParams);
        }
    }
}

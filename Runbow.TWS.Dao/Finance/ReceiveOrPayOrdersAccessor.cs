using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Data;
using System.Data.SqlClient;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Dao
{
    public class ReceiveOrPayOrdersAccessor : BaseAccessor
    {
        public void CompleteOrCancelInvoice(long id, bool currentCompleteState)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
                             new DbParam("@CompleteState", DbType.Boolean, !currentCompleteState, ParameterDirection.Input)
                             };

            base.ExecuteNoQuery("Proc_CompleteOrCancelInvoice", dbParams);
        }

        public void DeleteReceiveOrPayOrder(long id)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input)
                             };

            base.ExecuteNoQuery("Proc_DeleteReceiveOrPayOrder", dbParams);
        }

        public long AddReceiveOrPayOrders(ReceiveOrPayOrders receiveOrPayOrder)
        {
            DbParam[] dbParams = {
                             new DbParam("@PayOrReceiveNumber", DbType.String, receiveOrPayOrder.ReceiveOrPayNumber, ParameterDirection.Input),
                             new DbParam("@InvoiceID", DbType.Int64, receiveOrPayOrder.InvoiceID, ParameterDirection.Input),
                             new DbParam("@InvoiceNumber", DbType.String, receiveOrPayOrder.InvoiceNumber, ParameterDirection.Input),
                             new DbParam("@Target", DbType.Int32, receiveOrPayOrder.Target, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, receiveOrPayOrder.CustomerOrShipperID, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperName", DbType.String, receiveOrPayOrder.CustomerOrShipperName, ParameterDirection.Input),
                             new DbParam("@AMT", DbType.Decimal, receiveOrPayOrder.AMT, ParameterDirection.Input),
                             new DbParam("@Date", DbType.DateTime, receiveOrPayOrder.Date, ParameterDirection.Input),
                             new DbParam("@Remark", DbType.String, receiveOrPayOrder.Remark, ParameterDirection.Input),
                             new DbParam("@Creator", DbType.String, receiveOrPayOrder.Creator, ParameterDirection.Input),
                             new DbParam("@CreateTime", DbType.DateTime, receiveOrPayOrder.CreateTime, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, receiveOrPayOrder.Str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, receiveOrPayOrder.Str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, receiveOrPayOrder.Str3, ParameterDirection.Input),
                             new DbParam("@Str4", DbType.String, receiveOrPayOrder.Str4, ParameterDirection.Input),
                             new DbParam("@Str5", DbType.String, receiveOrPayOrder.Str5, ParameterDirection.Input),
                             new DbParam("@DateTime1", DbType.DateTime, receiveOrPayOrder.DateTime1 ?? SqlTypes.SqlDateTime.Null, ParameterDirection.Input),
                             new DbParam("@DateTime2", DbType.DateTime, receiveOrPayOrder.DateTime2 ?? SqlTypes.SqlDateTime.Null, ParameterDirection.Input),
                             new DbParam("@DateTime3", DbType.DateTime, receiveOrPayOrder.DateTime3 ?? SqlTypes.SqlDateTime.Null, ParameterDirection.Input),
                             new DbParam("@Decimal1", DbType.Decimal, receiveOrPayOrder.Decimal1??SqlTypes.SqlDecimal.Null, ParameterDirection.Input),
                             new DbParam("@Decimal2", DbType.Decimal, receiveOrPayOrder.Decimal2??SqlTypes.SqlDecimal.Null, ParameterDirection.Input),
                             new DbParam("@RelatedCustomerID", DbType.Int64, receiveOrPayOrder.RelatedCustomerID??0, ParameterDirection.Input)
                             };

            return base.ExecuteScalar("Proc_AddReceiveOrPayOrders", dbParams).ObjectToInt64();
        }

        public IEnumerable<ReceiveOrPayOrders> GetReceiveOrPayOrderByInvoiceID(long InvoiceID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, InvoiceID, ParameterDirection.Input)
            };
            return base.ExecuteDataTable("Proc_GetReceiveOrPayOrderByInvoiceID", dbParams).ConvertToEntityCollection<ReceiveOrPayOrders>();
        }

        public IEnumerable<ReceiveOrPayOrders> GetReceiveOrPayOrdersByInvoiceIDs(IEnumerable<long> InvoiceIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetReceiveOrPayOrderByInvoiceIDs", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceIDs", InvoiceIDs.Select(p => new IdsForInt64(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                IList<ReceiveOrPayOrders> returnReceiveOrPayOrders = new List<ReceiveOrPayOrders>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnReceiveOrPayOrders.Add(
                        new ReceiveOrPayOrders()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                            ReceiveOrPayNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            InvoiceID = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                            InvoiceNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Target = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            CustomerOrShipperID = reader.IsDBNull(5) ? 0 : reader.GetInt64(5),
                            CustomerOrShipperName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            AMT = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            Date = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                            Remark = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            Creator = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            CreateTime = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11),
                            Str1 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            Str2 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                            Str3 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            Str4 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            Str5 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                            DateTime1 = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                            DateTime2 = reader.IsDBNull(18) ? DateTime.MinValue : reader.GetDateTime(18),
                            Decimal1 = reader.IsDBNull(19) ? 0 : reader.GetDecimal(19),
                            Decimal2 = reader.IsDBNull(20) ? 0 : reader.GetDecimal(20)
                        });
                }

                return returnReceiveOrPayOrders;
            }
        }
    }
}

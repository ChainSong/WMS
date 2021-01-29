using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class InvoiceToDb : SqlDataRecord
    {
        public InvoiceToDb(Invoice invoice)
            : base(s_metadata)
        {
            SetSqlInt64(0, invoice.ID);
            SetSqlInt64(1, invoice.ProjectID);
            SetSqlString(2, invoice.SystemNumber);
            SetSqlString(3, invoice.InvoiceNumber);
            SetSqlInt64(4, invoice.InvoiceType??0);
            SetSqlString(5, invoice.InvoiceTypeName);
            SetSqlInt32(6, invoice.Target);
            SetSqlDecimal(7, invoice.Sum);
            SetSqlDecimal(8, invoice.Remain);
            SetSqlDateTime(9, invoice.EstimateDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt64(10, invoice.CustomerOrShipperID ?? 0);
            SetSqlString(11, invoice.CustomerOrShipperName);
            SetSqlString(12, invoice.TaxID);
            SetSqlString(13, invoice.Address);
            SetSqlString(14, invoice.Tel);
            SetSqlString(15, invoice.Bank);
            SetSqlString(16, invoice.BankAccount);
            SetSqlString(17, invoice.Remark);
            SetSqlBoolean(18, invoice.IsComplete);
            SetSqlBoolean(19, invoice.State);
            SetSqlString(20, invoice.Creator);
            SetSqlDateTime(21, invoice.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(22, invoice.Str1);
            SetSqlString(23, invoice.Str2);
            SetSqlString(24, invoice.Str3);
            SetSqlString(25, invoice.Str4);
            SetSqlString(26, invoice.Str5);
            SetSqlDateTime(27, invoice.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(28, invoice.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDecimal(29, invoice.Decimal1 ?? 0);
            SetSqlDecimal(30, invoice.Decimal2 ?? 0);
            SetSqlDecimal(31, invoice.Decimal3 ?? 0);
            SetSqlInt64(32, invoice.RelatedCustomerID ?? 0);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("InvoiceNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("InvoiceType", SqlDbType.BigInt),
            new SqlMetaData("InvoiceTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Target", SqlDbType.Int),
            new SqlMetaData("Sum", SqlDbType.Decimal,18,2),
            new SqlMetaData("Remain", SqlDbType.Decimal,18,2),
            new SqlMetaData("EstimateDate", SqlDbType.DateTime),
            new SqlMetaData("CustomerOrShipperID", SqlDbType.BigInt),
            new SqlMetaData("CustomerOrShipperName", SqlDbType.NVarChar, 50),
            new SqlMetaData("TaxID", SqlDbType.NVarChar, 50),
            new SqlMetaData("Address",SqlDbType.NVarChar,200),
            new SqlMetaData("Tel",SqlDbType.NVarChar,50),
            new SqlMetaData("Bank",SqlDbType.NVarChar,50),
            new SqlMetaData("BankAccount",SqlDbType.NVarChar,50),
            new SqlMetaData("Remark",SqlDbType.NVarChar,500),
            new SqlMetaData("IsComplete",SqlDbType.Bit),
            new SqlMetaData("State",SqlDbType.Bit),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("Decimal1", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal2", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal3", SqlDbType.Decimal,18,2),
            new SqlMetaData("RelatedCustomerID",SqlDbType.BigInt)
        };
    }
}

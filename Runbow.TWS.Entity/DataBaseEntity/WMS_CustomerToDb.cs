using System.Data;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMS_CustomerToDb : SqlDataRecord
    {
        public WMS_CustomerToDb(WMS_Customer customer)
            : base(s_metadata)
        {
            SetSqlString(0, customer.StorerKey);
            SetSqlString(1, customer.Company);
            SetSqlString(2, customer.ReceiptPrefix);
            SetSqlString(3, customer.OrderPrefix);
            SetSqlString(4, customer.AddressLine1);
            SetSqlString(5, customer.AddressLine2);
            SetSqlString(6, customer.Contact1);
            SetSqlString(7, customer.Contact2);
            SetSqlString(8, customer.PhoneNum1);
            SetSqlString(9, customer.PhoneNum2);
            SetSqlString(10, customer.FaxNum1);
            SetSqlString(11, customer.FaxNum2);
            SetSqlString(12, customer.Email1);
            SetSqlString(13, customer.Email2);
            SetSqlString(14, customer.City);
            SetSqlString(15, customer.Country);
            SetSqlString(16, customer.State);
            SetSqlString(17, customer.CustomerID.ToString());
            SetSqlString(18, customer.UserDef10);
            SetSqlString(19, customer.CompanyCode);
            SetSqlString(20, customer.UserDef2);
            SetSqlString(21, customer.UserDef3);
        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("StorerKey", SqlDbType.NVarChar,50),
            new SqlMetaData("Company", SqlDbType.NVarChar, 500),
            new SqlMetaData("ReceiptPrefix", SqlDbType.NVarChar, 50),
            new SqlMetaData("OrderPrefix", SqlDbType.NVarChar, 50),
            new SqlMetaData("AddressLine1", SqlDbType.NVarChar,500),
            new SqlMetaData("AddressLine2", SqlDbType.NVarChar,500),
            new SqlMetaData("Contact1", SqlDbType.NVarChar,500),
            new SqlMetaData("Contact2", SqlDbType.NVarChar,500),
            new SqlMetaData("PhoneNum1", SqlDbType.NVarChar, 50),
            new SqlMetaData("PhoneNum2", SqlDbType.NVarChar, 50),
            new SqlMetaData("FaxNum1",SqlDbType.NVarChar,50),
            new SqlMetaData("FaxNum2",  SqlDbType.NVarChar, 50),
            new SqlMetaData("Email1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Email2", SqlDbType.NVarChar, 50),
            new SqlMetaData("City", SqlDbType.NVarChar, 50),
            new SqlMetaData("Country", SqlDbType.NVarChar, 50),
            new SqlMetaData("State", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.NVarChar, 50),
            new SqlMetaData("UserDef10", SqlDbType.NVarChar, 50),
            new SqlMetaData("CompanyCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("UserDef2", SqlDbType.NVarChar, 50),
            new SqlMetaData("UserDef3", SqlDbType.NVarChar, 50),
        };
    }
}

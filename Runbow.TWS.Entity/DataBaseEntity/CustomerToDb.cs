using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class CustomerToDb : SqlDataRecord
    {
        public CustomerToDb(Customer customer)
            : base(s_metadata)
        {
            SetSqlInt64(0, customer.ID);
            SetSqlString(1, customer.Code);
            SetSqlString(2, customer.Name);
            SetSqlString(3, customer.Description);
            SetSqlInt32(4, customer.StoreType);
            SetSqlInt32(5, customer.Types);
            SetSqlInt32(6, customer.StoreStatus);
            SetSqlString(7, customer.CreditLine);
            SetSqlString(8, customer.ProvinceCity);
            SetSqlString(9, customer.Remark);
            SetSqlBoolean(10, customer.State);
            SetSqlString(11, customer.Email);
            SetSqlString(12, customer.LawPerson);
            SetSqlString(13, customer.PostCode);
            SetSqlString(14, customer.Address1);
            SetSqlString(15, customer.Address2);
            SetSqlString(16, customer.Bank);
            SetSqlString(17, customer.Account);
            SetSqlString(18, customer.TaxID);
            SetSqlString(19, customer.InvoiceTitle);
            SetSqlString(20, customer.Contactor1);
            SetSqlString(21, customer.Title1);
            SetSqlString(22, customer.Phone1);
            SetSqlString(23, customer.Fax1);
            SetSqlString(24, customer.Contactor2);
            SetSqlString(25, customer.Title2);
            SetSqlString(26, customer.Phone2);
            SetSqlString(27, customer.Fax2);
            SetSqlString(28, customer.WebSite);
            SetSqlString(29, customer.RegistAdd);  
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("Code", SqlDbType.NVarChar, 50),
            new SqlMetaData("Name", SqlDbType.NVarChar, 50),
            new SqlMetaData("Description", SqlDbType.NVarChar, 200),
            new SqlMetaData("StoreType", SqlDbType.Int),
            new SqlMetaData("Types", SqlDbType.Int),
            new SqlMetaData("StoreStatus", SqlDbType.Int),
            new SqlMetaData("CreditLine", SqlDbType.NVarChar, 50),
            new SqlMetaData("ProvinceCity", SqlDbType.NVarChar, 50),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("State",SqlDbType.Bit),
            new SqlMetaData("Email",  SqlDbType.NVarChar, 50),
            new SqlMetaData("LawPerson", SqlDbType.NVarChar, 50),
            new SqlMetaData("PostCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("Address1", SqlDbType.NVarChar, 100),
            new SqlMetaData("Address2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Bank", SqlDbType.NVarChar, 50),
            new SqlMetaData("Account", SqlDbType.NVarChar, 50),
            new SqlMetaData("TaxID", SqlDbType.NVarChar, 50),
            new SqlMetaData("InvoiceTitle", SqlDbType.NVarChar, 50),
            new SqlMetaData("Contactor1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Title1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Phone1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Fax1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Contactor2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Title2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Phone2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Fax2", SqlDbType.NVarChar, 50),
            new SqlMetaData("WebSite", SqlDbType.NVarChar, 100),
            new SqlMetaData("RegistAdd", SqlDbType.NVarChar, 100),
        };
    }
}

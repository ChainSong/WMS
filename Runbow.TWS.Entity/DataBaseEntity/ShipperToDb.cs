using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class ShipperToDb : SqlDataRecord
    {
        public ShipperToDb(Shipper shipper)
            : base(s_metadata)
        {
            SetSqlInt64(0, shipper.ID);
            SetSqlString(1, shipper.Code);
            SetSqlString(2, shipper.Name);
            SetSqlString(3, shipper.EnglishName);
            SetSqlBoolean(4, shipper.IsDangerous ?? SqlTypes.SqlBoolean.Null);
            SetSqlBoolean(5, shipper.IsCustoms ?? SqlTypes.SqlBoolean.Null);
            SetSqlString(6, shipper.Email);
            SetSqlString(7, shipper.LawPerson);
            SetSqlBoolean(8, shipper.IsSupplier ?? SqlTypes.SqlBoolean.Null);
            SetSqlBoolean(9, shipper.IsBalance ?? SqlTypes.SqlBoolean.Null);
            SetSqlString(10, shipper.PostCode);
            SetSqlString(11, shipper.Address1);
            SetSqlString(12, shipper.Address2);
            SetSqlString(13, shipper.Bank);
            SetSqlString(14, shipper.Account);
            SetSqlString(15, shipper.TaxID);
            SetSqlString(16, shipper.InvoiceTitle);
            SetSqlString(17, shipper.Contactor1);
            SetSqlString(18, shipper.Title1);
            SetSqlString(19, shipper.Phone1);
            SetSqlString(20, shipper.Fax1);
            SetSqlString(21, shipper.Contactor2);
            SetSqlString(22, shipper.Title2);
            SetSqlString(23, shipper.Phone2);
            SetSqlString(24, shipper.Fax2);
            SetSqlString(25, shipper.WebSite);
            SetSqlString(26, shipper.RegistAdd);
            SetSqlString(27, shipper.Comment);
            SetSqlString(28, shipper.Creater);
            SetSqlDateTime(29, shipper.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(30, shipper.Updater);
            SetSqlDateTime(31, shipper.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(32, shipper.InsuranceCompany);
            SetSqlString(33, shipper.InsuranceType);
            SetSqlString(34, shipper.InsuranceOrderNo);
            SetSqlDecimal(35, shipper.InsuranceCost ?? SqlTypes.SqlDecimal.Null);
            SetSqlDateTime(36, shipper.InsuranceStartTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(37, shipper.InsuranceEndTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlBoolean(38, shipper.State);
            SetSqlInt64(39, shipper.SegmentID ?? 0);
            SetSqlString(40, shipper.SegmentName);
            SetSqlString(41, shipper.Remark);
            SetSqlString(42, shipper.Str1);
            SetSqlString(43, shipper.Str2);
            SetSqlString(44, shipper.Str3);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("Code", SqlDbType.NVarChar, 50),
            new SqlMetaData("Name", SqlDbType.NVarChar, 50),
            new SqlMetaData("EnglishName", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsDangerous", SqlDbType.Bit),
            new SqlMetaData("IsCustoms", SqlDbType.Bit),
            new SqlMetaData("Email", SqlDbType.NVarChar, 50),
            new SqlMetaData("LawPerson", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsSupplier", SqlDbType.Bit),
            new SqlMetaData("IsBalance", SqlDbType.Bit),
            new SqlMetaData("PostCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("Address1", SqlDbType.NVarChar, 500),
            new SqlMetaData("Address2", SqlDbType.NVarChar, 500),
            new SqlMetaData("Bank", SqlDbType.NVarChar, 100),
            new SqlMetaData("Account", SqlDbType.NVarChar, 50),
            new SqlMetaData("TaxID", SqlDbType.NVarChar, 50),
            new SqlMetaData("InvoiceTitle", SqlDbType.NVarChar, 100),
            new SqlMetaData("Contactor1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Title1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Phone1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Fax1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Contactor2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Title2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Phone2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Fax2", SqlDbType.NVarChar, 50),
            new SqlMetaData("WebSite", SqlDbType.NVarChar, 50),
            new SqlMetaData("RegistAdd", SqlDbType.NVarChar, 100),
            new SqlMetaData("Comment", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creater", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updater", SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("InsuranceCompany", SqlDbType.NVarChar, 50),
            new SqlMetaData("InsuranceType", SqlDbType.NVarChar, 50),
            new SqlMetaData("InsuranceOrderNo", SqlDbType.NVarChar, 50),
            new SqlMetaData("InsuranceCost", SqlDbType.Decimal,18,2),
            new SqlMetaData("InsuranceStartTime", SqlDbType.DateTime),
            new SqlMetaData("InsuranceEndTime", SqlDbType.DateTime),
            new SqlMetaData("State", SqlDbType.Bit),
            new SqlMetaData("SegmentID", SqlDbType.BigInt),
            new SqlMetaData("SegmentName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 500),
        };
    }
}
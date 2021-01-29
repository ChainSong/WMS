using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class SettledPodToDb : SqlDataRecord
    {
        public SettledPodToDb(SettledPod settledPod)
            : base(s_metadata)
        {
            SetSqlInt64(0, settledPod.ID);
            SetSqlInt64(1, settledPod.ProjectID);
            SetSqlString(2, settledPod.SettledNumber);
            SetSqlInt64(3, settledPod.PodID);
            SetSqlString(4, settledPod.SystemNumber);
            SetSqlString(5, settledPod.CustomerOrderNumber);
            SetSqlInt32(6, settledPod.SettledType??0);
            SetSqlInt64(7, settledPod.CustomerOrShipperID??0);
            SetSqlString(8, settledPod.CustomerOrShipperName);
            SetSqlInt64(9, settledPod.StartCityID??0);
            SetSqlString(10, settledPod.StartCityName);
            SetSqlInt64(11, settledPod.EndCityID??0);
            SetSqlString(12, settledPod.EndCityName);
            SetSqlInt64(13, settledPod.ShipperTypeID??0);
            SetSqlString(14, settledPod.ShipperTypeName);
            SetSqlInt64(15, settledPod.PODTypeID??0);
            SetSqlString(16, settledPod.PODTypeName);
            SetSqlInt64(17, settledPod.TtlOrTplID??0);
            SetSqlString(18, settledPod.TtlOrTplName);
            SetSqlDateTime(19, settledPod.ActualDeliveryDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlDouble(20, settledPod.BoxNumber ?? 0);
            SetSqlDouble(21, settledPod.Weight ?? 0);
            SetSqlDouble(22, settledPod.GoodsNumber ?? 0);
            SetSqlDouble(23, settledPod.Volume ?? 0);
            SetSqlString(24, settledPod.Remark);
            SetSqlDecimal(25, settledPod.ShipAmt ?? 0);
            SetSqlDecimal(26, settledPod.BAFAmt ?? 0);
            SetSqlDecimal(27, settledPod.PointAmt ?? 0);
            SetSqlDecimal(28, settledPod.OtherAmt ?? 0);
            SetSqlDecimal(29, settledPod.Amt1 ?? 0);
            SetSqlDecimal(30, settledPod.Amt2 ?? 0);
            SetSqlDecimal(31, settledPod.Amt3 ?? 0);
            SetSqlDecimal(32, settledPod.Amt4 ?? 0);
            SetSqlDecimal(33, settledPod.Amt5 ?? 0);
            SetSqlDecimal(34, settledPod.TotalAmt ?? 0);
            SetSqlString(35, settledPod.Str1);
            SetSqlString(36, settledPod.Str2);
            SetSqlString(37, settledPod.Str3);
            SetSqlString(38, settledPod.Str4);
            SetSqlString(39, settledPod.Str5);
            SetSqlDateTime(40, settledPod.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(41, settledPod.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(42, settledPod.Creator);
            SetSqlDateTime(43, settledPod.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt64(44, settledPod.InvoiceID ?? 0);
            SetSqlInt64(45, settledPod.RelatedCustomerID?? 0);
            SetSqlBoolean(46, settledPod.IsAudit ?? true);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("SettledNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("PodID",SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("SettledType",SqlDbType.Int),
            new SqlMetaData("CustomerOrShipperID", SqlDbType.BigInt),
            new SqlMetaData("CustomerOrShipperName", SqlDbType.NVarChar, 50),
            new SqlMetaData("StartCityID", SqlDbType.BigInt),
            new SqlMetaData("StartCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("EndCityID", SqlDbType.BigInt),
            new SqlMetaData("EndCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ShipperTypeID", SqlDbType.BigInt),
            new SqlMetaData("ShipperTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("PODTypeID", SqlDbType.BigInt),
            new SqlMetaData("PODTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("TtlOrTplID", SqlDbType.BigInt),
            new SqlMetaData("TtlOrTplName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualDeliveryDate", SqlDbType.DateTime),            
            new SqlMetaData("BoxNumber", SqlDbType.Float),
            new SqlMetaData("Weight", SqlDbType.Float),
            new SqlMetaData("GoodsNumber", SqlDbType.Float),
            new SqlMetaData("Volume", SqlDbType.Float),
            new SqlMetaData("Remark",SqlDbType.NVarChar,500),
            new SqlMetaData("ShipAmt",SqlDbType.Decimal,18,2),
            new SqlMetaData("BAFAmt",SqlDbType.Decimal,18,2),
            new SqlMetaData("PointAmt",SqlDbType.Decimal,18,2),
            new SqlMetaData("OtherAmt",SqlDbType.Decimal,18,2),
            new SqlMetaData("Amt1",SqlDbType.Decimal,18,2),
            new SqlMetaData("Amt2",SqlDbType.Decimal,18,2),
            new SqlMetaData("Amt3",SqlDbType.Decimal,18,2),
            new SqlMetaData("Amt4",SqlDbType.Decimal,18,2),
            new SqlMetaData("Amt5",SqlDbType.Decimal,18,2),
            new SqlMetaData("TotalAmt",SqlDbType.Decimal,18,2),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("InvoiceID",SqlDbType.BigInt),
            new SqlMetaData("RelatedCustomerID",SqlDbType.BigInt),
            new SqlMetaData("IsAudit",SqlDbType.Bit)
        };
    }
}

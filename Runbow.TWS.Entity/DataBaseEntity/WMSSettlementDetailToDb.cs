using System.Data;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSSettlementDetailToDb : SqlDataRecord
    {
        public WMSSettlementDetailToDb(SettlementDetail wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.WSID);
            SetSqlInt64(2, wmsInfo.CustomerID);
            SetSqlString(3, wmsInfo.CustomerName);
            SetSqlInt64(4, wmsInfo.WarehouseID);
            SetSqlString(5, wmsInfo.WarehouseName);
            SetSqlString(6, wmsInfo.SettlementNumber);
            SetSqlInt32(7, wmsInfo.WhetherToSettle);
            SetSqlString(8, wmsInfo.LineNumber);
            SetSqlDateTime(9, wmsInfo.OrderDate);
            SetSqlString(10, wmsInfo.TransportatioType);
            SetSqlString(11, wmsInfo.OrderNumber);
            SetSqlString(12, wmsInfo.DeliveryStoreCode);
            SetSqlString(13, wmsInfo.DeliveryStoreName);
            SetSqlString(14, wmsInfo.ReceivingStoreCode);
            SetSqlString(15, wmsInfo.ReceivingStoreName);
            SetSqlDouble(16, wmsInfo.BoxQty);
            SetSqlDouble(17, wmsInfo.Qty);
            SetSqlDouble(18, wmsInfo.SafelockQty);
            SetSqlDouble(19, wmsInfo.HangerQty);
            SetSqlString(20, wmsInfo.Settler);
            SetSqlDateTime(21, wmsInfo.SettlementTime);
            SetSqlString(22, wmsInfo.CancelSettler);
            SetSqlDateTime(23, wmsInfo.CancelSettlementTime);
            SetSqlString(24, wmsInfo.ReClearingSettler);
            SetSqlDateTime(25, wmsInfo.ReClearingTime);
            SetSqlString(26, wmsInfo.Creator);
            SetSqlDateTime(27, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(28, wmsInfo.Updator);
            SetSqlDateTime(29, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(30, wmsInfo.Remark);

            SetSqlString(31, wmsInfo.str1);
            SetSqlString(32, wmsInfo.str2);
            SetSqlString(33, wmsInfo.str3);
            SetSqlString(34, wmsInfo.str4);
            SetSqlString(35, wmsInfo.str5);
            SetSqlString(36, wmsInfo.str6);
            SetSqlString(37, wmsInfo.str7);
            SetSqlString(38, wmsInfo.str8);
            SetSqlString(39, wmsInfo.str9);
            SetSqlString(40, wmsInfo.str10);
            SetSqlString(41, wmsInfo.str11);
            SetSqlString(42, wmsInfo.str12);
            SetSqlString(43, wmsInfo.str13);
            SetSqlString(44, wmsInfo.str14);
            SetSqlString(45, wmsInfo.str15);
            SetSqlString(46, wmsInfo.str16);
            SetSqlString(47, wmsInfo.str17);
            SetSqlString(48, wmsInfo.str18);
            SetSqlString(49, wmsInfo.str19);
            SetSqlString(50, wmsInfo.str20);
            SetSqlDateTime(51, wmsInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(52, wmsInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(53, wmsInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(54, wmsInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(55, wmsInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(56, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(57, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(58, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(59, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(60, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID",SqlDbType.BigInt),
            new SqlMetaData("WSID",SqlDbType.BigInt),
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerName",SqlDbType.NVarChar,50),
            new SqlMetaData("WarehouseID",SqlDbType.BigInt),
            new SqlMetaData("WarehouseName",SqlDbType.NVarChar,50),
            new SqlMetaData("SettlementNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("WhetherToSettle",SqlDbType.Int),
            new SqlMetaData("LineNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("OrderDate",SqlDbType.DateTime),
            new SqlMetaData("TransportatioType",SqlDbType.NVarChar,50),
            new SqlMetaData("OrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("DeliveryStoreCode",SqlDbType.NVarChar,50),
            new SqlMetaData("DeliveryStoreName",SqlDbType.NVarChar,50),
            new SqlMetaData("ReceivingStoreCode",SqlDbType.NVarChar,50),
            new SqlMetaData("ReceivingStoreName",SqlDbType.NVarChar,50),
            new SqlMetaData("BoxQty",SqlDbType.Decimal,18,2),
            new SqlMetaData("Qty",SqlDbType.Decimal,18,2),
            new SqlMetaData("SafelockQty",SqlDbType.Decimal,18,2),
            new SqlMetaData("HangerQty",SqlDbType.Decimal,18,2),
            new SqlMetaData("Settler",SqlDbType.NVarChar,50),
            new SqlMetaData("SettlementTime",SqlDbType.DateTime),
            new SqlMetaData("CancelSettler",SqlDbType.NVarChar,50),
            new SqlMetaData("CancelSettlementTime",SqlDbType.DateTime),
            new SqlMetaData("ReClearingSettler",SqlDbType.NVarChar,50),
            new SqlMetaData("ReClearingTime",SqlDbType.DateTime),
            new SqlMetaData("Creator",SqlDbType.NVarChar,50),
            new SqlMetaData("CreateTime",SqlDbType.DateTime),
            new SqlMetaData("Updator",SqlDbType.NVarChar,50),
            new SqlMetaData("UpdateTime",SqlDbType.DateTime),
            new SqlMetaData("Remark",SqlDbType.NVarChar,500),

            new SqlMetaData("str1",SqlDbType.NVarChar,50),
            new SqlMetaData("str2",SqlDbType.NVarChar,50),
            new SqlMetaData("str3",SqlDbType.NVarChar,50),
            new SqlMetaData("str4",SqlDbType.NVarChar,50),
            new SqlMetaData("str5",SqlDbType.NVarChar,50),
            new SqlMetaData("str6",SqlDbType.NVarChar,50),
            new SqlMetaData("str7",SqlDbType.NVarChar,50),
            new SqlMetaData("str8",SqlDbType.NVarChar,50),
            new SqlMetaData("str9",SqlDbType.NVarChar,50),
            new SqlMetaData("str10",SqlDbType.NVarChar,50),
            new SqlMetaData("str11",SqlDbType.NVarChar,50),
            new SqlMetaData("str12",SqlDbType.NVarChar,50),
            new SqlMetaData("str13",SqlDbType.NVarChar,50),
            new SqlMetaData("str14",SqlDbType.NVarChar,50),
            new SqlMetaData("str15",SqlDbType.NVarChar,50),
            new SqlMetaData("str16",SqlDbType.NVarChar,50),
            new SqlMetaData("str17",SqlDbType.NVarChar,50),
            new SqlMetaData("str18",SqlDbType.NVarChar,50),
            new SqlMetaData("str19",SqlDbType.NVarChar,50),
            new SqlMetaData("str20",SqlDbType.NVarChar,50),
            new SqlMetaData("DateTime1",SqlDbType.DateTime),
            new SqlMetaData("DateTime2",SqlDbType.DateTime),
            new SqlMetaData("DateTime3",SqlDbType.DateTime),
            new SqlMetaData("DateTime4",SqlDbType.DateTime),
            new SqlMetaData("DateTime5",SqlDbType.DateTime),
            new SqlMetaData("Int1",SqlDbType.Int),
            new SqlMetaData("Int2",SqlDbType.Int),
            new SqlMetaData("Int3",SqlDbType.Int),
            new SqlMetaData("Int4",SqlDbType.Int),
            new SqlMetaData("Int5",SqlDbType.Int),
    };
    }
}

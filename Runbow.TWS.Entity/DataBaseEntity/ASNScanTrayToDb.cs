using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using System.Data;
using SqlTypes = System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Receipt;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ASNScanTrayToDb : SqlDataRecord
    {
        public ASNScanTrayToDb(ASNScanTray item)
            : base(s_metas)
        {
            SetSqlInt64(0, item.ID);
            SetSqlString(1, item.ASNNumber);
            SetSqlString(2, item.ExternReceiptNumber);
            SetSqlInt64(3, item.CustomerID.Value);
            SetSqlString(4, item.CustomerName);
            SetSqlInt64(5, item.WarehouseID.Value);
            SetSqlString(6, item.WarehouseName);
            SetSqlString(7, item.TrayNumber);
            SetSqlString(8, item.BoxNumber);
            SetSqlString(9, item.Location);
            SetSqlInt32(10, item.Status.Value);
            SetSqlString(11, item.Creator);
            SetSqlDateTime(12, item.CreateTime ?? DateTime.Now);
            SetSqlString(13, item.Updator);
            SetSqlDateTime(14, item.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(15, item.Str1);
            SetSqlString(16, item.Str2);
            SetSqlString(17, item.Str3);
            SetSqlString(18, item.Str4);
            SetSqlString(19, item.Str5);
            SetSqlString(20, item.Str6);
            SetSqlString(21, item.Str7);
            SetSqlString(22, item.Str8);
            SetSqlString(23, item.Str9);
            SetSqlString(24, item.Str10);
            SetSqlDateTime(25, item.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(26, item.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(27, item.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(28, item.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(29, item.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(30, item.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(31, item.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(32, item.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(33, item.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(34, item.Int5 ?? SqlTypes.SqlInt32.Null);
        }



        private static readonly SqlMetaData[] s_metas = {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ASNNumber", SqlDbType.VarChar, 50),
            new SqlMetaData("ExternReceiptNumber", SqlDbType.VarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.VarChar, 50),
            new SqlMetaData("WarehouseID", SqlDbType.BigInt),
            new SqlMetaData("WarehouseName", SqlDbType.VarChar, 50),
            new SqlMetaData("TrayNumber", SqlDbType.VarChar, 100),
            new SqlMetaData("BoxNumber", SqlDbType.VarChar, 100),
            new SqlMetaData("Location", SqlDbType.VarChar, 100),
            new SqlMetaData("Status", SqlDbType.Int),
            new SqlMetaData("Creator", SqlDbType.VarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator", SqlDbType.VarChar, 50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.VarChar, 50),
            new SqlMetaData("Str2", SqlDbType.VarChar, 50),
            new SqlMetaData("Str3", SqlDbType.VarChar, 50),
            new SqlMetaData("Str4", SqlDbType.VarChar, 50),
            new SqlMetaData("Str5", SqlDbType.VarChar, 50),
            new SqlMetaData("Str6", SqlDbType.VarChar, 50),
            new SqlMetaData("Str7", SqlDbType.VarChar, 50),
            new SqlMetaData("Str8", SqlDbType.VarChar, 50),
            new SqlMetaData("Str9", SqlDbType.VarChar, 50),
            new SqlMetaData("Str10", SqlDbType.VarChar, 50),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("DateTime4", SqlDbType.DateTime),
            new SqlMetaData("DateTime5", SqlDbType.DateTime),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Int3", SqlDbType.Int),
            new SqlMetaData("Int4", SqlDbType.Int),
            new SqlMetaData("Int5", SqlDbType.Int)
        };
    }


}

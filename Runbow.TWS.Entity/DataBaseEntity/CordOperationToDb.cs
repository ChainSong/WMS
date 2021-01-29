using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data;
using Runbow.TWS.Entity.WMS.Log;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class CordOperationToDb : SqlDataRecord
    {
        public CordOperationToDb(WMS_Cord_Operation operation)
            : base(s_metadata)
        {
            SetSqlInt64(0, operation.ID);
            SetSqlInt32(1, operation.ProjectID);
            SetSqlString(2, operation.ProjectName);
            SetSqlInt32(3, operation.CustomerID);
            SetSqlString(4, operation.CustomerName);
            SetSqlInt32(5, operation.WarehouseID);
            SetSqlString(6, operation.WarehouseName);
            SetSqlString(7, operation.MenuName);
            SetSqlString(8, operation.Operation);
            SetSqlString(9, operation.OrderType);
            SetSqlString(10, operation.ExternOrderNumber);
            SetSqlString(11, operation.OrderNumber);
            SetSqlString(12, operation.Controller);
            SetSqlString(13, operation.Remark);
            SetSqlString(14, operation.Creator);
            SetSqlDateTime(15, operation.CreateTime);
            SetSqlString(16, operation.Str1);
            SetSqlString(17, operation.Str2);
            SetSqlString(18, operation.Str3);
            SetSqlString(19, operation.Str4);
            SetSqlString(20, operation.Str5);
            SetSqlInt32(21, operation.Int1);
            SetSqlInt32(22, operation.Int2);
            SetSqlInt32(23, operation.Int3);
            SetSqlString(24, operation.OrderID);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.Int),
            new SqlMetaData("ProjectName", SqlDbType.NVarChar,200),
            new SqlMetaData("CustomerID", SqlDbType.Int),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar,200),
            new SqlMetaData("WarehouseID", SqlDbType.Int),
            new SqlMetaData("WarehouseName", SqlDbType.NVarChar,200),
            new SqlMetaData("MenuName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Operation", SqlDbType.NVarChar, 50),
            new SqlMetaData("OrderType", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExternOrderNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("OrderNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("Controller", SqlDbType.NVarChar, 50),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 200),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Int3", SqlDbType.Int),
            new SqlMetaData("OrderID", SqlDbType.NVarChar,50),
        };
    }
}

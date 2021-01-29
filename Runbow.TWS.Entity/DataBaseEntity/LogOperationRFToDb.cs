using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Log;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class LogOperationRFToDb : SqlDataRecord
    {
        public LogOperationRFToDb(WMS_Log_OperationRF operation)
            : base(s_metadata)
        {
            SetSqlInt64(0, operation.ID);
            SetSqlString(1, operation.LogType);
            SetSqlString(2, operation.ReleateNumber);
            SetSqlString(3, operation.PackageNumber);
            SetSqlString(4, operation.SKU);
            SetSqlString(5, operation.Area);
            SetSqlString(6, operation.Location);
            SetSqlDouble(7, operation.Qty);
            SetSqlString(8, operation.Creator);
            SetSqlDateTime(9, operation.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(10, operation.Updator);
            SetSqlDateTime(11, operation.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(12, operation.Remark);
            SetSqlString(13, operation.Str1);
            SetSqlString(14, operation.Str2);
            SetSqlString(15, operation.Str3);
            SetSqlString(16, operation.Str4);
            SetSqlString(17, operation.Str5);
            SetSqlInt32(18, operation.Int1);
            SetSqlInt32(19, operation.Int2);
            SetSqlInt32(20, operation.Int3);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("LogType", SqlDbType.NVarChar, 50),
            new SqlMetaData("ReleateNumber", SqlDbType.NVarChar, 100),
            new SqlMetaData("PackageNumber", SqlDbType.NVarChar, 100),
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("Area", SqlDbType.NVarChar, 50),
             new SqlMetaData("Location", SqlDbType.NVarChar, 50),
              new SqlMetaData("Qty", SqlDbType.Float),
               new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
                new SqlMetaData("CreateTime", SqlDbType.DateTime),
                 new SqlMetaData("Updator", SqlDbType.NVarChar, 50),
                 new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 200),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Int3", SqlDbType.Int)
        };
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSDeliverToDb : SqlDataRecord
    {
        public WMSDeliverToDb(DeliverHeader wmsInfo)//交接主表
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(1, wmsInfo.DeliverKey);
            SetSqlString(2, wmsInfo.Status);
            SetSqlInt64(3, wmsInfo.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(4, wmsInfo.CustomerName);
            SetSqlInt64(5, wmsInfo.WarehouseID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(6, wmsInfo.WarehouseName);
            SetSqlString(7, wmsInfo.ExpressCompany);
            SetSqlString(8, wmsInfo.Creator);
            SetSqlDateTime(9, wmsInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(10, wmsInfo.Updator);
            SetSqlDateTime(11, wmsInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);

            SetSqlString(12, wmsInfo.str1);
            SetSqlString(13, wmsInfo.str2);
            SetSqlString(14, wmsInfo.str3);
            SetSqlString(15, wmsInfo.str4);
            SetSqlString(16, wmsInfo.str5);

            SetSqlInt32(17, wmsInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(18, wmsInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(19, wmsInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(20, wmsInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(21, wmsInfo.Int5 ?? SqlTypes.SqlInt32.Null);
        }


        private static readonly SqlMetaData[] s_metadata = 
        { 
             new SqlMetaData("ID",SqlDbType.BigInt),           
             new SqlMetaData("DeliverKey",SqlDbType.NVarChar,50), 
             new SqlMetaData("Status",SqlDbType.NVarChar,50), 
             new SqlMetaData("CustomerID",SqlDbType.BigInt), 
             new SqlMetaData("CustomerName",SqlDbType.NVarChar,50), 
             new SqlMetaData("WarehouseID",SqlDbType.BigInt), 
             new SqlMetaData("WarehouseName",SqlDbType.NVarChar,50), 
             new SqlMetaData("ExpressCompany",SqlDbType.NVarChar,50), 
             new SqlMetaData("Creator",SqlDbType.NVarChar,50), 
             new SqlMetaData("CreateTime",SqlDbType.DateTime), 
             new SqlMetaData("Updator",SqlDbType.NVarChar,50), 
             new SqlMetaData("UpdateTime",SqlDbType.DateTime), 

             new SqlMetaData("str1",SqlDbType.NVarChar,50), 
             new SqlMetaData("str2",SqlDbType.NVarChar,50), 
             new SqlMetaData("str3",SqlDbType.NVarChar,50), 
             new SqlMetaData("str4",SqlDbType.NVarChar,50), 
             new SqlMetaData("str5",SqlDbType.NVarChar,50), 

             new SqlMetaData("Int1",SqlDbType.Int), 
             new SqlMetaData("Int2",SqlDbType.Int), 
             new SqlMetaData("Int3",SqlDbType.Int), 
             new SqlMetaData("Int4",SqlDbType.Int), 
             new SqlMetaData("Int5",SqlDbType.Int), 
            
          };
    }
}

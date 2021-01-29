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
    public class WMSDeliverDetailToDb : SqlDataRecord
    {
        public WMSDeliverDetailToDb(DeliverDetail wmsDetail)
            : base(s_metadata)//交接单明细
        {
            SetSqlInt64(0, wmsDetail.ID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(1, wmsDetail.DeliverKey);
            SetSqlInt64(2, wmsDetail.OID ?? SqlTypes.SqlInt64.Null);
            SetSqlInt64(3, wmsDetail.DeliverID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(4, wmsDetail.DeliverDetailKey);
            SetSqlString(5, wmsDetail.OrderNumber);
            SetSqlString(6, wmsDetail.PackBoxKey);
            SetSqlString(7, wmsDetail.ExpressNumber);
            SetSqlString(8, wmsDetail.BoxWeight);
            SetSqlString(9, wmsDetail.Status);
            SetSqlInt64(10, wmsDetail.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(11, wmsDetail.CustomerName);
            SetSqlInt64(12, wmsDetail.WarehouseID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(13, wmsDetail.WarehouseName);
            SetSqlString(14, wmsDetail.Creator);
            SetSqlDateTime(15, wmsDetail.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(16, wmsDetail.Updator);
            SetSqlDateTime(17, wmsDetail.UpdateTime ?? SqlTypes.SqlDateTime.Null);

            SetSqlString(18, wmsDetail.str1);
            SetSqlString(19, wmsDetail.str2);
            SetSqlString(20, wmsDetail.str3);
            SetSqlString(21, wmsDetail.str4);
            SetSqlString(22, wmsDetail.str5);
            SetSqlString(23, wmsDetail.str18);
            SetSqlInt32(24, wmsDetail.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(25, wmsDetail.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(26, wmsDetail.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(27, wmsDetail.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(28, wmsDetail.Int5 ?? SqlTypes.SqlInt32.Null);
        }


        private static readonly SqlMetaData[] s_metadata = 
        {
                          
             new SqlMetaData("ID",SqlDbType.BigInt),           
             new SqlMetaData("DeliverKey",SqlDbType.NVarChar,50), 
             new SqlMetaData("OID",SqlDbType.BigInt), 
             new SqlMetaData("DeliverID",SqlDbType.BigInt), 
             new SqlMetaData("DeliverDetailKey",SqlDbType.NVarChar,50),
             new SqlMetaData("OrderNumber",SqlDbType.NVarChar,50),
             new SqlMetaData("PackBoxKey",SqlDbType.NVarChar,50),
             new SqlMetaData("ExpressNumber",SqlDbType.NVarChar,50),
             new SqlMetaData("BoxWeight",SqlDbType.NVarChar,50),
             new SqlMetaData("Status",SqlDbType.NVarChar,50),
             new SqlMetaData("CustomerID",SqlDbType.BigInt), 
             new SqlMetaData("CustomerName",SqlDbType.NVarChar,50), 
             new SqlMetaData("WarehouseID",SqlDbType.BigInt), 
             new SqlMetaData("WarehouseName",SqlDbType.NVarChar,50),              
             new SqlMetaData("Creator",SqlDbType.NVarChar,50), 
             new SqlMetaData("CreateTime",SqlDbType.DateTime), 
             new SqlMetaData("Updator",SqlDbType.NVarChar,50), 
             new SqlMetaData("UpdateTime",SqlDbType.DateTime), 

             new SqlMetaData("str1",SqlDbType.NVarChar,50), 
             new SqlMetaData("str2",SqlDbType.NVarChar,50), 
             new SqlMetaData("str3",SqlDbType.NVarChar,50), 
             new SqlMetaData("str4",SqlDbType.NVarChar,50), 
             new SqlMetaData("str5",SqlDbType.NVarChar,50), 
             new SqlMetaData("str18",SqlDbType.NVarChar,50), 
             new SqlMetaData("Int1",SqlDbType.Int), 
             new SqlMetaData("Int2",SqlDbType.Int), 
             new SqlMetaData("Int3",SqlDbType.Int), 
             new SqlMetaData("Int4",SqlDbType.Int), 
             new SqlMetaData("Int5",SqlDbType.Int), 
                     
        };

    }
}

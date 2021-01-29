using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class DirectAddInventoryToDb : SqlDataRecord
    {
        public DirectAddInventoryToDb(DirectAddInventory Direct)
            : base(s_metadata)
        {
            SetSqlInt64(0, Direct.Id ?? SqlTypes.SqlInt64.Null);
            SetSqlString(1, Direct.InventoryId);
            SetSqlInt64(2, Direct.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(3, Direct.Customer);
            SetSqlString(4, Direct.Brand);
            SetSqlString(5, Direct.Specifications);
            SetSqlDecimal(6, Direct.Price);
            SetSqlDecimal(7, Direct.GuidePrice);
            SetSqlInt32(8, Direct.Quantity);
            SetSqlDecimal(9, Direct.TotalPrice);
            SetSqlString(10, Direct.Creator);
            SetSqlDateTime(11, Direct.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(12, Direct.Str1);
            SetSqlString(13, Direct.Str2);
            SetSqlString(14, Direct.Str3);
            SetSqlString(15, Direct.Str4);
            SetSqlInt32(16, Direct.InventoryType);
            SetSqlString(17, Direct.BoxNumber);
            SetSqlString(18, Direct.BatchNumber);
            SetSqlDateTime(19, Direct.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(20, Direct.DateTime2 ?? SqlTypes.SqlDateTime.Null);
        }


        private static readonly SqlMetaData[] s_metadata =
        {
          new SqlMetaData("Id", SqlDbType.BigInt),
          new SqlMetaData("InventoryId", SqlDbType.NVarChar,50),
          new SqlMetaData("CustomerID", SqlDbType.BigInt),
          new SqlMetaData("Customer", SqlDbType.NVarChar,50),
          new SqlMetaData("Brand", SqlDbType.NVarChar, 50),
          new SqlMetaData("Specifications", SqlDbType.NVarChar, 50),
          new SqlMetaData("Price", SqlDbType.Decimal,18,2),
          new SqlMetaData("GuidePrice", SqlDbType.Decimal,18,2),
          new SqlMetaData("Quantity", SqlDbType.Int),
          new SqlMetaData("TotalPrice", SqlDbType.Decimal,18,2),
          new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
          new SqlMetaData("CreateTime", SqlDbType.DateTime),
          new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
          new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
          new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
          new SqlMetaData("Str4", SqlDbType.NVarChar, 50),
          new SqlMetaData("InventoryType", SqlDbType.Int) ,
          new SqlMetaData("BoxNumber",SqlDbType.NVarChar, 50),
          new SqlMetaData("BatchNumber", SqlDbType.NVarChar, 50),
          new SqlMetaData("DateTime1", SqlDbType.DateTime),
          new SqlMetaData("DateTime2", SqlDbType.DateTime),
        };
    }
}

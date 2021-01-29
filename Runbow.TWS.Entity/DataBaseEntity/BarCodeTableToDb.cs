using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class BarCodeTableToDb:SqlDataRecord
    {
        public BarCodeTableToDb(BarCodeInfo barcodes)
            : base(s_metadata)
        {
            SetSqlString(0, barcodes.SKU);
            SetSqlString(1, barcodes.BarCode);
            SetSqlString(2, barcodes.Type);
            SetSqlInt64(3, barcodes.OrderID);
            SetSqlString(4, barcodes.OrderNumber);
            SetSqlInt64(5, barcodes.DetailID);
            SetSqlInt64(6, barcodes.CustomerID == null ? 0 : Convert.ToInt64(barcodes.CustomerID));
            SetSqlString(7, barcodes.CustomerName);
            SetSqlInt64(8, barcodes.WarehouseID);
            SetSqlString(9, barcodes.WarehouseName);
            SetSqlString(10, barcodes.Creator);
            SetSqlDateTime(11, DateTime.Now);
            SetSqlString(12, barcodes.Str1);
            SetSqlString(13, barcodes.Str2);
            SetSqlString(14, barcodes.Str3);
            SetSqlString(15, barcodes.Str4);
            SetSqlString(16, barcodes.Str5);
            SetSqlInt64(17, barcodes.BarCodeNumber);
            SetSqlString(18, barcodes.PackageNumber);
            SetSqlInt64(19, barcodes.PackageID);
            SetSqlInt64(20, barcodes.PackageDetailID);
            SetSqlInt64(21, barcodes.Count);
        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("SKU", SqlDbType.NVarChar,200), 
          new SqlMetaData("BarCode",SqlDbType.NVarChar,50),
          new SqlMetaData("Type", SqlDbType.NVarChar,50), 
          new SqlMetaData("OrderID", SqlDbType.BigInt), 
          new SqlMetaData("OrderNumber", SqlDbType.NVarChar,50), 
          new SqlMetaData("DetailID", SqlDbType.BigInt), 
          new SqlMetaData("CustomerID", SqlDbType.BigInt), 
          new SqlMetaData("CustomerName", SqlDbType.NVarChar,50), 
          new SqlMetaData("WarehouseID", SqlDbType.BigInt), 
          new SqlMetaData("WarehouseName", SqlDbType.NVarChar,50), 
          new SqlMetaData("Creator",SqlDbType.NVarChar,50),
          new SqlMetaData("CreateTime",SqlDbType.DateTime),
          new SqlMetaData("Str1",SqlDbType.NVarChar,200),
          new SqlMetaData("Str2",SqlDbType.NVarChar,200),
          new SqlMetaData("Str3",SqlDbType.NVarChar,200),
          new SqlMetaData("Str4",SqlDbType.NVarChar,200),
          new SqlMetaData("Str5",SqlDbType.NVarChar,200),
          new SqlMetaData("BarCodeNumber",SqlDbType.BigInt),
          new SqlMetaData("PackageNumber",SqlDbType.NVarChar,50),
          new SqlMetaData("PackageID",SqlDbType.BigInt),
          new SqlMetaData("PackageDetailID",SqlDbType.BigInt),
          new SqlMetaData("Count", SqlDbType.BigInt), 
};
    }
}

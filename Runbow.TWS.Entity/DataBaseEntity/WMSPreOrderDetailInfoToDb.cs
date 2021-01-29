using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSPreOrderDetailInfoToDb : SqlDataRecord
    {
        public WMSPreOrderDetailInfoToDb(PreOrderDetail preInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, preInfo.ID);
            SetSqlInt64(1, preInfo.POID);
            SetSqlString(2, preInfo.PreOrderNumber);
            SetSqlString(3, preInfo.ExternOrderNumber);
            SetSqlInt64(4, preInfo.CustomerID);
            SetSqlString(5, preInfo.CustomerName);
            SetSqlString(6, preInfo.LineNumber);
            SetSqlInt64(7, preInfo.WarehouseId);
            SetSqlString(8, preInfo.Warehouse);

            SetSqlString(9, preInfo.Area);
            SetSqlString(10, preInfo.Location);

            SetSqlString(11, preInfo.SKU);
            SetSqlString(12, preInfo.UPC);
            SetSqlString(13, preInfo.GoodsName);
            SetSqlString(14, preInfo.GoodsType);
            SetSqlDouble(15, preInfo.OriginalQty);
            SetSqlDouble(16, preInfo.AllocatedQty);
            SetSqlString(17, preInfo.BatchNumber == "" ? null : preInfo.BatchNumber);
            SetSqlString(18, preInfo.Creator);
            SetSqlDateTime(19, preInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(20, preInfo.Updator);
            SetSqlDateTime(21, preInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(22, preInfo.Remark);
            #region 备用字段
            SetSqlString(23, preInfo.str1);
            SetSqlString(24, preInfo.str2);
            SetSqlString(25, preInfo.str3);
            SetSqlString(26, preInfo.str4);
            SetSqlString(27, preInfo.str5);
            SetSqlString(28, preInfo.str6);
            SetSqlString(29, preInfo.str7);
            SetSqlString(30, preInfo.str8);
            SetSqlString(31, preInfo.str9);
            SetSqlString(32, preInfo.str10);
            SetSqlString(33, preInfo.str11);
            SetSqlString(34, preInfo.str12);
            SetSqlString(35, preInfo.str13);
            SetSqlString(36, preInfo.str14);
            SetSqlString(37, preInfo.str15);
            SetSqlString(38, preInfo.str16);
            SetSqlString(39, preInfo.str17);
            SetSqlString(40, preInfo.str18);
            SetSqlString(41, preInfo.str19);
            SetSqlString(42, preInfo.str20);
            SetSqlDateTime(43, preInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(44, preInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(45, preInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(46, preInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(47, preInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(48, preInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(49, preInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(50, preInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(51, preInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(52, preInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            SetSqlString(53, preInfo.BoxNumber == "" ? null : preInfo.BoxNumber);
            SetSqlInt64(54, preInfo.IID);
            SetSqlString(55, preInfo.Unit == "" ? null : preInfo.Unit);
            SetSqlString(56, preInfo.Specifications == "" ? null : preInfo.Specifications);
            #endregion



        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("ID", SqlDbType.BigInt), 
          new SqlMetaData("POID", SqlDbType.BigInt), 
          new SqlMetaData("PreOrderNumber", SqlDbType.NVarChar, 50), 
          new SqlMetaData("ExternOrderNumber", SqlDbType.NVarChar, 50), 
          new SqlMetaData("CustomerID",SqlDbType.BigInt), 
          new SqlMetaData("CustomerName",SqlDbType.NVarChar, 50), 
          new SqlMetaData("LineNumber",SqlDbType.NVarChar, 50), 
          new SqlMetaData("WarehouseId",SqlDbType.BigInt), 
          new SqlMetaData("Warehouse",SqlDbType.NVarChar, 50), 
          new SqlMetaData("Area",SqlDbType.NVarChar, 50), 
          new SqlMetaData("Location",SqlDbType.NVarChar, 50), 
          new SqlMetaData("SKU",SqlDbType.NVarChar, 50), 
          new SqlMetaData("UPC",SqlDbType.NVarChar, 50), 
          new SqlMetaData("GoodsName",SqlDbType.NVarChar, 50),
          new SqlMetaData("GoodsType",SqlDbType.NVarChar, 50),
          new SqlMetaData("OriginalQty",SqlDbType.Float),
          new SqlMetaData("AllocatedQty",SqlDbType.Float), 
          new SqlMetaData("BatchNumber",SqlDbType.NVarChar, 50),
          new SqlMetaData("Creator",SqlDbType.NVarChar, 50),
          new SqlMetaData("CreateTime",SqlDbType.DateTime),
          new SqlMetaData("Updator",SqlDbType.NVarChar, 50),
          new SqlMetaData("UpdateTime",SqlDbType.DateTime),
          new SqlMetaData("Remark",SqlDbType.NVarChar, 500),
          new SqlMetaData("str1",SqlDbType.NVarChar, 50),
          new SqlMetaData("str2",SqlDbType.NVarChar, 50),
          new SqlMetaData("str3",SqlDbType.NVarChar, 50),
          new SqlMetaData("str4",SqlDbType.NVarChar, 50),
          new SqlMetaData("str5",SqlDbType.NVarChar, 50),
          new SqlMetaData("str6",SqlDbType.NVarChar, 50),
          new SqlMetaData("str7",SqlDbType.NVarChar, 50),
          new SqlMetaData("str8",SqlDbType.NVarChar, 50),
          new SqlMetaData("str9",SqlDbType.NVarChar, 50),
          new SqlMetaData("str10",SqlDbType.NVarChar, 50),
          new SqlMetaData("str11",SqlDbType.NVarChar, 50),
          new SqlMetaData("str12",SqlDbType.NVarChar, 50),
          new SqlMetaData("str13",SqlDbType.NVarChar, 50),
          new SqlMetaData("str14",SqlDbType.NVarChar, 50),
          new SqlMetaData("str15",SqlDbType.NVarChar, 50),
          new SqlMetaData("str16",SqlDbType.NVarChar, 200),
          new SqlMetaData("str17",SqlDbType.NVarChar, 200),
          new SqlMetaData("str18",SqlDbType.NVarChar, 200),
          new SqlMetaData("str19",SqlDbType.NVarChar, 500),
          new SqlMetaData("str20",SqlDbType.NVarChar, 500),
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
          new SqlMetaData("BoxNumber",SqlDbType.NVarChar, 200),
          new SqlMetaData("IID",SqlDbType.BigInt),
          new SqlMetaData("Unit",SqlDbType.NVarChar, 100),
          new SqlMetaData("Specifications",SqlDbType.NVarChar, 100)
     };
    }

}

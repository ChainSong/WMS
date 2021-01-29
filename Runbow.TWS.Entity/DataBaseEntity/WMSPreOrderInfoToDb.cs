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
    public class WMSPreOrderInfoToDb : SqlDataRecord
    {
          public WMSPreOrderInfoToDb(PreOrder preInfo)
            : base(s_metadata){
                SetSqlInt64(0, preInfo.ID);
                SetSqlString(1, preInfo.PreOrderNumber);
                SetSqlString(2, preInfo.ExternOrderNumber);
                SetSqlInt64(3, preInfo.CustomerID ?? SqlTypes.SqlInt64.Null);
                SetSqlString(4, preInfo.CustomerName);
                SetSqlString(5, preInfo.Warehouse);
                SetSqlString(6, preInfo.OrderType);
                SetSqlInt32(7, preInfo.Status ?? SqlTypes.SqlInt32.Null);
                SetSqlDateTime(8, preInfo.OrderTime ?? SqlTypes.SqlDateTime.Null);
                SetSqlString(9, preInfo.Province);
                SetSqlString(10, preInfo.City);
                SetSqlString(11, preInfo.District);
                SetSqlString(12, preInfo.Address);
                SetSqlString(13, preInfo.Consignee);
                SetSqlString(14, preInfo.Contact);
                SetSqlString(15, preInfo.ExpressCompany); 
                SetSqlInt32(16, preInfo.DetailCount ?? SqlTypes.SqlInt32.Null);
                SetSqlString(17, preInfo.Creator);
                SetSqlDateTime(18, preInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);
                SetSqlString(19, preInfo.Updator);
                SetSqlDateTime(20, preInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
                SetSqlString(21, preInfo.Remark);
                #region 备用字段
                SetSqlString(22, preInfo.str1);
                SetSqlString(23, preInfo.str2);
                SetSqlString(24, preInfo.str3);
                SetSqlString(25, preInfo.str4);
                SetSqlString(26, preInfo.str5);
                SetSqlString(27, preInfo.str6);
                SetSqlString(28, preInfo.str7);
                SetSqlString(29, preInfo.str8);
                SetSqlString(30, preInfo.str9);
                SetSqlString(31, preInfo.str10);
                SetSqlString(32, preInfo.str11);
                SetSqlString(33, preInfo.str12);
                SetSqlString(34, preInfo.str13);
                SetSqlString(35, preInfo.str14);
                SetSqlString(36, preInfo.str15);
                SetSqlString(37, preInfo.str16);
                SetSqlString(38, preInfo.str17);
                SetSqlString(39, preInfo.str18);
                SetSqlString(40, preInfo.str19);
                SetSqlString(41, preInfo.str20);
                SetSqlDateTime(42, preInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
                SetSqlDateTime(43, preInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
                SetSqlDateTime(44, preInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
                SetSqlDateTime(45, preInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null); 
                SetSqlDateTime(46, preInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
                SetSqlInt32(47, preInfo.Int1 ?? SqlTypes.SqlInt32.Null);
                SetSqlInt32(48, preInfo.Int2 ?? SqlTypes.SqlInt32.Null);
                SetSqlInt32(49, preInfo.Int3 ?? SqlTypes.SqlInt32.Null);
                SetSqlInt32(50, preInfo.Int4 ?? SqlTypes.SqlInt32.Null);
                SetSqlInt32(51, preInfo.Int5 ?? SqlTypes.SqlInt32.Null);
                #endregion

    }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("ID", SqlDbType.BigInt), 
          new SqlMetaData("PreOrderNumber", SqlDbType.NVarChar,50), 
          new SqlMetaData("ExternOrderNumber", SqlDbType.NVarChar,50), 
          new SqlMetaData("CustomerID", SqlDbType.BigInt), 
          new SqlMetaData("CustomerName", SqlDbType.NVarChar,50), 
          new SqlMetaData("Warehouse", SqlDbType.NVarChar,50), 
          new SqlMetaData("OrderType", SqlDbType.NVarChar,50), 
          new SqlMetaData("Status", SqlDbType.Int), 
          new SqlMetaData("OrderTime", SqlDbType.DateTime), 
          new SqlMetaData("Province", SqlDbType.NVarChar,50), 
          new SqlMetaData("City", SqlDbType.NVarChar,50), 
          new SqlMetaData("District", SqlDbType.NVarChar,50), 
          new SqlMetaData("Address", SqlDbType.NVarChar,200), 
          new SqlMetaData("Consignee", SqlDbType.NVarChar,50), 
          new SqlMetaData("Contact", SqlDbType.NVarChar,50), 
          new SqlMetaData("ExpressCompany", SqlDbType.NVarChar,50), 
          new SqlMetaData("DetailCount", SqlDbType.Int), 
          new SqlMetaData("Creator", SqlDbType.NVarChar,50), 
          new SqlMetaData("CreateTime",SqlDbType.DateTime),
          new SqlMetaData("Updator",SqlDbType.NVarChar, 50),
          new SqlMetaData("UpdateTime",SqlDbType.DateTime),
          new SqlMetaData("Remark",SqlDbType.NVarChar, 500),
          #region 备用字段
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
          #endregion 备用字段

              };
    }
}

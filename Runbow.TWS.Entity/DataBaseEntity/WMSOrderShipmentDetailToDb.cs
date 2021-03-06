﻿using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity.DataBaseEntity
{
  public  class WMSOrderShipmentDetailToDb: SqlDataRecord
    {

        public WMSOrderShipmentDetailToDb(WMS_OrderShipmentDetail preInfo)
                : base(s_metadata)
        {
            SetSqlInt64(0, preInfo.ID);
            SetSqlInt64(1, preInfo.SID);
            SetSqlString(2, preInfo.ShipmentNumber);
            SetSqlString(3, preInfo.ExternOrderNumber);
            SetSqlInt64(4, preInfo.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(5, preInfo.CustomerName);
            SetSqlInt64(6, preInfo.WarehouseID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(7, preInfo.WarehouseName);
            
            SetSqlString(8, preInfo.Creator);
            SetSqlDateTime(9, preInfo.CreateTime ?? SqlTypes.SqlDateTime.Null);

            SetSqlString(10, preInfo.Updator);
            SetSqlDateTime(11, preInfo.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(12, preInfo.Remark);
            #region 备用字段
            SetSqlString(13, preInfo.str1);
            SetSqlString(14, preInfo.str2);
            SetSqlString(15, preInfo.str3);
            SetSqlString(16, preInfo.str4);
            SetSqlString(17, preInfo.str5);
            SetSqlString(18, preInfo.str6);
            SetSqlString(19, preInfo.str7);
            SetSqlString(20, preInfo.str8);
            SetSqlString(21, preInfo.str9);
            SetSqlString(22, preInfo.str10);
            SetSqlString(23, preInfo.str11);
            SetSqlString(24, preInfo.str12);
            SetSqlString(25, preInfo.str13);
            SetSqlString(26, preInfo.str14);
            SetSqlString(27, preInfo.str15);
            SetSqlString(28, preInfo.str16);
            SetSqlString(29, preInfo.str17);
            SetSqlString(30, preInfo.str18);
            SetSqlString(31, preInfo.str19);
            SetSqlString(32, preInfo.str20);
            SetSqlDateTime(32, preInfo.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(33, preInfo.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(34, preInfo.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(35, preInfo.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(36, preInfo.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(37, preInfo.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(38, preInfo.Int2 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(39, preInfo.Int3 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(40, preInfo.Int4 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(41, preInfo.Int5 ?? SqlTypes.SqlInt32.Null);
            #endregion

        }

        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("ID", SqlDbType.BigInt),
          new SqlMetaData("SID", SqlDbType.BigInt),
          new SqlMetaData("ShipmentNumber", SqlDbType.NVarChar,50),
          new SqlMetaData("ExternOrderNumber", SqlDbType.NVarChar,50),
          new SqlMetaData("CustomerID", SqlDbType.BigInt),
          new SqlMetaData("CustomerName", SqlDbType.NVarChar,50),
           new SqlMetaData("WarehouseID", SqlDbType.BigInt),
          new SqlMetaData("WarehouseName", SqlDbType.NVarChar,50),
          
          new SqlMetaData("Creator", SqlDbType.NVarChar,50),
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

              };
    }
}

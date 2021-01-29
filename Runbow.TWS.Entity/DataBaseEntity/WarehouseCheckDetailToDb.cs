using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WarehouseCheckDetailToDb : SqlDataRecord
    {
        public WarehouseCheckDetailToDb(WarehouseCheckDetail Check)
            : base(s_metadata)
        {
            SetSqlInt64(0, Check.ID);
            SetSqlInt64(1, Check.CID);
            SetSqlString(2, Check.CheckNumber);
            SetSqlString(3, Check.ExternNumber);
            SetSqlInt64(4, Check.CustomerID);
            SetSqlString(5, Check.CustomerName);
            SetSqlString(6, Check.Warehouse);
            SetSqlString(7, Check.Area);
            SetSqlString(8, Check.Location);
            SetSqlString(9, Check.SKU);
            SetSqlString(10, Check.UPC);
            SetSqlString(11, Check.BatchNumber);
            SetSqlString(12, Check.BoxNumber);
            SetSqlString(13, Check.Unit);
            SetSqlString(14, Check.Specifications);
            SetSqlString(15, Check.GoodsType);
            SetSqlDecimal(16, Check.CheckQty);
            SetSqlDecimal(17, Check.ActualQty);
            SetSqlString(18, Check.IS_Difference);
            SetSqlString(19, Check.IS_Deal);
            SetSqlString(20, Check.Remark);
            SetSqlString(21, Check.Creator);
            SetSqlDateTime(22, Check.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(23, Check.Updator);
            SetSqlDateTime(24, Check.UpdateTime ?? SqlTypes.SqlDateTime.Null);
            //SetSqlString(25, Check.str1);
            //SetSqlString(26, Check.str2);
            //SetSqlString(27, Check.str3);
            //SetSqlString(28, Check.str4);
            //SetSqlString(29, Check.str5);
            //SetSqlDateTime(30, Check.DataTime1);
            //SetSqlDateTime(31, Check.DataTime2);
            //SetSqlDateTime(32, Check.DateTime3);
            //SetSqlDateTime(33, Check.DateTime4);
            //SetSqlDateTime(34, Check.DateTime5);
            //SetSqlInt32(35, Check.int1);
            //SetSqlInt32(36, Check.int2);
            //SetSqlInt32(37, Check.int3);
            //SetSqlInt32(38, Check.int4);
            //SetSqlInt32(39, Check.int5); 



        }


        private static readonly SqlMetaData[] s_metadata =
        { 
             new SqlMetaData("ID", SqlDbType.BigInt), 
            new SqlMetaData("CID", SqlDbType.BigInt), 
            new SqlMetaData("CheckNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("ExternNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar,50),
            new SqlMetaData("Warehouse", SqlDbType.NVarChar,50),
            new SqlMetaData("Area", SqlDbType.NVarChar,50),
            new SqlMetaData("Location", SqlDbType.NVarChar,50),
            new SqlMetaData("SKU", SqlDbType.NVarChar,50),
            new SqlMetaData("UPC", SqlDbType.NVarChar,50),
            new SqlMetaData("BatchNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("BoxNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("Unit", SqlDbType.NVarChar,50),
            new SqlMetaData("Specifications", SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsType", SqlDbType.NVarChar,50),
            new SqlMetaData("CheckQty", SqlDbType.Decimal,18,2),
            new SqlMetaData("ActualQty", SqlDbType.Decimal,18,2),
            new SqlMetaData("IS_Difference", SqlDbType.Char,10),
            new SqlMetaData("IS_Deal", SqlDbType.Char,10),
            new SqlMetaData("Remark", SqlDbType.VarChar,200),
            new SqlMetaData("Creator", SqlDbType.VarChar,50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator", SqlDbType.VarChar,50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime), 
                              
                              
        };
    }
}









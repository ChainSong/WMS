using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{

    public class ScanToDb : SqlDataRecord
    {
        public ScanToDb(ScanInfo info)
            : base(s_metadata)
        {
            SetSqlInt64(0, info.ID);
            SetSqlString(1, info.CustomerOrderNumber);
            SetSqlInt64(2, info.BoxNumber);
            SetSqlInt64(3, info.ScanBoxNumber);
            SetSqlString(4, info.TrailerNo);
            SetSqlString(5, info.PlateNumber);
            SetSqlInt64(6, info.ShipperID);
            SetSqlString(7, info.Shipper);
            SetSqlInt32(8, info.CloseFlag);
            SetSqlInt32(9, info.CompleteFlag);
            SetSqlString(10, info.Operator);
            SetSqlDateTime(11, info.OperateTime);
            SetSqlString(12, info.Creater);
            SetSqlDateTime(13, info.CreateTime);
            SetSqlString(14, info.Modifier);
            SetSqlDateTime(15, info.ModifyTime);
            SetSqlString(16, info.Remark);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderN",SqlDbType.VarChar, 100),
            new SqlMetaData("BoxNumber",SqlDbType.BigInt),
            new SqlMetaData("ScanBoxNumber",SqlDbType.BigInt),
            new SqlMetaData("TrailerNo",SqlDbType.VarChar,200),
            new SqlMetaData("PlateNumber",SqlDbType.NVarChar,200),
            new SqlMetaData("ShipperID",SqlDbType.BigInt),
            new SqlMetaData("Shipper", SqlDbType.NVarChar,100),
           new SqlMetaData("CloseFlag",SqlDbType.Int),
            new SqlMetaData("CompleteFlag",SqlDbType.Int),
            new SqlMetaData("Operator", SqlDbType.VarChar,100),
               new SqlMetaData("OperateTime", SqlDbType.DateTime),
              new SqlMetaData("Creater",SqlDbType.NVarChar,100),
            new SqlMetaData("CreateTime",SqlDbType.DateTime),
            new SqlMetaData("Modifier", SqlDbType.NVarChar,100),
            new SqlMetaData("ModifyTime", SqlDbType.DateTime),
             new SqlMetaData("Remark", SqlDbType.NVarChar,1000)
        };
    }
}

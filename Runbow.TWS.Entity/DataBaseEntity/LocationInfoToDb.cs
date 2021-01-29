using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
namespace Runbow.TWS.Entity
{
    public class LocationInfoToDb : SqlDataRecord
    {
        public LocationInfoToDb(LocationInfo info)
            : base(s_metadata)
        {
            SetSqlInt64(0, info.ID);
            SetSqlInt64(1, info.WarehouseID);
            SetSqlInt64(2, info.AreaID);
            SetSqlString(3, info.Location);
            SetSqlString(4, info.LocationStatus);
            SetInt32(5, info.LocationType);
            SetInt32(6, info.Classification);
            SetInt32(7, info.Category);
            SetInt32(8, info.Handling);
            SetSqlString(9, info.ABCClassification);
            SetBoolean(10, info.IsMultiLot);
            SetBoolean(11, info.IsMultiSKU);
            SetSqlString(12, info.LocationLevel);
            SetSqlString(13, info.GoodsPutOrder);
            SetSqlString(14, info.GoodsPickOrder);
            SetSqlString(15, info.Volume);
            SetSqlString(16, info.Weight);
            SetSqlString(17, info.MaxID);
            SetSqlString(18, info.MaxNumber == "" ? null : info.MaxNumber);
            SetSqlString(19, info.Length);
            SetSqlString(20, info.Width);
            SetSqlString(21, info.Height);
            SetSqlString(22, info.X_Coordinate);
            SetSqlString(23, info.Y_Coordinate);
            SetSqlString(24, info.Z_coordinate);
            SetSqlString(25, info.Remark);
            SetSqlString(26, info.Creator);
            SetSqlInt64(27, info.GoodsShelfID);
            SetSqlInt64(28, info.LevelsNumber);
            SetSqlInt64(29, info.SerialNumber);
            SetSqlString(30, info.GoodsShelvesName);
            SetInt32(31, info.Int1);
            SetSqlString(32, info.Str1);
            SetSqlString(33, info.Str2);
            SetSqlString(34, info.Str3);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("WarehouseID", SqlDbType.BigInt),
            new SqlMetaData("AreaID", SqlDbType.BigInt),
            new SqlMetaData("Location",SqlDbType.VarChar, 50),
            new SqlMetaData("LocationStatus",SqlDbType.NVarChar,50),
            new SqlMetaData("LocationType",SqlDbType.Int),
            new SqlMetaData("Classification",SqlDbType.Int),
            new SqlMetaData("Category",SqlDbType.Int),
            new SqlMetaData("Handling",SqlDbType.Int),
            new SqlMetaData("ABCClassification", SqlDbType.NVarChar,50),
            new SqlMetaData("IsMultiLot",SqlDbType.Bit),
            new SqlMetaData("IsMultiSKU",SqlDbType.Bit),
            new SqlMetaData("LocationLevel", SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsPutOrder", SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsPickOrder",SqlDbType.NVarChar,50),
            new SqlMetaData("Volume",SqlDbType.NVarChar,50),
            new SqlMetaData("Weight", SqlDbType.NVarChar,50),
            new SqlMetaData("MaxID", SqlDbType.NVarChar,50),
            new SqlMetaData("MaxNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("Length",SqlDbType.NVarChar,50),
            new SqlMetaData("Width", SqlDbType.NVarChar,50),
            new SqlMetaData("Height", SqlDbType.NVarChar,50),
            new SqlMetaData("X_Coordinate", SqlDbType.NVarChar,50),
            new SqlMetaData("Y_Coordinate", SqlDbType.NVarChar,50),
            new SqlMetaData("Z_coordinate", SqlDbType.NVarChar,50),
            new SqlMetaData("Remark", SqlDbType.NVarChar,500),
            new SqlMetaData("Creator", SqlDbType.NVarChar,50),
            new SqlMetaData("GoodsShelfID", SqlDbType.BigInt),
             new SqlMetaData("LevelsNumber", SqlDbType.BigInt),
              new SqlMetaData("SerialNumber", SqlDbType.BigInt),
               new SqlMetaData("GoodsShelvesName", SqlDbType.NVarChar,50),
               new SqlMetaData("Int1",SqlDbType.Int),
               new SqlMetaData("Str1", SqlDbType.NVarChar,50),
               new SqlMetaData("Str2", SqlDbType.NVarChar,50),
               new SqlMetaData("Str3", SqlDbType.NVarChar,50)
        };
    }
}

using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodDetailToDb : SqlDataRecord
    {
        public PodDetailToDb(PodDetail podDetail)
            : base(s_metadata)
        {
            SetSqlInt64(0, podDetail.ID);
            SetSqlInt64(1, podDetail.PodID);
            SetSqlString(2, podDetail.SystemNumber);
            SetSqlString(3, podDetail.CustomerOrderNumber);
            SetSqlString(4, podDetail.GoodCode);
            SetSqlString(5, podDetail.GoodName);
            SetSqlString(6, podDetail.UnitCode);
            SetSqlString(7, podDetail.UnitName);
            SetSqlString(8, podDetail.ExpectedAmount);
            SetSqlString(9, podDetail.ActualAmount);
            SetSqlString(10, podDetail.Remark);
            SetSqlString(11, podDetail.Creator);
            SetSqlDateTime(12, podDetail.CreateTime);
            SetSqlString(13, podDetail.Str1);
            SetSqlString(14, podDetail.Str2);
            SetSqlString(15, podDetail.Str3);
            SetSqlString(16, podDetail.Str4);
            SetSqlString(17, podDetail.Str5);
            SetSqlDateTime(18, podDetail.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(19, podDetail.Str6);
            SetSqlString(20, podDetail.Str7);
            SetSqlString(21, podDetail.Str8);
            SetSqlString(22, podDetail.Str9);
            SetSqlString(23, podDetail.Str10);
            SetSqlString(24, podDetail.Str11);
            SetSqlString(25, podDetail.Str12);
            SetSqlString(26, podDetail.Str13);
            SetSqlString(27, podDetail.Str14);
            SetSqlString(28, podDetail.Str15);
            SetSqlString(29, podDetail.Str16);
            SetSqlString(30, podDetail.Str17);
            SetSqlString(31, podDetail.Str18);
            SetSqlString(32, podDetail.Str19);
            SetSqlString(33, podDetail.Str20);
            SetSqlDateTime(34, podDetail.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(35, podDetail.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(36, podDetail.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(37, podDetail.DateTime5 ?? SqlTypes.SqlDateTime.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("PodID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodNameName", SqlDbType.NVarChar, 50),
            new SqlMetaData("UnitCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("UnitName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpectedAmount", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualAmount", SqlDbType.NVarChar, 50),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 100),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str11", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str12", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str13", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str14", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str15", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str16", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str17", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str18", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str19", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str20", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("DateTime4", SqlDbType.DateTime),
            new SqlMetaData("DateTime5", SqlDbType.DateTime)
        };
    }
}
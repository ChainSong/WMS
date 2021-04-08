using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMS_SFDetailToDb : SqlDataRecord
    {
        public WMS_SFDetailToDb(WMS_SFDetail sd)
            : base(s_metadata)
        {
            SetSqlInt64(0, sd.ID);
            SetSqlInt64(1, sd.OID ?? 0);
            SetSqlString(2, sd.OrderNumber);
            SetSqlString(3, sd.waybillNo);
            SetSqlString(4, sd.sourceTransferCode);
            SetSqlString(5, sd.sourceCityCode);
            SetSqlString(6, sd.sourceDeptCode);
            SetSqlString(7, sd.sourceTeamCode);
            SetSqlString(8, sd.destCityCode);
            SetSqlString(9, sd.destDeptCode);
            SetSqlString(10, sd.destDeptCodeMapping);
            SetSqlString(11, sd.destTeamCode);
            SetSqlString(12, sd.destTeamCodeMapping);
            SetSqlString(13, sd.destTransferCode);
            SetSqlString(14, sd.destRouteLabel);
            SetSqlString(15, sd.proName);
            SetSqlString(16, sd.cargoTypeCode);
            SetSqlString(17, sd.limitTypeCode);
            SetSqlString(18, sd.expressTypeCode);
            SetSqlString(19, sd.codingMapping);
            SetSqlString(20, sd.codingMappingOut);
            SetSqlString(21, sd.xbFlag);
            SetSqlString(22, sd.printFlag);
            SetSqlString(23, sd.twoDimensionCode);
            SetSqlString(24, sd.proCode);
            SetSqlString(25, sd.printIcon);
            SetSqlString(26, sd.abFlag);
            SetSqlString(27, sd.destPortCode);
            SetSqlString(28, sd.destCountry);
            SetSqlString(29, sd.destPostCode);
            SetSqlString(30, sd.goodsValueTotal);
            SetSqlString(31, sd.currencySymbol);
            SetSqlString(32, sd.goodsNumber);
            SetSqlString(33, sd.checkCode);           
            SetSqlString(34, sd.str1);
            SetSqlString(35, sd.str2);
            SetSqlString(36, sd.str3);
            SetSqlString(37, sd.str4);
            SetSqlString(38, sd.str5);
            SetSqlString(39, sd.str6);
            SetSqlString(40, sd.str7);
            SetSqlString(41, sd.str8);
            SetSqlString(42, sd.str9);
            SetSqlString(43, sd.str10);
            SetSqlString(44, sd.Creator);
            SetSqlDateTime(45, sd.CreateTime ?? SqlTypes.SqlDateTime.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("OID", SqlDbType.BigInt),
            new SqlMetaData("OrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("waybillNo", SqlDbType.NVarChar, 200),
            new SqlMetaData("sourceTransferCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("sourceCityCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("sourceDeptCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("sourceTeamCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destCityCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destDeptCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destDeptCodeMapping", SqlDbType.NVarChar, 200),
            new SqlMetaData("destTeamCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destTeamCodeMapping", SqlDbType.NVarChar, 200),
            new SqlMetaData("destTransferCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destRouteLabel", SqlDbType.NVarChar, 500),
            new SqlMetaData("proName", SqlDbType.NVarChar, 200),
            new SqlMetaData("cargoTypeCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("limitTypeCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("expressTypeCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("codingMapping", SqlDbType.NVarChar, 200),
            new SqlMetaData("codingMappingOut", SqlDbType.NVarChar, 200),
            new SqlMetaData("xbFlag", SqlDbType.NVarChar, 200),
            new SqlMetaData("printFlag", SqlDbType.NVarChar, 200),
            new SqlMetaData("twoDimensionCode", SqlDbType.NVarChar, 1000),
            new SqlMetaData("proCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("printIcon", SqlDbType.NVarChar, 200),
            new SqlMetaData("abFlag", SqlDbType.NVarChar, 200),
            new SqlMetaData("destPortCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("destCountry", SqlDbType.NVarChar, 200),
            new SqlMetaData("destPostCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("goodsValueTotal", SqlDbType.NVarChar, 200),
            new SqlMetaData("currencySymbol", SqlDbType.NVarChar, 200),
            new SqlMetaData("goodsNumber", SqlDbType.NVarChar, 200),
            new SqlMetaData("checkCode", SqlDbType.NVarChar, 200),
            new SqlMetaData("str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("str4", SqlDbType.NVarChar, 50),
            new SqlMetaData("str5", SqlDbType.NVarChar, 50),
            new SqlMetaData("str6", SqlDbType.NVarChar, 100),
            new SqlMetaData("str7", SqlDbType.NVarChar, 100),
            new SqlMetaData("str8", SqlDbType.NVarChar, 200),
            new SqlMetaData("str9", SqlDbType.NVarChar, 200),
            new SqlMetaData("str10", SqlDbType.NVarChar, 500),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
        };
    }
}

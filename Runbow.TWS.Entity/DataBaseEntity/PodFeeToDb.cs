using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodFeeToDb : SqlDataRecord
    {
        public PodFeeToDb(PodFee podFee)
            : base(s_metadata)
        {
            SetSqlInt64(0, podFee.ID);
            SetSqlInt64(1, podFee.PodID);
            SetSqlString(2, podFee.SystemNumber);
            SetSqlString(3, podFee.CustomerOrderNumber);
            SetSqlString(4, podFee.Creator);
            SetSqlDateTime(5, podFee.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(6, podFee.Str1);
            SetSqlString(7, podFee.Str2);
            SetSqlString(8, podFee.Str3);
            SetSqlString(9, podFee.Str4);
            SetSqlString(10, podFee.Str5);
            SetSqlString(11, podFee.Str6);
            SetSqlString(12, podFee.Str7);
            SetSqlString(13, podFee.Str8);
            SetSqlString(14, podFee.Str9);
            SetSqlString(15, podFee.Str10);
            SetSqlDateTime(16, podFee.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(17, podFee.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(18, podFee.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(19, podFee.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(20, podFee.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDecimal(21, podFee.Decimal1 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(22, podFee.Decimal2 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(23, podFee.Decimal3 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(24, podFee.Decimal4 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(25, podFee.Decimal5 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(26, podFee.Decimal6 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(27, podFee.Decimal7 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(28, podFee.Decimal8 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(29, podFee.Decimal9 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(30, podFee.Decimal10 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(31, podFee.Decimal11 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(32, podFee.Decimal12 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(33, podFee.Decimal13 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(34, podFee.Decimal14 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(35, podFee.Decimal15 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(36, podFee.Decimal16 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(37, podFee.Decimal17 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(38, podFee.Decimal18 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(39, podFee.Decimal18 ?? SqlTypes.SqlDecimal.Null);
            SetSqlDecimal(40, podFee.Decimal20 ?? SqlTypes.SqlDecimal.Null);
            SetSqlInt32(41, podFee.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(42, podFee.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlInt32(43, podFee.Int1 ?? SqlTypes.SqlInt32.Null);
            SetSqlBoolean(44, podFee.Bit1 ?? SqlTypes.SqlBoolean.Null);
            SetSqlBoolean(45, podFee.Bit1 ?? SqlTypes.SqlBoolean.Null);
            SetSqlBoolean(46, podFee.Bit1 ?? SqlTypes.SqlBoolean.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("PodID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 200),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("DateTime4", SqlDbType.DateTime),
            new SqlMetaData("DateTime5", SqlDbType.DateTime),
            new SqlMetaData("Decimal1", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal2", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal3", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal4", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal5", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal6", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal7", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal8", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal9", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal10", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal11", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal12", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal13", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal14", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal15", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal16", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal17", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal18", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal19", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal20", SqlDbType.Decimal,18,2),
            new SqlMetaData("Int1",SqlDbType.Int),
            new SqlMetaData("Int2",SqlDbType.Int),
            new SqlMetaData("Int3",SqlDbType.Int),
            new SqlMetaData("Bit1",SqlDbType.Bit),
            new SqlMetaData("Bit2",SqlDbType.Bit),
            new SqlMetaData("Bit3",SqlDbType.Bit)
        };
    }
}

using System.Data;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.POD.Distribution;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class SettledPodDistributionToDb : SqlDataRecord
    {
        public SettledPodDistributionToDb(SettlePodDistribution settlePodDistribution)
            : base(s_metadata)
        {
            SetSqlInt64(0, settlePodDistribution.FeePodID);
            SetSqlString(1, settlePodDistribution.FeeSystemNumber);
            SetSqlString(2, settlePodDistribution.FeeCustomOrerderNunber);
            SetSqlString(3, settlePodDistribution.FeeCreator);
            SetSqlDateTime(4, settlePodDistribution.FeeCreatorTime);
            SetSqlString(5, settlePodDistribution.FeeStr1);
            SetSqlString(6, settlePodDistribution.FeeStr2);
            SetSqlString(7, settlePodDistribution.FeeStr3);
            SetSqlString(8, settlePodDistribution.FeeStr4);
            SetSqlString(9, settlePodDistribution.FeeStr5);
            SetSqlString(10, settlePodDistribution.FeeStr10);
            SetSqlDecimal(11, settlePodDistribution.FeeDecimal1??0);
            SetSqlDecimal(12, settlePodDistribution.FeeDecimal2 ?? 0);
            SetSqlDecimal(13, settlePodDistribution.FeeDecimal3 ?? 0);
            SetSqlDecimal(14, settlePodDistribution.FeeDecimal4 ?? 0);
            SetSqlDecimal(15, settlePodDistribution.FeeDecimal5 ?? 0);
            SetSqlDecimal(16, settlePodDistribution.FeeDecimal6 ?? 0);
            SetSqlDecimal(17, settlePodDistribution.FeeDecimal7 ?? 0);
            SetSqlDecimal(18, settlePodDistribution.FeeDecimal8 ?? 0);
            SetSqlDecimal(19, settlePodDistribution.FeeDecimal9 ?? 0);
            SetSqlDecimal(20, settlePodDistribution.FeeDecimal10 ?? 0);
            SetSqlDecimal(21, settlePodDistribution.FeeDecimal11 ?? 0);
            SetSqlInt64(22, settlePodDistribution.FeeInt1 ?? 0);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("FeePodID", SqlDbType.BigInt),
            new SqlMetaData("FeeSystemNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("FeeCustomOrerderNunber",SqlDbType.NVarChar,50),
            new SqlMetaData("FeeCreator",SqlDbType.NVarChar,50),
            new SqlMetaData("FeeCreatorTime", SqlDbType.DateTime),
            new SqlMetaData("FeeStr1", SqlDbType.NVarChar, 50),
            new SqlMetaData("FeeStr2",SqlDbType.NVarChar,50),
            new SqlMetaData("FeeStr3", SqlDbType.NVarChar,50),
            new SqlMetaData("FeeStr4", SqlDbType.NVarChar,50),
            new SqlMetaData("FeeStr5", SqlDbType.NVarChar,50),
            new SqlMetaData("FeeStr10", SqlDbType.NVarChar, 500),
            new SqlMetaData("FeeDecimal1",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal2",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal3",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal4",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal5",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal6",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal7",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal8",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal9",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal10",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeDecimal11",SqlDbType.Decimal,18,2),
            new SqlMetaData("FeeInt1",SqlDbType.BigInt)
        };
    }
}

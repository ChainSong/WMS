using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class QuotedPricesToDb : SqlDataRecord
    {
        public QuotedPricesToDb(QuotedPrices price)
            : base(s_metadata)
        {
            SetSqlInt64(0, price.ID);
            SetSqlInt64(1, price.ProjectID);
            SetSqlString(2, price.ProjectName);
            SetSqlInt32(3, price.Target);
            SetSqlString(4, price.TargetName);
            SetSqlString(5, price.StartCityName);
            SetSqlString(6, price.EndCityName);
            SetSqlString(7, price.PodTypeName);
            SetSqlString(8, price.ShipperTypeName);
            SetSqlDecimal(9, price.P200 ?? 0);
            SetSqlDecimal(10, price.P500 ?? 0);
            SetSqlDecimal(11, price.P1000 ?? 0);
            SetSqlDecimal(12, price.P2000 ?? 0);
            SetSqlDecimal(13, price.P5000 ?? 0);
            SetSqlDecimal(14, price.P10000 ?? 0);
            SetSqlDecimal(15, price.P20000 ?? 0);
            SetSqlDecimal(16, price.P30000 ?? 0);
            SetSqlDecimal(17, price.P99999 ?? 0);
            SetSqlString(18, price.RelatedCustomerName);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID",SqlDbType.BigInt),
            new SqlMetaData("ProjectID",SqlDbType.BigInt),
            new SqlMetaData("ProjectName",SqlDbType.NVarChar,50),
            new SqlMetaData("Target",SqlDbType.Int),
            new SqlMetaData("TargetName",SqlDbType.NVarChar,50),
            new SqlMetaData("StartCityName",SqlDbType.NVarChar,50),
            new SqlMetaData("EndCityName",SqlDbType.NVarChar,50),
            new SqlMetaData("PodTypeName",SqlDbType.NVarChar,50),
            new SqlMetaData("ShipperTypeName",SqlDbType.NVarChar,50),
            new SqlMetaData("P200",SqlDbType.Decimal,18,3),
            new SqlMetaData("P500",SqlDbType.Decimal,18,3),
            new SqlMetaData("P1000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P2000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P5000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P10000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P20000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P30000",SqlDbType.Decimal,18,3),
            new SqlMetaData("P99999",SqlDbType.Decimal,18,3),
            new SqlMetaData("RelatedCustomerName",SqlDbType.NVarChar,50),
        };

    }
}

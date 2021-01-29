using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class UdtUpdateNikePodBGToDb : SqlDataRecord
    {
        public UdtUpdateNikePodBGToDb(UpdateNikePodBG pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.CustomerID);
            SetSqlString(1, pod.CustomerOrderNumber);
            SetSqlDouble(2, pod.BoxNumber ??SqlTypes.SqlDouble.Null);
            SetSqlDouble(3, pod.GoodsNumber ?? SqlTypes.SqlDouble.Null);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("BoxNumber",SqlDbType.Float),
            new SqlMetaData("GoodsNumber",SqlDbType.Float)


        };
    }
}



using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSOrderToDb : SqlDataRecord
    {
        public WMSOrderToDb(OrderBackStatus wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlDateTime(1, wmsInfo.UpdateTime);
            SetSqlString(2, wmsInfo.Updator ?? SqlTypes.SqlString.Null);
           

        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),          
            new SqlMetaData("UpdateTime",  SqlDbType.DateTime),
            new SqlMetaData("Updator",SqlDbType.NVarChar, 50)


          
        };
    }
}

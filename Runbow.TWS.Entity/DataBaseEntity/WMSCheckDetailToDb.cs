using System.Data;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Warehouse;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSCheckDetailToDb : SqlDataRecord
    {
        public WMSCheckDetailToDb(WarehouseCheckDetail wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlDecimal(1, wmsInfo.ActualQty);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),          
            new SqlMetaData("ActualQty", SqlDbType.Decimal)
           
        };
    }
}

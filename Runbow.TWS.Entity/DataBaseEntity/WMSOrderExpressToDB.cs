using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSOrderExpressToDB : SqlDataRecord
    {
        public WMSOrderExpressToDB(OrderInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlString(0, wmsInfo.OrderNumber);
            SetSqlString(1, wmsInfo.ExpressCompany); 
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("OrderNumber",SqlDbType.NVarChar, 50)   ,  
            new SqlMetaData("ExpressCompany", SqlDbType.NVarChar, 50)  
        };
    }
}

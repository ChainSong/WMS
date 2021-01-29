using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class OrderNumberToDb : SqlDataRecord
    {
        public OrderNumberToDb(OrderNumbers numbers)
            : base(s_metadata)
        {
            SetSqlString(0, numbers.OrderNumber);
            SetSqlString(1, numbers.SerialNumber);
        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("OrderNumber", SqlDbType.NVarChar,50), 
          new SqlMetaData("SerialNumber", SqlDbType.NVarChar,50), 
};
    }
}

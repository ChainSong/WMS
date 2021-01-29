using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Order;
using System.Data;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ExternOrderNumberToDb : SqlDataRecord
    {
        public ExternOrderNumberToDb(OrderNumbers numbers)
           : base(s_metadata)
        {
            SetSqlString(0, numbers.OrderNumber);
            SetSqlString(1, numbers.SerialNumber);
            SetSqlString(2, numbers.ExternOrderNumber);
        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("OrderNumber", SqlDbType.NVarChar,50),
          new SqlMetaData("SerialNumber", SqlDbType.NVarChar,50),
          new SqlMetaData("ExternOrderNumber",SqlDbType.NVarChar,50)
        };
    }
}

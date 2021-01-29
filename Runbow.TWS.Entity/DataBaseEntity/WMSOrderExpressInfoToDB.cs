using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSOrderExpressInfoToDB : SqlDataRecord
    {
        public WMSOrderExpressInfoToDB(OrderInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlString(0, wmsInfo.OrderNumber);
            SetSqlString(1, wmsInfo.ExpressCompany);
            SetSqlString(2, wmsInfo.ExpressNumber);
            SetSqlString(3, wmsInfo.ExternOrderNumber);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("OrderNumber",SqlDbType.NVarChar, 50)   ,
            new SqlMetaData("ExpressCompany", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpressNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("ExternOrderNumber",SqlDbType.NVarChar,50)
        };
    }
}

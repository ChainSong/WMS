using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.POD
{
    public class UpdateAdidasPurchasePodDb : SqlDataRecord
    {
        public UpdateAdidasPurchasePodDb(UpdateAdidasPurchasePod pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.CustomerID);
            SetSqlString(1, pod.CustomerOrderNumber);
            SetSqlDateTime(2, pod.ActualDeliveryDate);

            SetSqlString(3, pod.TS.ToString());
            SetSqlString(4, pod.Volume.ToString());
            SetSqlString(5, pod.EndCityName);
            SetSqlString(6, pod.str1);
        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("ActualDeliveryDate",SqlDbType.DateTime),
      
            new SqlMetaData("TS",SqlDbType.NVarChar,50),
            new SqlMetaData("Volume",SqlDbType.NVarChar,50),
      
            new SqlMetaData("EndCityName",SqlDbType.NVarChar,50),
            new SqlMetaData("str1",SqlDbType.NVarChar,50)
        };
    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.POD
{
    public class UdtUpdateAdidasPodToDb : SqlDataRecord
    {
        public UdtUpdateAdidasPodToDb(UpdateAdidasPod pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.CustomerID);
            SetSqlString(1, pod.CustomerOrderNumber);
            SetSqlDateTime(2, pod.ActualDeliveryDate);
       
            SetSqlString(3, pod.TS.ToString());
   
            SetSqlString(4, pod.EndCityName);
        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("ActualDeliveryDate",SqlDbType.DateTime),
      
            new SqlMetaData("TS",SqlDbType.NVarChar,50),
      
            new SqlMetaData("EndCityName",SqlDbType.NVarChar,50)

        };
    }
}

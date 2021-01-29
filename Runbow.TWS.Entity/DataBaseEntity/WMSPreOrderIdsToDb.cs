using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSPreOrderIdsToDb : SqlDataRecord
    {
        public WMSPreOrderIdsToDb(PreOrderIds preorderidsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, preorderidsInfo.ID);
           
        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("ID", SqlDbType.BigInt), 
};
    }
}

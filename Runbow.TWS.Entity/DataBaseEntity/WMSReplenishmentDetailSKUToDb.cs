using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Inventory;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSReplenishmentDetailSKUToDb : SqlDataRecord
    {
        public WMSReplenishmentDetailSKUToDb(ReplenishmentDetailSKUs skus)
            : base(s_metadata)
        {
            SetSqlString(0, skus.SKU);
           
        }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("SKU", SqlDbType.NVarChar,20), 
};
    }
}

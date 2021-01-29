using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMSPreOrderToDb
        : SqlDataRecord
    {
        public WMSPreOrderToDb(PreOrderBackStatus wmsInfo)
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

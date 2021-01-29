using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.POD
{
    public class BAFPriceInfoToDb: SqlDataRecord
    {
        public BAFPriceInfoToDb(BAFPriceInfo BAF)
            : base(s_metadata)
        {
            SetSqlInt32(0, BAF.ID);
            SetSqlInt32(1, BAF.ProjectID);
            SetSqlString(2, BAF.ProjectName);
       
            SetSqlInt32(3, BAF.TragetID);
   
            SetSqlString(4, BAF.TragetName);
            SetSqlDecimal(5, BAF.BAFPrice);
             SetSqlDateTime(6, BAF.BAFStartTime);
             SetSqlDateTime(7, BAF.BAFEndTime);
             
        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID",SqlDbType.Int),
            new SqlMetaData("ProjectID",SqlDbType.Int),
            new SqlMetaData("ProjectName",SqlDbType.NVarChar,50),
            new SqlMetaData("TragetID",SqlDbType.Int),
            new SqlMetaData("TragetName",SqlDbType.NVarChar,50),
           new SqlMetaData("BAFPrice",SqlDbType.Decimal),
           new SqlMetaData("BAFStartTime",SqlDbType.DateTime),
           new SqlMetaData("BAFEndTime",SqlDbType.DateTime),
      
        };
    }
}

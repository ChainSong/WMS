using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSGoodsShelfRowAndCellToDb : SqlDataRecord
    {
        public WMSGoodsShelfRowAndCellToDb(GoodsShelfInfo wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt32(0, wmsInfo.RowNumber);
            SetSqlInt32(1, wmsInfo.CellNumber);
            SetSqlString(2, wmsInfo.GoodsShelvesName);   
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("RowNumber", SqlDbType.Int),
            new SqlMetaData("CellNumber", SqlDbType.Int),
            new SqlMetaData("GoodsShelvesName", SqlDbType.NVarChar,50)
        };
    }
}

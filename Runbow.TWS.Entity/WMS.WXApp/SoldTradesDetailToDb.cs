using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    public class SoldTradesDetailToDb : SqlDataRecord
    {
        public SoldTradesDetailToDb(SoldTradeOrder preInfo) : base(s_metadata)
        {
            //SetSqlInt32(0, preInfo.stid);
            SetSqlString(0, preInfo.tid);
            SetSqlString(1, preInfo.sku_id);
            SetSqlString(2, preInfo.num_id);
            SetSqlString(3, preInfo.outer_sku_id);
            SetSqlString(4, preInfo.title);
            SetSqlString(5, preInfo.sku_properties_name);
            SetSqlDecimal(6, preInfo.price);
            SetSqlInt32(7, preInfo.num);
            SetSqlString(8, preInfo.pic_path);
            SetSqlString(9, preInfo.refund_status);
        }
        private static readonly SqlMetaData[] s_metadata = {
            //new SqlMetaData("stid", SqlDbType.Int),
            new SqlMetaData("tid", SqlDbType.VarChar,50),
            new SqlMetaData("sku_id", SqlDbType.VarChar,50),
            new SqlMetaData("num_id", SqlDbType.VarChar,50),
            new SqlMetaData("outer_sku_id", SqlDbType.VarChar,50),
            new SqlMetaData("title", SqlDbType.NVarChar,50),
            new SqlMetaData("sku_properties_name", SqlDbType.NVarChar,50),
            new SqlMetaData("price",SqlDbType.Decimal,18,2),
            new SqlMetaData("num",SqlDbType.Int),
            new SqlMetaData("pic_path",SqlDbType.VarChar,50),
            new SqlMetaData("refund_status",SqlDbType.VarChar,50),
        };
    }
}

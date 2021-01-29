using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = System.Data.SqlTypes;
using System.Data;
using Runbow.TWS.Entity.WMS.WXApp;


namespace Runbow.TWS.Entity
{
    public class WXSoldProductDetailToDb : SqlDataRecord
    {
        public WXSoldProductDetailToDb(ProductSku sku)
            : base(sqlMetas)
        {
            SetSqlInt32(0, sku.num_iid);
            SetSqlString(1, sku.sku_id);
            SetSqlString(2, sku.outer_sku_id);
            SetSqlInt32(3, sku.quantity);
            SetSqlDecimal(4, sku.price);
            SetSqlString(5, sku.sku_properties_name);
        }

        private static readonly SqlMetaData[] sqlMetas =
        {
            new SqlMetaData("num_iid",SqlDbType.Int),
            new SqlMetaData("sku_id",SqlDbType.NVarChar,200),
            new SqlMetaData("outer_sku_id",SqlDbType.NVarChar,200),
            new SqlMetaData("quantity",SqlDbType.Int),
            new SqlMetaData("price",SqlDbType.Decimal,18,2),
            new SqlMetaData("sku_properties_name",SqlDbType.NVarChar,500)
        };
    }
}

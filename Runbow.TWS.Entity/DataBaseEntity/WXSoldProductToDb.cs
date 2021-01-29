using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.WXApp;
using Microsoft.SqlServer.Server;
using SqlTypes = System.Data.SqlTypes;
using System.Data;

namespace Runbow.TWS.Entity
{
    public class WXSoldProductToDb : SqlDataRecord
    {
        public WXSoldProductToDb(Product product)
               : base(sqlMetas)
        {
            SetSqlInt32(0, product.cid);
            SetSqlString(1, product.cat_name);
            SetSqlInt32(2, product.brand_id);
            SetSqlString(3, product.brand_name);
            SetSqlInt32(4, product.type_id);
            SetSqlString(5, product.type_name);
            SetSqlInt32(6, product.num_iid);
            SetSqlString(7, product.outer_id);
            SetSqlString(8, product.title);
            SetSqlString(9, product.pic_url.Count() > 0 ? string.Join(",", product.pic_url.ToArray()) : "");
            SetSqlString(10, product.desc);
            SetSqlString(11, product.wap_desc);
            SetSqlDateTime(12, product.list_time ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(13, product.modified ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt32(14, product.display_sequence);
            SetSqlString(15, product.approve_status);
            SetSqlInt32(16, product.sold_quantity);
            SetSqlString(17, product.props_name.Replace("#cln#", ":").Replace("#scln#", ";"));
            SetSqlInt32(18, product.num);
            SetSqlInt32(19, product.sub_stock);
            SetSqlDecimal(20, product.price);
        }

        private static readonly SqlMetaData[] sqlMetas = {
            new SqlMetaData("cid",SqlDbType.Int),
            new SqlMetaData("cat_name",SqlDbType.NVarChar,200),
            new SqlMetaData("brand_id",SqlDbType.Int),
            new SqlMetaData("brand_name",SqlDbType.NVarChar,200),
            new SqlMetaData("type_id",SqlDbType.Int),
            new SqlMetaData("type_name",SqlDbType.NVarChar,200),
            new SqlMetaData("num_iid",SqlDbType.Int),
            new SqlMetaData("outer_id",SqlDbType.NVarChar,200),
            new SqlMetaData("title",SqlDbType.NVarChar,200),
            new SqlMetaData("pic_url",SqlDbType.NVarChar,4000),
            new SqlMetaData("desc",SqlDbType.NVarChar,200),
            new SqlMetaData("wap_desc",SqlDbType.NVarChar,4000),
            new SqlMetaData("list_time",SqlDbType.DateTime),
            new SqlMetaData("modified",SqlDbType.DateTime),
            new SqlMetaData("display_sequence",SqlDbType.Int),
            new SqlMetaData("approve_status",SqlDbType.NVarChar,200),
            new SqlMetaData("sold_quantity",SqlDbType.Int),
            new SqlMetaData("props_name",SqlDbType.NVarChar,200),
            new SqlMetaData("num",SqlDbType.Int),
            new SqlMetaData("sub_stock",SqlDbType.Int),
            new SqlMetaData("price",SqlDbType.Decimal,18,2)
        };
    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    public class SoldTradesToDb : SqlDataRecord
    {
        public SoldTradesToDb(SoldTrade preInfo) : base(s_metadata)
        {
            SetSqlString(0, preInfo.tid);
            SetSqlString(1, preInfo.buyer_memo);
            SetSqlString(2, preInfo.seller_memo);
            SetSqlString(3, preInfo.seller_flag);
            SetSqlDecimal(4, preInfo.discount_fee);
            SetSqlString(5, preInfo.status);
            SetSqlString(6, preInfo.close_memo);
            SetSqlDateTime(7, preInfo.created ?? SqlDateTime.Null);
            SetSqlDateTime(8, preInfo.modified ?? SqlDateTime.Null);
            SetSqlDateTime(9, preInfo.pay_time ?? SqlDateTime.Null);
            SetSqlDateTime(10, preInfo.consign_time ?? SqlDateTime.Null);
            SetSqlString(11, preInfo.end_time);
            SetSqlString(12, preInfo.buyer_uname);
            SetSqlString(13, preInfo.buyer_email);
            SetSqlString(14, preInfo.buyer_nick);
            SetSqlString(15, preInfo.buyer_area);
            SetSqlString(16, preInfo.receiver_name);
            SetSqlString(17, preInfo.receiver_state);
            SetSqlString(18, preInfo.receiver_city);
            SetSqlString(19, preInfo.receiver_district);
            SetSqlString(20, preInfo.receiver_town);
            SetSqlString(21, preInfo.receiver_address);
            SetSqlString(22, preInfo.receiver_zip);
            SetSqlString(23, preInfo.receiver_mobile);
            SetSqlDecimal(24, preInfo.invoice_fee);
            SetSqlString(25, preInfo.invoice_title);
            SetSqlString(26, preInfo.payment);
            SetSqlInt32(27, preInfo.storeId ?? SqlInt32.Null);
        }
        private static readonly SqlMetaData[] s_metadata = {
            new SqlMetaData("tid", SqlDbType.VarChar,50),
            new SqlMetaData("buyer_memo", SqlDbType.NVarChar,50),
            new SqlMetaData("seller_memo", SqlDbType.NVarChar,50),
            new SqlMetaData("seller_flag", SqlDbType.NVarChar,50),
            new SqlMetaData("discount_fee", SqlDbType.Decimal,18,2),
            new SqlMetaData("status", SqlDbType.VarChar,50),
            new SqlMetaData("close_memo", SqlDbType.NVarChar,50),
            new SqlMetaData("created", SqlDbType.DateTime),
            new SqlMetaData("modified", SqlDbType.DateTime),
            new SqlMetaData("pay_time", SqlDbType.DateTime),
            new SqlMetaData("consign_time", SqlDbType.DateTime),
            new SqlMetaData("end_time", SqlDbType.VarChar,50),
            new SqlMetaData("buyer_uname", SqlDbType.NVarChar,50),
            new SqlMetaData("buyer_email", SqlDbType.NVarChar,50),
            new SqlMetaData("buyer_nick", SqlDbType.NVarChar,50),
            new SqlMetaData("buyer_area", SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_name", SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_state", SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_city",SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_district",SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_town",SqlDbType.NVarChar,50),
            new SqlMetaData("receiver_address",SqlDbType.NVarChar,500),
            new SqlMetaData("receiver_zip",SqlDbType.VarChar,50),
            new SqlMetaData("receiver_mobile",SqlDbType.VarChar,50),
            new SqlMetaData("invoice_fee",SqlDbType.Decimal,18,2),
            new SqlMetaData("invoice_title",SqlDbType.NVarChar,50),
            new SqlMetaData("payment",SqlDbType.VarChar,50),
            new SqlMetaData("storeId",SqlDbType.Int),
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SqlTypes = System.Data.SqlTypes;
using Runbow.TWS.Entity.WMS.WXApp;
using System.Data;

namespace Runbow.TWS.Entity
{
    public class WXSoldRefundDetailToDb : SqlDataRecord
    {
        public WXSoldRefundDetailToDb(RefundDetail rd)
            : base(sqlMetaDatas)
        {
            SetSqlInt32(0, rd.returnId);
            SetSqlString(1, rd.orderId);
            SetSqlString(2, rd.userName);
            SetSqlDateTime(3, rd.applyForTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(4, rd.adminRemark);
            SetSqlString(5, rd.userRemark);
            SetSqlString(6, rd.handleStatus);
            SetSqlString(7, rd.orderStatus);
            SetSqlInt32(8, rd.quantity);
            SetSqlString(9, rd.skuId);
            SetSqlString(10, rd.productName);
            SetSqlString(11, rd.returnReason);
            SetSqlString(12, rd.userCredentials);
            SetSqlString(13, rd.adminShipAddress);
            SetSqlDecimal(14, rd.refundAmount);
            SetSqlDateTime(15, rd.agreedOrRefusedTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(16, rd.finishTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(17, rd.shipOrderNumber);
            SetSqlDateTime(18, rd.userSendGoodsTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(19, rd.handleTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(20, rd.expressCompanyName);
            SetSqlString(21, rd.expressCompanyAbb);
            SetSqlString(22, rd.adminShipTo);
            SetSqlString(22, rd.outerSkuId);
        }

        private static readonly SqlMetaData[] sqlMetaDatas =
        {
            new SqlMetaData("returnId",SqlDbType.Int),
            new SqlMetaData("orderId",SqlDbType.NVarChar,200),
            new SqlMetaData("userName",SqlDbType.NVarChar,200),
            new SqlMetaData("applyForTime",SqlDbType.DateTime),
            new SqlMetaData("adminRemark",SqlDbType.NVarChar,500),
            new SqlMetaData("userRemark",SqlDbType.NVarChar,500),
            new SqlMetaData("handleStatus",SqlDbType.NVarChar,200),
            new SqlMetaData("orderStatus",SqlDbType.NVarChar,200),
            new SqlMetaData("quantity",SqlDbType.Int),
            new SqlMetaData("skuId",SqlDbType.NVarChar,200),
            new SqlMetaData("productName",SqlDbType.NVarChar,500),
            new SqlMetaData("returnReason",SqlDbType.NVarChar,500),
            new SqlMetaData("userCredentials",SqlDbType.NVarChar,200),
            new SqlMetaData("adminShipAddress",SqlDbType.NVarChar,500),
            new SqlMetaData("refundAmount",SqlDbType.Decimal,18,2),
            new SqlMetaData("agreedOrRefusedTime",SqlDbType.DateTime),
            new SqlMetaData("finishTime",SqlDbType.DateTime),
            new SqlMetaData("shipOrderNumber",SqlDbType.NVarChar,200),
            new SqlMetaData("userSendGoodsTime",SqlDbType.DateTime),
            new SqlMetaData("handleTime",SqlDbType.DateTime),
            new SqlMetaData("expressCompanyName",SqlDbType.NVarChar,200),
            new SqlMetaData("expressCompanyAbb",SqlDbType.NVarChar,200),
            new SqlMetaData("adminShipTo",SqlDbType.NVarChar,200),
            new SqlMetaData("outerSkuId",SqlDbType.NVarChar,200)
        };
    }
}

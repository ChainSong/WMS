using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Shelves;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ShelvesToDb : SqlDataRecord
    {
        public ShelvesToDb(Shelves shelves)
            : base(s_metadata)
        {
            SetSqlString(0, shelves.LineNumber);
            SetSqlString(1, shelves.SKU);
            SetSqlDateTime(2, shelves.ReceiptDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlDecimal(3, shelves.QtyExpected);
            SetSqlDecimal(4, shelves.QtyReceived);
            SetSqlString(5, shelves.Remark);

        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("LineNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("SKU", SqlDbType.NVarChar,50),
            new SqlMetaData("ReceiptDate", SqlDbType.DateTime),
            new SqlMetaData("QtyExpected", SqlDbType.Int),
            new SqlMetaData("QtyReceived", SqlDbType.Int),
            new SqlMetaData("Remark", SqlDbType.NVarChar,500)
        };
    }
}
using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;
using Runbow.TWS.Entity.WMS.Product;


namespace Runbow.TWS.Entity
{
    public class WMSProductSearch : SqlDataRecord
    {
        public WMSProductSearch(ProductSearch wmspro)
            : base(s_metadata)
        {
            SetSqlString(0, wmspro.SKU);
            SetSqlString(1, wmspro.Str8);
           SetSqlString(2, wmspro.Str9);
           SetSqlString(3, wmspro.Str10);
           SetSqlString(4, wmspro.UPC);
        }

        private static readonly SqlMetaData[] s_metadata =
        {    
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str9", SqlDbType.NVarChar,50),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 50),
             new SqlMetaData("UPC", SqlDbType.NVarChar, 50)
        };
    }
}

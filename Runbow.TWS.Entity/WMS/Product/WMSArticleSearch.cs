using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;
using Runbow.TWS.Entity.WMS.Product;


namespace Runbow.TWS.Entity
{
    public class WMSArticleSearch : SqlDataRecord
    {
        public WMSArticleSearch(ArticleSearch wmspro)
            : base(s_metadata)
        {
            SetSqlString(0, wmspro.ArticleNo);
            SetSqlString(1, wmspro.Division);
           SetSqlString(2, wmspro.GPC);
           SetSqlString(3, wmspro.UOM);
            
        }

        private static readonly SqlMetaData[] s_metadata =
        {    
            new SqlMetaData("ArticleNo", SqlDbType.NVarChar, 50),
            new SqlMetaData("Division", SqlDbType.NVarChar, 50),
            new SqlMetaData("GPC", SqlDbType.NVarChar,50),
            new SqlMetaData("UOM", SqlDbType.NVarChar, 50)
            
        };
    }
}

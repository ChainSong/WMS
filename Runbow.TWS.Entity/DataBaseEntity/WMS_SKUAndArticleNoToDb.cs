using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMS_SKUAndArticleNoToDb : SqlDataRecord
    {
        public WMS_SKUAndArticleNoToDb(WMS_SKUAndArticleNoTable info)
            : base(s_metadata)
        {
            SetSqlString(0, info.SKU);
            SetSqlString(1, info.ArticleNo);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("ArticleNo", SqlDbType.NVarChar, 50)
        };
    }
}

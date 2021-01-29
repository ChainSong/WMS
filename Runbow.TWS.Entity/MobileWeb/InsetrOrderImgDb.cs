using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.MobileWeb
{
    public class InsetrOrderImgDb : SqlDataRecord
    {
        public InsetrOrderImgDb(InsetrOrderImg Img)
            : base(s_metadata)
        {
            SetSqlString(0, Img.CustomerOrderNumber);
            SetSqlString(1, Img.FileName);
            SetSqlString(2, Img.FileType);
            SetSqlString(3, Img.ServerName);
            SetSqlString(4, Img.FilePath);

            SetSqlString(5, Img.OrderNo);
            SetSqlString(6, Img.Creator);
            SetSqlString(7, Img.CreateTime);
            SetSqlString(8, Img.CreateUserID);

        }
        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("CustomerOrderNumber",SqlDbType.NVarChar,50),
            new SqlMetaData("FileName",SqlDbType.NVarChar,50),
            new SqlMetaData("FileType",SqlDbType.NVarChar,50),
             new SqlMetaData("ServerName",SqlDbType.NVarChar,50),
           
            new SqlMetaData("FilePath",SqlDbType.NVarChar,50),
            new SqlMetaData("OrderNo",SqlDbType.NVarChar,50),
           new SqlMetaData("Creator",SqlDbType.NVarChar,50),
           new SqlMetaData("CreateTime",SqlDbType.NVarChar,50),
           new SqlMetaData("CreateUserID",SqlDbType.NVarChar,50)
 
        };
    }

}

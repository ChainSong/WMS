using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class NumbersToDb : SqlDataRecord
        {
        public NumbersToDb(string number)
                : base(s_metadata)
            {
                SetString(0, number);
            }

            private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("Number",SqlDbType.NVarChar,50)
        };
        }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class VehicleLocationToDb : SqlDataRecord
    {
        public VehicleLocationToDb(VehicleLocation vehiclelocation)
            : base(s_metadata)
        {
            SetSqlInt64(0, vehiclelocation.ID); 
            SetSqlDateTime(1, vehiclelocation.Times ?? SqlTypes.SqlDateTime.Null);         
            SetSqlString(2, vehiclelocation.Phone);
            SetSqlString(3, vehiclelocation.VehicleNumber);
            SetSqlString(4, vehiclelocation.Longitude);
            SetSqlString(5, vehiclelocation.Latitude);
            SetSqlString(6, vehiclelocation.Str1);
            SetSqlString(7, vehiclelocation.Str2);
            SetSqlString(8, vehiclelocation.Str3);
            SetSqlString(9, vehiclelocation.Str4);
            SetSqlString(10,vehiclelocation.Str5);
            SetSqlInt64(11, vehiclelocation.Int1);
            SetSqlInt64(12, vehiclelocation.Int2);
            SetSqlInt64(13, vehiclelocation.Int3);           
            SetSqlDateTime(14, vehiclelocation.Datetime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(15, vehiclelocation.Datetime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(16, vehiclelocation.Datetime3 ?? SqlTypes.SqlDateTime.Null);
           

        }


        private static readonly SqlMetaData[] s_metadata =
        { 
            new SqlMetaData("ID", SqlDbType.BigInt), 
            new SqlMetaData("Times", SqlDbType.DateTime),
            new SqlMetaData("Phone", SqlDbType.NVarChar,50),
            new SqlMetaData("VehicleNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("Longitude", SqlDbType.NVarChar, 50),
            new SqlMetaData("Latitude", SqlDbType.NVarChar,50),
            new SqlMetaData("Str1", SqlDbType.NVarChar,50),
            new SqlMetaData("Str2", SqlDbType.NVarChar,50),
            new SqlMetaData("Str3", SqlDbType.NVarChar,50),
            new SqlMetaData("Str4", SqlDbType.NVarChar,50),
            new SqlMetaData("Str5", SqlDbType.NVarChar,50), 
            new SqlMetaData("Int1", SqlDbType.BigInt),
            new SqlMetaData("Int2", SqlDbType.BigInt),
            new SqlMetaData("Int3", SqlDbType.BigInt), 
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
           
        };
    }
}

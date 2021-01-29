using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.ShipperManagement;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class InsertShipperToDb : SqlDataRecord
    {//Name,TransportMode,Code,CreateTime,Creater
        public InsertShipperToDb(InsertShipperExcel insertShipper)
            : base(s_metadata)
        {
            SetSqlString(0, insertShipper.Name);
            SetSqlString(1, insertShipper.TransportMode);
            SetSqlString(2, insertShipper.Code);
            
            SetSqlString(3, insertShipper.Creater);
            SetSqlDateTime(4, insertShipper.CreateTime);
            
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("Name", SqlDbType.NVarChar, 100),
            new SqlMetaData("TransportMode", SqlDbType.NVarChar, 100),
            new SqlMetaData("Code", SqlDbType.NVarChar, 100),
            
            new SqlMetaData("Creater", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime)
             
        };


    }
}

using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Dao.WebApi
{
    public class VehicleLocationAccessor : BaseAccessor
    {
        public void AddVehicleLocations(VehicleLocationRequest rece)
        {
            using (SqlConnection conn = new SqlConnection(Runbow.TWS.Dao.BaseAccessor._dataBase.ConnectionString))
            {
                    SqlCommand cmd = new SqlCommand("Pro_InsertVehicleLocation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Vehiclelocation", rece.vehiclelocation.Select(vehiclelocation => new VehicleLocationToDb(vehiclelocation)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    conn.Open();
                    cmd.ExecuteNonQuery();
            }
        }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao
{
    public class MenuAccessor : BaseAccessor
    {
        public IEnumerable<CheckedMenu> GetMenuByProjectRoleID(long projectRoleID, bool getAll = false)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectRoleID", DbType.Int64, projectRoleID, ParameterDirection.Input),
                new DbParam("@GetAll", DbType.Boolean, getAll, ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_GetMenuByProjectRoleID", dbParams).ConvertToEntityCollection<CheckedMenu>();
        }

        public bool SetProjectRoleMenu(long projectRoleID, IEnumerable<int> menuIDs)
        {
            //Do not use Enterprise liberary to operate db because of the user defined type
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetProjectRoleMenu", conn);
                cmd.CommandTimeout = 500;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectRoleID", projectRoleID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@MenuIDs", menuIDs == null ? null : menuIDs.Select(id => new IdsForInt32(id)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ReturnValue", -1);
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                conn.Open();

                cmd.ExecuteNonQuery();

                if (!cmd.Parameters[2].Value.ObjectToBoolean())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
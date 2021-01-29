using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.ForecastWarehouse;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.ForecastWarehouse;
using System.Data.SqlClient;

namespace Runbow.TWS.Dao.ForecastWarehouse
{
    public class GapPickingNoteAccessor : BaseAccessor
    {
        public GapPickingNoteResponse GetGapPickingNote(GapPickingNoteRequest request)
        {
            GapPickingNoteResponse response = new GapPickingNoteResponse();

            string sql = "SELECT * FROM dbo.User_Code_Mapping WHERE [User] = '" + request.User + "'";

            DataTable dt = this.ExecuteDataTableBySqlString(sql, null);

            response.Code = dt.ConvertToEntity<UserCode>();

            return response;

        }

        public string AddGapPickingNote(GapPickingNote gp)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    #region
                    //StoreCode
                    cmd.Parameters.AddWithValue("@StoreCode", gp.StoreCode);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.Parameters[0].Direction = ParameterDirection.Input;
                    //StoreName
                    cmd.Parameters.AddWithValue("@StoreName", gp.StoreName);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters[1].Direction = ParameterDirection.Input;
                    //City
                    cmd.Parameters.AddWithValue("@City", gp.City);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 50;
                    cmd.Parameters[2].Direction = ParameterDirection.Input;
                    //TransferorReturn
                    cmd.Parameters.AddWithValue("@TransferorReturn", gp.TransferorReturn);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 50;
                    cmd.Parameters[3].Direction = ParameterDirection.Input;
                    //ServiceDetail
                    cmd.Parameters.AddWithValue("@ServiceDetail", gp.ServiceDetail);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Size = 50;
                    cmd.Parameters[4].Direction = ParameterDirection.Input;
                    //CartonQuantity
                    cmd.Parameters.AddWithValue("@CartonQuantity", gp.CartonQuantity);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Size = 200;
                    cmd.Parameters[5].Direction = ParameterDirection.Input;
                    //DestinationCode
                    cmd.Parameters.AddWithValue("@DestinationCode", gp.DestinationCode);
                    cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[6].Size = 100;
                    cmd.Parameters[6].Direction = ParameterDirection.Input;
                    //Remark
                    cmd.Parameters.AddWithValue("@Remark", gp.Remark);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[7].Size = 100;
                    cmd.Parameters[7].Direction = ParameterDirection.Input;
                    //Brand
                    cmd.Parameters.AddWithValue("@Brand", gp.Brand);
                    cmd.Parameters[8].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[8].Size = 50;
                    cmd.Parameters[8].Direction = ParameterDirection.Input;
                    //ExpectedDeliveryDate
                    cmd.Parameters.AddWithValue("@ExpectedDeliveryDate", gp.ExpectedDeliveryDate);
                    cmd.Parameters[9].SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters[9].Direction = ParameterDirection.Input;
                    //ExpectedArrivalDate
                    cmd.Parameters.AddWithValue("@ExpectedArrivalDate", gp.ExpectedArrivalDate);
                    cmd.Parameters[10].SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters[10].Direction = ParameterDirection.Input;
                    //CreatTime
                    cmd.Parameters.AddWithValue("@CreatTime", gp.CreatTime);
                    cmd.Parameters[11].SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters[11].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[12].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[12].Size = 10;
                    cmd.Parameters[12].Direction = ParameterDirection.Output;
                    #endregion
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    return message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}

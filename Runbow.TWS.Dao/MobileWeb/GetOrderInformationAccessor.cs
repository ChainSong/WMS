using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.MobilePOD;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.MobileWeb;
using System.Data.SqlClient;

namespace Runbow.TWS.Dao.MobilePod
{
    public class GetOrderInformationAccessor : BaseAccessor
    {

        public IEnumerable<OrderManagementInfo> GetQueryInformations(int UserType, string permissions, string conditions, int PageIndex, int PageSize, out int RowCount)//string ShipperId,
        {
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(conditions))
            {
                sqlWhere = string.Format("and  CustomerOrderNumber+CustomerName+CustomerOrderNumber+ShipperName+EndCityName like ('%{0}%')", conditions);
            }
            if (UserType == 2)//客户
            {
                sqlWhere += string.Format(" and CustomerID in ({0})", permissions);
            }
            else if (UserType == 1)//承运商
            {
                sqlWhere += string.Format("and ShipperId in ({0})", conditions);
            }
            else if (UserType == 0)
            {
                sqlWhere += string.Format(" and CustomerID in ({0})", conditions);
            }
            //if (!string.IsNullOrEmpty(permissions))
            //{
            //    sqlWhere += string.Format(" and CustomerID in ({0})", permissions);
            //}
            //if (!string.IsNullOrEmpty(ShipperId))
            //{
            //    sqlWhere += string.Format("and ShipperId in ({1})", ShipperId);
            //}
            //if()
            //{

            //}

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetOrderInformation_Mobile", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<OrderManagementInfo>();
        }
        public OrderManagementInfo QueryOrderInformation(string id)
        {
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@id", DbType.String, id, ParameterDirection.Input),
                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryOrderInformation", dbParams);
            //  RowCount = (int)dbParams[3].Value;
            //return dt.ConvertToEntity<ProductStorerInfo>();
            return dt.ConvertToEntity<OrderManagementInfo>();
        }

        public IEnumerable<Runbow.TWS.Entity.MobilePOD.PodStatusTrack> addGPS(string id, string SystemNumber, string CustomerOrderNumber, string address, string lng, string lat)
        {
            //string sqlWhere = "";
            //if (!string.IsNullOrEmpty(id))
            //{
            //    sqlWhere();
            //}
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(lng))
            {

                DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@id", DbType.String, id, ParameterDirection.Input),
                  new DbParam("@SystemNumber", DbType.String, SystemNumber, ParameterDirection.Input),
                    new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
                      new DbParam("@address", DbType.String, address, ParameterDirection.Input),
                  new DbParam("@lat", DbType.String, lat, ParameterDirection.Input),
                    new DbParam("@lng", DbType.String,lng, ParameterDirection.Input),

                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_AddGPS", dbParams);
            }
            else
            {
                DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
                

                //new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                //new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                //new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_QueryGPS", dbParams);
            }

            //RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<Runbow.TWS.Entity.MobilePOD.PodStatusTrack>();
            //  return true;
        }
        public OrderManagementInfo PODInformation(string id)
        {
            string sqlWhere = string.Format("and CustomerOrderNumber='{0}'", id);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@SqlWhere", DbType.String, sqlWhere, ParameterDirection.Input),
   
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryPODInformation_Mobile", dbParams);
            return dt.ConvertToEntity<OrderManagementInfo>();
        }
        public bool InsertImg(IEnumerable<InsetrOrderImg> insetrOrderImg)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<InsetrOrderImg> result = new List<InsetrOrderImg>();
                SqlCommand cmd = new SqlCommand("Proc_InsetrImg_MobileWeb", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsertImg", insetrOrderImg == null ? null : insetrOrderImg.Select(a => new InsetrOrderImgDb(a)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(
                        new InsetrOrderImg()
                        {
                            CustomerOrderNumber = reader.GetString(0),
                        });
                }
                if (result.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


                //return result.Select(a=>a.CustomerOrderNumber).ToString();
            }
        }
    }
}

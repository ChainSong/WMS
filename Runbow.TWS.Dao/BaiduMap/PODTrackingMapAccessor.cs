
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.BaiduMap;
using Runbow.TWS.MessageContracts.BaiduMap;
using Runbow.TWS.Common;
using System.Collections.Generic;
using System.Text;
namespace Runbow.TWS.Dao.BaiduMap
{
    public class PODTrackingMapAccessor : BaseAccessor
    {
        public PODTrackingMapResponse GPSALLGlobalTracking(PODTrackingMapRequest Request)
        {
            PODTrackingMapResponse Response = new PODTrackingMapResponse();
            string SqlWhere = "";
            if (Request != null)
            {
                SqlWhere = this.SqlWhere(Request);
            }
            DbParam[] dbParams = {
                           new DbParam("@sqlWhere",DbType.String,SqlWhere,ParameterDirection.Input) 
                          };
            DataTable dt = this.ExecuteDataTable("[Proc_GPSALLGlobalTracking]", dbParams);
            Response.Response = dt.ConvertToEntityCollection<PODTrackingMap>();
            //Response.ResponseHub = dt.Tables[1].ConvertToEntityCollection<PODTrackingMap>();
            //Response.PODTrackingMap = dt.ConvertToEntityCollection<PodStatusLogMap>();
            return Response;
        }

        public IEnumerable<PodStatusLogMap> GetCarInfoPOD(PODTrackingMapRequest Request)
        {
            string SqlWhere = "";
            if (Request != null)
            {
                SqlWhere = this.SqlWhere(Request);
            }
            DbParam[] dbParams = {
                           new DbParam("@sqlWhere",DbType.String,SqlWhere,ParameterDirection.Input) 
                          };
            DataTable dt = this.ExecuteDataTable("[Prox_GetCarInfoPOD]", dbParams);
            //Response.Response = ds.Tables[0].ConvertToEntityCollection<PODTrackingMap>();

            return dt.ConvertToEntityCollection<PodStatusLogMap>();
        }
        private string SqlWhere(PODTrackingMapRequest re)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(re.Customerordernumber))
            {
                sb.Append(" and b.customerordernumber ='" + re.Customerordernumber + "'");
            }
            if (!string.IsNullOrEmpty(re.Type))
            {
                if (re.Type != "Hub")
                {
                    sb.Append(" and b.str4 ='" + re.Type + "'");
                }
            }
            if (!string.IsNullOrEmpty(re.CarNo))
            {
                if (re.Type == "Hub")
                {
                    sb.Append(" and b.str9 ='" + re.CarNo + "'");
                }
                else
                {
                    sb.Append(" and  b.datetime3 is null  and b.str2 ='" + re.CarNo + "'");
                }
            }
            if (!string.IsNullOrEmpty(re.EndCustomer))
            {
                sb.Append("  and a.Str4 ='" + re.EndCustomer + "'");
            }
            if (!string.IsNullOrEmpty(re.Destination))
            {
                sb.Append("  and a.EndCityName ='" + re.Destination + "'");
            }
            if (re.start_DeliveryDate != null)
            {
                sb.Append("  and a.ActualDeliveryDate >='" + re.start_DeliveryDate + "'");
            }
            if (re.end_DeliveryDate != null)
            {
                sb.Append("  and a.ActualDeliveryDate <'" + re.end_DeliveryDate + "'");
            }
            if (re.start_PlanArrive != null)
            {
                sb.Append("  and a.DateTime6 >='" + re.start_PlanArrive + "'");
            }
            if (re.end_PlanArrive != null)
            {
                sb.Append("  and a.DateTime6 <'" + re.end_PlanArrive + "'");
            }

            return sb.ToString();
        }
        public PODTrackingMapResponse PartialPODView(PODTrackingMapRequest Request)
        {
            PODTrackingMapResponse Response = new PODTrackingMapResponse();
            DbParam[] dbParams = {
                           new DbParam("@sqlWhere",DbType.String,Request.ID,ParameterDirection.Input) 
                          };
            DataTable dt = new DataTable();
            //if (Request.Type == "Car")
            //{
            //    dt = this.ExecuteDataTable("Proc_GetVehicleTrajectoryCar", dbParams);
            //}
            //else
            // {
            dt = this.ExecuteDataTable("Proc_GetVehicleTrajectoryOrder", dbParams);
            //}
            Response.Response = dt.ConvertToEntityCollection<PODTrackingMap>();
            //Response.PODTrackingMap = dt.ConvertToEntityCollection<PodStatusLogMap>();
            return Response;
        }
    }
}

using Runbow.TWS.Dao.WebApi;
using Runbow.TWS.MessageContracts.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.WebApi
{
   public  class VehiclelocationService:BaseService
    {
       VehicleLocationAccessor accessor = new VehicleLocationAccessor();
       public bool AddVehicleLocations(VehicleLocationRequest request) {
           try
           {
               accessor.AddVehicleLocations(request);
               return true;
           }
           catch (Exception ex )
           {
               LogError(ex);
               return false;
           }
       }
    }
}

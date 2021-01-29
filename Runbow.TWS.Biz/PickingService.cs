using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao.WMS;

namespace Runbow.TWS.Biz
{
    public class PickingService
    {
        public bool CreatePickingAndDetail(string Creator, string IDs)
        {
            try
            {
                PickingAccessor accessor = new PickingAccessor();
                accessor.CreatePickingAndDetail(Creator,IDs);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

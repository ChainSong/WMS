using Runbow.TWS.Dao.RFWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.RFWeb
{
    public class BestLocationManagementService : BaseService
    {
        /// <summary>
        /// 获取推荐库位
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WarehouseName"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string GetBestLocation(string CustomerID, string WarehouseName, string BoxNumber)
        {
            try
            {
                BestLocationManagementAccessor accessor = new BestLocationManagementAccessor();
                return accessor.GetBestLocation(CustomerID, WarehouseName, BoxNumber);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 重新推荐库位
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WarehouseName"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string RefreshLocation(string CustomerID, string WarehouseName, string BoxNumber,string Location )
        {
            try
            {
                BestLocationManagementAccessor accessor = new BestLocationManagementAccessor();
                return accessor.RefreshLocation(CustomerID, WarehouseName, BoxNumber, Location);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

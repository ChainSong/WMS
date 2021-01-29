using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao.Inventory;
using Runbow.TWS.Entity.InventoryApi;
using Runbow.TWS.MessageContracts.Inventory;
namespace Runbow.TWS.Biz.Inventory
{
    public class BizOSRStorage : BaseService
    {
        public OSRStorageResponses GetOSRStorage(OSRStorageCondition request)
        {
            OSRStorageResponses response = new OSRStorageResponses();
            try
            {
               response = new DaoOSRStorage().GetOSRStorage(request);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public OSRStorageResponses GetOSRReceivingDetailed(OSRStorageCondition request)
        {
            OSRStorageResponses response = new OSRStorageResponses();
            try
            {
                response = new DaoOSRStorage().GetOSRReceivingDetailed(request);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public OSRThelibraryResponses GetOSRThelibrary(OSRThelibraryCondition request)
        {
            OSRThelibraryResponses response = new OSRThelibraryResponses();
            try
            {
                response = new DaoOSRStorage().GetOSRThelibrary(request);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public OSRThelibraryResponses GetOSROutboundDetailed(OSRThelibraryCondition request)
        {
            OSRThelibraryResponses response = new OSRThelibraryResponses();
            try
            {
                response = new DaoOSRStorage().GetOSROutboundDetailed(request);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public OSRStockResponses GetOSRStock(OSRStockCondition request)
        {
            OSRStockResponses response = new OSRStockResponses();
            try
            {
                response = new DaoOSRStorage().GetOSRStock(request);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public OSRJobResponses GetOSRJob(OSRJobCondition request)
        {
            OSRJobResponses response = new OSRJobResponses();
            try
            {
                response = new DaoOSRStorage().GetOSRJob(request);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public string GetImport(DataTable dt)
        {
            string response = string.Empty;
            try
            {
                response = new DaoOSRStorage().GetImport(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        public string GetOSRLineNumber(DataTable dt)
        {
            string response = string.Empty;
            try
            {
                response = new DaoOSRStorage().GetOSRLineNumber(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        public string GetOSRLineNumberGZ(DataTable dt)
        {
            string response = string.Empty;
            try
            {
                response = new DaoOSRStorage().GetOSRLineNumberGZ(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        public string GetNFSLineNumberGZ(DataTable dt)
        {
            string response = string.Empty;
            try
            {
                response = new DaoOSRStorage().GetNFSLineNumberGZ(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public string GetNFSLineNumberBJ(DataTable dt)
        {
            string response = string.Empty;
            try
            {
                response = new DaoOSRStorage().GetNFSLineNumberBJ(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        
        

        public DataTable GetDropDownList()
        {
            try
            {
                return new DaoOSRStorage().GetDropDownList();
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}

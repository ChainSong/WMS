using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.Inventory;
using Runbow.TWS.Entity.InventoryApi;
using Runbow.TWS.MessageContracts.Inventory;

namespace Runbow.TWS.Biz.Inventory
{
   public class BizSampleStorage:BaseService
    {
       public SampleStorageResponses GetSampleStorage(SampleStorageCondition request)
       {
           SampleStorageResponses response = new SampleStorageResponses();
           try
           {
                   response = new DaoSampleStorage().GetStorage(request);
              
               //    response = new DaoSampleStorage().GetSampleStock(request);
              
           }
           catch (Exception ex)
           {
               LogError(ex);
           }
           return response;
       }
       public SampleThelibraryResponses GetSampleThelibrary(SampleThelibraryCondition request)
       {
           SampleThelibraryResponses response = new SampleThelibraryResponses();
           try
           {
               response = new DaoSampleStorage().GetThelibrary(request);
           }
           catch (Exception ex)
           {
               LogError(ex);
           }
           return response;
       }

       public SampleStockResponses GetSampleStock(SampleStockCondition request)
       {
           SampleStockResponses response = new SampleStockResponses();
           try
           {
               response = new DaoSampleStorage().GetSampleStock(request);
           }
           catch (Exception ex)
           {
               LogError(ex);
           }
           return response;
       }
       public SampleJobResponses GetSampleJob(SampleJobCondition request)
       {
           SampleJobResponses response = new SampleJobResponses();
           try
           {
               response = new DaoSampleStorage().GetSampleJob(request);
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
               response = new DaoSampleStorage().GetImport(dt);
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
               return  new DaoSampleStorage().GetDropDownList();
           }
           catch (Exception)
           {

               throw;
           }
        
       }
    }
}

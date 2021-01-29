using Runbow.TWS.Common;
using Runbow.TWS.Dao.RFWeb;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.Biz.RFWeb
{
    public class InventoryManagementService : BaseService
    {
        public IEnumerable<Inventorys> GetInventoryForRFBySKU(long CustomerID, string WarehouseName, string SKU)
        {
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                return accessor.GetInventoryForRFBySKU(CustomerID, WarehouseName, SKU);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int CheckBoxNumber(long CustomerID, string WarehouseName, string BoxNumber)
        {
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                return accessor.CheckBoxNumber(CustomerID, WarehouseName, BoxNumber);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int MoveLocation(long CustomerID, string WarehouseName, string BoxNumber, string Location, string Creator)
        {
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                return accessor.MoveLocation(CustomerID, WarehouseName, BoxNumber, Location, Creator);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IEnumerable<WarehouseCheck> GetWarehouseCheck(long CustomerID, string WarehouseName)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                return accessor.GetWarehouseCheck(CustomerID, WarehouseName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool InsertCheckDetailList(IEnumerable<WarehouseCheckDetail> checkDetailList, string name, string CheckNumber)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertcheckDetailList(checkDetailList, name, CheckNumber);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }
    }
}

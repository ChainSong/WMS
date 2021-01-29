using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.BarCode;

namespace Runbow.TWS.Biz.WMS
{
    public class BarCodeService:BaseService
    {
        public string GenerateBarCode(IEnumerable<BarCodeInfo> list)
        {
            string message = "";
            try
            {
                BarCodeAccessor accessor = new BarCodeAccessor();
                message = accessor.GenerateBarCode(list);
            }
            catch (Exception)
            {
                
                throw;
            }
            
            return message;
        }
        public string GenerateBarCodeOrder(IEnumerable<BarCodeInfo> list)
        {
            string message = "";
            try
            {
                BarCodeAccessor accessor = new BarCodeAccessor();
                message = accessor.GenerateBarCodeOrder(list);
            }
            catch (Exception)
            {

                throw;
            }

            return message;
        }

        public IEnumerable<BarCodeInfo> GetBarCodeByOrderID(long OrderID, string Type, long DetailID, string SKU)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            return accessor.GetBarCodeByOrderID(OrderID, Type, DetailID, SKU);
        }
        public IEnumerable<BarCodeInfo> GetBarCodeByOID(long OrderID)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            return accessor.GetBarCodeByOID(OrderID);
            
        }
        public IEnumerable<BarCodeInfo> GetBarCodeByDetailIDS(string DetailIDS)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            return accessor.GetBarCodeByDetailIDS(DetailIDS);
        }
        public DataTable GetSKUAndBarCodeByBarCode(string BarCode)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            return accessor.GetSKUAndBarCodeByBarCode(BarCode);
        }
        public IEnumerable<BarCodeInfo> GetBarCodeByIDS(string IDS)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            return accessor.GetBarCodeByIDS(IDS);
        }

        public Response<GetBarCodeByConditionResponse> QueryBarCodeList(GetBarCodeByConditionRequest request)
        {
            Response<GetBarCodeByConditionResponse> response = new Response<GetBarCodeByConditionResponse>();
            try
            {
                int RowCount = 0;
                response.Result = new BarCodeAccessor().QueryBarCodeList(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public string DeleteBarCode(string PackageKey)
        {
            try
            {
                BarCodeAccessor accessor = new BarCodeAccessor();
                if (accessor.DeleteBarCode(PackageKey))
                    return "";
                return "-1";
            }
            catch (Exception ex)
            {
                return "-1";
            }

            
        }

        public IEnumerable<BarCodeInfo> CheckScanBarCode(IEnumerable<BarCodeInfo> list)
        {
            IEnumerable<BarCodeInfo> list2;
            try
            {
                BarCodeAccessor accessor = new BarCodeAccessor();
                list2 = accessor.CheckScanBarCode(list);
            }
            catch (Exception)
            {

                throw;
            }

            return list2;
        }

        public string GenerateBarCodeByScan(long ID, IEnumerable<BarCodeInfo> list)
        {
            string message = "";
            try
            {
                message = new BarCodeAccessor().GenerateBarCodeByScan(ID, list);
            }
            catch (Exception)
            {

                throw;
            }

            return message;
        }

        public string SupplyBarCode(long ID)
        {
            string message = "";
            try
            {
                BarCodeAccessor accessor = new BarCodeAccessor();
                message = accessor.SupplyBarCode(ID);
            }
            catch (Exception)
            {

                throw;
            }

            return message;
        }
        public void GetBarCodeCount(long ID, out int BarCodeCount, out int QtyCount)
        {
            BarCodeAccessor accessor = new BarCodeAccessor();
            accessor.GetBarCodeCount(ID, out BarCodeCount, out QtyCount);
        }
    }
}

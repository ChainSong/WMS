using Runbow.TWS.Common;
using Runbow.TWS.Dao.RFWeb;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Biz.RFWeb
{
    public class ReceiptManagementService : BaseService
    {
        public string GetTarLocation(string receiptnumber, string sku)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetTarLocation(receiptnumber, sku);
            }
            catch
            {
                return "";
            }
        }
        public string GetAreaForLocationAndStore(string location, string receiptNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAreaForLocationAndStore(location, receiptNumber);
            }
            catch
            {
                return "";
            }
        }
        public string CheckBindBoxNumber(string CustomerID, string BoxNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckBindBoxNumber(CustomerID, BoxNumber);
            }
            catch
            {
                return "";
            }
        }
        public string GetAreaForLocationAndStoreBack(string location, string receiptNumber, string BoxNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAreaForLocationAndStoreBack(location, receiptNumber, BoxNumber);
            }
            catch
            {
                return "";
            }
        }
        public int GetAreaForLocationAndStoreBack_Whole(string location, string receiptNumber, string BoxNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAreaForLocationAndStoreBack_Whole(location, receiptNumber, BoxNumber);
            }
            catch
            {
                return 0;
            }
        }
        
        public ReceiptReceiving CheckBoxNumberBackNew(string CustomerID, string BoxNumber, string oldBoxNumber, string ReceiptNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckBoxNumberBackNew(CustomerID, BoxNumber, oldBoxNumber, ReceiptNumber);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 扫描箱号获取库位
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public ReceiptReceiving GetLocationByBoxNumber(string CustomerID, string BoxNumber, string ReceiptNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetLocationByBoxNumber(CustomerID, BoxNumber, ReceiptNumber);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string CheckLocationForAreaAdjustMent(string location, string StoreCode)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationForAreaAdjustMent(location, StoreCode);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string CheckLocationForAreaAdjustMentByCustomerID(string location, string StoreCode, string CustomerID)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationForAreaAdjustMentByCustomerID(location, StoreCode, CustomerID);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string CheckAdjustMentStatus(string AdjustMentNumber)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckAdjustMentStatus(AdjustMentNumber);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string CheckLocationSKUAdjustMent(string location, string SKU, string StoreCode)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationSKUAdjustMent(location, SKU, StoreCode);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public List<Inventorys> CheckLocationSKUAdjustMentByCustomerID(string location, string SKU, string StoreCode, string CustomerID)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationSKUAdjustMentByCustomerID(location, SKU, StoreCode, CustomerID);
            }
            catch (Exception ex)
            {
                return new List<Inventorys>();
            }
        }

        public string CheckLocationByNewLocationAdjustMent(string Area, string location, string StoreCode)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationByNewLocationAdjustMent(Area, location, StoreCode);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string CheckLocationByNewLocationAdjustMentByCustomerID(string Area, string location, string StoreCode, string CustomerID)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.CheckLocationByNewLocationAdjustMentByCustomerID(Area, location, StoreCode, CustomerID);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public IEnumerable<Inventorys> GetLocationAndQtyBySKU(string Area, string SKU, string StoreCode)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetLocationAndQtyBySKU(Area, SKU, StoreCode);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Inventorys> GetLocationAndQtyBySKUByCustomerID(string Area, string SKU, string StoreCode, string CustomerID)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetLocationAndQtyBySKUByCustomerID(Area, SKU, StoreCode, CustomerID);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ReceiptDetail> GetReceiptDetailListByProc(string receiptnum, string customerid, string warehousename, string ProcName)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptDetailListByProc(receiptnum, customerid, warehousename, ProcName);

            }
            catch (Exception ex)
            {
                return new List<ReceiptDetail>();

            }

        }
        public IEnumerable<Receipt> GetReceiptList(string customerid, string warehouseid, string ExternNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptList(customerid, warehouseid, ExternNumber);

            }
            catch (Exception ex)
            {
                return new List<Receipt>();

            }

        }
        public IEnumerable<ASN> GetASNList(string customerid, string warehouseid, string ExternNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAsnList(customerid, warehouseid, ExternNumber);

            }
            catch (Exception ex)
            {
                return new List<ASN>();

            }

        }
        public IEnumerable<PackageInfo> GetPackageDtailRF(string StoreCode, string OrderTime, string OrderType)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetPackageDtailRF(StoreCode, OrderTime, OrderType);

            }
            catch (Exception ex)
            {
                return new List<PackageInfo>();

            }

        }

        public IEnumerable<Adjustment> GetAdjustmentList(string customerid, string AdjustNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAdjustmentList(customerid, AdjustNumber);

            }
            catch (Exception ex)
            {
                return new List<Adjustment>();

            }

        }
        public IEnumerable<Adjustment> GetAdjustmentListBatch(string customerid, string AdjustNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAdjustmentListBatch(customerid, AdjustNumber);

            }
            catch (Exception ex)
            {
                return new List<Adjustment>();

            }

        }
        public IEnumerable<WarehouseCheck> GetPDList(string PDNumber, string CustomerID)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetPDList(PDNumber, CustomerID);

            }
            catch (Exception ex)
            {
                return new List<WarehouseCheck>();

            }

        }
        /// <summary>
        /// RF查询库存
        /// </summary>
        /// <param name="ScanNumber"></param>
        /// <returns></returns>
        public IEnumerable<Inventorys> GetInventoryList(string ScanNumber, string CustomerID)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetInventoryList(ScanNumber, CustomerID);

            }
            catch (Exception ex)
            {
                return new List<Inventorys>();

            }

        }
        public IEnumerable<AdjustmentDetail> GetAdjustmentDetailList(string AdjustNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAdjustmentDetailList(AdjustNumber);

            }
            catch (Exception ex)
            {
                return new List<AdjustmentDetail>();

            }

        }

        public IEnumerable<WarehouseCheckDetail> GetWarehouseCheckDetailList(string PDNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetWarehouseCheckDetailList(PDNumber);

            }
            catch (Exception ex)
            {
                return new List<WarehouseCheckDetail>();

            }

        }
        /// <summary>
        /// 查看自己领用的箱子进展
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IEnumerable<WMS_Log_OperationRF> GetBoxNumStatusList(string username)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetBoxNumStatusList(username);

            }
            catch (Exception ex)
            {
                return new List<WMS_Log_OperationRF>();

            }

        }
        public GetShelvesByConditionResponse GetReceiptByBoxNum(string BoxNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptByBoxNum(BoxNumber);

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// 获取当前用户领用数量
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public GetShelvesByConditionResponse GetRFLogQty(string username)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetRFLogQty(username);

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// 获取入库单
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public Receipt GetReceipt(string ReceiptNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceipt(ReceiptNumber);

            }
            catch (Exception ex)
            {
                return new Receipt();

            }

        }
        /// <summary>
        /// 获取ASN
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public ASN GetASN(string ASNNumber)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAsn(ASNNumber);

            }
            catch (Exception ex)
            {
                return new ASN();

            }

        }
        public IEnumerable<ReceiptDetail> GetReceiptDetailList(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptDetailList(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptDetail>();

            }

        }
        public IEnumerable<ReceiptDetail> GetReceiptDetailList_CustomerID(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptDetailList_CustomerID(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptDetail>();

            }

        }

        public IEnumerable<SKUFloar> GetSKUFloar()
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetSKUFloar();

            }
            catch (Exception ex)
            {
                return new List<SKUFloar>();

            }

        }
        public IEnumerable<ASNDetail> GetAsnDetailListArticle(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAsnDetailListArticle(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ASNDetail>();

            }

        }
        public IEnumerable<ASNDetail> GetAsnDetailList(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAsnDetailList(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ASNDetail>();

            }

        }

        public IEnumerable<ASNDetail> GetAsnDetailListABC(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetAsnDetailListABC(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ASNDetail>();

            }

        }

        public IEnumerable<ReceiptReceiving> GetReceiptReceivingList(string receiptnum, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptReceivingList(receiptnum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptReceiving>();

            }

        }
        public IEnumerable<ReceiptReceiving> GetReceiptReceivingListByStr2(string str2, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptReceivingListByStr2(str2, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptReceiving>();

            }

        }
        public IEnumerable<ReceiptReceiving> GetReceiptReceivingListByReceiptNumberAndStr2(string ReceiptNumber, string str2, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptReceivingListByReceiptNumberAndStr2(ReceiptNumber, str2, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptReceiving>();

            }

        }
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool InsertReceiptReceiving(IEnumerable<ReceiptReceiving> receiptReceivinglist, string name)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertReceiptReceiving(receiptReceivinglist, name);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
                throw ex;
            }
            return IsSuccess;
        }
        /// <summary>
        /// ABC扫描的时候回退一箱
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool DeleteScanABCByBoxNumber(string AsnNumber, string boxnumber)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.DeleteScanABCByBoxNumber(AsnNumber, boxnumber);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
                throw ex;
            }
            return IsSuccess;
        }
        /// <summary>
        /// RF上架本箱重扫
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public bool DeleteScanBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.DeleteScanBoxNumber(ReceiptNumber, BoxNumber);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
                throw ex;
            }
            return IsSuccess;
        }
        /// <summary>
        /// 提交ABC扫描数据
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool InsertAsnDetailScanABC(IEnumerable<ASNDetail> list)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertAsnDetailScanABC(list);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
                throw ex;
            }
            return IsSuccess;
        }
        /// <summary>
        /// RF盘点提交数据
        /// </summary>
        /// <param name="warehouseCheckDetails"></param>
        /// <returns></returns>
        public bool InsertWarehouseCheckScan(IEnumerable<WarehouseCheckDetail> warehouseCheckDetails)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertWarehouseCheckScan(warehouseCheckDetails);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }
        public bool UpdateReceiptStatus(string ReceiptNumber, string name)
        {
            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                IsSuccess = accessor.UpdateReceiptStatus(ReceiptNumber, name);
            }
            catch (Exception ex)
            {

            }
            return IsSuccess;
        }
        public Response<string> AddReceiptAndReceiptDetail_ScanSKU(AddReceiptAndReceiptDetailRequest request, long CustomerID, string Creator)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail_ScanSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ReceiptManagementAccessor().AddReceiptAndReceiptDetail_ScanSKU(request, CustomerID, Creator);
                if (message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;
                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }
        public bool WarehouseCheckOverRF(string PDNumber)
        {
            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                IsSuccess = accessor.WarehouseCheckOverRF(PDNumber);
            }
            catch (Exception ex)
            {

            }
            return IsSuccess;
        }
        /// <summary>
        /// RF创建移库单
        /// </summary>
        /// <param name="adjustment"></param>
        /// <returns></returns>
        public bool AddAdjustMentRF(List<Adjustment> adjustment)
        {
            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                IsSuccess = accessor.AddAdjustMentRF(adjustment);
            }
            catch (Exception ex)
            {

            }
            return IsSuccess;
        }
        public bool AdjustMentDetailDeleteRF(long ID)
        {
            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                IsSuccess = accessor.AdjustMentDetailDeleteRF(ID);
            }
            catch (Exception ex)
            {

            }
            return IsSuccess;
        }

        public string AdjustMentCompleteRF(string adjustmentnumber)
        {
            string msg = "";
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                msg = accessor.AdjustMentCompleteRF(adjustmentnumber);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public string AdjustMentCompleteRFReturn(string adjustmentnumber)
        {
            string msg = "";
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                msg = accessor.AdjustMentCompleteRFReturn(adjustmentnumber);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public string AddAdjustMentDetailRF(List<AdjustmentDetail> adjustmentdetails)
        {
            string message = "";
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                message = accessor.AddAdjustMentDetailRF(adjustmentdetails);
            }
            catch (Exception ex)
            {
                message = ex.Message;

            }
            return message;
        }
        public bool DeleteRecByBoxnumber(string ReceiptNumber, string Boxnumber, string Username)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.DeleteRecByBoxnumber(ReceiptNumber, Boxnumber, Username);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }
        public bool InsertReceiptReceivingByStr2(IEnumerable<ReceiptReceiving> receiptReceivinglist, string name, string str2, out string msg)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };
            msg = "";
            bool IsSuccess = false;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertReceiptReceivingByStr2(receiptReceivinglist, name, str2);
                //response.IsSuccess = true;
                msg = "";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }

        public bool IsExist(string receiptnum)
        {
            ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
            bool bl = accessor.IsExist(receiptnum);
            return bl;
        }

        public IEnumerable<ReceiptDetail> GetReceiptDetailListByReceiptNumberAndStr2(string receiptnum, string str2, string customerid, string warehousename)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetReceiptDetailListByReceiptNumberAndStr2(receiptnum, str2, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<ReceiptDetail>();

            }

        }

        /// <summary>
        /// 爱库存RF登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public DataSet RFLogin(string UserName, string Password)
        {

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.RFLogin(UserName, Password);

            }
            catch (Exception ex)
            {
                return null;

            }

        }

        /// <summary>
        /// 验证是否存在托盘号,库位号
        /// </summary>
        /// <param name="TrayNum"></param>
        /// <param name="WarehouseID"></param>
        /// <param name="Status">0：库位表，其他：Tray表</param>
        /// <returns></returns>
        public bool IsExistTrayNumBindTray(string TrayNum, string WarehouseID, int Status = 0)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.IsExistTrayNumBindTray(TrayNum, WarehouseID, Status);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<ASNDetail> GetASNDetailScanBindTray(string BoxNum, string CustomerID)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetASNDetailScanBindTray(BoxNum, CustomerID);
            }
            catch (Exception ex)
            {
                return new List<ASNDetail>();
            }
        }
        
         public  int UpdateReceiptReceivingByLocationBack_Whole(string name,string Warehouse, string ReceiptNumber, string BoxNumber, string Location)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.UpdateReceiptReceivingByLocationBack_Whole(name, Warehouse,ReceiptNumber, BoxNumber, Location);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int StagingLocation(string name, string Warehouse, string ReceiptNumber, string BoxNumber )
        {
            try
            {
                 
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.StagingLocation(name, Warehouse, ReceiptNumber, BoxNumber);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string AgainRecommended(string CustomerID, string name, string Warehouse, string ReceiptNumber, string BoxNumber)
        {
            try
            {

                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.AgainRecommended(CustomerID,name, Warehouse, ReceiptNumber, BoxNumber);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
        public IEnumerable<ASNScanTray> GetTrayNumList(string TrayNum, int Status)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.GetTrayNumList(TrayNum, Status);
            }
            catch (Exception ex)
            {
                return new List<ASNScanTray>();
            }
        }

        /// <summary>
        /// Tray表新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Creator"></param>
        /// <param name="Status">0：箱绑托，1：托绑库位</param>
        /// <returns></returns>
        public IEnumerable<ASNScanTray> AddASNScanTray(IEnumerable<ASNScanTray> list, string Creator, int Status = 0)
        {
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                return accessor.AddASNScanTray(list, Creator, Status);
            }
            catch (Exception ex)
            {
                return new List<ASNScanTray>();
            }
        }

    }
}

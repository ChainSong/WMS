using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using System.Data;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS.Warehouse;
using System.Windows.Forms;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Dao.RFWeb
{
    public class ReceiptManagementAccessor : BaseAccessor
    {

        public string GetAreaForLocationAndStore(string location, string receiptNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                         new DbParam("@ReceiptNumber", DbType.String,receiptNumber,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByLocationAndRec", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string CheckBindBoxNumber(string CustomerID, string BoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input),
                         new DbParam("@BoxNumber", DbType.String,BoxNumber,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_CheckBindBoxNumber", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string GetAreaForLocationAndStoreBack(string location, string receiptNumber, string BoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                         new DbParam("@ReceiptNumber", DbType.String,receiptNumber,ParameterDirection.Input),
                         new DbParam("@BoxNumber", DbType.String,BoxNumber,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByLocationAndRecBackNew", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }


        public ReceiptReceiving CheckBoxNumberBackNew(string CustomerID, string BoxNumber, string oldBoxNumber, string ReceiptNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input),
                         new DbParam("@BoxNumber", DbType.String,BoxNumber,ParameterDirection.Input),
                         new DbParam("@oldBoxNumber", DbType.String,oldBoxNumber,ParameterDirection.Input),
                         new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_CheckBoxNumberBackNew", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].ConvertToEntity<ReceiptReceiving>();
                }
                else
                {
                    return null;
                }
            }
        }
        public ReceiptReceiving GetLocationByBoxNumber(string CustomerID, string BoxNumber, string ReceiptNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input),
                         new DbParam("@BoxNumber", DbType.String,BoxNumber,ParameterDirection.Input),
                         new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                    };
                DataSet ds = ExecuteDataSet("Proc_GetLocationByBoxNumber", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].ConvertToEntity<ReceiptReceiving>();
                }
                else
                {
                    return null;
                }
            }
        }
        public string CheckLocationForAreaAdjustMent(string location, string StoreCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByLocationAdjustMent", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string CheckLocationForAreaAdjustMentByCustomerID(string location, string StoreCode, string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByLocationAdjustMentByCustomerID", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string CheckAdjustMentStatus(string AdjustMentNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@AdjustMentNumber", DbType.String,AdjustMentNumber,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAdjustMentStatusByAdjustMentNumber", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string CheckLocationSKUAdjustMent(string location, string SKU, string StoreCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@SKU", DbType.String,SKU,ParameterDirection.Input),
                         new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetQtyByLocationSKUAdjustMent", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public List<Inventorys> CheckLocationSKUAdjustMentByCustomerID(string location, string SKU, string StoreCode, string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Loction", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@SKU", DbType.String,SKU,ParameterDirection.Input),
                         new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input),
                          new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetQtyByLocationSKUAdjustMentByCustomerID", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].ConvertToEntityCollection<Inventorys>().ToList();
                }
                else
                {
                    return new List<Inventorys>();
                }
            }
        }
        public string CheckLocationByNewLocationAdjustMent(string Area, string location, string StoreCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Area", DbType.String,Area,ParameterDirection.Input),
                        new DbParam("@Location", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByByNewLocationAdjustMent", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string CheckLocationByNewLocationAdjustMentByCustomerID(string Area, string location, string StoreCode, string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Area", DbType.String,Area,ParameterDirection.Input),
                        new DbParam("@Location", DbType.String,location,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input),
                        new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetAreaByByNewLocationAdjustMentByCustomerID", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public IEnumerable<Inventorys> GetLocationAndQtyBySKU(string Area, string SKU, string StoreCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Area", DbType.String,Area,ParameterDirection.Input),
                        new DbParam("@SKU", DbType.String,SKU,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetLocationAndQtyBySKUAdjustMent", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].ConvertToEntityCollection<Inventorys>();
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<Inventorys> GetLocationAndQtyBySKUByCustomerID(string Area, string SKU, string StoreCode, string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@Area", DbType.String,Area,ParameterDirection.Input),
                        new DbParam("@SKU", DbType.String,SKU,ParameterDirection.Input),
                        new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetLocationAndQtyBySKUAdjustMentByCustomerID", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].ConvertToEntityCollection<Inventorys>();
                }
                else
                {
                    return null;
                }
            }
        }
        public string GetTarLocation(string receipt, string sku)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,receipt,ParameterDirection.Input),
                         new DbParam("@SKU", DbType.String,sku,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_GetTarLocation", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public IEnumerable<ReceiptDetail> GetReceiptDetailListByProc(string ReceiptNumber, string customerid, string warehousename, string ProcName)
        {
            IEnumerable<ReceiptDetail> list = new List<ReceiptDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet(ProcName, param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptDetail>();
                    return list;
                }

            }
        }
        public IEnumerable<Receipt> GetReceiptList(string customerid, string warehouseid, string ExternNumber)
        {
            IEnumerable<Receipt> list = new List<Receipt>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseID", DbType.String,warehouseid,ParameterDirection.Input),
                              new DbParam("@ExternNumber", DbType.String,ExternNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetReceiptList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<Receipt>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<Receipt>();
                    return list;
                }

            }
        }
        public IEnumerable<ASN> GetAsnList(string customerid, string warehouseid, string ExternNumber)
        {
            IEnumerable<ASN> list = new List<ASN>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseID", DbType.String,warehouseid,ParameterDirection.Input),
                              new DbParam("@ExternNumber", DbType.String,ExternNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAsnList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ASN>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ASN>();
                    return list;
                }

            }
        }
        public IEnumerable<PackageInfo> GetPackageDtailRF(string StoreCode, string OrderTime, string OrderType)
        {
            IEnumerable<PackageInfo> list = new List<PackageInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@StoreCode", DbType.String,StoreCode,ParameterDirection.Input),
                              new DbParam("@OrderTime", DbType.String,OrderTime,ParameterDirection.Input),
                              new DbParam("@OrderType", DbType.String,OrderType,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetPackageDtailRF", param);
                    list = ds.Tables[0].ConvertToEntityCollection<PackageInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<PackageInfo>();
                    return list;
                }

            }
        }
        public IEnumerable<Adjustment> GetAdjustmentList(string customerid, string AdjustNumber)
        {
            IEnumerable<Adjustment> list = new List<Adjustment>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@AdjustNumber", DbType.String,AdjustNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAdjustmentList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<Adjustment>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<Adjustment>();
                    return list;
                }

            }
        }

        public IEnumerable<Adjustment> GetAdjustmentListBatch(string customerid, string AdjustNumber)
        {
            IEnumerable<Adjustment> list = new List<Adjustment>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@AdjustNumber", DbType.String,AdjustNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAdjustmentListBatch", param);
                    list = ds.Tables[0].ConvertToEntityCollection<Adjustment>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<Adjustment>();
                    return list;
                }

            }
        }

        public IEnumerable<WarehouseCheck> GetPDList(string PDNumber, string CustomerID)
        {
            IEnumerable<WarehouseCheck> list = new List<WarehouseCheck>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                              new DbParam("@PDNumber", DbType.String,PDNumber,ParameterDirection.Input),
                              new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetPDListByCustomerID", param);
                    //DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetPDList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<WarehouseCheck>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<WarehouseCheck>();
                    return list;
                }

            }
        }

        public IEnumerable<Inventorys> GetInventoryList(string ScanNumber, string CustomerID)
        {
            IEnumerable<Inventorys> list = new List<Inventorys>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                              new DbParam("@ScanNumber", DbType.String,ScanNumber,ParameterDirection.Input),
                              new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetInventoryListByCustomerID", param);
                    list = ds.Tables[0].ConvertToEntityCollection<Inventorys>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<Inventorys>();
                    return list;
                }

            }
        }

        public IEnumerable<AdjustmentDetail> GetAdjustmentDetailList(string AdjustNumber)
        {
            IEnumerable<AdjustmentDetail> list = new List<AdjustmentDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                              new DbParam("@AdjustNumber", DbType.String,AdjustNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAdjustmentDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<AdjustmentDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<AdjustmentDetail>();
                    return list;
                }

            }
        }

        public IEnumerable<WarehouseCheckDetail> GetWarehouseCheckDetailList(string PDNumber)
        {
            IEnumerable<WarehouseCheckDetail> list = new List<WarehouseCheckDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                              new DbParam("@PDNumber", DbType.String,PDNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetWarehouseCheckDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<WarehouseCheckDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<WarehouseCheckDetail>();
                    return list;
                }

            }
        }
        public IEnumerable<WMS_Log_OperationRF> GetBoxNumStatusList(string username)
        {
            IEnumerable<WMS_Log_OperationRF> list = new List<WMS_Log_OperationRF>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@username", DbType.String,username,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetBoxNumStatusList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<WMS_Log_OperationRF>();
                    return list;
                }
                catch (Exception ex)
                {
                    list = new List<WMS_Log_OperationRF>();
                    return list;
                }

            }
        }
        public GetShelvesByConditionResponse GetReceiptByBoxNum(string BoxNumber)
        {
            GetShelvesByConditionResponse shelvesByConditionResponse = new GetShelvesByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@BoxNumber", DbType.String,BoxNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetReceiptByBoxNum", param);
                    shelvesByConditionResponse.storesByGetReceipt = ds.Tables[0].ConvertToEntity<StoresByGetReceipt>();
                    shelvesByConditionResponse.Shelves = ds.Tables[1].ConvertToEntityCollection<Shelves>();
                    return shelvesByConditionResponse;
                }
                catch (Exception ex)
                {
                    return shelvesByConditionResponse;
                }

            }
        }

        public GetShelvesByConditionResponse GetRFLogQty(string username)
        {
            GetShelvesByConditionResponse shelvesByConditionResponse = new GetShelvesByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@username", DbType.String,username,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetRFLogQty", param);
                    shelvesByConditionResponse.ReceiptDetailCollection = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    return shelvesByConditionResponse;
                }
                catch (Exception ex)
                {
                    return shelvesByConditionResponse;
                }

            }
        }

        /// <summary>
        /// 获取入库单
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public Receipt GetReceipt(string ReceiptNumber)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                Receipt receipt = new Receipt();
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input)
                    };
                    DataTable dt = ExecuteDataTable("Proc_WMS_RF_GetReceipt", param);
                    receipt = dt.ConvertToEntity<Receipt>();
                    return receipt;

                }
                catch (Exception ex)
                {

                    return receipt;
                }

            }
        }/// <summary>
         /// 获取ASN
         /// </summary>
         /// <param name="ReceiptNumber"></param>
         /// <returns></returns>
        public ASN GetAsn(string AsnNumber)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                ASN receipt = new ASN();
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@AsnNumber", DbType.String,AsnNumber,ParameterDirection.Input)
                    };
                    DataTable dt = ExecuteDataTable("Proc_WMS_RF_GetAsn", param);
                    receipt = dt.ConvertToEntity<ASN>();
                    return receipt;

                }
                catch (Exception ex)
                {

                    return receipt;
                }

            }
        }
        public IEnumerable<ReceiptDetail> GetReceiptDetailList(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<ReceiptDetail> list = new List<ReceiptDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                           new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetReceiptDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptDetail>();
                    return list;
                }

            }
        }
        public IEnumerable<ReceiptDetail> GetReceiptDetailList_CustomerID(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<ReceiptDetail> list = new List<ReceiptDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                           new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetReceiptDetailList_CustomerID", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptDetail>();
                    return list;
                }

            }
        }

        public IEnumerable<SKUFloar> GetSKUFloar()
        {
            IEnumerable<SKUFloar> list = new List<SKUFloar>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {

                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetSKUFloar", param);
                    list = ds.Tables[0].ConvertToEntityCollection<SKUFloar>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<SKUFloar>();
                    return list;
                }

            }
        }
        public IEnumerable<ASNDetail> GetAsnDetailListArticle(string AsnNumber, string customerid, string warehousename)
        {
            IEnumerable<ASNDetail> list = new List<ASNDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@AsnNumber", DbType.String,AsnNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                           new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAsnDetailListArticle", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ASNDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ASNDetail>();
                    return list;
                }

            }
        }
        public IEnumerable<ASNDetail> GetAsnDetailList(string AsnNumber, string customerid, string warehousename)
        {
            IEnumerable<ASNDetail> list = new List<ASNDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@AsnNumber", DbType.String,AsnNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                           new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAsnDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ASNDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ASNDetail>();
                    return list;
                }

            }
        }

        public IEnumerable<ASNDetail> GetAsnDetailListABC(string AsnNumber, string customerid, string warehousename)
        {
            IEnumerable<ASNDetail> list = new List<ASNDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@AsnNumber", DbType.String,AsnNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                           new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetAsnDetailListABC", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ASNDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ASNDetail>();
                    return list;
                }

            }
        }

        public IEnumerable<ReceiptReceiving> GetReceiptReceivingList(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<ReceiptReceiving> list = new List<ReceiptReceiving>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_ReceiptReceivingList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptReceiving>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptReceiving>();
                    return list;
                }

            }
        }

        public IEnumerable<ReceiptReceiving> GetReceiptReceivingListByStr2(string str2, string customerid, string warehousename)
        {
            IEnumerable<ReceiptReceiving> list = new List<ReceiptReceiving>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@str2", DbType.String,str2,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_ReceiptReceivingListByStr2", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptReceiving>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptReceiving>();
                    return list;
                }

            }
        }

        public bool DeleteRecByBoxnumber(string ReceiptNumber, string Boxnumber, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_RFDeleteRecByStr2]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptNumber", ReceiptNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;


                cmd.Parameters.AddWithValue("@Boxnumber", Boxnumber);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;

                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;

                conn.Open();
                int defcount = cmd.ExecuteNonQuery();
                conn.Close();
                if (defcount > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool InsertReceiptReceiving(IEnumerable<ReceiptReceiving> Request, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                bool message = false;
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddReceiptReceiving_RF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Receipt", Request.Select(p => new ReceiptReceivingToDbRF(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    conn.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        message = true;
                    }
                    else
                    {
                        message = false;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return message;
            }
        }
        public bool DeleteScanABCByBoxNumber(string AsnNumber, string boxnumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_DeleteScanABCByBoxNumber_RF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnNumber", AsnNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@boxnumber", boxnumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    if (message == "删除成功")
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        public bool DeleteScanBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_DeleteScanBoxNumber_RF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptNumber", ReceiptNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@BoxNumber", BoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    if (message == "删除成功")
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        public bool InsertAsnDetailScanABC(IEnumerable<ASNDetail> Request)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                bool message = false;
                try
                {
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddAsnDetailScanABC_RF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AsnDetail", Request.Select(p => new WMSASNDetailToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    conn.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        message = true;
                    }
                    else
                    {
                        message = false;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return message;
            }
        }
        public bool InsertWarehouseCheckScan(IEnumerable<WarehouseCheckDetail> Request)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddCheckDetailScan_RF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WarehouseCheckDetail", Request.Select(p => new WarehouseCheckDetailToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (message == "")
                {
                    return true;
                }
                return false;
            }
        }
        public bool UpdateReceiptStatus(string ReceiptNumber, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddReceiptReceiving_RF_UpdateStatus]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptNumber", ReceiptNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                int defcount = cmd.ExecuteNonQuery();
                conn.Close();
                if (defcount > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public string AddReceiptAndReceiptDetail_ScanSKU(AddReceiptAndReceiptDetailRequest rece, long CustomerID, string Creator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    IList<Receipt> result = new List<Receipt>();
                    IList<ReceiptDetail> receiptDetail = new List<ReceiptDetail>();
                    //SqlCommand cmd = new SqlCommand("Proc_WMS_AddReceiptANDReceiptDetail_ScanSKU", conn);
                    //SqlCommand cmd = new SqlCommand("Proc_WMS_AddReceiptANDReceiptDetail_ScanSKU_ByLocationLabel", conn);
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddReceiptANDReceiptDetail_ScanSKU_ByLocationLabelNew", conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@ReceiptDetail", rece.ReceiptDetails.Select(receiptDetali => new WMSReceiptDetailToDb(receiptDetali)));
                    cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }
        }
        public bool WarehouseCheckOverRF(string PDNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_WarehouseCheckOverRF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PDNumber", PDNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                int defcount = cmd.ExecuteNonQuery();
                conn.Close();
                if (defcount > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool AddAdjustMentRF(List<Adjustment> adjustment)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddAdjustMentRF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Adjustment", adjustment.Select(a => new WMSAdjutmentToDb(a)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (message == "添加成功")
                {
                    return true;
                }
                return false;
            }
        }

        public bool AdjustMentDetailDeleteRF(long ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AdjustMentDetailDeleteRF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (message == "删除成功")
                {
                    return true;
                }
                return false;
            }
        }

        public string AdjustMentCompleteRF(string adjustmentnumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AdjustMentCompleteRF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adjustmentnumber", adjustmentnumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Inventorytype", "库存移动单");
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }
        }

        public string AdjustMentCompleteRFReturn(string adjustmentnumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AdjustMentCompleteRF_Return]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adjustmentnumber", adjustmentnumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Inventorytype", "库存移动单");
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }
        }
        public string AddAdjustMentDetailRF(List<AdjustmentDetail> adjustmentdetails)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddAdjustMentDetailRF]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdjustmentDetail", adjustmentdetails.Select(a => new WMSAdjutmentDetailToDb(a)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].Size = 50;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }

        }
        public bool InsertReceiptReceivingByStr2(IEnumerable<ReceiptReceiving> Request, string UserName, string str2)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_AddReceiptReceivingByStr2]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@str2", str2);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Receipt", Request.Select(p => new ReceiptReceivingToDb(p)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsExist(string receiptnum)
        {
            DbParam[] param = new DbParam[] {
                new DbParam("@ReceiptNumber", DbType.String,receiptnum,ParameterDirection.Input),
            };
            var obj = ExecuteScalar("IsExistReceipt", param);
            int count = Convert.ToInt32(obj);
            if (count > 0)
                return true;
            else
                return false;
        }

        public IEnumerable<ReceiptReceiving> GetReceiptReceivingListByReceiptNumberAndStr2(string ReceiptNumber, string str2, string customerid, string warehousename)
        {
            IEnumerable<ReceiptReceiving> list = new List<ReceiptReceiving>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,str2,ParameterDirection.Input),
                        new DbParam("@str2", DbType.String,str2,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_ReceiptReceivingListByReceiptNumberAndStr2", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptReceiving>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptReceiving>();
                    return list;
                }

            }
        }


        public IEnumerable<ReceiptDetail> GetReceiptDetailListByReceiptNumberAndStr2(string ReceiptNumber, string str2, string customerid, string warehousename)
        {
            IEnumerable<ReceiptDetail> list = new List<ReceiptDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                        new DbParam("@ReceiptNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                        new DbParam("@PackageNumber", DbType.String,str2,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetReceiptDetailList_YXDRByPackageNumber", param);
                    list = ds.Tables[0].ConvertToEntityCollection<ReceiptDetail>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<ReceiptDetail>();
                    return list;
                }

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
            IEnumerable<ReceiptDetail> list = new List<ReceiptDetail>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                        new DbParam("@Name", DbType.String,UserName,ParameterDirection.Input),
                        new DbParam("@Password", DbType.String,Password,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("RF_Login_AKC", param);
                    return ds;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public bool IsExistTrayNumBindTray(string TrayNum, string WarehouseID, int Status)
        {
            DbParam[] dbParams = new DbParam[] {
                new DbParam("@TrayNum",DbType.String,TrayNum,ParameterDirection.Input),
                new DbParam("@WarehouseID",DbType.String,WarehouseID,ParameterDirection.Input),
                new DbParam("@Status",DbType.Int32,Status,ParameterDirection.Input)
            };
            DataTable dt = base.ExecuteDataTable("Proc_IsExistTrayNumBindTray", dbParams);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            return false;
        }

        public IEnumerable<ASNScanTray> GetTrayNumList(string TrayNum, int Status)
        {
            DbParam[] dbParams = new DbParam[] {
                new DbParam("@TrayNum",DbType.String,TrayNum,ParameterDirection.Input),
                new DbParam("@Status",DbType.Int32,Status,ParameterDirection.Input)
            };
            DataTable dt = base.ExecuteDataTable("Proc_GetTrayNumList", dbParams);
            return dt.ConvertToEntityCollection<ASNScanTray>();
        }

        public IEnumerable<ASNDetail> GetASNDetailScanBindTray(string BoxNum, string CustomerID)
        {
            DbParam[] dbParams = new DbParam[] {
                new DbParam("@BoxNum",DbType.String,BoxNum,ParameterDirection.Input),
                new DbParam("@CustomerID",DbType.String,CustomerID,ParameterDirection.Input)
            };
            DataTable dt = base.ExecuteDataTable("Proc_GetASNDetailScanBindTray", dbParams);
            return dt.ConvertToEntityCollection<ASNDetail>();
        }

        public IEnumerable<ASNScanTray> AddASNScanTray(IEnumerable<ASNScanTray> list, string Creator, int Status)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                SqlCommand cmd = new SqlCommand("Proc_AddASNScanTray", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Trays", list.Select(i => new ASNScanTrayToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Creator", Creator);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[1].Size = 50;
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[3].Size = 50;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].ToString();
                return dt.ConvertToEntityCollection<ASNScanTray>();
            }
        }

        public int UpdateReceiptReceivingByLocationBack_Whole(string Name, string Warehouse, string ReceiptNumber, string BoxNumber, string Location)
        {
            string StrSql = @"insert into [WMS_ReceiptReceiving] 
	([RID]
      ,[RDID]
      ,[ReceiptNumber]
      ,[ExternReceiptNumber]
      ,[CustomerID]
      ,[CustomerName]
      ,[LineNumber]
      ,[SkuLineNumber]
      ,[SKU]
      ,[UPC]
      ,[QtyReceived]
      ,[Warehouse]
      ,[GoodsName]
      ,[Area]
      ,[Location]
      ,[GoodsType]
      ,[BoxNumber]
      ,[BatchNumber]
      ,[Unit]
      ,[Specifications]
      ,[Creator]
      ,[CreateTime]
      ,[Updator]
      ,[UpdateTime]
      ,[str1]
      ,[str2]
      ,[str3]
      ,[str4]
      ,[str5]
      ,[str6]
      ,[str7]
      ,[str8]
      ,[str9]
      ,[str10]
      ,[str11]
      ,[str12]
      ,[str13]
      ,[str14]
      ,[str15]
      ,[str16]
      ,[str17]
      ,[str18]
      ,[str19]
      ,[str20]
      ,[DateTime1]
      ,[DateTime2]
      ,[DateTime3]
      ,[DateTime4]
      ,[DateTime5]
      ,[Int1]
      ,[Int2]
      ,[Int3]
      ,[Int4]
      ,[Int5]
      ,[Remark]
      ,[Status])
	(select rd.[RID]
      ,rd.ID
      ,rd.[ReceiptNumber]
      ,rd.[ExternReceiptNumber]
      ,rd.[CustomerID]
      ,rd.[CustomerName]
      ,rd.[LineNumber]
      ,rd.[LineNumber]
      ,rd.[SKU]
      ,rd.[UPC]
      ,rd.[QtyReceived]
      ,'" + Warehouse + @"'
      ,rd.[GoodsName]
      ,it.[Area]
      ,it.[Location]
      ,rd.[GoodsType]
      ,rd.[BoxNumber]
      ,rd.[BatchNumber]
      ,rd.[Unit]
      ,rd.[Specifications]
      ,'" + Name + @"'
      ,getdate()
      ,''
      ,''
      ,rd.[str1]
      ,rd.[str2]
      ,rd.[str3]
      ,rd.[str4]
      ,rd.[str5]
      ,rd.[str6]
      ,rd.[str7]
      ,rd.[str8]
      ,rd.[str9]
      ,rd.[str10]
      ,rd.[str11]
      ,rd.[str12]
      ,rd.[str13]
      ,rd.[str14]
      ,rd.[str15]
      ,rd.[str16]
      ,rd.[str17]
      ,rd.[str18]
      ,rd.[str19]
      ,rd.[str20]
      ,rd.[DateTime1]
      ,rd.[DateTime2]
      ,rd.[DateTime3]
      ,rd.[DateTime4]
      ,rd.[DateTime5]
      ,rd.[Int1]
      ,rd.[Int2]
      ,rd.[Int3]
      ,rd.[Int4]
      ,rd.[Int5]
      ,''
      ,1 from  WMS_Inventory_103_Temp it 
	left join [dbo].[WMS_ReceiptDetail] rd
	on it.RDID =rd.ID 
	where it.Status=1 and  it.Str2='" + BoxNumber + @"' and it.ReceiptNumber='" + ReceiptNumber + "'  )" +
    "  update  WMS_Inventory_103_Temp set Status=9 where  Status=1 and  Str2='" + BoxNumber + @"' and ReceiptNumber='" + ReceiptNumber + @"' ";
            return this.ScanExecuteNonQuery(StrSql);

        }
        /// <summary>
        /// Check 库位是不是和推荐库位一致
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public int GetAreaForLocationAndStoreBack_Whole(string location, string receiptNumber, string BoxNumber)
        {
            string StrSql = @"select * from WMS_Inventory_103_Temp where ReceiptNumber='" + receiptNumber + @"' and  Location='" + location + @"' and Str2='" + BoxNumber + @"'";
            return this.ScanExecuteNonQuery(StrSql);

        }

        /// <summary>
        /// 库位放不下，将货物放到暂存库位
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public int StagingLocation(string Name, string Warehouse, string ReceiptNumber, string BoxNumber)
        {
            string StrSql = @"insert into [WMS_ReceiptReceiving] 
	        ([RID]
           ,[RDID]
           ,[ReceiptNumber]
           ,[ExternReceiptNumber]
           ,[CustomerID]
           ,[CustomerName]
           ,[LineNumber]
           ,[SkuLineNumber]
           ,[SKU]
           ,[UPC]
           ,[QtyReceived]
           ,[Warehouse]
           ,[GoodsName]
           ,[Area]
           ,[Location]
           ,[GoodsType]
           ,[BoxNumber]
           ,[BatchNumber]
           ,[Unit]
           ,[Specifications]
           ,[Creator]
           ,[CreateTime]
           ,[Updator]
           ,[UpdateTime]
           ,[str1]
           ,[str2]
           ,[str3]
           ,[str4]
           ,[str5]
           ,[str6]
           ,[str7]
           ,[str8]
           ,[str9]
           ,[str10]
           ,[str11]
           ,[str12]
           ,[str13]
           ,[str14]
           ,[str15]
           ,[str16]
           ,[str17]
           ,[str18]
           ,[str19]
           ,[str20]
           ,[DateTime1]
           ,[DateTime2]
           ,[DateTime3]
           ,[DateTime4]
           ,[DateTime5]
           ,[Int1]
           ,[Int2]
           ,[Int3]
           ,[Int4]
           ,[Int5]
           ,[Remark]
           ,[Status])
	      (select rd.[RID]
           ,rd.ID
           ,rd.[ReceiptNumber]
           ,rd.[ExternReceiptNumber]
           ,rd.[CustomerID]
           ,rd.[CustomerName]
           ,rd.[LineNumber]
           ,rd.[LineNumber]
           ,rd.[SKU]
           ,rd.[UPC]
           ,rd.[QtyReceived]
           ,'" + Warehouse + @"'
           ,rd.[GoodsName]
           ,'Virtual'
           ,'Virtual'
           ,rd.[GoodsType]
           ,rd.[BoxNumber]
           ,rd.[BatchNumber]
           ,rd.[Unit]
           ,rd.[Specifications]
           ,'" + Name + @"'
           ,getdate()
           ,''
           ,''
           ,rd.[str1]
           ,rd.[str2]
           ,rd.[str3]
           ,rd.[str4]
           ,rd.[str5]
           ,rd.[str6]
           ,rd.[str7]
           ,rd.[str8]
           ,rd.[str9]
           ,rd.[str10]
           ,rd.[str11]
           ,rd.[str12]
           ,rd.[str13]
           ,rd.[str14]
           ,rd.[str15]
           ,rd.[str16]
           ,rd.[str17]
           ,rd.[str18]
           ,rd.[str19]
           ,rd.[str20]
           ,rd.[DateTime1]
           ,rd.[DateTime2]
           ,rd.[DateTime3]
           ,rd.[DateTime4]
           ,rd.[DateTime5]
           ,rd.[Int1]
           ,rd.[Int2]
           ,rd.[Int3]
           ,rd.[Int4]
           ,rd.[Int5]
           ,''
           ,1 from  WMS_Inventory_103_Temp it 
	      left join [dbo].[WMS_ReceiptDetail] rd
	      on it.RDID =rd.ID 
	      where it.Status=1 and  it.Str2='" + BoxNumber + @"' and it.ReceiptNumber='" + ReceiptNumber + "'  )" +
       "  update  WMS_Inventory_103_Temp set Status=9 where  Str2='" + BoxNumber + @"' and ReceiptNumber='" + ReceiptNumber + @"' ";
            return this.ScanExecuteNonQuery(StrSql);

        }

        public string AgainRecommended(string CustomerId, string Name, string Warehouse, string ReceiptNumber, string BoxNumber)
        {
            string SKU = "";
            //判断有没有推荐过

            string Str = "select * from  WMS_Inventory_103_Temp where  str2='" + BoxNumber + "'";

            IEnumerable<Inventorys> IsCheck = this.ScanDataTable(Str.ToString()).ConvertToEntityCollection<Inventorys>();

            //获取SKU
            string GetSKU = "select * from WMS_ReceiptDetail where str2='" + BoxNumber + "'";
            IEnumerable<ReceiptDetail> ReceiptDetail = this.ScanDataTable(GetSKU.ToString()).ConvertToEntityCollection<ReceiptDetail>();
            if (ReceiptDetail.Count() > 0)
            {
                SKU = ReceiptDetail.First().SKU;
            }
            //获取该SKU所在的库位
            string SqlLocation = @" select  distinct  i. Location from WMS_Inventory_103 i
            where InventoryType = 1  and SKU = '" + SKU + "'";

            string SqlLocationTemp = @"select distinct Location   from[dbo].[WMS_Inventory_103_Temp] where Status = 1
            and SKU = '" + SKU + "'";


            StringBuilder sb = new StringBuilder();

            sb.Append(@"select Area,l.Location,SUM(l.Str2) Str2,SUM(Qty) Qty from ( select Area,Location,COUNT(Str2) Str2,SUM(Qty) Qty from WMS_Inventory_103 where InventoryType=1
            and Location in (" + SqlLocation + @")
            group by Location,Area
            union (select Area, Location,COUNT(Str2) Str2,SUM(Qty) Qty from WMS_Inventory_103_Temp
            where Location in (" + SqlLocationTemp + @")
            group by Location,Area)) as l 
			left join WMS_Warehouse_Area wa
            on l.Area=wa.AreaName
            left join  WMS_Warehouse_Location wl
            on l.Location=wl.Location and wa.ID=wl.AreaID");
            sb.Append(" where l.Location not in (");
            foreach (var item in IsCheck)
            {
                sb.Append("'" + item + "',");
            }
            sb.Append("'')");
            sb.Append(@"GROUP BY l.Location,wl.MaxNumber,Area
            HAVING COUNT(l.Str2) < isnull(wl.MaxNumber, 12)
            ");
            IEnumerable<Inventorys> inventorys = this.ScanDataTable(sb.ToString()).ConvertToEntityCollection<Inventorys>();
            LocationInfo info = new LocationInfo();
            if (inventorys == null)
            {
                info = SqlGetNullLocationfun();
            }
            else
            {
                foreach (var item in inventorys)
                {
                    info.Location = item.Location;
                    info.AreaName = item.Area;
                    break;
                }
                if (string.IsNullOrEmpty(info.Location))
                {
                    info = SqlGetNullLocationfun();
                }
            }
            if (!string.IsNullOrEmpty(info.Location))
            {
                string InvEntoryTemp = (@" 
                update   WMS_Inventory_103_Temp set Status=9 where  str2='"+ BoxNumber + @"'
                and Status=1
                insert into WMS_Inventory_103_Temp
	            ([RID]
                ,[RDID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[SKU]
                ,[UPC]
                ,[BoxNumber]
                ,[BatchNumber]
                ,[GoodsName]
                ,[Qty]
	            ,[Creator]
	            ,[CreateTime]
                ,[Area]
                ,[Location]
                ,[Status]
                ,[Str2])
	            select top 1000 [RID] 
                ,[ID]
                ,[ReceiptNumber]
                ,[ExternReceiptNumber]
                ,[CustomerID]
                ,[CustomerName]
                ,[SKU]
                ,[UPC]
                ,[BoxNumber]
                ,[BatchNumber]
                ,[GoodsName]
                ,[QtyExpected]
                ,[Creator]
                ,[CreateTime]
	            ,'" + info.AreaName + @"'
	            , '" + info.Location + @"'
                , 1
                ,[str2] from WMS_ReceiptDetail where CustomerID = " + CustomerId + @"
                and str2 = '" + BoxNumber + "' ");

                this.ScanDataTable(InvEntoryTemp).ConvertToEntityCollection<Inventorys>();

                return info.Location;
            }
            return "";

        }
        private LocationInfo SqlGetNullLocationfun()
        {
            string SqlGetNullLocation = @"select top 1  * from WMS_Warehouse_Location wl
             left join WMS_Warehouse_Area wa
             on wl.AreaID=wa.ID where
             not exists (select * from WMS_Inventory_103 i where i.Location=wa.AreaName and i.Location=wl.Location) 
             order by Location  ";

            return this.ScanDataTable(SqlGetNullLocation).ConvertToEntity<LocationInfo>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.WMS.Models.ReceiptManagement;
using Runbow.TWS.Web.Common;
using ASNModel = Runbow.TWS.Web.Areas.WMS.Models.ASNManagement.IndexViewModel;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using System.Drawing;
using SysIO = System.IO;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Log;
using CSRedis;
using System.Net;
using Runbow.TWS.MessageContracts.WMS.JCApi;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class ReceiptManagementFGController : BaseController
    {
        public ActionResult Index(int? PageIndex, long? customerID)
        {
            //Session["SearchConditionModel"] = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IndexViewModel vm = new IndexViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName+"_FG");
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            vm.SearchCondition = new ReceiptSearchCondition();
            vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            vm.SearchCondition.EndCreateTime = DateTime.Now;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            GetReceiptByConditionRequest getReceiptByConditionRequest = new GetReceiptByConditionRequest();
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in WarehouseList)
                {
                    sb.Append("" + item.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            ViewBag.WarehouseList = WarehouseList;
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            //}
            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.SearchCondition.Model = "产品";

            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;
            var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptByCondition(getReceiptByConditionRequest);
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            }
            // Session["SearchConditionModel"] = vm.SearchCondition;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            ViewBag.CustomerID = customerID == null ? base.UserInfo.CustomerID : customerID;
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }



        public ActionResult IndexFG(int? PageIndex, long? customerID)
        {
            //Session["SearchConditionModel"] = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IndexViewModel vm = new IndexViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            vm.SearchCondition = new ReceiptSearchCondition();
            vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            vm.SearchCondition.EndCreateTime = DateTime.Now;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            GetReceiptByConditionRequest getReceiptByConditionRequest = new GetReceiptByConditionRequest();
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in WarehouseList)
                {
                    sb.Append("" + item.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            ViewBag.WarehouseList = WarehouseList;
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            //}
            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.SearchCondition.Model = "产品";

            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;
            var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptByConditionFG(getReceiptByConditionRequest);
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            }
            // Session["SearchConditionModel"] = vm.SearchCondition;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            ViewBag.CustomerID = customerID == null ? base.UserInfo.CustomerID : customerID;
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }
        /// <summary>
        /// 下发上架任务
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string ReceiptTask(string ID)
        {
            ReceiptManagementService service = new ReceiptManagementService();
            List<WMS_Task> tasks = new List<WMS_Task>();
            string response = "";
            try
            {
                response = service.ReceiptTask(ID, base.UserInfo.Name).Result;
                var responseDetail = service.GetReceiptDetailByIDS(ID);

                foreach (var item in responseDetail.Result.ReceiptDetailCollection.GroupBy(c => c.ReceiptNumber).Select(a => a.Key))
                {
                    List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
                    foreach (var itemdetail in responseDetail.Result.ReceiptDetailCollection)
                    {
                        if (item == itemdetail.ReceiptNumber)
                        {
                            receiptDetails.Add(new ReceiptDetail()
                            {
                                ExternReceiptNumber = itemdetail.ExternReceiptNumber,
                                WarehouseName = itemdetail.WarehouseName,
                                GoodsName = itemdetail.GoodsName,
                                CustomerID = itemdetail.CustomerID,
                                CustomerName = itemdetail.CustomerName,
                                ID = itemdetail.ID,
                                RID = itemdetail.RID,
                                LineNumber = itemdetail.LineNumber,
                                GoodsType = itemdetail.GoodsType,
                                ReceiptNumber = itemdetail.ReceiptNumber,
                                SKU = itemdetail.SKU,
                                UPC = itemdetail.UPC,
                                BatchNumber = itemdetail.BatchNumber,
                                Unit = itemdetail.Unit,
                                Specifications = itemdetail.Specifications,
                                QtyExpected = itemdetail.QtyExpected,
                                QtyReceived = 0

                            });
                        }
                    }
                    RedisOperation.Del(item);
                    RedisOperation.SetList(item, receiptDetails);
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// 回退RF装箱数据
        /// </summary>
        /// <param name="ExternReceiptNumber"></param>
        /// <returns></returns>
        public string BackCloseBox(string ExternReceiptNumber)
        {
            string msg = "";
            try
            {
                msg = new ReceiptManagementService().BackCloseBox(ExternReceiptNumber);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="customerID"></param>
        /// <param name="type">1.代表传入的是系统ordernumber，2.代表快递单号，3.代表外部单号</param>
        /// <returns></returns>
        public string ValidOrderCancel(string OrderNumber, long customerID, string warehouse, int type)
        {
            //查询订单是否是取消单调不同存过
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
            }
            return new DeliverConfirmService().ValidOrderCancel(OrderNumber, customerID, wms.FirstOrDefault().Name, warehouse, type);
        }
        private void GenQueryReceiptViewModel(IndexViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_Receipt").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_Receipt");
            }
            if (Configs.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_ReceiptDetail");
            }
            //var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            //if (Configs.Where(t => t.Name == "WMS_Receipt").Count() == 0)
            //{
            //    Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            //}
            //vm.Config = Configs.First(t => t.Name == "WMS_Receipt");

            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerList = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerList = Enumerable.Empty<SelectListItem>();
            }
        }
        private void GenQueryReceiptDetailViewModel(IndexViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.receipt.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_Receipt").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_Receipt");
            }
            if (Configs.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_ReceiptDetail");
            }


            //vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt");
            //vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail");

            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerList = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerList = Enumerable.Empty<SelectListItem>();
            }
        }
        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex, string Action)
        {
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;


            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
                vm.SearchCondition.CustomerID = 0;
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (vm.SearchCondition.WarehouseID != 0)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }

            ViewBag.WarehouseList = WarehouseList;
            ViewBag.CustomerID = vm.SearchCondition.CustomerID == null ? base.UserInfo.CustomerID : vm.SearchCondition.CustomerID;
            vm.SearchCondition.UserType = base.UserInfo.UserType;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.SearchCondition.Model = "产品";

            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptByCondition(getReceiptByConditionRequest);

            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出入库单")
                {
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);

                    IEnumerable<Column> columnReceipt;
                    IEnumerable<Column> columnReceiptDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
                    {
                        columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }
                    else
                    {
                        columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    //IEnumerable<Column> columnReceiptDetail = tables.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    if (vm.SearchCondition.CustomerID == 0)
                    {
                        columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                        //        }

                        //        return c;
                        //    }
                        //});
                        var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                        //        }

                        //        return c;
                        //    }
                        //});
                        columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                        columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
                    }
                    Export(getReceiptByConditionResponse.Result, columnReceipt, columnReceiptDetail);

                }
                if (Action == "导出上架单")
                {
                    IEnumerable<WMSConfig> wms2 = null;
                    try
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition_" + base.UserInfo.ProjectName);


                    }
                    catch
                    {
                    }
                    if (wms2 == null)
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition");


                    }
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition2(getReceiptByConditionRequest, wms2.FirstOrDefault().Name);
                    //getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                    {
                        if ((ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(null, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                        }
                        else
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                        }
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection.Where(t => t.IsShowInList == true);
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    ExportReceiptReceiving_DC(getReceiptByConditionResponse.Result, columnReceipt);

                }
                if (Action == "导出上架信息")
                {
                    IEnumerable<WMSConfig> wms2 = null;
                    try
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition_" + base.UserInfo.ProjectName);


                    }
                    catch
                    {
                    }
                    if (wms2 == null)
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition");


                    }
                    getReceiptByConditionRequest.ProdName = wms2.FirstOrDefault().Name;
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    ExportReceiptReceiving(getReceiptByConditionResponse.Result, columnReceipt);

                }

            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }


        [HttpPost]
        public ActionResult IndexFG(IndexViewModel vm, int? PageIndex, string Action)
        {
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;


            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
                vm.SearchCondition.CustomerID = 0;
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (vm.SearchCondition.WarehouseID != 0)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }

            ViewBag.WarehouseList = WarehouseList;
            ViewBag.CustomerID = vm.SearchCondition.CustomerID == null ? base.UserInfo.CustomerID : vm.SearchCondition.CustomerID;
            vm.SearchCondition.UserType = base.UserInfo.UserType;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptByCondition(getReceiptByConditionRequest);

            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出入库单")
                {
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);

                    IEnumerable<Column> columnReceipt;
                    IEnumerable<Column> columnReceiptDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
                    {
                        columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }
                    else
                    {
                        columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    //IEnumerable<Column> columnReceiptDetail = tables.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    if (vm.SearchCondition.CustomerID == 0)
                    {
                        columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                        //        }

                        //        return c;
                        //    }
                        //});
                        var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                        //        }

                        //        return c;
                        //    }
                        //});
                        columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                        columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
                    }
                    Export(getReceiptByConditionResponse.Result, columnReceipt, columnReceiptDetail);

                }
                if (Action == "导出上架单")
                {
                    IEnumerable<WMSConfig> wms2 = null;
                    try
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition_" + base.UserInfo.ProjectName);


                    }
                    catch
                    {
                    }
                    if (wms2 == null)
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition");


                    }
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition2(getReceiptByConditionRequest, wms2.FirstOrDefault().Name);
                    //getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                    {
                        if ((ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(null, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                        }
                        else
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                        }
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection.Where(t => t.IsShowInList == true);
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    ExportReceiptReceiving_DC(getReceiptByConditionResponse.Result, columnReceipt);

                }
                if (Action == "导出上架信息")
                {
                    IEnumerable<WMSConfig> wms2 = null;
                    try
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition_" + base.UserInfo.ProjectName);


                    }
                    catch
                    {
                    }
                    if (wms2 == null)
                    {
                        wms2 = ApplicationConfigHelper.GetWMS_Config("GetImportReceiptByCondition");


                    }
                    getReceiptByConditionRequest.ProdName = wms2.FirstOrDefault().Name;
                    getReceiptByConditionResponse = new ReceiptManagementService().GetImportReceiptByCondition(getReceiptByConditionRequest);

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                    ExportReceiptReceiving(getReceiptByConditionResponse.Result, columnReceipt);

                }

            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }
        //导出
        private void Export(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt, IEnumerable<Column> columnReceiptDetail)
        {
            IEnumerable<Receipt> receipts = response.ReceiptCollection;
            IEnumerable<ReceiptDetail> receiptDetails = response.ReceiptDetailCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            DataTable dtReceiptDetail = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            foreach (var receiptDetail in columnReceiptDetail)
            {
                dtReceiptDetail.Columns.Add(receiptDetail.DisplayName, typeof(string));
            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.Receipt).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });
            receiptDetails.Each((i, s) =>
            {
                DataRow drReceipt = dtReceiptDetail.NewRow();
                foreach (var receiptDetail in columnReceiptDetail)
                {
                    drReceipt[receiptDetail.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receiptDetail.DbColumnName).GetValue(s);

                }
                dtReceiptDetail.Rows.Add(drReceipt);
            });
            dtReceipt.TableName = "入库单主信息";
            dtReceiptDetail.TableName = "入库单明细信息";
            ds.Tables.Add(dtReceipt);
            ds.Tables.Add(dtReceiptDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "入库单" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "入库单" + DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
        //导出上架信息
        private void ExportReceiptReceiving(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReceiptDetail> receipts = response.ReceiptDetailCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);


            }
            catch
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("GoodsTypes");


            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    if (receipt.DbColumnName == "Warehouse")
                    {
                        drReceipt[receipt.DisplayName] = "需填";
                    }
                    else if (receipt.DbColumnName == "Area")
                    {
                        drReceipt[receipt.DisplayName] = "需填";
                    }
                    else if (receipt.DbColumnName == "Location")
                    {
                        drReceipt[receipt.DisplayName] = "需填";
                    }
                    else if (receipt.DbColumnName == "Remark")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "GoodsType")
                    {
                        if (typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s).ToString() == "" || typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s) == null)
                        {
                            drReceipt[receipt.DisplayName] = wms.FirstOrDefault().Name;
                        }
                        else
                        {
                            drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                        }
                    }
                    else
                    {
                        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                    }
                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "上架单信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "上架单模板" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "上架单模板" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        //导出上架单
        private void ExportReceiptReceiving_DC(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReceiptDetail> receipts = response.ReceiptDetailCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);


            }
            catch
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("GoodsTypes");


            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    if (receipt.DbColumnName == "Warehouse")
                    {
                    //drReceipt[receipt.DisplayName] = "";
                    if (drReceipt[receipt.DisplayName] != null)
                        {
                            drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                        }
                        else
                        {
                            drReceipt[receipt.DisplayName] = "";
                        }
                    }
                    else if (receipt.DbColumnName == "Area")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "Location")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                //else if (receipt.DbColumnName == "Remark")
                //{
                //    drReceipt[receipt.DisplayName] = "";
                //}
                else if (receipt.DbColumnName == "GoodsType")
                    {
                        if (typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s).ToString() == "" || typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s) == null)
                        {
                            drReceipt[receipt.DisplayName] = wms.FirstOrDefault().Name;
                        }
                        else
                        {
                            drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                        }
                    ////drReceipt[receipt.DisplayName] = "A品";
                    //if (drReceipt[receipt.DisplayName].ToString() != "")
                    //{
                    //    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                    //}
                    //else
                    //{

                    //    //drReceipt[receipt.DisplayName] = "A品";
                    //    //如果没有品级 则获取默认品级
                    //    drReceipt[receipt.DisplayName] = wms.FirstOrDefault().Name;
                    //}
                }
                    else
                    {
                        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                    }
                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "上架单";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "上架单" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "上架单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        //导出选中上架单
        public void ExportShelves(string ids, long? CustomerID, string type)
        {
            if (type == "2")
            {
                IEnumerable<WMSConfig> wms2 = null;
                try
                {
                    wms2 = ApplicationConfigHelper.GetWMS_Config("GetShelvesByIDs_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms2 == null)
                {
                    wms2 = ApplicationConfigHelper.GetWMS_Config("GetShelvesByIDs");
                }
                var getReceiptByConditionResponse = new ReceiptManagementService().GetShelvesByIDs2(ids, wms2.FirstOrDefault().Name);
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                }
                //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //IEnumerable<Table> tables = module.Tables.TableCollection;
                //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
                ExportReceiptReceiving(getReceiptByConditionResponse.Result, columnReceipt);
            }
            else
            {
                IEnumerable<WMSConfig> wms2 = null;
                try
                {
                    wms2 = ApplicationConfigHelper.GetWMS_Config("GetShelvesByIDs_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms2 == null)
                {
                    wms2 = ApplicationConfigHelper.GetWMS_Config("GetShelvesByIDs");
                }
                var getReceiptByConditionResponse = new ReceiptManagementService().GetShelvesByIDs2(ids, wms2.FirstOrDefault().Name);
                //var getReceiptByConditionResponse = new ReceiptManagementService().GetShelvesByIDs(ids);
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                {
                    if ((ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving_DC").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(null, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection;
                    }
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving_DC").ColumnCollection.Where(t => t.IsShowInList == true);
                }
                ExportReceiptReceiving_DC(getReceiptByConditionResponse.Result, columnReceipt);
            }
        }
        //按照选中导出
        public void ExportOrder(string ids, long? CustomerID)
        {
            var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptByIDs(ids);

            IEnumerable<Column> columnReceipt;
            IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
            {
                columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }
            else
            {
                columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }

            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Table> tables = module.Tables.TableCollection;
            //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Receipt").ColumnCollection;
            //IEnumerable<Column> columnReceiptDetail = tables.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            if (CustomerID == 0)
            {
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
            }
            else
            {
                var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                //.Select(c =>
                //{
                //    if (c.InnerColumns.Count == 0)
                //    {
                //        return c;
                //    }
                //    else
                //    {
                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                //        {
                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                //        }

                //        return c;
                //    }
                //});
                var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                //.Select(c =>
                //{
                //    if (c.InnerColumns.Count == 0)
                //    {
                //        return c;
                //    }
                //    else
                //    {
                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                //        {
                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                //        }

                //        return c;
                //    }
                //});
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
            }
            Export(getReceiptByConditionResponse.Result, columnReceipt, columnReceiptDetail);

        }


        [HttpGet]
        public ActionResult ReceiptCreateFG(long ID = 0, int ViewType = 0, int CustomerID = 0, int Flag = 0, int PageType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            vm.SearchCondition = new ReceiptSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (ID != 0)
            {
                GetAsnOrReceiptOrDetailByConditionRequest request = new GetAsnOrReceiptOrDetailByConditionRequest();
                ASNDetailSearchCondition asnDetail = new ASNDetailSearchCondition();
                //if (ViewType == 1)
                //{

                //    var GetResponse = new ReceiptManagementService().ASNDetailQuery(request, ID);
                //    //vm.receipt = GetResponse.Result.Receipt;
                //    //vm.ReceiptDetailCollection = GetResponse.Result.ReceiptDetailCollection;
                //    vm.dtAsn = GetResponse.Result.dtAsn;
                //    vm.dtAsnDetail = GetResponse.Result.dtAsnDetail;
                //    IEnumerable<Column> AsnColumn;
                //    IEnumerable<Column> AsnDetailColumn;
                //    IEnumerable<Column> ReceiptColumn;
                //    IEnumerable<Column> ReceiptDetailColumn;
                //    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                //    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
                //    {
                //        AsnColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                //    }
                //    else
                //    {
                //        AsnColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                //    }
                //    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
                //    {
                //        AsnDetailColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                //    }
                //    else
                //    {
                //        AsnDetailColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                //    }
                //    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
                //    {
                //        ReceiptColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                //    }
                //    else
                //    {
                //        ReceiptColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                //    }
                //    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
                //    {
                //        ReceiptDetailColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                //    }
                //    else
                //    {
                //        ReceiptDetailColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                //    }

                //    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                //    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //    //IEnumerable<Table> tables = module.Tables.TableCollection;
                //    //IEnumerable<Column> AsnColumn = tables.First(t => t.Name == "WMS_ASN").ColumnCollection;
                //    //IEnumerable<Column> AsnDetailColumn = tables.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                //    //IEnumerable<Column> ReceiptColumn = tables.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                //    //IEnumerable<Column> ReceiptDetailColumn = tables.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                //    AsnColumn = AsnColumn.Where(c => (c.IsKey == true)).Union(AsnColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));

                //    AsnDetailColumn = AsnDetailColumn.Where(c => (c.IsKey == true)).Union(AsnDetailColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));
                //    ReceiptColumn = ReceiptColumn.Where(c => (c.IsKey == true)).Union(ReceiptColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));
                //    ReceiptDetailColumn = ReceiptDetailColumn.Where(c => (c.IsKey == true)).Union(ReceiptDetailColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));

                //    DataTable dtAsn = InitASNFromDataTable(vm.dtAsn, AsnColumn);
                //    DataTable dtAsnDetail = InitASNDetailFromDataTable(vm.dtAsnDetail, AsnDetailColumn);
                //    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                //    StringBuilder sb = new StringBuilder();
                //    IEnumerable<Receipt> Receipts = this.InitReceiptFromDataTable(dtAsn, ReceiptColumn, useCustomerOrderNumber, sb);
                //    IEnumerable<ReceiptDetail> ReceiptDetailss = this.InitReceiptDetailFromDataTable(dtAsnDetail, ReceiptDetailColumn, useCustomerOrderNumber, sb);
                //    vm.receipt = Receipts.FirstOrDefault();
                //    vm.ReceiptDetailCollection = ReceiptDetailss;
                //    vm.SearchCondition.CustomerID = CustomerID;
                //}
                //else
                //{
                var GetResponse = new ReceiptManagementService().ReceiptDetailQueryFG(request, ID);
                vm.receipt = GetResponse.Result.Receipt;
                vm.ReceiptDetailCollection = GetResponse.Result.ReceiptDetailCollection;
                var SkuList = GetResponse.Result.ReceiptDetailCollection.Select(t => new { Value = t.SKU + "/" + t.BatchNumber, Text = t.SKU + "/" + t.BatchNumber }).Distinct().Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
                var BoxList = GetResponse.Result.ReceiptDetailCollection.Select(t => new { Value = t.BoxNumber, Text = t.BoxNumber }).Distinct().Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
                ViewBag.SkuList = SkuList;
                ViewBag.BoxList = BoxList;
                vm.SearchCondition.CustomerID = CustomerID;
                //}
            }
            else
            {
                vm.receipt = new Receipt();
                vm.receipt.CustomerID = CustomerID;
            }
            vm.ViewType = ViewType;
            vm.Flag = Flag;

            Session["PageType"] = PageType;
            this.GenQueryReceiptDetailViewModel(vm);
            return View(vm);
        }
        [HttpGet]
        public ActionResult ReceiptCreate(long ID = 0, int ViewType = 0, int CustomerID = 0, int Flag = 0, int PageType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            vm.SearchCondition = new ReceiptSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (ID != 0)
            {
                GetAsnOrReceiptOrDetailByConditionRequest request = new GetAsnOrReceiptOrDetailByConditionRequest();
                ASNDetailSearchCondition asnDetail = new ASNDetailSearchCondition();
                if (ViewType == 1)
                {

                    var GetResponse = new ReceiptManagementService().ASNDetailQuery(request, ID);
                    //vm.receipt = GetResponse.Result.Receipt;
                    //vm.ReceiptDetailCollection = GetResponse.Result.ReceiptDetailCollection;
                    vm.dtAsn = GetResponse.Result.dtAsn;
                    vm.dtAsnDetail = GetResponse.Result.dtAsnDetail;
                    IEnumerable<Column> AsnColumn;
                    IEnumerable<Column> AsnDetailColumn;
                    IEnumerable<Column> ReceiptColumn;
                    IEnumerable<Column> ReceiptDetailColumn;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
                    {
                        AsnColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    else
                    {
                        AsnColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
                    {
                        AsnDetailColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }
                    else
                    {
                        AsnDetailColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
                    {
                        ReceiptColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    else
                    {
                        ReceiptColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
                    {
                        ReceiptDetailColumn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }
                    else
                    {
                        ReceiptDetailColumn = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> AsnColumn = tables.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    //IEnumerable<Column> AsnDetailColumn = tables.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    //IEnumerable<Column> ReceiptColumn = tables.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    //IEnumerable<Column> ReceiptDetailColumn = tables.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    AsnColumn = AsnColumn.Where(c => (c.IsKey == true)).Union(AsnColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));

                    AsnDetailColumn = AsnDetailColumn.Where(c => (c.IsKey == true)).Union(AsnDetailColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));
                    ReceiptColumn = ReceiptColumn.Where(c => (c.IsKey == true)).Union(ReceiptColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));
                    ReceiptDetailColumn = ReceiptDetailColumn.Where(c => (c.IsKey == true)).Union(ReceiptDetailColumn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID)));

                    DataTable dtAsn = InitASNFromDataTable(vm.dtAsn, AsnColumn);
                    DataTable dtAsnDetail = InitASNDetailFromDataTable(vm.dtAsnDetail, AsnDetailColumn);
                    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<Receipt> Receipts = this.InitReceiptFromDataTable(dtAsn, ReceiptColumn, useCustomerOrderNumber, sb);
                    IEnumerable<ReceiptDetail> ReceiptDetailss = this.InitReceiptDetailFromDataTable(dtAsnDetail, ReceiptDetailColumn, useCustomerOrderNumber, sb);
                    vm.receipt = Receipts.FirstOrDefault();
                    vm.ReceiptDetailCollection = ReceiptDetailss;
                    vm.SearchCondition.CustomerID = CustomerID;
                }
                else
                {
                    var GetResponse = new ReceiptManagementService().ReceiptDetailQuery(request, ID);
                    vm.receipt = GetResponse.Result.Receipt;
                    vm.ReceiptDetailCollection = GetResponse.Result.ReceiptDetailCollection;
                    var SkuList = GetResponse.Result.ReceiptDetailCollection.Select(t => new { Value = t.SKU + "/" + t.BatchNumber, Text = t.SKU + "/" + t.BatchNumber }).Distinct().Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
                    var BoxList = GetResponse.Result.ReceiptDetailCollection.Select(t => new { Value = t.BoxNumber, Text = t.BoxNumber }).Distinct().Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
                    ViewBag.SkuList = SkuList;
                    ViewBag.BoxList = BoxList;
                    vm.SearchCondition.CustomerID = CustomerID;
                }
            }
            else
            {
                vm.receipt = new Receipt();
                vm.receipt.CustomerID = CustomerID;
            }
            vm.ViewType = ViewType;
            vm.Flag = Flag;

            Session["PageType"] = PageType;
            this.GenQueryReceiptDetailViewModel(vm);
            return View(vm);
        }
        [HttpPost]
        public JsonResult AddReceiptAndReceiptDetail(string JsonTable, string ASNNumber, string ASNID, string ExternReceiptNumber,
            string CustomerName, string CustomerID, string Receipttype, DateTime? ReceiptDate, string JsonField, long WarehouseID, String WarehouseName)
        {
            IEnumerable<Column> columns;

            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID.ObjectToInt64()).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }

            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID.ObjectToInt64()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
            DataTable dt = JsonToDataTable(JsonTable);
            //columns = columns
            //   .Select(c =>
            //   {
            //       if (c.InnerColumns.Count == 0)
            //       {
            //           return c;
            //       }
            //       else
            //       {
            //           if (c.InnerColumns.Any(innerc => innerc.CustomerID == CustomerID.ObjectToInt64()))
            //           {
            //               return c.InnerColumns.First(innerc => innerc.CustomerID == CustomerID.ObjectToInt64());
            //           }

            //           return c;
            //       }
            //   });

            bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
            StringBuilder sb = new StringBuilder();
            IEnumerable<ReceiptDetail> ReceiptDetailss = this.InitReceiptDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);

            IndexViewModel vm = new IndexViewModel();
            vm.SearchCondition = new ReceiptSearchCondition();
            var responseJsonFieldsets = jsonlist<Receipt>(JsonField);

            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            IList<Receipt> Receipts = new List<Receipt>();
            IList<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
            if (responseJsonFieldsets.Count == 0)
            {
                responseJsonFieldsets.Add(
                    new Receipt()
                    {
                        ASNID = ASNID.ObjectToInt64(),
                        ReceiptType = Receipttype,
                        ReceiptDate = ReceiptDate,
                        CustomerID = CustomerID.ObjectToInt64(),
                        CustomerName = CustomerName,
                        WarehouseID = WarehouseID,
                        WarehouseName = WarehouseName,
                        ExternReceiptNumber = ExternReceiptNumber,
                        ASNNumber = ASNNumber,
                        CreateTime = DateTime.Now,
                        Creator = base.UserInfo.Name.ToString(),
                        Status = 1
                    }
                    );
            }
            else
            {
                responseJsonFieldsets.Each((i, receipt) =>
                {
                    receipt.ASNID = ASNID.ObjectToInt64();
                    receipt.ReceiptType = Receipttype;
                    receipt.ReceiptDate = ReceiptDate;
                    receipt.CustomerID = CustomerID.ObjectToInt64();
                    receipt.CustomerName = CustomerName;
                    receipt.WarehouseID = WarehouseID;
                    receipt.WarehouseName = WarehouseName;
                    receipt.ExternReceiptNumber = ExternReceiptNumber;
                    receipt.ASNNumber = ASNNumber;
                    receipt.CreateTime = DateTime.Now;
                    receipt.Creator = base.UserInfo.Name.ToString();
                    receipt.Status = 1;
                });
            }

            ReceiptDetailss.Each((i, receiptDetail) =>
            {
                receiptDetail.ExternReceiptNumber = ExternReceiptNumber;
                receiptDetail.SKU = receiptDetail.SKU.Trim();
                receiptDetail.LineNumber = receiptDetail.LineNumber.Trim();
                receiptDetail.QtyExpected = receiptDetail.QtyExpected;
                receiptDetail.CreateTime = DateTime.Now;
                receiptDetail.Creator = base.UserInfo.Name.ToString();
                receiptDetail.CustomerID = CustomerID.ObjectToInt64();
                receiptDetail.CustomerName = CustomerName;
                receiptDetail.ASNNumber = ASNNumber;
                receiptDetail.ASNID = ASNID.ObjectToInt64();
            });


            request.Receipts = responseJsonFieldsets;
            request.ReceiptDetails = ReceiptDetailss;
            var response = new ReceiptManagementService().AddReceiptAndReceiptDetail(request);
            if (response.IsSuccess)
            {
                return Json(new { Errorcode = 1, Result = response.Result });
            }
            else
            {
                return Json(new { Errorcode = 0, Result = response.Result });
            }


        }
        [HttpPost]
        public string EditReceiptAndReceiptDetail(string JsonTable, long ID, DateTime? ReceiptDate, string ReceiptType,
            int CustomerID, string CustomerName, string ReceiptNumber, int ASNID, string ASNNumber, string ExternNumber, string JsonField)
        {
            IEnumerable<Column> columns;

            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID.ObjectToInt64()).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            }

            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
            var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
            DataTable dt = JsonToDataTable(JsonTable);
            //columns = columns
            //   .Select(c =>
            //   {
            //       if (c.InnerColumns.Count == 0)
            //       {
            //           return c;
            //       }
            //       else
            //       {
            //           if (c.InnerColumns.Any(innerc => innerc.CustomerID == CustomerID.ObjectToInt64()))
            //           {
            //               return c.InnerColumns.First(innerc => innerc.CustomerID == CustomerID.ObjectToInt64());
            //           }

            //           return c;
            //       }
            //   });

            bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
            StringBuilder sb = new StringBuilder();
            IEnumerable<ReceiptDetail> ReceiptDetailss = this.InitReceiptDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);

            var responseJsonFieldsets = jsonlist<Receipt>(JsonField);
            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            IList<Receipt> Receipts = new List<Receipt>();
            IList<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
            if (responseJsonFieldsets.Count == 0)
            {
                responseJsonFieldsets.Add(
                    new Receipt()
                    {
                        ID = ID,
                        ASNID = ASNID.ObjectToInt64(),
                        ReceiptType = ReceiptType,
                        CustomerID = CustomerID.ObjectToInt64(),
                        CustomerName = CustomerName,
                        ExternReceiptNumber = ExternNumber,
                        ASNNumber = ASNNumber,
                        CreateTime = DateTime.Now,
                        Creator = base.UserInfo.Name.ToString(),
                        Updator = base.UserInfo.Name.ToString(),
                        UpdateTime = DateTime.Now,
                        ReceiptDate = ReceiptDate,
                        ReceiptNumber = ReceiptNumber,
                    });
            }
            else
            {
                responseJsonFieldsets.Each((i, receipt) =>
                {
                    receipt.ID = ID;
                    receipt.ASNID = ASNID.ObjectToInt64();
                    receipt.ReceiptType = ReceiptType;
                    receipt.CustomerID = CustomerID.ObjectToInt64();
                    receipt.CustomerName = CustomerName;
                    receipt.ExternReceiptNumber = ExternNumber;
                    receipt.ASNNumber = ASNNumber;
                    receipt.CreateTime = DateTime.Now;
                    receipt.Creator = base.UserInfo.Name.ToString();
                    receipt.Updator = base.UserInfo.Name.ToString();
                    receipt.UpdateTime = DateTime.Now;
                    receipt.ReceiptDate = ReceiptDate;
                    receipt.ReceiptNumber = ReceiptNumber;
                }
                    );
            }

            ReceiptDetailss.Each((i, receiptDetail) =>
            {
                receiptDetail.RID = ID;
                receiptDetail.ExternReceiptNumber = ExternNumber;
                receiptDetail.ReceiptNumber = ReceiptNumber;
                receiptDetail.SKU = receiptDetail.SKU.Trim();
                receiptDetail.LineNumber = receiptDetail.LineNumber.Trim();
                receiptDetail.QtyExpected = receiptDetail.QtyExpected.ObjectToInt32();
                receiptDetail.CreateTime = DateTime.Now;
                receiptDetail.Creator = base.UserInfo.Name.ToString();
                receiptDetail.UpdateTime = DateTime.Now;
                receiptDetail.Updator = base.UserInfo.Name.ToString();
                receiptDetail.CustomerID = CustomerID.ObjectToInt64();
                receiptDetail.CustomerName = CustomerName;
                receiptDetail.ASNNumber = ASNNumber;
                receiptDetail.ASNID = ASNID.ObjectToInt64();
            });


            request.Receipts = responseJsonFieldsets;
            request.ReceiptDetails = ReceiptDetailss;
            var response = new ReceiptManagementService().EditReceiptAndReceiptDetail(request);
            return response.Result;

        }
        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }
        [HttpGet]
        public ActionResult ASNQuery(int? CustomerID)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            if (CustomerID == 0)
            {



                ViewBag.CustomerList = CustomerList;
            }
            else
            {
                ViewBag.CustomerList = CustomerList.Where(c => c.Value == CustomerID.ToString());
            }
            ASNModel vm = new ASNModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ASNTypes = st;
            vm.ASNCondition = new ASNSearchCondition();
            vm.ASNCondition.CustomerID = CustomerID;
            GetAsnByConditionRequest request = new GetAsnByConditionRequest();
            ASNSearchCondition asn = new ASNSearchCondition();
            request.SearchCondition = asn;
            request.PageSize = UtilConstants.ASNQueryPageSize;
            request.PageIndex = 0;
            request.SearchCondition.CustomerID = CustomerID;


            var GetResponse = new ReceiptManagementService().ASNQuery(request);
            vm.ASNCollection = GetResponse.Result.ASNCollection;
            if (GetResponse.IsSuccess)
            {
                vm.ASNCollection = GetResponse.Result.ASNCollection;
                vm.PageIndex = GetResponse.Result.PageIndex;
                vm.PageCount = GetResponse.Result.PageCount;
            }
            Session["ASNQuery_CustomerID"] = CustomerID;
            return View(vm);
        }
        [HttpPost]
        public ActionResult ASNQuery(ASNModel vm, int? PageIndex)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            var CustomerID = Session["ASNQuery_CustomerID"];
            if (CustomerID.ObjectToInt32() == 0)
            {
                ViewBag.CustomerList = CustomerList;
            }
            else
            {
                ViewBag.CustomerList = CustomerList.Where(c => c.Value == CustomerID.ToString());
            }
            GetAsnByConditionRequest request = new GetAsnByConditionRequest();
            request.SearchCondition = vm.ASNCondition;
            request.PageSize = UtilConstants.ASNQueryPageSize;
            request.PageIndex = PageIndex ?? 0;
            var GetResponse = new ReceiptManagementService().ASNQuery(request);
            vm.ASNCollection = GetResponse.Result.ASNCollection;
            if (GetResponse.IsSuccess)
            {
                vm.ASNCollection = GetResponse.Result.ASNCollection;
                vm.PageIndex = GetResponse.Result.PageIndex;
                vm.PageCount = GetResponse.Result.PageCount;
            }
            return View(vm);
        }
        [HttpPost]
        public string BackStatus(string ID, string ToStatus)
        {
            IList<Receipt> Receipts = new List<Receipt>();
            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            //判断是否存在已入库的订单
            IEnumerable<Receipt> receipts = new ReceiptManagementService().GetRceiptInfoByIDs(ID);
            if (receipts == null || !receipts.Any())
            {
                return "未找到需要回退的订单，请检查后重试！";
            }
            if (receipts.Where(m => m.Status == 9).Count() > 0)
            {
                return "存在已入库的订单，不允许回退，请检查后重试！";
            }
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Receipts.Add(new Receipt
                    {
                        ID = str.ObjectToInt64(),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        ReceiptDate = DateTime.Now,
                        CompleteDate = DateTime.Now
                    });
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-状态回退";
                    operation.OrderType = "Receipt";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = str;
                    operation.Remark = ToStatus;
                    logs.Add(operation);
                }
            }
            else
            {
                Receipts.Add(new Receipt
                {
                    ID = ID.ObjectToInt64(),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    ReceiptDate = DateTime.Now,
                    CompleteDate = DateTime.Now
                });
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "入库单管理";
                operation.Operation = "入库单-状态回退";
                operation.OrderType = "Receipt";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                operation.Remark = ToStatus;
                logs.Add(operation);
            }
            request.Receipts = Receipts;

            new LogOperationService().AddLogOperation(logs);//csc对接，故采取先加日志方式，删除增量库存
            var GetResponse = new ReceiptManagementService().ReceiptStatusBack(request, ToStatus.ObjectToInt32());

            return GetResponse.Result;

        }
        public static DataTable JsonToDataTable(string strJson)
        {
            //取出表名  
            //Regex rg = new Regex(@"(?<={)[^:]+(?=:/[)", RegexOptions.IgnoreCase);  
            //string strName = rg.Match(strJson).Value;  
            DataTable tb = null;
            //去除表名  
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value.Replace("\"", "");
                string[] strRows = Regex.Split(strRow, "!,", RegexOptions.IgnoreCase);

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "ReceiptDetail";
                    foreach (string str in strRows)
                    {
                        if (str != "")
                        {
                            DataColumn dc = new DataColumn();
                            string[] strCell = str.Split(':');
                            dc.ColumnName = strCell[0].ToString();
                            tb.Columns.Add(dc);
                        }
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
        private IEnumerable<ReceiptDetail> InitReceiptDetailFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ReceiptDetail ReceiptDetail = new ReceiptDetail();
                string columnName;
                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey == true))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        var propertyInfoTemp = typeof(ReceiptDetail).GetProperty(column.DbColumnName);
                                        try
                                        {
                                            propertyInfoTemp.SetValue(ReceiptDetail, Convert.ChangeType(value, (Nullable.GetUnderlyingType(propertyInfoTemp.PropertyType) ?? propertyInfoTemp.PropertyType)), null);
                                        }
                                        catch
                                        {
                                            propertyInfoTemp.SetValue(ReceiptDetail, null);
                                        }
                                        //typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ReceiptDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(ReceiptDetail, Convert.ChangeType(value, (Nullable.GetUnderlyingType(propertyInfoTemp.PropertyType) ?? propertyInfoTemp.PropertyType)), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(ReceiptDetail, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                }

                foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
                {
                    //foreach (var column in col.InnerColumns)
                    //{
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ReceiptDetail).GetProperty(column.DbColumnName).SetValue(ReceiptDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ReceiptDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(ReceiptDetail, Convert.ChangeType(value, (Nullable.GetUnderlyingType(propertyInfoTemp.PropertyType) ?? propertyInfoTemp.PropertyType)), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(ReceiptDetail, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                    //}
                }
                ReceiptDetails.Add(ReceiptDetail);
            }

            return ReceiptDetails;
        }
        private IEnumerable<Receipt> InitReceiptFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<Receipt> Receipts = new List<Receipt>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Receipt receipt = new Receipt();
                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey == true))
                {

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (column.DisplayName == "入库状态")
                            {
                                if (value == "待入库")
                                {
                                    receipt.Status = 1;
                                }

                            }
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, "1", null);
                                        }
                                        else
                                        {
                                            typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(Receipt).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(receipt, Convert.ChangeType(value, (Nullable.GetUnderlyingType(propertyInfoTemp.PropertyType) ?? propertyInfoTemp.PropertyType)), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(receipt, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                }
                foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
                {
                    //foreach (var column in col.InnerColumns)
                    //{
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, "1", null);
                                        }
                                        else
                                        {
                                            typeof(Receipt).GetProperty(column.DbColumnName).SetValue(receipt, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(Receipt).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(receipt, Convert.ChangeType(value, (Nullable.GetUnderlyingType(propertyInfoTemp.PropertyType) ?? propertyInfoTemp.PropertyType)), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(receipt, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                    //}
                }

                Receipts.Add(receipt);
            }

            return Receipts;
        }
        private DataTable InitASNFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig)
        {
            DataTable dtAsns = dt.Copy();
            foreach (var column in columnsConfig.Where(c => c.IsKey == true))
            {
                for (int j = 0; j < dtAsns.Columns.Count; j++)
                {
                    if (string.Equals(dtAsns.Columns[j].ColumnName.Trim(), column.DbColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (column.DisplayName == "预入库类型")
                        {
                            dtAsns.Columns[j].ColumnName = "入库类型";
                        }
                        else if (column.DisplayName == "预入库日期")
                        {
                            dtAsns.Columns[j].ColumnName = "入库日期";
                        }
                        else if (column.DisplayName == "ID")
                        {
                            dtAsns.Columns[j].ColumnName = "ASNID";
                        }
                        else
                        {
                            dtAsns.Columns[j].ColumnName = column.DisplayName;
                        }

                    }
                }
            }
            foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
            {
                //foreach (var column in col.InnerColumns)
                //{
                for (int j = 0; j < dtAsns.Columns.Count; j++)
                {

                    if (string.Equals(dtAsns.Columns[j].ColumnName.Trim(), column.DbColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        dtAsns.Columns[j].ColumnName = column.DisplayName;
                    }
                }
                //}
            }



            return dtAsns;
        }
        private DataTable InitASNDetailFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig)
        {
            DataTable dtAsns = dt.Copy();
            foreach (var column in columnsConfig.Where(c => c.IsKey == true))
            {
                for (int j = 0; j < dtAsns.Columns.Count; j++)
                {
                    if (string.Equals(dtAsns.Columns[j].ColumnName.Trim(), column.DbColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (column.DisplayName == "预入库单类型")
                        {
                            dtAsns.Columns[j].ColumnName = "入库类型";
                        }
                        else if (column.DisplayName == "预入库日期")
                        {
                            dtAsns.Columns[j].ColumnName = "入库日期";
                        }
                        else
                        {
                            dtAsns.Columns[j].ColumnName = column.DisplayName;
                        }

                    }
                }
            }
            foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
            {
                //foreach (var column in col.InnerColumns)
                //{
                for (int j = 0; j < dtAsns.Columns.Count; j++)
                {

                    if (string.Equals(dtAsns.Columns[j].ColumnName.Trim(), column.DbColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        dtAsns.Columns[j].ColumnName = column.DisplayName;
                    }
                }
                //}
            }



            return dtAsns;
        }
        [HttpPost]
        public string ImportReceiptUpdate_Batch()
        {
            IndexViewModel vm = new IndexViewModel();
            string js = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    DataTable dtReceitp = ds.Tables["入库单主信息"];
                    DataTable dtReceitpdetail = ds.Tables["入库单明细信息"];
                    IEnumerable<Column> columnReceipt;
                    IEnumerable<Column> columnReceiptDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Receipt").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptDetail").Count() == 0)
                    {
                        columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }
                    else
                    {
                        columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Column> columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Receipt").ColumnCollection;
                    //IEnumerable<Column> columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptDetail").ColumnCollection;

                    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<Receipt> Receipts = this.InitReceiptFromDataTable(dtReceitp, columnReceipt, useCustomerOrderNumber, sb);
                    IEnumerable<ReceiptDetail> ReceiptDetailss = this.InitReceiptDetailFromDataTable(dtReceitpdetail, columnReceiptDetail, useCustomerOrderNumber, sb);

                    AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
                    List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                    Receipts.Each((i, receipt) =>
                    {
                        WMS_Log_Operation operation = new WMS_Log_Operation();
                        operation.MenuName = "入库单管理";
                        operation.Operation = "入库单-导入";
                        operation.OrderType = "Receipt";
                        operation.Controller = Request.RawUrl;
                        operation.Creator = base.UserInfo.Name;
                        operation.CreateTime = DateTime.Now;
                        operation.ProjectID = (int)base.UserInfo.ProjectID;
                        operation.ProjectName = base.UserInfo.ProjectName;
                        operation.CustomerID = (int)receipt.CustomerID;
                        operation.CustomerName = receipt.CustomerName;
                        operation.WarehouseID = (int)receipt.WarehouseID;
                        operation.WarehouseName = receipt.WarehouseName;
                        operation.OrderNumber = receipt.ReceiptNumber;
                        operation.ExternOrderNumber = receipt.ExternReceiptNumber;
                        logs.Add(operation);
                    });

                    ReceiptDetailss.Each((i, receiptDetail) =>
                    {

                        receiptDetail.Creator = base.UserInfo.Name;
                        receiptDetail.CreateTime = DateTime.Now;

                    });
                    request.Receipts = Receipts;
                    request.ReceiptDetails = ReceiptDetailss;
                    var response = new ReceiptManagementService().EditReceiptAndReceiptDetail_ImportPatch(request);
                    if (response.IsSuccess)
                    {
                        new LogOperationService().AddLogOperation(logs);
                        return new { result = "<h3><font color='#00dd00'>批量更新成功！</font></h3>", IsSuccess = true }.ToJsonString();
                    }

                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>部分更新失败！失败单号如下：</font></h3>" + response.Result.Substring(0, response.Result.Length - 1), IsSuccess = false }.ToJsonString();
                    }

                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }
        private DataSet GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, Runbow.TWS.Common.Constants.TEMPFOLDER);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
            string fullPath = Path.Combine(targetPath, fileName);
            hpf.SaveAs(fullPath);
            hpf.InputStream.Close();

            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(fullPath);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();
            excelHelper.Dispose();
            MyFile.Delete(fullPath);

            return ds;
        }
        [HttpGet]
        public ActionResult PrintShelvesNike(string rid, int Flag)
        {
            Session["PrintShelvesRid"] = null;
            Session["PrintShelvesRid"] = rid;
            ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            rd.ReceiptType = st;
            ViewBag.Id = rid;
            var results = new ReceiptManagementService().PrintShelvesNike(rid, Flag).Result;

            rd.ReceiptCollection = results.ReceiptCollection;
            rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //rd.ReceiptCollection.Each((a, b) =>
            //{
            //    string strGUID = "Receipt" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(rd);
        }
        [HttpGet]
        public ActionResult PrintShelvesYXDR(string rid, int Flag)
        {
            Session["PrintShelvesRid"] = null;
            Session["PrintShelvesRid"] = rid;
            ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            rd.ReceiptType = st;
            ViewBag.Id = rid;
            var results = new ReceiptManagementService().PrintShelvesYXDR(rid, Flag).Result;

            rd.ReceiptCollection = results.ReceiptCollection;
            rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //rd.ReceiptCollection.Each((a, b) =>
            //{
            //    string strGUID = "Receipt" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(rd);
        }
        [HttpGet]
        public ActionResult PrintShelvesYFBLD(string rid, int Flag)
        {
            Session["PrintShelvesRid"] = null;
            Session["PrintShelvesRid"] = rid;
            ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            rd.ReceiptType = st;
            ViewBag.Id = rid;
            var results = new ReceiptManagementService().PrintShelvesYFBLD(rid, Flag).Result;

            rd.ReceiptCollection = results.ReceiptCollection;
            rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //rd.ReceiptCollection.Each((a, b) =>
            //{
            //    string strGUID = "Receipt" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(rd);
        }
        [HttpGet]
        public ActionResult PrintShelves(string rid, int Flag)
        {
            Session["PrintShelvesRid"] = null;
            Session["PrintShelvesRid"] = rid;
            ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            rd.ReceiptType = st;
            ViewBag.Id = rid;
            ViewBag.Flag = Flag;
            GetReceiptAndReceriptDetailsResponse results = new GetReceiptAndReceriptDetailsResponse();
            results = new ReceiptManagementService().PrintShelves(rid, Flag).Result;

            rd.ReceiptCollection = results.ReceiptCollection;
            rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));
            //rd.ReceiptCollection.Each((a, b) =>
            //{
            //    string strGUID = "Receipt" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(rd);
        }
        [HttpPost]
        public ActionResult PrintShelves()
        {
            string id = Session["PrintShelvesRid"].ToString();
            //IEnumerable<Receipt> data1 = Enumerable.Empty<Receipt>();
            GetReceiptAndReceriptDetailsResponse results = new GetReceiptAndReceriptDetailsResponse();
            if (base.UserInfo.ProjectID == 12)
            {
                results = new ReceiptManagementService().PrintShelvesYXDR(id, 1).Result;
            }
            else
            {
                results = new ReceiptManagementService().PrintShelves(id, 1).Result;
            }
            // JavaScriptSerializer js = new JavaScriptSerializer();
            var data1 = results.ReceiptCollection;
            var data2 = results.ReceiptDetailCollection;

            return Json(new { data1 = data1, data2 = data2 });
        }
        //[HttpGet]
        //public ActionResult PrintShelvesYXDR(string rid, int Flag)
        //{
        //    Session["PrintShelvesYXDRRid"] = null;
        //    Session["PrintShelvesYXDRRid"] = rid;
        //    ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
        //    ViewBag.Id = rid;
        //    var results = new ReceiptManagementService().PrintShelvesYXDR(rid, Flag).Result;

        //    rd.ReceiptCollection = results.ReceiptCollection;
        //    rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
        //    deleteTmpFiles(Server.MapPath("~/TotalImage/"));

        //    rd.ReceiptCollection.Each((a, b) =>
        //    {
        //        string strGUID = "Receipt" + Guid.NewGuid().ToString();
        //        b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
        //        b.PageIndex = "page" + (a + 1);
        //    });
        //    return View(rd);
        //}


        //[HttpPost]
        //public ActionResult PrintShelvesYXDR()
        //{
        //    string id = Session["PrintShelvesYXDRRid"].ToString();
        //    var results = new ReceiptManagementService().PrintShelvesYXDR(id, 1).Result;

        //    // JavaScriptSerializer js = new JavaScriptSerializer();
        //    var data1 = results.ReceiptCollection;
        //    var data2 = results.ReceiptDetailCollection;

        //    return Json(new { data1 = data1, data2 = data2 });
        //}
        [HttpPost]
        public int GetSkuTotal(string ID, string SKU = null, string BoxNumber = null, string Batchs = null)
        {
            int m = new ReceiptManagementService().GetSkuTotal(ID, SKU, BoxNumber, Batchs);
            return m;
        }
        public ActionResult PrintDemo()
        {
            return View();
            //return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
        }
        private string GetDimensionalCode(string link, string filename)
        {
            Bitmap bmp = null;

            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                //int version = Convert.ToInt16(cboVersion.Text);
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                bmp = qrCodeEncoder.Encode(link, Encoding.UTF8);
                Image ima = bmp;
                string paths = Server.MapPath("~/TotalImage/");
                ima.Save(paths + filename);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Invalid version !");
            }

            return filename;
        }
        private void deleteTmpFiles(string strPath)
        {
            //删除这个目录下的所有子目录
            //if (SysIO.Directory.GetDirectories(strPath).Length > 0)
            //{
            //    foreach (string var in SysIO.Directory.GetDirectories(strPath))
            //    {
            //        //DeleteDirectory(var);
            //        SysIO.Directory.Delete(var, true);
            //        //DeleteDirectory(var);
            //    }
            //}
            //删除这个目录下的所有文件
            if (SysIO.Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string va in SysIO.Directory.GetFiles(strPath))
                {
                    if (va.Contains("Receipt"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
        }
        [HttpGet]
        public ActionResult GenerateBarCode(long ID)
        {
            IndexViewModel vm = new IndexViewModel();
            var GetResponse = new ReceiptManagementService().ReceiptDetailAndBarCodeQuery(null, ID);
            vm.receipt = GetResponse.Result.Receipt;
            vm.ReceiptDetailCollection = GetResponse.Result.ReceiptDetailCollection;

            //var responseBarcode = new OrderManagementService().GetBarCodeByOrderID(ID, "Receipt");
            //vm.BarCodeCollection = responseBarcode.Result;
            return View(vm);
        }
        public string GetBarCode(long OrderID, long DetailID, string SKU, string Type)
        {
            IEnumerable<BarCodeInfo> list = new BarCodeService().GetBarCodeByOrderID(OrderID, Type, DetailID, SKU);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(list);
            return json;
        }
        public string GeneraateBarCodeByJson(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //List<BarCodeTable> list = jss.Deserialize<List<BarCodeTable>>(json);
            List<BarCodeInfo> list = jss.Deserialize<List<BarCodeInfo>>(json);
            BarCodeService service = new BarCodeService();
            string message = service.GenerateBarCode(list);
            return message;
        }
        public ActionResult PrintBarCode(string DetailIDs, string IDS)
        {
            IndexViewModel vm = new IndexViewModel();
            BarCodeService service = new BarCodeService();
            IEnumerable<BarCodeInfo> list;
            if (!string.IsNullOrEmpty(IDS))
            {
                list = service.GetBarCodeByIDS(IDS);
            }
            else
            {
                list = service.GetBarCodeByDetailIDS(DetailIDs);
            }

            vm.BarCodeCollection = list;
            return View(vm);
        }
        public ActionResult ScanBarCode(long ID)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.receipt = new Receipt();
            vm.receipt.ID = ID;
            IEnumerable<BarCodeInfo> list_barCode = new BarCodeService().GetBarCodeByOID(ID);
            vm.BarCodeCollection = list_barCode;
            int BarCodeCount = 0;
            int QtyCount = 0;
            new BarCodeService().GetBarCodeCount(ID, out BarCodeCount, out QtyCount);
            ViewBag.BarCodeCount = BarCodeCount;
            ViewBag.QtyCount = QtyCount;
            return View(vm);
        }
        public string CheckScanBarCode(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //List<BarCodeTable> list = jss.Deserialize<List<BarCodeTable>>(json);
            List<BarCodeInfo> list = jss.Deserialize<List<BarCodeInfo>>(json);
            BarCodeService service = new BarCodeService();
            IEnumerable<BarCodeInfo> list2 = service.CheckScanBarCode(list);
            foreach (var item in list)
            {
                if (list2.Where(c => c.BarCode == item.BarCode).Count() > 0)
                {
                    var codes = list2.Where(c => c.BarCode == item.BarCode);
                    item.Result = "sku:" + list2.Where(c => c.BarCode == item.BarCode).First().SKU;
                }
                else
                {
                    item.Result = "条码错误！";
                }
            }
            return jss.Serialize(list);
        }
        public string SaveScanBarCode(long ID, string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<BarCodeInfo> list = jss.Deserialize<List<BarCodeInfo>>(json);
            string message = new BarCodeService().GenerateBarCodeByScan(ID, list);
            return message;
        }
        public string SupplyBarCode(long ID)
        {
            string message = new BarCodeService().SupplyBarCode(ID);
            return message;
        }
        /// <summary>
        /// 入库单推送鲸仓
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public JsonResult ReceiptSend(string JCRequestList)
        {
            IList<JCAPiResponse> listResponses = new List<JCAPiResponse>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                string url = UtilConstants.JCSendAPIAddress + "ReceiptSend";
                List<JCRequestLists> list = jsonlist<JCRequestLists>(JCRequestList.Substring(18, JCRequestList.Length - 19));
                string res = this.HTTPPost(url, JCRequestList);
                listResponses = jsonlist<JCAPiResponse>(res);
                foreach (var item in listResponses)
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "入库单管理",
                        Operation = "入库单-推送鲸仓",
                        OrderType = "ReceiptSend",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderNumber = item.relatednumber,
                        Str1 = list.Where(c => c.RelateNumber == item.relatednumber).ToJsonString(),//请求报文
                        Str2 = item.message   //返回结果
                    });
                }
                new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception ex)
            {
                logs.Add(new WMS_Log_Operation()
                {
                    MenuName = "入库单管理",
                    Operation = "入库单-推送鲸仓",
                    OrderType = "ReceiptSend",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderNumber = "",
                    Str1 = JCRequestList,
                    Str2 = ex.Message
                });
                new LogOperationService().AddLogOperation(logs);
            }

            return Json(new { Result = listResponses });
        }

        /// <summary>
        /// 爱库存点击上架直接加入库存，箱号作为库位
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddInventoryAKC(string ID)
        {
            string msg = "";
            bool IsSuccess = new ReceiptManagementService().AddInventoryAKC(ID, base.UserInfo.Name, out msg);
            if (IsSuccess && msg == "")
            {
                return new { code = 0, message = msg }.ToJsonString();
            }
            else
            {
                return new { code = 402, message = msg }.ToJsonString();
            }
        }

        /// <summary>
        /// HABA更新订单总体积
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateReceiptVolume(string ID, string Volume)
        {
            string msg = "";
            bool IsSuccess = new ReceiptManagementService().UpdateReceiptVolume(ID, Volume, base.UserInfo.Name, out msg);
            if (IsSuccess && msg == "")
            {
                return new { code = 0, message = msg }.ToJsonString();
            }
            else
            {
                return new { code = 402, message = msg }.ToJsonString();
            }
        }
        /// <summary>
        /// POST提交
        /// </summary>
        private string HTTPPost(string url, string request)
        {
            string responseStr = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = @"POST";
            req.ContentType = "application/json;charset=UTF-8";

            //req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "text/xml";
            if (!string.IsNullOrEmpty(request))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(request);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            WebResponse wr;// = req.GetResponse();



            try
            {
                wr = req.GetResponse();
            }
            catch (WebException ex)
            {
                wr = ex.Response;
                throw ex;
            }

            Stream responseStream = wr.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        /// <summary>
        /// 吉特上架单
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintShelves_JT(string rid, int Flag)
        {
            Session["PrintShelvesRid"] = null;
            Session["PrintShelvesRid"] = rid;
            ReceiptDetailViewModel rd = new ReceiptDetailViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG");
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            rd.ReceiptType = st;
            ViewBag.Id = rid;
            ViewBag.Flag = Flag;
            GetReceiptAndReceriptDetailsResponse results = new GetReceiptAndReceriptDetailsResponse();
            results = new ReceiptManagementService().PrintShelves_JT(rid, Flag).Result;

            rd.ReceiptCollection = results.ReceiptCollection;
            rd.ReceiptDetailCollection = results.ReceiptDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));
            //rd.ReceiptCollection.Each((a, b) =>
            //{
            //    string strGUID = "Receipt" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.ReceiptNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(rd);
        }
        [HttpPost]
        public ActionResult PrintShelves_JT()
        {
            string id = Session["PrintShelvesRid"].ToString();
            //IEnumerable<Receipt> data1 = Enumerable.Empty<Receipt>();
            GetReceiptAndReceriptDetailsResponse results = new GetReceiptAndReceriptDetailsResponse();
            if (base.UserInfo.ProjectID == 12)
            {
                results = new ReceiptManagementService().PrintShelvesYXDR(id, 1).Result;
            }
            else
            {
                results = new ReceiptManagementService().PrintShelves_JT(id, 1).Result;
            }
            // JavaScriptSerializer js = new JavaScriptSerializer();
            var data1 = results.ReceiptCollection;
            var data2 = results.ReceiptDetailCollection;

            return Json(new { data1 = data1, data2 = data2 });
        }

    }
}

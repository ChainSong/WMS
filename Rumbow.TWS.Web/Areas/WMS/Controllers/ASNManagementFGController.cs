using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using Runbow.TWS.Web.Areas.WMS.Models.ASNManagement;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS;
using SMedia = System.Media;
using Runbow.TWS.Entity.WMS.Receipt;
using System.Web.Script.Serialization;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class ASNManagementFGController : BaseController
    {
        ///// <summary>
        ///// 查看扫描ASN差异
        ///// </summary>
        ///// <param name="AsnNumber"></param>
        ///// <returns></returns>
        //public ActionResult ShowDiff(string AsnNumber)
        //{
        //    IndexViewModel vm = new IndexViewModel();
        //    var response = new ASNManagementService().GetASNScanByAsnNumber(AsnNumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.ExpectTotalBox = response.Result.ExpectTotalBox;
        //        vm.ReceiveTotalBox = response.Result.ReceiveTotalBox;
        //        vm.ExpectTotalSKU = response.Result.ExpectTotalSKU;
        //        vm.ReceiveTotalSKU = response.Result.ReceiveTotalSKU;
        //        vm.ASNScanBoxSKUCollection = response.Result.ASNScanBoxSKUCollection;
        //        vm.ASNScanBoxDetailSKUCollection = response.Result.ASNScanBoxDetailSKUCollection;
        //    }
        //    return View(vm);
        //}

        /// <summary>
        /// 扫描更新数量
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="str2"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        //public string AsnScanQtyUpdate(string AsnNumber, string str2, string SKU)
        //{
        //    var response = "";
        //    try
        //    {
        //        response = new ASNManagementService().AsnScanQtyUpdate(AsnNumber, str2, SKU, base.UserInfo.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ex.Message;
        //    }
        //    return response;
        //}
        //[HttpGet]
        //public ActionResult AsnScanIndex(string AsnNumber)
        //{
        //    IndexViewModel vm = new IndexViewModel();
        //    var response = new ASNManagementService().GetASNDetailForScanByAsnNumber(AsnNumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.ASNDetailCollection = response.Result.AsnDetailCollection;
        //    }
        //    return View(vm);
        //}
        //[HttpGet]
        //public ActionResult AsnScanLocationPrint(string AsnNumber)
        //{
        //    IndexViewModel vm = new IndexViewModel();
        //    var response = new ASNManagementService().GetASNDetailForScanByAsnNumber(AsnNumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.ASNDetailCollection = response.Result.AsnDetailCollection;
        //    }
        //    return View(vm);
        //}
        //[HttpGet]
        //public ActionResult PopupIndex(int? PageIndex, long? customerID)
        //{
        //    Session["ASNConditionModel"] = null;
        //    var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
        //    var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
        //    var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
        //    ViewBag.CustomerList = CustomerList;
        //    IndexViewModel vm = new IndexViewModel();
        //    IEnumerable<WMSConfig> wms = null;
        //    try
        //    {
        //        wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    if (wms == null)
        //    {
        //        wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
        //    }
        //    List<SelectListItem> st = new List<SelectListItem>();
        //    foreach (WMSConfig w in wms)
        //    {
        //        st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
        //    }
        //    vm.ASNTypes = st;
        //    vm.ASNCondition = new ASNSearchCondition();
        //    vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

        //    if (base.UserInfo.UserType == 0)
        //    {
        //        vm.ASNCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
        //    }
        //    else if (base.UserInfo.UserType == 2)
        //    {
        //        if (customerID.HasValue)
        //        {
        //            vm.ASNCondition.CustomerID = customerID;
        //        }
        //        else
        //        {
        //            var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
        //            if (customerIDs != null && customerIDs.Count() == 1)
        //            {
        //                vm.ASNCondition.CustomerID = customerIDs.First();
        //            }
        //            else
        //            {
        //                vm.ASNCondition.CustomerID = 0;
        //            }
        //        }
        //    }
        //    IEnumerable<SelectListItem> WarehouseList = null;
        //    if (vm.ASNCondition.CustomerID == 0)
        //    {
        //        WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //                                            .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
        //        StringBuilder sb = new StringBuilder();

        //        foreach (var i in CustomerListID)
        //        {
        //            sb.Append("" + i + ",");
        //        }
        //        if (sb.Length > 1)
        //        {
        //            vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
        //        }
        //        else
        //        {
        //            vm.ASNCondition.CustomerIDs = "0";
        //        }
        //    }
        //    else
        //    {
        //        WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //                             .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

        //    }
        //    if (WarehouseList != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        foreach (var i in WarehouseList)
        //        {
        //            //sb.Append("'" + i.Value + "',");
        //            sb.Append("" + i.Value + ",");
        //        }
        //        if (sb.Length > 1)
        //        {
        //            vm.ASNCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
        //            //vm.ASNCondition.WarehouseID = Int64.Parse(vm.ASNCondition.WarehouseName);
        //        }
        //    }
        //    ViewBag.WarehouseList = WarehouseList;

        //    #region 屏蔽

        //    //if (base.UserInfo.UserType == 0)
        //    //{
        //    //    vm.ASNCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
        //    //}
        //    //else if (base.UserInfo.UserType == 2)
        //    //{
        //    //    if (customerID.HasValue)
        //    //    {
        //    //        vm.ASNCondition.CustomerID = customerID;
        //    //    }
        //    //    else
        //    //    {
        //    //        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
        //    //        if (customerIDs != null && customerIDs.Count() == 1)
        //    //        {
        //    //            vm.ASNCondition.CustomerID = customerIDs.First();
        //    //        }
        //    //    }
        //    //}
        //    //IEnumerable<SelectListItem> WarehouseList = null;
        //    //var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
        //    //if (vm.ASNCondition.CustomerID == null)
        //    //{
        //    //    WarehouseList = WarehouseListAll.Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //    //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
        //    //    StringBuilder sb = new StringBuilder();

        //    //    foreach (var i in CustomerListID)
        //    //    {
        //    //        sb.Append("" + i + ",");
        //    //    }
        //    //    if (sb.Length > 1)
        //    //    {
        //    //        vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
        //    //    }
        //    //    else
        //    //    {
        //    //        vm.ASNCondition.CustomerIDs = "0";
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    WarehouseList = WarehouseListAll.Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //    //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
        //    //}

        //    //ViewBag.WarehouseList = WarehouseList;
        //    //if (CustomerList.Count() == 1)
        //    //{
        //    //    vm.ASNCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
        //    //}
        //    //if (WarehouseList.Count() == 1)
        //    //{
        //    //    vm.ASNCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
        //    //}

        //    //vm.ASNCondition.StartExpectDate = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
        //    //vm.ASNCondition.EndExpectDate = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();
        //    #endregion 

        //    vm.ASNCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
        //    vm.ASNCondition.EndCreateTime = DateTime.Now;
        //    //vm.ASNCondition.Status = 1;
        //    Session["ASNConditionModel"] = vm.ASNCondition;
        //    this.GenQueryASNViewModel(vm);
        //    GetASNByConditionRequest getAsnByConditionRequest = new GetASNByConditionRequest();
        //    getAsnByConditionRequest.SearchCondition = vm.ASNCondition;
        //    getAsnByConditionRequest.PageSize = UtilConstants.PAGESIZE;
        //    getAsnByConditionRequest.PageIndex = PageIndex ?? 0;
        //    var getReceiptByConditionResponse = new ASNManagementService().GetASNByCondition(getAsnByConditionRequest);
        //    if (getReceiptByConditionResponse.IsSuccess)
        //    {
        //        vm.ASNCollection = getReceiptByConditionResponse.Result.ASNCollection;
        //        vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
        //        vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
        //    }

        //    return View(vm);
        //}

        /// <summary>
        /// ASN预检删除重扫本箱
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        //public string ClearAsnBoxNumber(string AsnNumber, string ScanBoxNumber)
        //{
        //    string msg = "";
        //    try
        //    {
        //        msg = new ASNManagementService().ClearAsnBoxNumber(AsnNumber, ScanBoxNumber);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    return msg;
        //}

        /// <summary>
        /// 检查箱差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        //public string CheckDiff(string AsnNumber, string ScanBoxNumber)
        //{
        //    var response = new ASNManagementService().CheckDiff(AsnNumber, ScanBoxNumber);
        //    //if (response == "Diff")
        //    //{
        //    //    PlayError();
        //    //}
        //    return response;
        //}

        /// <summary>
        /// 检查箱差异退货仓
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        //public JsonResult CheckDiffReturn(string AsnNumber, string ScanBoxNumber)
        //{
        //    List<ASNDetail> lists = new List<ASNDetail>();
        //    try
        //    {
        //        lists = new ASNManagementService().CheckDiffReturn(AsnNumber, ScanBoxNumber);
        //        if (lists.Count() > 0)
        //        {
        //            return Json(new { Code = "1", data = lists });
        //        }
        //        else
        //        {
        //            return Json(new { Code = "0" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Code = "-1" });
        //    }

        //}
        /// <summary>
        /// 获取ASN预检当前箱总件数
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        //public JsonResult GetAsnScanBoxSum(string AsnNumber, string ScanBoxNumber)
        //{
        //    List<ASNDetail> lists = new List<ASNDetail>();
        //    try
        //    {
        //        lists = new ASNManagementService().GetAsnScanBoxSum(AsnNumber, ScanBoxNumber);
        //        if (lists.Count() > 0)
        //        {
        //            return Json(new { Code = "1", data = lists });
        //        }
        //        else
        //        {
        //            return Json(new { Code = "0" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Code = "-1" });
        //    }

        //}
        /// <summary>
        /// 导出差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        //public void ExportDiff(string AsnNumber)
        //{
        //    IndexViewModel vm = new IndexViewModel();
        //    var response = new ASNManagementService().GetASNScanByAsnNumber(AsnNumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.ExpectTotalBox = response.Result.ExpectTotalBox;
        //        vm.ReceiveTotalBox = response.Result.ReceiveTotalBox;
        //        vm.ExpectTotalSKU = response.Result.ExpectTotalSKU;
        //        vm.ReceiveTotalSKU = response.Result.ReceiveTotalSKU;
        //        vm.ASNScanBoxSKUCollection = response.Result.ASNScanBoxSKUCollection;
        //        vm.ASNScanBoxDetailSKUCollection = response.Result.ASNScanBoxDetailSKUCollection;
        //        IEnumerable<ASNScan> TotalBox = null;
        //        DataSet ds = new DataSet();
        //        DataTable dtTotalBox = new DataTable();
        //        DataRow drTotalBox = dtTotalBox.NewRow();
        //        dtTotalBox.Columns.Add("期望总箱数");
        //        dtTotalBox.Columns.Add("实收总箱数");
        //        drTotalBox["期望总箱数"] = vm.ExpectTotalBox.ExpectTotalBox;
        //        drTotalBox["实收总箱数"] = vm.ReceiveTotalBox.ReceiveTotalBox;
        //        dtTotalBox.Rows.Add(drTotalBox);
        //        dtTotalBox.TableName = "总箱数差异";

        //        DataTable dtTotalSku = new DataTable();
        //        DataRow drTotalSku = dtTotalSku.NewRow();
        //        dtTotalSku.Columns.Add("期望总件数");
        //        dtTotalSku.Columns.Add("实收总件数");
        //        drTotalSku["期望总件数"] = vm.ExpectTotalSKU.ExpectTotalSKU;
        //        drTotalSku["实收总件数"] = vm.ReceiveTotalSKU.ReceiveTotalSKU;
        //        dtTotalSku.Rows.Add(drTotalSku);
        //        dtTotalSku.TableName = "总件数差异";

        //        DataTable dtBoxSKU = new DataTable();
        //        dtBoxSKU.Columns.Add("箱号");
        //        dtBoxSKU.Columns.Add("期望件数");
        //        dtBoxSKU.Columns.Add("实收件数");
        //        foreach (var item in vm.ASNScanBoxSKUCollection)
        //        {
        //            DataRow drBoxSKU = dtBoxSKU.NewRow();
        //            drBoxSKU["箱号"] = item.str2;
        //            drBoxSKU["期望件数"] = item.ExpectBoxSKU;
        //            drBoxSKU["实收件数"] = item.ReceiveBoxSKU;
        //            dtBoxSKU.Rows.Add(drBoxSKU);
        //        }
        //        dtBoxSKU.TableName = "箱件数差异";

        //        DataTable dtBoxDetailSKU = new DataTable();
        //        dtBoxDetailSKU.Columns.Add("箱号");
        //        dtBoxDetailSKU.Columns.Add("产品编码");
        //        dtBoxDetailSKU.Columns.Add("期望数量");
        //        dtBoxDetailSKU.Columns.Add("实收数量");
        //        foreach (var item in vm.ASNScanBoxDetailSKUCollection)
        //        {
        //            DataRow drBoxDetailSKU = dtBoxDetailSKU.NewRow();
        //            drBoxDetailSKU["箱号"] = item.str2;
        //            drBoxDetailSKU["产品编码"] = item.SKU;
        //            drBoxDetailSKU["期望数量"] = item.ExpectBoxDetailSKU;
        //            drBoxDetailSKU["实收数量"] = item.BoxDetailSKU;
        //            dtBoxDetailSKU.Rows.Add(drBoxDetailSKU);
        //        }
        //        dtBoxDetailSKU.TableName = "箱明细差异";
        //        ds.Tables.Add(dtTotalBox);
        //        ds.Tables.Add(dtTotalSku);
        //        ds.Tables.Add(dtBoxSKU);
        //        ds.Tables.Add(dtBoxDetailSKU);
        //        //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "ASN差异明细_" + AsnNumber.ToString());
        //        EPPlusOperation.ExportDataSetByEPPlus(ds, "ASN差异明细_" + AsnNumber.ToString());
        //    }
        //}

        /// <summary>
        /// 播放错误提示音
        /// </summary>
        //public void PlayError()
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory;
        //    SMedia.SoundPlayer player = new SMedia.SoundPlayer();
        //    player.SoundLocation = path + "Media\\error.wav";
        //    player.Load();
        //    player.Play();
        //}
        //查询和导出
        //[HttpPost]
        //public ActionResult PopupIndex(IndexViewModel vm, int? PageIndex, string Action)
        //{
        //    IEnumerable<WMSConfig> wms = null;
        //    try
        //    {
        //        wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    if (wms == null)
        //    {
        //        wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
        //    }
        //    List<SelectListItem> st = new List<SelectListItem>();
        //    foreach (WMSConfig w in wms)
        //    {
        //        st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
        //    }
        //    vm.ASNTypes = st;

        //    if (vm.ASNCondition == null && Session["ASNConditionModel"] != null)
        //    {
        //        vm.ASNCondition = (ASNSearchCondition)Session["ASNConditionModel"];
        //    }
        //    else if (vm.ASNCondition == null && Session["ASNConditionModel"] == null)
        //    {
        //        vm.ASNCondition = new ASNSearchCondition();
        //        Session["ASNConditionModel"] = null;
        //        Session["ASNConditionModel"] = vm.ASNCondition;
        //    }
        //    var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
        //    var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
        //    var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
        //    ViewBag.CustomerList = CustomerList;
        //    //ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
        //    //.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
        //    IEnumerable<SelectListItem> WarehouseList = null;
        //    if (vm.ASNCondition.CustomerID == null)
        //    {
        //        WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //                                            .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
        //        StringBuilder sb = new StringBuilder();

        //        foreach (var i in CustomerListID)
        //        {
        //            sb.Append("" + i + ",");
        //        }
        //        if (sb.Length > 1)
        //        {
        //            vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
        //        }
        //        else
        //        {
        //            vm.ASNCondition.CustomerIDs = "0";
        //        }
        //        vm.ASNCondition.CustomerID = 0;
        //    }
        //    else
        //    {
        //        WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
        //                             .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
        //    }

        //    if (vm.ASNCondition.WarehouseID != 0)
        //    {
        //        //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
        //    }
        //    else
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        foreach (var i in WarehouseList)
        //        {
        //            //sb.Append("'" + i.Value + "',");
        //            sb.Append("" + i.Value + ",");
        //        }
        //        if (sb.Length > 1)
        //        {
        //            vm.ASNCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
        //        }
        //    }
        //    ViewBag.WarehouseList = WarehouseList;
        //    vm.ASNCondition.UserType = base.UserInfo.UserType;
        //    vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
        //    var getASNByConditionRequest = new GetASNByConditionRequest();
        //    if (Action == "查询" || Action == "PopupIndex")
        //    {
        //        getASNByConditionRequest.SearchCondition = vm.ASNCondition;
        //        getASNByConditionRequest.PageSize = UtilConstants.PAGESIZE;
        //        getASNByConditionRequest.PageIndex = PageIndex ?? 0;
        //    }
        //    else if (Action == "导出")
        //    {
        //        getASNByConditionRequest.SearchCondition = vm.ASNCondition;
        //        getASNByConditionRequest.PageSize = UtilConstants.PAGESIZE;
        //        getASNByConditionRequest.PageIndex = 0;
        //    }

        //    var getASNByConditionResponse = new ASNManagementService().GetASNDetailByConditionResponse(getASNByConditionRequest);

        //    if (getASNByConditionResponse.IsSuccess || Action == "下载模板")
        //    {
        //        vm.ASNCollection = getASNByConditionResponse.Result.AsnCollection;
        //        vm.PageIndex = getASNByConditionResponse.Result.PageIndex;
        //        vm.PageCount = getASNByConditionResponse.Result.PageCount;
        //        if (Action == "导出" || Action == "下载模板")
        //        {
        //            IEnumerable<Column> columnasn;
        //            IEnumerable<Column> columnasnDetail;
        //            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.ASNCondition.CustomerID).ProjectCollection.First();
        //            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
        //            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
        //            {
        //                columnasn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
        //            }
        //            else
        //            {
        //                columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
        //            }
        //            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
        //            {
        //                columnasnDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
        //            }
        //            else
        //            {
        //                columnasnDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
        //            }


        //            if (vm.ASNCondition.CustomerID == 0)
        //            {
        //                columnasn = columnasn.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
        //                columnasnDetail = columnasnDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
        //            }
        //            else
        //            {
        //                var notKeyColumns1 = columnasn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.ASNCondition.CustomerID));
        //                //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID)))
        //                //.Select(c =>
        //                //{
        //                //    if (c.InnerColumns.Count == 0)
        //                //    {
        //                //        return c;
        //                //    }
        //                //    else
        //                //    {
        //                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID))
        //                //        {
        //                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID);
        //                //        }

        //                //        return c;
        //                //    }
        //                //});
        //                var notKeyColumns2 = columnasnDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.ASNCondition.CustomerID));
        //                //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID)))
        //                //.Select(c =>
        //                //{
        //                //    if (c.InnerColumns.Count == 0)
        //                //    {
        //                //        return c;
        //                //    }
        //                //    else
        //                //    {
        //                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID))
        //                //        {
        //                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID);
        //                //        }
        //                //        return c;
        //                //    }
        //                //});
        //                columnasn = columnasn.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
        //                columnasnDetail = columnasnDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
        //            }
        //            if (Action == "导出")
        //            {
        //                //查询要导出的数据
        //                getASNByConditionResponse = new ASNManagementService().GetExportAsnandDetailByCondition(getASNByConditionRequest);//导出数据的查询

        //                Export(getASNByConditionResponse.Result, columnasn, columnasnDetail, 1);
        //            }
        //            else if (Action == "下载模板")
        //            {
        //                Export(getASNByConditionResponse.Result, columnasn.Where(c => (c.DbColumnName != "ASNNumber" &&
        //                    c.DbColumnName != "ASNStatusName" && c.DbColumnName != "CompleteDate" && c.DbColumnName != "CustomerName")),
        //                    columnasnDetail.Where(m => (m.DbColumnName != "ASNNumber" && m.DbColumnName != "LineNumber"
        //                        && m.DbColumnName != "GoodsName" && m.DbColumnName != "Qty" && m.DbColumnName != "CustomerName")), 2);
        //            }

        //        }
        //    }
        //    GenQueryASNViewModel(vm);
        //    return View(vm);
        //}

        [HttpGet]
        public ActionResult Index(int? PageIndex, long? customerID)
        {
            Session["ASNConditionModel"] = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
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
            vm.ASNTypes = st;
            vm.ASNCondition = new ASNSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            if (base.UserInfo.UserType == 0)
            {
                vm.ASNCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.ASNCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null)//&& customerIDs.Count() == 1
                    {
                        vm.ASNCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.ASNCondition.CustomerID = 0;
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.ASNCondition.CustomerID == 0)
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
                    vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.ASNCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }
            if (WarehouseList != null)
            {
                vm.ASNCondition.WarehouseID = Int64.Parse(WarehouseList.FirstOrDefault().Value);//add default warehouse
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.ASNCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                    //vm.ASNCondition.WarehouseID = Int64.Parse(vm.ASNCondition.WarehouseName);
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            #region 屏蔽

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.ASNCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    if (customerID.HasValue)
            //    {
            //        vm.ASNCondition.CustomerID = customerID;
            //    }
            //    else
            //    {
            //        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            //        if (customerIDs != null && customerIDs.Count() == 1)
            //        {
            //            vm.ASNCondition.CustomerID = customerIDs.First();
            //        }
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            //if (vm.ASNCondition.CustomerID == null)
            //{
            //    WarehouseList = WarehouseListAll.Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.ASNCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = WarehouseListAll.Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;
            //if (CustomerList.Count() == 1)
            //{
            //    vm.ASNCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.ASNCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            //}

            //vm.ASNCondition.StartExpectDate = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            //vm.ASNCondition.EndExpectDate = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();
            #endregion 

            vm.ASNCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.ASNCondition.EndCreateTime = DateTime.Now;
            Session["ASNConditionModel"] = vm.ASNCondition;

            this.GenQueryASNViewModel(vm);

            GetASNByConditionRequest getAsnByConditionRequest = new GetASNByConditionRequest();
            getAsnByConditionRequest.SearchCondition = vm.ASNCondition;
            getAsnByConditionRequest.SearchCondition.Model = "产品";
            getAsnByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getAsnByConditionRequest.PageIndex = PageIndex ?? 0;
            var getReceiptByConditionResponse = new ASNManagementService().GetASNByCondition(getAsnByConditionRequest);
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ASNCollection = getReceiptByConditionResponse.Result.ASNCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            }

            return View(vm);
        }
        /// <summary>
        /// 入库单状态统计
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public ActionResult ReceiptStatusStatis(long CustomerID)
        {
            IndexViewModel vm = new IndexViewModel();
            #region 获取状态（receipt）
            IEnumerable<WMSConfig> receiptstatus = null;
            receiptstatus = ApplicationConfigHelper.GetWMS_Config("ReceiptStatus");
            List<SelectListItem> receiptst = new List<SelectListItem>();
            foreach (var item in receiptstatus)
            {
                receiptst.Add(new SelectListItem() { Text = item.Name, Value = item.Code });
            }
            ViewBag.ReceiptSt = receiptst;
            #endregion
            vm.ASNCondition = new ASNSearchCondition();

            //获取customerList
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Where(m => m.CustomerID == CustomerID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            if (CustomerList.Count() == 1)
            {
                vm.ASNCondition.CustomerID = CustomerList.Select(m => m.Value).FirstOrDefault().ObjectToInt32();
            }
            //获取客户对应的仓库
            var warehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID))
                               .Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                               .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.WarehouseList = warehouseList;

            if (warehouseList.Count() == 1)
            {
                vm.ASNCondition.WarehouseName = warehouseList.Select(c => c.Text).FirstOrDefault();
            }
            vm.ASNCondition.StartCreateTime = DateTime.Now;
            vm.ASNCondition.EndCreateTime = DateTime.Now;
            GetASNByConditionRequest getAsnByConditionRequest = new GetASNByConditionRequest();
            getAsnByConditionRequest.SearchCondition = vm.ASNCondition;
            getAsnByConditionRequest.SearchCondition.Model = "产品";
            getAsnByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getAsnByConditionRequest.PageIndex = 0;
            var getReceiptByConditionResponse = new ASNManagementService().GetASNStatusByCondition(getAsnByConditionRequest);
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ASNCollection = getReceiptByConditionResponse.Result.ASNCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            }

            return View(vm);
        }
        /// <summary>
        /// 入库单状态统计
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReceiptStatusStatis(IndexViewModel vm, string Action)
        {
            #region 获取状态（receipt）
            IEnumerable<WMSConfig> receiptstatus = null;
            receiptstatus = ApplicationConfigHelper.GetWMS_Config("ReceiptStatus");
            List<SelectListItem> receiptst = new List<SelectListItem>();
            foreach (var item in receiptstatus)
            {
                receiptst.Add(new SelectListItem() { Text = item.Name, Value = item.Code });
            }
            ViewBag.ReceiptSt = receiptst;
            #endregion

            //获取customerList
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Where(m => m.CustomerID == vm.ASNCondition.CustomerID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //获取客户对应的仓库
            var warehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID))
                               .Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                               .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.WarehouseList = warehouseList;


            ViewBag.ProjectName = base.UserInfo.ProjectName;
            if (warehouseList.Count() == 1)
            {
                vm.ASNCondition.WarehouseName = warehouseList.Select(c => c.Text).FirstOrDefault();
            }

            GetASNByConditionRequest getAsnByConditionRequest = new GetASNByConditionRequest();
            getAsnByConditionRequest.SearchCondition = vm.ASNCondition;

            var getReceiptByConditionResponse = new ASNManagementService().GetASNStatusByCondition(getAsnByConditionRequest);
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ASNCollection = getReceiptByConditionResponse.Result.ASNCollection;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            }

            return View(vm);
        }
        /// <summary>
        /// 根据入库单状态查询订单
        /// </summary>
        /// <returns></returns>
        public JsonResult SearchReceiptOrderTotal(long CustomerID, string Warehouose, int Status, DateTime? StartTime, DateTime? EndTime, int type)
        {
            IEnumerable<ASN> info = null;//最后统计的订单信息
            ASNSearchCondition search = new ASNSearchCondition();
            search.CustomerID = CustomerID;
            search.WarehouseName = Warehouose;
            search.Status = Status;
            search.StartCreateTime = StartTime;
            search.EndCreateTime = EndTime;
            var response = new ASNManagementService().SearchReceiptOrderTotal(search, type);
            if (response.IsSuccess && response.Result.ASNCollection.Count() > 0)
            {
                info = response.Result.ASNCollection;
                var data = from q in info
                           select new
                           {
                               q.ASNNumber,
                               q.ExternReceiptNumber,
                               q.str1,
                               q.ASNType,
                               q.Int1,
                               q.WarehouseName
                           };
                return Json(new { Errorcode = 1, data = data });
            }
            else
            {
                return Json(new { Errorcod = 0, data = "未查询到数据" });
            }
        }
        private void GenQueryASNViewModel(IndexViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.ASNCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_ASN").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_ASN");
            }
            if (Configs.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_ASNDetail");
            }
            ////vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID))).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN");
            //var Projectss = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.ASNCondition.CustomerID)).ProjectCollection.FirstOrDefault();
            ////.FirstOrDefault(p => p.Id == base.UserInfo.ProjectID.ToString());
            ////var Projectss = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID))).ProjectCollection.FirstOrDefault(p => p.Id == base.UserInfo.ProjectID.ToString());
            //IEnumerable<Module> ModuleCollection = null;
            //if (Projectss != null)
            //{
            //    ModuleCollection = Projectss.ModuleCollection;
            //}
            //if (ModuleCollection != null)
            //{

            //    if (Projectss.ModuleCollection.FirstOrDefault().Tables.TableCollection.FirstOrDefault(t => t.Name == "WMS_ASN") == null)
            //    {
            //        Projectss = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.FirstOrDefault();
            //        //.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //    }
            //    vm.Config = ModuleCollection.FirstOrDefault(m => m.Id == "M002").Tables.TableCollection.FirstOrDefault(t => t.Name == "WMS_ASN");
            //}

            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerList = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerList = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).FirstOrDefault(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerList = Enumerable.Empty<SelectListItem>();
            }
        }
        private void GenQueryASNDetailViewModel(IndexViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.AsnandDetails.asn.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_ASN").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_ASN");
            }
            if (Configs.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_ASNDetail");
            }
            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerList = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerList = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerList = Enumerable.Empty<SelectListItem>();
            }
        }
        //查询和导出
        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex, string Action)
        {
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
            vm.ASNTypes = st;

            if (vm.ASNCondition == null && Session["ASNConditionModel"] != null)
            {
                vm.ASNCondition = (ASNSearchCondition)Session["ASNConditionModel"];
            }
            else if (vm.ASNCondition == null && Session["ASNConditionModel"] == null)
            {
                vm.ASNCondition = new ASNSearchCondition();
                Session["ASNConditionModel"] = null;
                Session["ASNConditionModel"] = vm.ASNCondition;
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.ASNCondition.CustomerID == null)
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
                    vm.ASNCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.ASNCondition.CustomerIDs = "0";
                }
                vm.ASNCondition.CustomerID = 0;
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ASNCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (vm.ASNCondition.WarehouseID != 0)
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
                    vm.ASNCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            vm.ASNCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getASNByConditionRequest = new GetASNByConditionRequest();

            if (Action == "查询" || Action == "Index")
            {
                getASNByConditionRequest.SearchCondition = vm.ASNCondition;
                getASNByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getASNByConditionRequest.PageIndex = PageIndex ?? 0;
            }
            else if (Action == "导出")
            {
                getASNByConditionRequest.SearchCondition = vm.ASNCondition;
                getASNByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getASNByConditionRequest.PageIndex = 0;
            }
            getASNByConditionRequest.SearchCondition.Model = "产品";

            var getASNByConditionResponse = new ASNManagementService().GetASNDetailByConditionResponse(getASNByConditionRequest);

            if (getASNByConditionResponse.IsSuccess || Action == "下载模板")
            {
                vm.ASNCollection = getASNByConditionResponse.Result.AsnCollection;
                vm.PageIndex = getASNByConditionResponse.Result.PageIndex;
                vm.PageCount = getASNByConditionResponse.Result.PageCount;
                if (Action == "导出" || Action == "下载模板")
                {
                    IEnumerable<Column> columnasn;
                    IEnumerable<Column> columnasnDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.ASNCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
                    {
                        columnasn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    else
                    {
                        columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
                    {
                        columnasnDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }
                    else
                    {
                        columnasnDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }


                    if (vm.ASNCondition.CustomerID == 0)
                    {
                        columnasn = columnasn.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnasnDetail = columnasnDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnasn.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.ASNCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID);
                        //        }

                        //        return c;
                        //    }
                        //});
                        var notKeyColumns2 = columnasnDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.ASNCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID)))
                        //.Select(c =>
                        //{
                        //    if (c.InnerColumns.Count == 0)
                        //    {
                        //        return c;
                        //    }
                        //    else
                        //    {
                        //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID))
                        //        {
                        //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.ASNCondition.CustomerID);
                        //        }
                        //        return c;
                        //    }
                        //});
                        columnasn = columnasn.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                        columnasnDetail = columnasnDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
                    }
                    if (Action == "导出")
                    {
                        //查询要导出的数据
                        getASNByConditionResponse = new ASNManagementService().GetExportAsnandDetailByCondition(getASNByConditionRequest);//导出数据的查询

                        Export(getASNByConditionResponse.Result, columnasn, columnasnDetail, 1);
                    }
                    else if (Action == "下载模板")
                    {
                        Export(getASNByConditionResponse.Result, columnasn.Where(c => (c.DbColumnName != "ASNNumber" &&
                            c.DbColumnName != "ASNStatusName" && c.DbColumnName != "CompleteDate" && c.DbColumnName != "CustomerName")),
                            columnasnDetail.Where(m => (m.DbColumnName != "ASNNumber" && m.DbColumnName != "LineNumber"
                                && m.DbColumnName != "GoodsName" && m.DbColumnName != "Qty" && m.DbColumnName != "CustomerName")), 2);
                    }

                }
            }
            GenQueryASNViewModel(vm);
            return View(vm);
        }
        //导出
        private void Export(GetASNDetailByConditionResponse response, IEnumerable<Column> columnasn, IEnumerable<Column> columnasnDetail, int flag)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            int customerID = Convert.ToInt32(CustomerListAll.Select(m => m.CustomerID).First());

            IEnumerable<ASN> asn = response.AsnCollection;
            IEnumerable<ASNDetail> asnDetails = response.AsnDetailCollection;
            DataSet ds = new DataSet();
            DataTable dtasn = new DataTable();
            DataTable dtasnDetail = new DataTable();
            foreach (var asns in columnasn)
            {
                dtasn.Columns.Add(asns.DisplayName, typeof(string));
            }
            foreach (var asnDetail in columnasnDetail)
            {
                dtasnDetail.Columns.Add(asnDetail.DisplayName, typeof(string));
            }
            asn.Each((i, s) =>
            {
                DataRow drasn = dtasn.NewRow();
                foreach (var asnss in columnasn)
                {
                    drasn[asnss.DisplayName] = typeof(Runbow.TWS.Entity.ASN).GetProperty(asnss.DbColumnName).GetValue(s);
                }
                dtasn.Rows.Add(drasn);
            });
            asnDetails.Each((i, s) =>
            {
                DataRow drasndetail = dtasnDetail.NewRow();
                foreach (var asnDetail in columnasnDetail)
                {
                    drasndetail[asnDetail.DisplayName] = typeof(Runbow.TWS.Entity.ASNDetail).GetProperty(asnDetail.DbColumnName).GetValue(s);
                }
                dtasnDetail.Rows.Add(drasndetail);
            });
            if (flag == 2)
            {
                DataRow dr1 = dtasn.NewRow();
                dr1["外部入库单号"] = "XXXXXXXX";
                dr1["仓库名称"] = "XXXX仓";
                dr1["预入库日期"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr1["预入库单类型"] = "采购入库";
                dtasn.Rows.Add(dr1);

                DataRow dr11 = dtasnDetail.NewRow();
                dr11["外部入库单号"] = "XXXXXXXX";
                dr11["产品编码"] = "XXXXXX";
                dr11["预收数量"] = "100";
                dr11["批次号"] = "XXXX";
                dtasnDetail.Rows.Add(dr11);
            }
            dtasn.TableName = "预入库单主信息";
            dtasnDetail.TableName = "预入库单明细信息";
            ds.Tables.Add(dtasn);
            ds.Tables.Add(dtasnDetail);
            if (flag == 1)
            {
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "预入库单" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "预入库单" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            #region YXDRBJ的预入库导入模板下载 lrg
            else if ((flag == 2 && customerID == 79) || (flag == 2 && customerID == 83))
            {
                DataSet asds = new DataSet();
                DataTable asdt = new DataTable();
                asdt.TableName = "YXDR预入库单导入模板";
                try
                {
                    asdt.Columns.Add("预入库单类型", typeof(string));
                    asdt.Columns.Add("预入库日期", typeof(string));
                    asdt.Columns.Add("订单号", typeof(string));
                    asdt.Columns.Add("仓库名称", typeof(string));
                    asdt.Columns.Add("客户", typeof(string));
                    asdt.Columns.Add("产品编码", typeof(string));
                    asdt.Columns.Add("预收数量", typeof(string));
                    asdt.Columns.Add("单位", typeof(string));
                    asdt.Columns.Add("物品类型", typeof(string));
                    asdt.Columns.Add("产品等级", typeof(string));
                    asdt.Columns.Add("箱号", typeof(string));
                    asdt.Columns.Add("备注1", typeof(string));
                    asdt.Columns.Add("备注2", typeof(string));
                    asdt.Columns.Add("备注3", typeof(string));
                    asdt.Columns.Add("备注4", typeof(string));
                    asdt.Columns.Add("备注5", typeof(string));
                    asdt.Columns.Add("备注6", typeof(string));

                    DataRow dr = asdt.NewRow();
                    dr["预入库单类型"] = "大仓补货入库";
                    dr["预入库日期"] = DateTime.Now.ToString("YYYY/MM/DD");
                    dr["订单号"] = "";
                    dr["仓库名称"] = "XXXX仓";
                    dr["客户"] = "";
                    dr["产品编码"] = "";
                    dr["预收数量"] = "100";
                    dr["单位"] = "件";
                    dr["物品类型"] = "APP";
                    dr["产品等级"] = "A品";
                    dr["箱号"] = "";
                    dr["备注1"] = "";
                    dr["备注2"] = "";
                    dr["备注3"] = "";
                    dr["备注4"] = "";
                    dr["备注5"] = "";
                    dr["备注6"] = "";
                    asdt.Rows.Add(dr);
                    asds.Tables.Add(asdt);
                }
                catch
                {
                }
                //ExportDataToExcelHelper.ExportDataSetToExcel(asds, "预入库单导入模板");
                EPPlusOperation.ExportDataSetByEPPlus(asds, "预入库单导入模板");
            }
            #endregion

            else if (flag == 2)
            {
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "预入库单导入模板");
                EPPlusOperation.ExportDataSetByEPPlus(ds, "预入库单导入模板");
            }
        }
        //按照选中导出
        public void ExportOrder(string ids, long? CustomerID)
        {
            var getReceiptByConditionResponse = new ASNManagementService().GetReceiptByIDs(ids);
            IEnumerable<Column> columnReceipt;
            IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
            {
                columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }
            else
            {
                columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }
            if (CustomerID == 0)
            {
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
            }
            else
            {
                var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
            }
            Export(getReceiptByConditionResponse.Result, columnReceipt, columnReceiptDetail, 1);
        }
        //单条取消
        [HttpPost]
        public string ASNDelete(int ID)
        {
            //ApplicationConfigHelper.RefreshASNInfo();
            if (new ASNManagementService().ASNDelete(ID).IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "ASN管理";
                operation.Operation = "ASN-取消";
                operation.OrderType = "ASN";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "取消成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "取消失败！", IsSuccess = false }).ToString();
            }
        }
        //批量取消
        [HttpPost]
        public string ASNStatusReturn(string asnnumberlist)
        {
            ApplicationConfigHelper.RefreshASNInfo();
            if (new ASNManagementService().StatusBacks(asnnumberlist))
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                foreach (string item in asnnumberlist.Split(','))
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "ASN管理";
                    operation.Operation = "ASN-批量取消";
                    operation.OrderType = "ASN";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = item.Replace("'", "").Replace("'", "");
                    logs.Add(operation);
                }
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "取消成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "取消失败！", IsSuccess = false }).ToString();
            }
        }
        //完成
        [HttpPost]
        public string Complet(int ID)
        {
            if (new ASNManagementService().Complet(ID))
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "ASN管理";
                operation.Operation = "ASN-完成";
                operation.OrderType = "ASN";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "失败！", IsSuccess = false }).ToString();
            }
        }
        //批量完成
        [HttpPost]
        public string CompletALLSelect(string asnid)
        {
            if (new ASNManagementService().CompletALLSelect(asnid))
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                foreach (string item in asnid.Split(','))
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "ASN管理";
                    operation.Operation = "ASN-批量完成";
                    operation.OrderType = "ASN";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = item.Replace("'", "").Replace("'", "");
                    logs.Add(operation);
                }
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "失败！", IsSuccess = false }).ToString();
            }
        }
        // 0查看 1新增 2编辑
        [HttpGet]
        public ActionResult ASNCreateOrEdit(int ID, long? customerID, int ViewType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            #region ASNType赋值
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
            vm.ASNTypes = st;
            #endregion
            #region 产品中心赋值
            IEnumerable<WMSConfig> wms_product_center = null;
            try
            {
                wms_product_center = ApplicationConfigHelper.GetWMS_Config("ProductCenter_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms_product_center == null)
            {
                wms_product_center = ApplicationConfigHelper.GetWMS_Config("ProductCenter");
            }
            List<SelectListItem> st_product_center = new List<SelectListItem>();
            foreach (WMSConfig w in wms_product_center)
            {
                st_product_center.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.str8 = st_product_center;
            #endregion

            vm.ASNCondition = new ASNSearchCondition();
            vm.ReturnViewType = "get";
            if (!string.IsNullOrEmpty(customerID.ToString()))
            {
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID&&c.ID==customerID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;

                ViewBag.CustomerList = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                    .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                if (WarehouseList.Count() == 1)
                {
                    vm.ASNCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
                }
            }
            else
            {
                var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
                var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
                var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                      .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.CustomerList = CustomerList;
                if (CustomerList.Count() > 0)
                {
                    vm.ASNCondition.CustomerID = CustomerList.Select(c => c.Value).First().ObjectToInt64();
                }
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
                if (WarehouseList.Count() > 0)
                {
                    vm.ASNCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
                }
            }

            vm.ASNCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getASNByConditionResponse = new ASNManagementService().GetASNInfos(new GetASNByConditionRequest() { ID = ID, SearchCondition=new ASNSearchCondition() {  Model="产品"} });


            if (getASNByConditionResponse.IsSuccess)
            {
                //vm = new IndexViewModel()
                //{
                vm.AsnandDetails = getASNByConditionResponse.Result;
                //};
            }
            if (ViewType == 1)
            {
                vm.AsnandDetails = new ASNAndASNDetail();
                vm.AsnandDetails.asn = new ASN();
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AsnandDetails.asn.CustomerID = base.UserInfo.CustomerOrShipperID;
                    vm.ASNCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AsnandDetails.asn.CustomerID = customerID;
                        vm.ASNCondition.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AsnandDetails.asn.CustomerID = customerIDs.First();
                            vm.ASNCondition.CustomerID = customerIDs.First();
                        }
                    }
                }
            }
            #region 产品中心字段显示中文
            //wms_product_center.Where("")
            #endregion
            vm.ViewType = ViewType;
            var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.ASNCondition.CustomerID, 0);

            var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

            var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
            var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            ViewBag.UnitList = UnitList;
            ViewBag.UnitAndSpecificationsList = UnitAndSpecificationsLists;
            ViewBag.SpecificationsList = SpecificationsList;
            vm.Units = ViewBag.UnitList;
            vm.UnitAndSpecificationss = UnitAndSpecificationsLists;
            vm.Specificationss2 = SpecificationsList;
            //vm.SpecificationsList = ViewBag.SpecificationsList;
            if (ViewType == 1)
            {
                vm.AsnandDetails.asn.ExpectDate = DateTime.Now;
            }

            this.GenQueryASNDetailViewModel(vm);
            return View(vm);
        }
        //public void Test()
        //{
        //    List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
        //    WMS_Log_Operation operation = new WMS_Log_Operation();
        //    operation.Controller = "test";
        //    operation.MenuName = "test";
        //    operation.OrderNumber = "test";
        //    operation.OrderType = "test";
        //    operation.CreateTime = DateTime.Now;
        //    logs.Add(operation);
        //    new LogOperationService().AddLogOperation(logs);
        //}
        //新增
        [HttpPost]
        public string AddASNAndASNDetails(string JsonTable, string ExternReceiptNumber, string CustomerName, int CustomerID, string ASNType, string Remark, int WarehouseID, string Warehousename, DateTime ExpectDate, string JsonField)
        {
            var countAsn = new ASNManagementService().ExternKeyCheck(ExternReceiptNumber, "1", long.Parse(CustomerID.ToString()));
            if (countAsn > 0)
            {
                return "外部单号已存在";
            }
            IEnumerable<Column> columns;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }

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
            IEnumerable<ASNDetail> ASNDetails = this.InitASNDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);



            AddASNandASNDetailRequest request = new AddASNandASNDetailRequest();
            IList<ASN> asn = new List<ASN>();
            IList<ASNDetail> asnDetails = new List<ASNDetail>();
            var responseJsonFieldsets = jsonlist<ASN>(JsonField);
            //List<ASN> responseJsonFieldsets = new List<ASN>();
            //responseJsonFieldsets.Add(responseJsonFieldset);
            //if (responseJsonFieldsets.Count == 0)
            //{
            //    responseJsonFieldsets.Add(
            //        new ASN()
            //        {
            //            ASNType = ASNType,
            //            ExpectDate = ExpectDate,
            //            CustomerID = CustomerID.ObjectToInt64(),
            //            CustomerName = CustomerName,
            //            WarehouseID = WarehouseID,
            //            WarehouseName = Warehousename,
            //            ExternReceiptNumber = ExternReceiptNumber,
            //            CreateTime = DateTime.Now,
            //            Creator = base.UserInfo.Name,
            //            Status = 1,
            //            Remark = Remark == "" ? null : Remark,

            //        }
            //        );
            //}
            //else
            //{
            bool validation = true;
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                bool IsInt1 = false;
                #region
                IEnumerable<WMS_Config_Type> ctype = null;
                ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", 0, 0, 0);
                ctype = ctype.Where(c => c.Code == "AKZO" && c.Name == CustomerID.ToString());
                #endregion
                responseJsonFieldsets.Each((i, asns) =>
                {
                    asns.ASNType = ASNType;
                    asns.ExpectDate = ExpectDate;
                    asns.CustomerID = CustomerID.ObjectToInt64();
                    asns.CustomerName = CustomerName;
                    asns.WarehouseID = WarehouseID;
                    asns.WarehouseName = Warehousename;
                    asns.ExternReceiptNumber = ExternReceiptNumber;
                    asns.CreateTime = DateTime.Now;
                    asns.Creator = base.UserInfo.Name;
                    asns.Status = 1;
                    asns.Remark = Remark == "" ? null : Remark;

                    //if ((CustomerID == 75 || CustomerID == 74 || CustomerID == 10089) && ASNType != "调整入库")
                    if (ctype != null && ctype.Count() > 0 && ASNType != "调整入库")
                    {
                        IsInt1 = true;
                        asns.Int1 = 1;
                        asns.Int2 = 1;
                    }
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "ASN管理";
                    operation.Operation = "ASN-新增";
                    operation.OrderType = "ASN";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ExternOrderNumber = asns.ExternReceiptNumber;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.CustomerID = CustomerID;
                    operation.CustomerName = CustomerName;
                    operation.WarehouseID = WarehouseID;
                    operation.WarehouseName = Warehousename;
                    logs.Add(operation);
                });
                //}

                ASNDetails.Each((i, asnDetail) =>
                {
                    //if (ApplicationConfigHelper.GetALLProductStorerList().Where(c => (c.SKU == asnDetail.SKU && (c.UPC == asnDetail.UPC || string.IsNullOrEmpty(asnDetail.UPC)) && c.StorerID == CustomerID.ObjectToInt64())).Select(m => m.GoodsName).FirstOrDefault() == null)
                    //{
                    //    validation = false;
                    //}
                    asnDetail.ExternReceiptNumber = ExternReceiptNumber;
                    asnDetail.SKU = asnDetail.SKU.Trim();
                    asnDetail.UPC = asnDetail.UPC == null ? "" : asnDetail.UPC;
                    asnDetail.LineNumber = asnDetail.LineNumber.Trim();
                    asnDetail.QtyExpected = asnDetail.QtyExpected;
                    asnDetail.GoodsName = asnDetail.GoodsName;
                    asnDetail.GoodsType = asnDetail.GoodsType;
                    asnDetail.CreateTime = DateTime.Now;
                    asnDetail.Creator = base.UserInfo.Name;
                    asnDetail.CustomerID = CustomerID.ObjectToInt64();
                    asnDetail.CustomerName = CustomerName;
                    if (ctype != null && ctype.Count() > 0 && IsInt1)
                    {
                        asnDetail.Int1 = 1;
                        asnDetail.Int2 = 1;
                    }
                });
            }
            catch (Exception)
            {
                return "操作失败";
            }
            if (!validation)
            {
                return "条码可能不存在";
            }
            request.asn = responseJsonFieldsets;
            request.asnDetails = ASNDetails;
            var response = new ASNManagementService().AddasnAndasnDetail(request);
            #region 记录操作日志
            new LogOperationService().AddLogOperation(logs);
            #endregion
            return response.Result;
        }
        //编辑
        [HttpPost]
        public string UpdateASNAndASNDetails(string JsonTable, int ID, string ExternReceiptNumber, string CustomerName, int CustomerID, string ASNType, string Remark, int WarehouseID, string Warehousename, DateTime ExpectDate, DateTime Createtime, string Creator, string ASNNumber, string JsonField)
        {
            IEnumerable<Column> columns;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
            }
            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID,CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
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
            IEnumerable<ASNDetail> ASNDetails = this.InitASNDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);


            AddASNandASNDetailRequest request = new AddASNandASNDetailRequest();
            IList<ASN> asn = new List<ASN>();
            IList<ASNDetail> asnDetails = new List<ASNDetail>();
            var responseJsonFieldsets = jsonlist<ASN>(JsonField);

            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            foreach (var responseJsonFieldset in responseJsonFieldsets)
            {
                asn.Add(new ASN
                {
                    ID = ID,
                    ASNNumber = ASNNumber,
                    ASNType = ASNType,
                    CustomerID = CustomerID,
                    CustomerName = CustomerName,
                    ExternReceiptNumber = ExternReceiptNumber,
                    WarehouseID = WarehouseID,
                    WarehouseName = Warehousename,
                    UpdateTime = DateTime.Now,
                    Updator = base.UserInfo.Name,
                    Status = 1,
                    Remark = Remark == "" ? null : Remark,
                    ExpectDate = ExpectDate,
                    CreateTime = Createtime,
                    Creator = Creator,
                    str1 = responseJsonFieldset.str1,
                    str2 = responseJsonFieldset.str2,
                    str3 = responseJsonFieldset.str3,
                    str4 = responseJsonFieldset.str4,
                    str5 = responseJsonFieldset.str5,
                    str6 = responseJsonFieldset.str6,
                    str7 = responseJsonFieldset.str7,
                    str8 = responseJsonFieldset.str8,
                    str9 = responseJsonFieldset.str9,
                    str10 = responseJsonFieldset.str10,
                    str11 = responseJsonFieldset.str11,
                    str12 = responseJsonFieldset.str12,
                    str13 = responseJsonFieldset.str13,
                    str14 = responseJsonFieldset.str14,
                    str15 = responseJsonFieldset.str15,
                    str16 = responseJsonFieldset.str16,
                    str17 = responseJsonFieldset.str17,
                    str18 = responseJsonFieldset.str18,
                    str19 = responseJsonFieldset.str19,
                    DateTime1 = responseJsonFieldset.DateTime1,
                    DateTime2 = responseJsonFieldset.DateTime2,
                    DateTime3 = responseJsonFieldset.DateTime3,
                    DateTime4 = responseJsonFieldset.DateTime4,
                    DateTime5 = responseJsonFieldset.DateTime5,
                    Int1 = responseJsonFieldset.Int1,
                    Int2 = responseJsonFieldset.Int2,
                    Int3 = responseJsonFieldset.Int3,
                    Int4 = responseJsonFieldset.Int4,
                    Int5 = responseJsonFieldset.Int5,
                });
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "ASN管理";
                operation.Operation = "ASN-修改";
                operation.OrderType = "ASN";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ExternOrderNumber = ExternReceiptNumber;
                operation.OrderNumber = ASNNumber;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.CustomerID = CustomerID;
                operation.CustomerName = CustomerName;
                operation.WarehouseID = WarehouseID;
                operation.WarehouseName = Warehousename;

                logs.Add(operation);
            }
            foreach (var Details in ASNDetails)
            {
                asnDetails.Add(new ASNDetail()
                {
                    ASNID = ID,
                    ASNNumber = ASNNumber,
                    ExternReceiptNumber = ExternReceiptNumber,
                    CustomerID = CustomerID,
                    CustomerName = CustomerName,
                    LineNumber = Details.LineNumber,
                    SKU = Details.SKU,
                    UPC = Details.UPC,
                    BatchNumber = Details.BatchNumber,
                    BoxNumber = Details.BoxNumber,
                    Unit = Details.Unit,
                    Specifications = Details.Specifications,
                    QtyExpected = Details.QtyExpected,
                    QtyReceived = 0,
                    GoodsType = Details.GoodsType,
                    GoodsName = Details.GoodsName,
                    Creator = Creator,
                    CreateTime = Createtime,
                    Updator = base.UserInfo.Name,
                    UpdateTime = DateTime.Now,
                    str1 = Details.str1,
                    str2 = Details.str2,
                    str3 = Details.str3,
                    str4 = Details.str4,
                    str5 = Details.str5,
                    str6 = Details.str6,
                    str7 = Details.str7,
                    str8 = Details.str8,
                    str9 = Details.str9,
                    str10 = Details.str10,
                    str11 = Details.str11,
                    str12 = Details.str12,
                    str13 = Details.str13,
                    str14 = Details.str14,
                    str15 = Details.str15,
                    str16 = Details.str16,
                    str17 = Details.str17,
                    str18 = Details.str18,
                    str19 = Details.str19,
                    str20 = Details.str20,
                    DateTime1 = Details.DateTime1,
                    DateTime2 = Details.DateTime2,
                    DateTime3 = Details.DateTime3,
                    DateTime4 = Details.DateTime4,
                    DateTime5 = Details.DateTime5,
                    Int1 = Details.Int1,
                    Int2 = Details.Int2,
                    Int3 = Details.Int3,
                    Int4 = Details.Int4,
                    Int5 = Details.Int5

                });
            }
            request.asn = asn;
            request.asnDetails = asnDetails;
            if (new ASNManagementService().UpdateasnAndasnDetail(request).IsSuccess)
            {
                #region 记录操作日志
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "修改成功!", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "修改失败!", IsSuccess = true }).ToString();
            }
        }
        //批量转入库单
        public JsonResult InsertIntoReceiptAndReceiptDetails(string ASNIDs)
        {
            if (new ASNManagementService().InsertIntoReceiptAndReceiptDetails(ASNIDs).IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                foreach (string item in ASNIDs.Split(','))
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "ASN管理";
                    operation.Operation = "ASN-批量转入库单";
                    operation.OrderType = "ASN";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = item.Replace("'", "").Replace("'", "");
                    logs.Add(operation);
                }
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "转入库单成功!", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "转入库单失败!", IsSuccess = false });
            }
        }
        public static T jsonlistWeb<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public static List<T> jsonlist<T>(string str)
        {

            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(str);
            return objs;
            //return JsonConvert.DeserializeObject<List<T>>(str);
        }
        //批量导入
        [HttpPost]
        public string ImportASN(string customerid)
        {
            //ApplicationConfigHelper.RefreshASNInfo();
            IndexViewModel vm = new IndexViewModel();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds == null)
                    {
                        return new { result = "Excel格式有误！", IsSuccess = false }.ToJsonString();
                    }
                    //DataTable dtpo = ds.Tables["预出库单主信息$"];
                    // DataTable    dtpodetail = ds.Tables["预出库单明细信息$"]
                    string ErrorMessage = "";
                    TransData(customerid, 0, ref ds, ref ErrorMessage);
                    if (ErrorMessage != "")
                    {
                        return new { result = ErrorMessage, IsSuccess = false }.ToJsonString();
                    }
                    if (ds == null)
                    {
                        return new { result = "Excel格式有误！", IsSuccess = false }.ToJsonString();
                    }
                    DataTable dtasn = ds.Tables["预入库单主信息"];
                    DataTable dtasndetail = ds.Tables["预入库单明细信息"];
                    IEnumerable<Column> columnasn;
                    IEnumerable<Column> columnasnDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, customerid.ObjectToInt64()).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASN").Count() == 0)
                    {
                        columnasn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    else
                    {
                        columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ASNDetail").Count() == 0)
                    {
                        columnasnDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }
                    else
                    {
                        columnasnDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, customerid.ObjectToInt64()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Column> columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_ASN").ColumnCollection;
                    //IEnumerable<Column> columnasnDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ASNDetail").ColumnCollection;

                    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<ASN> asns = this.InitASNFromDataTable(dtasn, columnasn, useCustomerOrderNumber, sb);

                    var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, customerid.ObjectToInt64(), 0);

                    var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                    var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
                    var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                    for (int i = 0; i < dtasndetail.Rows.Count; i++)
                    {
                        if (!Regex.IsMatch(dtasndetail.Rows[i]["预收数量"].ToString(), @"^[-]?\d+[.]?\d*$"))
                        {
                            return new { result = dtasndetail.Rows[i]["预收数量"].ToString() + " 不是数字！", IsSuccess = false }.ToJsonString();
                        }
                        if (!Regex.IsMatch(dtasndetail.Rows[i]["预收数量"].ToString(), @"^[1-9]{1}[\d]*$"))
                        {
                            return new { result = dtasndetail.Rows[i]["预收数量"].ToString() + " 数量必须大于0！", IsSuccess = false }.ToJsonString();
                        }
                        if (UnitList.Where(c => c.Value == dtasndetail.Rows[i]["单位"].ToString()).Count() == 0)
                        {
                            return new { result = "单位" + dtasndetail.Rows[i]["单位"].ToString() + " 不存在,请配置！", IsSuccess = false }.ToJsonString();
                        }
                        if (SpecificationsList.Where(c => c.Value == dtasndetail.Rows[i]["规格"].ToString()).Count() == 0)
                        {
                            return new { result = "规格" + dtasndetail.Rows[i]["规格"].ToString() + " 不存在,请配置！", IsSuccess = false }.ToJsonString();
                        }
                        if (UnitAndSpecificationsLists.Where(c => c.Value == dtasndetail.Rows[i]["单位"].ToString() && c.Text == dtasndetail.Rows[i]["规格"].ToString()).Count() == 0)
                        {
                            return new { result = "单位" + dtasndetail.Rows[i]["单位"].ToString() + " 与规格" + dtasndetail.Rows[i]["规格"].ToString() + " 不匹配,请修改！", IsSuccess = false }.ToJsonString();
                        }
                    }
                    IEnumerable<ASNDetail> asnDetailss = this.InitASNDetailFromDataTable(dtasndetail, columnasnDetail, useCustomerOrderNumber, sb);

                    List<string> ExterReceiptNumberlist = new List<string>();
                    string asnstring = "";
                    string asndstring = "";
                    foreach (var asn in asns.Select(c => c.ExternReceiptNumber))
                    {
                        var countAsn = new ASNManagementService().ExternKeyCheck(asn, "1", long.Parse(customerid));
                        if (countAsn > 0)
                        {
                            return new { result = "外部单号" + asn + "已存在！", IsSuccess = false }.ToJsonString();
                        }
                        if (asnDetailss.Where(c => c.ExternReceiptNumber == asn).Count() == 0)
                        {
                            asnstring += asn + ",";
                        }
                    }
                    foreach (var asnd in asnDetailss.Select(c => c.ExternReceiptNumber).Distinct())
                    {
                        if (asns.Where(c => c.ExternReceiptNumber == asnd).Count() == 0)
                        {
                            asndstring += asnd + ",";
                        }
                    }

                    if (asnstring != "" && asndstring != "")
                    {
                        return new { result = "EXCEL中主表下列外部单号：" + asnstring.Substring(0, asnstring.Length - 1) + "在子表中不存在！</br> EXCEL中子表下列外部单号：" + asndstring.Substring(0, asndstring.Length - 1) + "在主表中不存在！", IsSuccess = false }.ToJsonString();
                    }
                    if (asnstring != "" && asndstring == "")
                    {
                        return new { result = "EXCEL中主表下列外部单号：" + asnstring.Substring(0, asnstring.Length - 1) + "在子表中不存在！", IsSuccess = false }.ToJsonString();
                    }
                    if (asnstring == "" && asndstring != "")
                    {
                        return new { result = "EXCEL中子表下列外部单号：" + asndstring.Substring(0, asndstring.Length - 1) + "在主表中不存在！", IsSuccess = false }.ToJsonString();
                    }
                    int Rows_count = 1;
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
                    foreach (var asn in asns)
                    {
                        if (ExterReceiptNumberlist.Contains(asn.ExternReceiptNumber))
                        {
                            return new { result = "EXCEL中存在相同的外部单号【" + asn.ExternReceiptNumber + "】", IsSuccess = false }.ToJsonString();
                        }
                        ExterReceiptNumberlist.Add(asn.ExternReceiptNumber);
                        if (CountAsn(asn.ExternReceiptNumber, long.Parse(customerid)) != 0)
                        {
                            return new { result = "Excel中第【" + Rows_count + "】行的单据【" + asn.ExternReceiptNumber + "】不能编辑！", IsSuccess = false }.ToJsonString();
                        }
                        if (ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(m => m.WarehouseName == asn.WarehouseName).Select(m => m.WarehouseID).FirstOrDefault() == 0)
                        {
                            return new { result = "第【" + Rows_count + "】行的仓库名称【" + asn.WarehouseName + "】在系统中不存在！", IsSuccess = false }.ToJsonString();
                        }
                        //if (ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.Name == asn.CustomerName).Select(m => m.ID).FirstOrDefault()==0)
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>第" + Rows_count + "行的客户名称" + asn.CustomerName + "在系统不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        //}

                        if (wms.Where(m => m.Name == asn.ASNTypeName).Count() == 0)
                        {
                            return new { result = "第【" + Rows_count + "】行的预入库类型【" + asn.ASNTypeName + "】在系统中不存在！", IsSuccess = false }.ToJsonString();
                        }
                        Rows_count++;
                    }
                    List<ProductSearch> ListPs = new List<ProductSearch>();

                    foreach (var asndetails in asnDetailss)
                    {
                        ProductSearch ps = new ProductSearch();
                        ps.SKU = asndetails.SKU;
                        ps.UPC = asndetails.UPC;
                        ListPs.Add(ps);
                    }
                    IEnumerable<ProductSearch> resualtProList = ApplicationConfigHelper.GetSearchProduct(customerid.ToString().ObjectToInt64(), ListPs, "UPC");
                    int Rows_counts = 1;
                    foreach (var asndetails in asnDetailss)
                    {

                        //if (ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.Name == asndetails.CustomerName).Select(m => m.ID).FirstOrDefault() == 0)
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>第" + Rows_counts + "行的客户名称" +asndetails.CustomerName + "在系统中不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        //}
                        if (resualtProList.Where(c => (c.SKU == asndetails.SKU && (c.UPC == asndetails.UPC || string.IsNullOrEmpty(asndetails.UPC)))).Select(m => m.GoodsName).FirstOrDefault() == null)
                        {
                            return new { result = "第【" + Rows_counts + "】行的产品编码或者条码【" + asndetails.SKU + "】【" + asndetails.UPC + "】在系统中不存在！", IsSuccess = false }.ToJsonString();
                        }
                        //if (ApplicationConfigHelper.GetALLProductStorerList().Where(c => (c.SKU == asndetails.SKU &&  && c.StorerID == customerid.ObjectToInt64())).Select(m => m.GoodsName).FirstOrDefault() == null)
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>第" + Rows_counts + "行的SKU" + asndetails.SKU + "在系统中不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        //}
                        Rows_counts++;
                    }
                    bool IsInt1 = false;
                    List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();

                    #region
                    IEnumerable<WMS_Config_Type> ctype = null;
                    ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", 0, 0, 0);
                    ctype = ctype.Where(c => c.Code == "AKZO" && c.Name == customerid);
                    #endregion

                    asns.Each((i, asn) =>
                    {
                        asn.Creator = base.UserInfo.Name;
                        asn.CreateTime = DateTime.Now;
                        asn.WarehouseID = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(m => m.WarehouseName == asn.WarehouseName).Select(m => m.WarehouseID).FirstOrDefault();
                        asn.CustomerID = customerid.ObjectToInt64();
                        asn.CustomerName = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.ID == customerid.ObjectToInt64()).Select(m => m.Name).FirstOrDefault();
                        //ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.Name == asn.CustomerName).Select(m => m.ID).FirstOrDefault();
                        asn.ASNType = asn.ASNTypeName;
                        //ApplicationConfigHelper.GetWMS_Config("ASNType").Where(m => m.Name == asn.ASNTypeName).Select(m => m.Code).FirstOrDefault();
                        //if ((customerid == "75" || customerid == "74" || customerid == "10089") && asn.ASNType != "调整入库")
                        if (ctype != null && ctype.Count() > 0 && asn.ASNType != "调整入库")
                        {
                            IsInt1 = true;
                            asn.Int1 = 1;
                            asn.Int2 = 1;
                        }
                        WMS_Log_Operation operation = new WMS_Log_Operation();
                        operation.MenuName = "ASN管理";
                        operation.Operation = "ASN-导入";
                        operation.OrderType = "ASN";
                        operation.Controller = Request.RawUrl;
                        operation.Creator = base.UserInfo.Name;
                        operation.CreateTime = DateTime.Now;
                        operation.ExternOrderNumber = asn.ExternReceiptNumber;
                        operation.ProjectID = (int)base.UserInfo.ProjectID;
                        operation.ProjectName = base.UserInfo.ProjectName;
                        operation.CustomerID = (int)asn.CustomerID;
                        operation.CustomerName = asn.CustomerName;
                        operation.WarehouseID = (int)asn.WarehouseID;
                        operation.WarehouseName = asn.WarehouseName;
                        logs.Add(operation);
                    });

                    //汇总
                    var asnDetailssInfo = (from q in asnDetailss.AsParallel()
                                           group q by new { q.ExternReceiptNumber, q.SKU, q.UPC, q.Unit, q.Specifications, q.BatchNumber, q.BoxNumber, q.str2 } into r
                                           select new ASNDetail
                                           {
                                               ExternReceiptNumber = r.Max(a => a.ExternReceiptNumber),
                                               SKU = r.Max(a => a.SKU),
                                               UPC = r.Max(a => a.UPC),
                                               Unit = r.Max(a => a.Unit),
                                               Specifications = r.Max(a => a.Specifications),
                                               BatchNumber = r.Max(a => a.BatchNumber),
                                               BoxNumber = r.Max(a => a.BoxNumber),
                                               QtyExpected = r.Sum(a => a.QtyExpected),
                                               str2 = r.Max(a => a.str2),
                                           }).ToList();
                    //asnDetailss = asnDetailssInfo.ToList();

                    asnDetailssInfo.Each((i, asnDetail) =>
                    {
                        asnDetail.Creator = base.UserInfo.Name;
                        asnDetail.CreateTime = DateTime.Now;
                        asnDetail.CustomerID = customerid.ObjectToInt64();
                        asnDetail.CustomerName = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.ID == customerid.ObjectToInt64()).Select(m => m.Name).FirstOrDefault();
                        //ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.Name == asnDetail.CustomerName).Select(m => m.ID).FirstOrDefault();
                        asnDetail.GoodsName = resualtProList.Where(c => c.SKU == asnDetail.SKU).Select(m => m.GoodsName).FirstOrDefault();
                        //if ((customerid == "75" || customerid == "74" || customerid == "10089") && IsInt1)
                        if (ctype != null && ctype.Count() > 0 && IsInt1)
                        {
                            asnDetail.Int1 = 1;
                            asnDetail.Int2 = 1;
                        }
                    });
                    AddASNandASNDetailRequest request = new AddASNandASNDetailRequest();
                    request.asn = asns;
                    request.asnDetails = asnDetailssInfo;
                    var response = new ASNManagementService().AddasnAndasnDetail(request);
                    if (response.IsSuccess)
                    {
                        new LogOperationService().AddLogOperation(logs);
                        return new { result = "", IsSuccess = true }.ToJsonString();
                    }
                    else
                    {
                        return new { result = response.Result, IsSuccess = false }.ToJsonString();
                    }
                }
            }
            return new { result = "请选择文件！", IsSuccess = false }.ToJsonString();
        }
        public void TransData(string CustomerID, long WareHouseID, ref DataSet transData, ref string message)
        {
            Object[] parameters = new Object[5];
            parameters[0] = "Asn";
            parameters[1] = Convert.ToInt64(CustomerID);
            parameters[2] = base.UserInfo.ProjectID;
            parameters[3] = ApplicationConfigHelper.GetCacheInfo().First(p => p.UserName == base.UserInfo.Name).WarehouseID;//Bob
            parameters[4] = transData;

            string transDataInstanceName = transDataInstanceNameStr(Convert.ToInt64(CustomerID), WareHouseID);
            ITransData transDataInstance = Activator.CreateInstance(Type.GetType(transDataInstanceName), parameters) as ITransData;
            transData = transDataInstance.TransData(ref message);
        }
        private string transDataInstanceNameStr(long? CustomerID, long WareHouseID)
        {
            if (ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.Where(p => p.Id == base.UserInfo.ProjectID.ToString()).Count() > 0)
            {
                var tranDataConfigCollection = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
                           .First(p => p.Id == base.UserInfo.ProjectID.ToString())
                           .TransDataConfigs.TransDataConfigCollection;

                string transDataInstanceName = "Runbow.TWS.Web.TransDataInstances.DefaultTransData";

                if (tranDataConfigCollection != null && tranDataConfigCollection.Any())
                {
                    var customerTransDataConfig = tranDataConfigCollection.FirstOrDefault(t => t.CustomerID == CustomerID);

                    if (customerTransDataConfig != null)
                    {
                        transDataInstanceName = string.IsNullOrEmpty(customerTransDataConfig.DefaultTransDataInstance) ? transDataInstanceName : customerTransDataConfig.DefaultTransDataInstance;

                        if (customerTransDataConfig.WarehouseConfigCollection != null && customerTransDataConfig.WarehouseConfigCollection.Any())
                        {
                            var finalTransDataConfig = customerTransDataConfig.WarehouseConfigCollection.FirstOrDefault(t => t.WarehouseID == WareHouseID);

                            if (finalTransDataConfig != null)
                            {
                                transDataInstanceName = string.IsNullOrEmpty(finalTransDataConfig.AllocateInstance) ? transDataInstanceName : finalTransDataConfig.TransDataInstances;
                            }
                        }
                    }
                }
                return transDataInstanceName;
            }
            else
            {
                return "Runbow.TWS.Web.TransDataInstances.DefaultTransData";
            }
        }
        private DataSet GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), Runbow.TWS.Common.Constants.TEMPFOLDER);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = base.UserInfo.ID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
            string fullPath = Path.Combine(targetPath, fileName);
            hpf.SaveAs(fullPath);
            hpf.InputStream.Close();

            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(fullPath);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();

            excelHelper.Dispose();
            MyFile.Delete(fullPath);

            return ds;
        }
        private static DataTable JsonToDataTable(string strJson)
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
                //string[] strRows = strRow.Split('!');
                string[] strRows = Regex.Split(strRow, "!,", RegexOptions.IgnoreCase);
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "ASNDetail";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].ToString();
                        tb.Columns.Add(dc);
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
        private IEnumerable<ASNDetail> InitASNDetailFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<ASNDetail> AsnDetails = new List<ASNDetail>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ASNDetail asnDetail = new ASNDetail();

                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey == true))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
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
                                            typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ASNDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
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
                                        typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
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
                                            typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ASNDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ASNDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                    //}
                }
                AsnDetails.Add(asnDetail);
            }

            return AsnDetails;
        }
        private IEnumerable<ASN> InitASNFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<ASN> asns = new List<ASN>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ASN asn = new ASN();

                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey == true && c.IsImportColumn == true))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, dttemp, null);
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
                                            typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ASN).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asn, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asn, null);
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
                                        typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, dttemp, null);
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
                                            typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, "1", null);
                                        }
                                        else
                                        {
                                            typeof(ASN).GetProperty(column.DbColumnName).SetValue(asn, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(ASN).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asn, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asn, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                    //}
                }
                asns.Add(asn);
            }

            return asns;
        }
        public JsonResult CountReceipt(int ID)
        {
            var s = new ASNManagementService().CountReceipt(ID);
            var s1 = from o in s select new { o.ID, o.ReceiptNumber, o.Status };
            return Json(s1, JsonRequestBehavior.AllowGet);

        }
        public int CountAsn(string ExternNumber, long customerID)
        {
            int s = new ASNManagementService().CountAsn(ExternNumber, customerID);

            return s;

        }

        /// <summary>
        /// 收货异常跟踪
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ASNAbnormalTrackingIndex(long? customerID)
        {
            return View();
        }

        /// <summary>
        /// 获取异常页面需要的下拉条件
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetASNAbnormalWhere(long? customerID)
        {
            try
            {
                ASNAbnormalTrackingModel vm = new ASNAbnormalTrackingModel();
                vm.SearchCondition = new ASNAbnormalSearchCondition();
                vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
                var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
                var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                //ViewBag.CustomerList = CustomerList;
                vm.CustomerList = CustomerList;
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
                        if (customerIDs != null && customerIDs.Count() == 1)
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
                }
                else
                {
                    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                }
                vm.WarehouseList = WarehouseList;
                return Json(new
                {
                    data = vm,
                    code = 0
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = "",
                    code = 402
                });
            }
        }

        /// <summary>
        ///获取异常数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetASNAbnormalList(RequestModel request)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            ASNAbnormalSearchCondition searchCondition = new ASNAbnormalSearchCondition();
            try
            {
                searchCondition = JsonConvert.DeserializeObject<ASNAbnormalSearchCondition>(request.requestData);
            }
            catch (Exception ex)
            {
                res.msg = "查询条件有误";
                res.code = 402;
                return Json(new { res });
            }
            try
            {
                //查询数据
                if (searchCondition.CustomerID != null && searchCondition.CustomerID > 0)
                {
                    searchCondition.PageIndex = request.page > 0 ? request.page - 1 : 0;
                    searchCondition.PageSize = request.limit > 0 ? request.limit : 20;
                    IEnumerable<ASNAbnormalTracking> asnAbnormallist;
                    int rowcounts = 0;
                    asnAbnormallist = new ASNManagementService().GetASNAbnormalList(searchCondition, out msg, out rowcounts);
                    if (asnAbnormallist != null && asnAbnormallist.Any() && msg == "")
                    {
                        res.code = 0;
                        res.count = rowcounts;
                        res.data = asnAbnormallist;
                    }
                    else
                    {
                        res.code = 402;
                        res.count = rowcounts;
                    }
                }
                else
                {
                    msg = "获取数据失败";
                    res.code = 402;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                res.code = 402;
            }
            res.msg = msg;
            return Json(new { res });
        }

        /// <summary>
        /// 新增、修改asn异常信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddorUpdateASNAbnormal(RequestModel request)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            ASNAbnormalTracking Abnormal = new ASNAbnormalTracking();
            try
            {
                Abnormal = JsonConvert.DeserializeObject<ASNAbnormalTracking>(request.requestData);
            }
            catch (Exception ex)
            {
                res.msg = "错误:" + ex.Message.ToString();
                res.code = 402;
                return Json(new { res });
            }
            try
            {
                //提交
                Abnormal.Creator = base.UserInfo.Name;
                bool result = new ASNManagementService().AddorUpdateASNAbnormal(Abnormal, request.IsSign.ObjectToInt32(), out msg);
                //成功
                if (result && msg == "")
                {
                    res.code = 0;
                }
                else
                {
                    res.code = 401;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                res.code = 402;
            }
            res.msg = msg;
            return Json(new { res });
        }

        /// <summary>
        /// 导出asn异常信息
        /// </summary>
        /// <param name="request"></param>
        public void ExportASNAbnormal(string request)
        {
            ASNAbnormalSearchCondition searchCondition = new ASNAbnormalSearchCondition();
            try
            {
                searchCondition = JsonConvert.DeserializeObject<ASNAbnormalSearchCondition>(request);
                IEnumerable<ASNAbnormalTracking> asnAbnormallist = new ASNManagementService().ExportASNAbnormal(searchCondition);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.TableName = "收货异常跟踪信息";
                dt.Columns.Add("收货日期", typeof(string));
                dt.Columns.Add("登记人", typeof(string));
                dt.Columns.Add("PO单号", typeof(string));
                dt.Columns.Add("索赔箱号", typeof(string));
                dt.Columns.Add("箱号", typeof(string));
                dt.Columns.Add("SKU", typeof(string));
                dt.Columns.Add("款号", typeof(string));
                dt.Columns.Add("尺码", typeof(string));
                dt.Columns.Add("计划数量", typeof(string));
                dt.Columns.Add("实收数量", typeof(string));
                dt.Columns.Add("差异", typeof(string));
                dt.Columns.Add("异常类型", typeof(string));
                dt.Columns.Add("门店", typeof(string));
                dt.Columns.Add("冻结状态", typeof(string));
                dt.Columns.Add("上架库位", typeof(string));
                dt.Columns.Add("备注", typeof(string));
                dt.Columns.Add("索赔编号", typeof(string));
                dt.Columns.Add("索赔日期", typeof(string));
                dt.Columns.Add("冻结单号", typeof(string));
                dt.Columns.Add("NIKE调查结果", typeof(string));
                dt.Columns.Add("调整数数量", typeof(string));
                dt.Columns.Add("系统调整结果", typeof(string));
                dt.Columns.Add("系统调整日期", typeof(string));
                dt.Columns.Add("发送IT调整邮件日期", typeof(string));
                dt.Columns.Add("IT回复上传文件时间", typeof(string));
                dt.Columns.Add("上传文件IT", typeof(string));

                foreach (var item in asnAbnormallist)
                {
                    DataRow row = dt.NewRow();
                    row["收货日期"] = item.ReceiptTime;
                    row["登记人"] = item.Registrant;
                    row["PO单号"] = item.ExternReceiptNumber;
                    row["索赔箱号"] = item.BoxNo;
                    row["箱号"] = item.BoxNumber;
                    row["SKU"] = item.SKU;
                    row["款号"] = item.Article;
                    row["尺码"] = item.Size;
                    row["计划数量"] = item.QtyExpected;
                    row["实收数量"] = item.QtyReceived;
                    row["差异"] = item.QtyDiff;
                    row["异常类型"] = item.ReasonCode;
                    row["门店"] = item.StorerKey;
                    row["冻结状态"] = item.FreeStatus;
                    row["上架库位"] = item.Location;
                    row["备注"] = item.Remark;
                    row["索赔编号"] = item.ClaimNumber;
                    row["索赔日期"] = item.DateTime1;
                    row["冻结单号"] = item.FreeNumber;
                    row["NIKE调查结果"] = item.SurveyResult;
                    row["调整数数量"] = item.QtyAdj;
                    row["系统调整结果"] = item.QtyAdjResult;
                    row["系统调整日期"] = item.AdjTime;
                    row["发送IT调整邮件日期"] = item.SendITTime;
                    row["IT回复上传文件时间"] = item.ITReplyTime;
                    row["上传文件IT"] = "";
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "收货异常跟踪信息" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "收货异常跟踪信息" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 删除异常记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult DeleteASNAbnormal(long ID)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            try
            {
                bool result = new ASNManagementService().DeleteASNAbnormal(ID, out msg);
                if (result && msg == "")
                {
                    res.code = 0;
                }
                else
                {
                    res.code = 401;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                res.code = 402;
            }
            res.msg = msg;
            return Json(new { res });
        }

        /// <summary>
        /// NIKE退货仓-新增箱标贴
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ASNNewBoxLabel(int? PageIndex, long? customerID)
        {
            ASNNewBoxLabelViewModel vm = new ASNNewBoxLabelViewModel();
            vm.SearchCondition = new ASNNewBoxLabelSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.CustomerList = CustomerList;
            ViewBag.ProjectName = base.UserInfo.ProjectName;

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
                    if (customerIDs != null && customerIDs.Count() == 1)
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
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                if (WarehouseList != null && WarehouseList.Count() == 1)
                {
                    vm.SearchCondition.WarehouseID = long.Parse(WarehouseList.First().Value.ToString());
                }
                else
                {
                    vm.SearchCondition.CustomerID = 0;
                }
            }

            ViewBag.WarehouseList = WarehouseList;
            vm.WarehouseList = WarehouseList;

            var getASNNewBoxLabelByConditionRequest = new GetASNNewBoxLabelByConditionRequest();
            getASNNewBoxLabelByConditionRequest.SearchCondition = vm.SearchCondition;
            getASNNewBoxLabelByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getASNNewBoxLabelByConditionRequest.PageIndex = PageIndex ?? 0;

            var getASNNewBoxLabelByConditionResponse = new ASNManagementService().GetASNNewBoxLabelByConditionResponse(getASNNewBoxLabelByConditionRequest);

            if (getASNNewBoxLabelByConditionResponse.IsSuccess)
            {
                vm.ASNNewBoxLabelList = getASNNewBoxLabelByConditionResponse.Result.asnBoxCollection;
                vm.SearchCondition.PageIndex = getASNNewBoxLabelByConditionResponse.Result.PageIndex;
                vm.SearchCondition.PageCount = getASNNewBoxLabelByConditionResponse.Result.PageCount;
            }

            return View(vm);
        }

        /// <summary>
        /// NIKE退货仓-新增箱标贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNNewBoxLabel(ASNNewBoxLabelViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.CustomerList = CustomerList;
            ViewBag.ProjectName = base.UserInfo.ProjectName;

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
                    vm.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.CustomerIDs = "0";
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
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.WarehouseName = "0";
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            vm.WarehouseList = WarehouseList;

            var getASNNewBoxLabelByConditionRequest = new GetASNNewBoxLabelByConditionRequest();
            if (Action == "查询" || Action == "ASNNewBoxLabel")
            {
                getASNNewBoxLabelByConditionRequest.SearchCondition = vm.SearchCondition;
                getASNNewBoxLabelByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getASNNewBoxLabelByConditionRequest.PageIndex = PageIndex ?? 0;
            }

            var getASNNewBoxLabelByConditionResponse = new ASNManagementService().GetASNNewBoxLabelByConditionResponse(getASNNewBoxLabelByConditionRequest);

            if (getASNNewBoxLabelByConditionResponse.IsSuccess)
            {
                vm.ASNNewBoxLabelList = getASNNewBoxLabelByConditionResponse.Result.asnBoxCollection;
                vm.SearchCondition.PageIndex = getASNNewBoxLabelByConditionResponse.Result.PageIndex;
                vm.SearchCondition.PageCount = getASNNewBoxLabelByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        /// <summary>
        /// ASN打库位标签
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanSKU"></param>
        /// <returns></returns>
        //public string GetLocationLabelBySKU(string AsnNumber,string ScanSKU,string GoodsType)
        //{
        //    string msg = "";
        //    try
        //    {
        //        msg = new ASNManagementService().GetLocationLabelBySKU(AsnNumber, ScanSKU, GoodsType);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "-1";
        //    }
        //    return msg;
        //}
        public JsonResult GetLocationLabelBySKU(string AsnNumber, string ScanBoxNumber, string ScanSKU)
        {
            List<ASNDetailLocation> lists = new List<ASNDetailLocation>();
            List<ASNDetailLocation> listresult = new List<ASNDetailLocation>();
            try
            {
                if (RedisOperation.Exists("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber))
                {
                    lists = RedisOperation.GetList<List<ASNDetailLocation>>("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber);
                    foreach (var item in lists)
                    {
                        if (item.SKU == ScanSKU && item.Qty > item.QtyReceived)
                        {
                            item.QtyReceived = item.QtyReceived + 1;
                            listresult.Add(new ASNDetailLocation()
                            {
                                ExternReceiptNumber = item.ExternReceiptNumber,
                                Location = item.Location,
                                GoodsType = item.GoodsType
                            });
                            break;
                        }
                    }
                    if (listresult.Count() > 0)
                    {
                        RedisOperation.SetList("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber, lists);
                        return Json(new { Code = "1", data = listresult });
                    }
                    else
                    {
                        return Json(new { Code = "0" });
                    }
                }
                return Json(new { Code = "0" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }
        public JsonResult GetLocationLabelBySKUAndLocation(string AsnNumber, string ScanBoxNumber, string ScanSKU, string Location)
        {
            List<ASNDetailLocation> lists = new List<ASNDetailLocation>();
            try
            {
                if (RedisOperation.Exists("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber))
                {
                    lists = RedisOperation.GetList<List<ASNDetailLocation>>("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber).Where(c => c.SKU == ScanSKU && c.Location == Location).ToList();
                    if (lists.Count() > 0)
                    {
                        return Json(new { Code = "1", data = lists });
                    }
                    else
                    {
                        return Json(new { Code = "0" });
                    }
                }
                return Json(new { Code = "0" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }


        public JsonResult GetLocationLabelBySKUAll(string AsnNumber, string ScanBoxNumber)
        {
            List<ASNDetailLocation> lists = new List<ASNDetailLocation>();
            List<ASNDetailLocation> listresult = new List<ASNDetailLocation>();
            try
            {
                if (RedisOperation.Exists("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber))
                {
                    lists = RedisOperation.GetList<List<ASNDetailLocation>>("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber);
                    if (lists.Count() > 0)
                    {
                        return Json(new { Code = "1", data = lists });
                    }
                    else
                    {
                        return Json(new { Code = "0" });
                    }
                }
                return Json(new { Code = "0" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }


        /// <summary>
        /// 检查是否是箱号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        public string CheckLocationLabelByBoxNumber(string AsnNumber, string ScanBoxNumber)
        {
            string msg = "";
            if (RedisOperation.Exists("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber))
            {
                msg = "1";
            }
            return msg;
        }
        /// <summary>
        /// 检查是否是SKU
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        public string CheckLocationLabelBySKU(string AsnNumber, string ScanBoxNumber, string SKU)
        {
            string msg = "";
            if (RedisOperation.Exists("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber))
            {
                if (RedisOperation.GetList<List<ASNDetailLocation>>("LocationPrint:" + AsnNumber + ":" + ScanBoxNumber).Where(c => c.SKU == SKU).Count() > 0)
                {
                    msg = "1";
                }
            }
            return msg;
        }
        /// <summary>
        /// 选asn单号把数据加载到redis中
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public string GetLocationLabelByASNNumber(string AsnNumber)
        {
            List<ASNDetailLocation> lists = new List<ASNDetailLocation>();
            string msg = "";
            try
            {
                lists = new ASNManagementService().GetLocationLabelByASNNumber(AsnNumber).ToList();
                if (lists.Count() > 0)
                {
                    if (!RedisOperation.Exists("LocationPrint:" + AsnNumber))
                    {


                        List<ASNDetailLocation> asnDetails = new List<ASNDetailLocation>();
                        var response = new ASNManagementService().GetLocationLabelByASNNumber(AsnNumber);
                        asnDetails = response.ToList();
                        RedisOperation.SetList("LocationPrint:" + AsnNumber, asnDetails);
                        foreach (var item in asnDetails.GroupBy(c => c.BoxNumber).Select(a => new ReceiptDetail() { BoxNumber = a.Key }))
                        {
                            RedisOperation.SetList("LocationPrint:" + AsnNumber + ":" + item.BoxNumber, asnDetails.Where(a => a.BoxNumber == item.BoxNumber));
                        }

                    }
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }
        /// <summary>
        /// NIKE退货仓-打印箱号
        /// </summary>
        /// <returns></returns>
        public JsonResult newbox(string customerid, string ExternReceiptNumber, int total, string warehouseid, string GoodsType)
        {
            total = new ASNManagementService().Insertnewbox(customerid, ExternReceiptNumber, total, warehouseid, GoodsType);
            return Json(new { Result = total });
        }
        /// <summary>
        /// NIKE退货仓-打印箱号
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult PrintASNNewBoxLabel(string ids, int type)
        {
            ASNNewBoxLabelViewModel model = new ASNNewBoxLabelViewModel();
            model.ASNNewBoxLabelList = new ASNManagementService().GetPrintASNNewBoxLabel(ids, type);
            ViewBag.CustomerID = model.ASNNewBoxLabelList.Select(c => c.CustomerID).FirstOrDefault().ToString();
            ViewBag.WarehouseID = model.ASNNewBoxLabelList.Select(c => c.WarehouseID).FirstOrDefault().ToString();
            return View(model);
        }

        public ActionResult PrintBoxLabel(string ids, int type)
        {
            ASNNewBoxLabelViewModel model = new ASNNewBoxLabelViewModel();
            model.ASNNewBoxLabelList = new ASNManagementService().GetPrintASNNewBoxLabel(ids, type);
            ViewBag.CustomerID = model.ASNNewBoxLabelList.Select(c => c.CustomerID).FirstOrDefault().ToString();
            ViewBag.WarehouseID = model.ASNNewBoxLabelList.Select(c => c.WarehouseID).FirstOrDefault().ToString();
            return View(model);
        }

        /// <summary>
        /// NIKE退货仓-更新打印次数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UDnewboxPrintedTimes(string customerid, string warehouseid, string boxids)
        {
            int UDboxids = new ASNManagementService().UDnewboxPrintedTimes(customerid, warehouseid, boxids);
            return Json(new { Result = UDboxids });
        }

        //生成上架库位
        [HttpPost]
        public string CreateShelfLocation(string ID)
        {
            if (new ASNManagementService().CreateShelfLocation(ID).IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "ASN管理";
                operation.Operation = "ASN-生成上架库位";
                operation.OrderType = "ASN";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return Json(new { Message = "生成成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "生成失败！", IsSuccess = false }).ToString();
            }
        }


    }
}

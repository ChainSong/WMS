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
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Runbow.TWS.Web.Areas.WMS.Models.MachiningManagement;
using Runbow.TWS.MessageContracts.WMS.Machining;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using SysIO = System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class MachiningManagementController : BaseController
    {
      
        [HttpGet]
        public ActionResult WarhouseMachiningIndex(int? PageIndex, long? customerID, bool? ShowSubmit)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ShowSubmit = ShowSubmit;
            IndexViewModel vm = new IndexViewModel();
            vm.searchCondition = new MachiningSearchCondition();
            vm.searchCondition.MachiningType = "库内加工单";
            GetMachiningByConditionRequest getMachiningByConditionRequest = new GetMachiningByConditionRequest();
            getMachiningByConditionRequest.SearchCondition = vm.searchCondition;
            getMachiningByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getMachiningByConditionRequest.PageIndex = PageIndex ?? 0; 
            var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByCondition(getMachiningByConditionRequest);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult WarhouseMachiningIndex(IndexViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.searchCondition.MachiningType = "库内加工单";
            GetMachiningByConditionRequest getMachiningByConditionRequest = new GetMachiningByConditionRequest();
            getMachiningByConditionRequest.SearchCondition = vm.searchCondition;
            getMachiningByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getMachiningByConditionRequest.PageIndex = PageIndex ?? 0;
            var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByCondition(getMachiningByConditionRequest);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult BucketMachiningIndex(int? PageIndex, long? customerID, bool? ShowSubmit)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ShowSubmit = ShowSubmit;
            IndexViewModel vm = new IndexViewModel();
            vm.searchCondition = new MachiningSearchCondition();
            vm.searchCondition.MachiningType = "槽车加工单";
            GetMachiningByConditionRequest getMachiningByConditionRequest = new GetMachiningByConditionRequest();
            getMachiningByConditionRequest.SearchCondition = vm.searchCondition;
            getMachiningByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getMachiningByConditionRequest.PageIndex = PageIndex ?? 0;
            var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByCondition(getMachiningByConditionRequest);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            } 
            return View(vm);
        }
        [HttpPost]
        public ActionResult BucketMachiningIndex(IndexViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;  
            vm.searchCondition.MachiningType = "槽车加工单";
            GetMachiningByConditionRequest getMachiningByConditionRequest = new GetMachiningByConditionRequest();
            getMachiningByConditionRequest.SearchCondition = vm.searchCondition;
            getMachiningByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getMachiningByConditionRequest.PageIndex = PageIndex ?? 0;
            var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByCondition(getMachiningByConditionRequest);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult WarhouseMachiningAddView(int? PageIndex, string ShowSubmit)
        {
            
            IndexViewModel vm = new IndexViewModel();
            vm.ShowSubmit = ShowSubmit;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            GetMachiningByConditionRequest requests = new GetMachiningByConditionRequest();
            vm.searchCondition = new MachiningSearchCondition();
            requests.SearchCondition = vm.searchCondition;
            requests.PageSize = UtilConstants.PAGESIZE;
            requests.PageIndex = PageIndex ?? 0;          
                var getMachiningByConditionResponse = new MachiningManagementService().GetInventoryBySearchCondition(requests);
                if (getMachiningByConditionResponse.IsSuccess)
                {
                    vm.InventoryCollection = getMachiningByConditionResponse.Result.InventoryCollection;
                    vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                    vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
                }
           
            return View(vm);
        }

        [HttpPost]
        public ActionResult WarhouseMachiningAddView(IndexViewModel vm, int? PageIndex, string Action)
        {          
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            GetMachiningByConditionRequest requests = new GetMachiningByConditionRequest();
            requests.SearchCondition = vm.searchCondition;
            requests.PageSize = UtilConstants.PAGESIZE;
            requests.PageIndex = PageIndex ?? 0;
            var getMachiningByConditionResponse = new MachiningManagementService().GetInventoryBySearchCondition(requests);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.InventoryCollection = getMachiningByConditionResponse.Result.InventoryCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }


        [HttpGet]
        public ActionResult BucketMachiningAddSave(int? PageIndex, string CustomerName, int Flag,string ShowSubmit, long CustomerID=0,long ID=0)
        { //Flag 1新增 2查看 3编辑
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;  
            IndexViewModel vm = new IndexViewModel();
            vm.ViewType = Flag;
            vm.ShowSubmit = ShowSubmit;
            GetMachiningByConditionRequest requests = new GetMachiningByConditionRequest();
            vm.searchCondition = new MachiningSearchCondition();
            vm.searchCondition.ExpectDate = DateTime.Now;
            vm.searchCondition.CustomerID = CustomerID;
            requests.SearchCondition = vm.searchCondition;
            requests.PageSize = UtilConstants.PAGESIZE;
            requests.PageIndex = PageIndex ?? 0;
            var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, CustomerID, 14);
            var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
            var SpecificationsList = UnitAndSpecificationsLists.Where(m => m.Value == "桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            var WashSpecificationsList = UnitAndSpecificationsLists.Where(m => m.Value == "冲洗桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            var MoreThanSpecificationsList = UnitAndSpecificationsLists.Where(m => m.Value == "余料桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            ViewBag.SpecificationsList = SpecificationsList;
            ViewBag.WashSpecificationsList = WashSpecificationsList;
            ViewBag.MoreThanSpecificationsList = MoreThanSpecificationsList;
            if (Flag == 2 || Flag == 3)
            {
                var getMachiningByConditionResponse1 = new MachiningManagementService().GetMachiningByID(ID);
                if (getMachiningByConditionResponse1.IsSuccess)
                {
                    vm.MachiningCollection = getMachiningByConditionResponse1.Result.MachiningCollection;
                }
            }
            var getMachiningByConditionResponse = new MachiningManagementService().GetLittleInventoryBySearchCondition(requests);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.InventoryCollection = getMachiningByConditionResponse.Result.InventoryCollection;
             
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult WarhouseMachiningAddSave(string IDS,string CustomerName, int Flag, string SKU, string GoodsName, string ShowSubmit, long CustomerID = 0, float Qty = 0, long ID = 0)
        {
            //Flag 1新增 2查看 3编辑
            IndexViewModel vm = new IndexViewModel();
            GetMachiningByConditionRequest requests = new GetMachiningByConditionRequest();
            vm.searchCondition = new MachiningSearchCondition();
            vm.searchCondition.ExpectDate = DateTime.Now;
            vm.searchCondition.CustomerID = CustomerID;
            requests.SearchCondition = vm.searchCondition;
            vm.ViewType = Flag;
            ViewBag.Qty = Qty;
            ViewBag.IDS = IDS;
            ViewBag.CustomerID = CustomerID;
            ViewBag.CustomerName = CustomerName;
            ViewBag.SKU = SKU;
            ViewBag.GoodsName = GoodsName;
            vm.ShowSubmit=ShowSubmit;
            var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID,CustomerID,14);
            var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
            var SpecificationsList = UnitAndSpecificationsLists.Where(m=>m.Value=="桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            var WashSpecificationsList = UnitAndSpecificationsLists.Where(m => m.Value == "冲洗桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            var MoreThanSpecificationsList = UnitAndSpecificationsLists.Where(m => m.Value == "余料桶").Select(t => new { Code = t.Text, Name = t.Text }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
            ViewBag.SpecificationsList = SpecificationsList;
            ViewBag.WashSpecificationsList = WashSpecificationsList;
            ViewBag.MoreThanSpecificationsList = MoreThanSpecificationsList;
            if (Flag == 1)
            {
              
                vm.searchCondition = new MachiningSearchCondition();
                vm.searchCondition.ExpectDate = DateTime.Now;
            }
            if (Flag == 2 || Flag==3)
            {
                var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByID(ID);
                if (getMachiningByConditionResponse.IsSuccess)
                {
                    vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;                    
                }
            }
            var getMachiningByConditionResponse2 = new MachiningManagementService().GetLittleInventoryBySearchCondition(requests);
            if (getMachiningByConditionResponse2.IsSuccess)
            {
                vm.InventoryCollection = getMachiningByConditionResponse2.Result.InventoryCollection; 
            }
            return View(vm);
        }

        [HttpPost]
        public JsonResult WarhouseMachiningAddSave(string MachiningNumber, string CarOrBoxNumber, DateTime ExpectDate, string Tel, long CustomerID, string CustomerName, string DetailJson, string IDS, string IDDS, string MachiningType, int Flag, int ViewType)
        {//Flag 1暂存 2库内加工提交 3槽车直接出库 4槽车直接入库
            if (ViewType == 1)
            {
                var countAsn = new MachiningManagementService().CheckMachiningNumber(MachiningNumber);
                if (countAsn > 0)
                {
                    return Json(new { ErrorCode = "1", OrderInfo = "加工单号已存在" });
                }
            }
            GetMachiningByConditionRequest requests = new GetMachiningByConditionRequest();            
            var Detail = jsonlist<WMS_MachiningHeaderAndDetail>(DetailJson);
            Detail.Each((i, s) =>
            {
                s.ExpectDate = ExpectDate;
                s.MachiningNumber = MachiningNumber;
                s.CarOrBoxNumber = CarOrBoxNumber;
                s.Tel = Tel;
                s.CreateTime = DateTime.Now;
                s.CustomerID = CustomerID;
                s.CustomerName = CustomerName;
                s.IDS = IDS;
                s.MachiningType = MachiningType;
                s.SKU = s.ProportioningSKU == "" ? s.SKU : s.ProportioningSKU;
                s.GoodsName = s.ProportioningSKU == "" ? s.GoodsName : s.ProportioningSKU;
                s.IDDS = IDDS == "" ? null : IDDS;
            });
            requests.MachiningCollection = Detail;
            var re = "";
            if (Flag == 1)
            {
                re=new MachiningManagementService().SaveMachining(requests, base.UserInfo.Name);
            }
            if (Flag == 2)
            {
                re = new MachiningManagementService().AddMachining(requests, base.UserInfo.Name, IDS == "" ? IDS : IDS.Substring(0, IDS.Length - 1), IDDS == null ? IDDS : IDDS.Substring(0, IDDS.Length - 1));
            }
            if (Flag == 3)
            {
                re = new MachiningManagementService().BucketOutMachining(requests, base.UserInfo.Name, IDS == "" ? IDS : IDS.Substring(0, IDS.Length - 1));
            }
            if (Flag == 4)
            {
                re = new MachiningManagementService().BucketInMachining(requests, base.UserInfo.Name,IDS==""?IDS:IDS.Substring(0, IDS.Length - 1));
            }
            if (re == "OK")
            {
                return Json(new { ErrorCode = "0", OrderInfo = re });
            }
            else
            {
                return Json(new { ErrorCode = "1", OrderInfo = re });
            }
        }

        public static IEnumerable<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        [HttpGet]
        public ActionResult GetMachiningByID(long ID)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IndexViewModel vm = new IndexViewModel();
            var getMachiningByConditionResponse = new MachiningManagementService().GetMachiningByID(ID);
            if (getMachiningByConditionResponse.IsSuccess)
            {
                vm.MachiningCollection = getMachiningByConditionResponse.Result.MachiningCollection;
                vm.PageIndex = getMachiningByConditionResponse.Result.PageIndex;
                vm.PageCount = getMachiningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        [HttpGet]
        public string MachiningDelete(long ID)
        {
            var messages = new MachiningManagementService().MachiningDelete(ID);
            return messages;
        }
        [HttpGet]
        public ActionResult PrintMachining(string id)
        {

            IndexViewModel vm = new IndexViewModel();
            ViewBag.Id = id;

            var response = new MachiningManagementService().GetPrintMachining(id);

            vm.MachiningCollection = response.Result.MachiningCollection;
            deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            vm.MachiningCollection.Each((a, b) =>
            {
                string strGUID = "Machining" + Guid.NewGuid().ToString();
                b.PictureStr = GetDimensionalCode(b.MachiningNumber, strGUID + ".jpg");
                b.PageIndex = "page" + (a + 1);
            });
            return View(vm);

        }
        private void deleteTmpFiles(string strPath)
        {
            if (SysIO.Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string va in SysIO.Directory.GetFiles(strPath))
                {
                    if (va.Contains("Machining"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
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
    }
}

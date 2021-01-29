using System;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Areas.System.Models;
using UtilConstants = Runbow.TWS.Common.Constants;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.MessageContracts.System;
using Runbow.TWS.Entity.WMS.Product;
using System.Web;
using System.Data;
using System.IO;
using MyFile = System.IO.File;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class WMS_CustomerController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int? customerType)
        {

            GetWMS_CustomerByConditionRequest vmNew = new GetWMS_CustomerByConditionRequest();

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vmNew.CustomerID = base.UserInfo.CustomerOrShipperID.ToString();
            }
            else if (base.UserInfo.UserType == 2)
            {

                var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                if (customerIDs != null && customerIDs.Count() == 1)
                {
                    vmNew.CustomerID = customerIDs.First().ToString();
                }

            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vmNew.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID.ToString() == vmNew.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {

            }
            ViewBag.WarehouseList = WarehouseList;
            GetWMS_CustomerByConditionRequest request = new GetWMS_CustomerByConditionRequest();
            WMS_CustomerListViewModel vm = new WMS_CustomerListViewModel();
            if (vmNew.CustomerID != null)
            {
                vm.CustomerID = long.Parse(vmNew.CustomerID);
            }




            IEnumerable<SelectListItem> cuslist = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3)).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            string CustomerIDNew = null;
            foreach (var item in cuslist)
            {

                CustomerIDNew += item.Value.ToString() + ",";


            }
            var response = new WMS_CustomerService().GetCustomerByConditon(new GetWMS_CustomerByConditionRequest() { StorerKey = string.IsNullOrEmpty(vm.StorerKey) ? "" : vm.StorerKey, CustomerID = CustomerIDNew, Contact1 = string.IsNullOrEmpty(vm.Contact1) ? "" : vm.Contact1, PhoneNum = string.IsNullOrEmpty(vm.PhoneNum1) ? "" : vm.PhoneNum1, Company = string.IsNullOrEmpty(vm.Company) ? "" : vm.Company, PageSize = UtilConstants.PAGESIZE, PageIndex = 0 });
            if (response.IsSuccess)
            {

                vm.Customer = response.Result.Customer;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.StorerID = cuslist;

            }
            else
            {
                ViewBag.Message = "查询失败！";
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(WMS_CustomerListViewModel vm, int? PageIndex)
        {

            IEnumerable<SelectListItem> cuslist = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3)).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            string CustomerIDNew = null;
            if (vm.CustomerID != null)
            {
                CustomerIDNew = vm.CustomerID.ToString();
            }
            else
            {
                foreach (var item in cuslist)
                {
                    CustomerIDNew += item.Value.ToString() + ",";
                }
            }
            var response = new WMS_CustomerService().GetCustomerByConditon(new GetWMS_CustomerByConditionRequest() { StorerKey = string.IsNullOrEmpty(vm.StorerKey) ? "" : vm.StorerKey, CustomerID = CustomerIDNew, Contact1 = string.IsNullOrEmpty(vm.Contact1) ? "" : vm.Contact1, PhoneNum = string.IsNullOrEmpty(vm.PhoneNum1) ? "" : vm.PhoneNum1, Company = string.IsNullOrEmpty(vm.Company) ? "" : vm.Company, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });

            // var response = new WMS_CustomerService().GetCustomerByConditon(new GetWMS_CustomerByConditionRequest() { StorerKey = string.IsNullOrEmpty(vm.StorerKey) ? "" : vm.StorerKey, CustomerID = string.IsNullOrEmpty(vm.CustomerID.ToString()) ? "" : vm.CustomerID.ToString(), Contact1 = string.IsNullOrEmpty(vm.Contact1) ? "" : vm.Contact1, PhoneNum = string.IsNullOrEmpty(vm.PhoneNum1) ? "" : vm.PhoneNum1, Company = string.IsNullOrEmpty(vm.Company) ? "" : vm.Company, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            if (response.IsSuccess)
            {
                vm.Customer = response.Result.Customer;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.StorerID = cuslist;
            }
            else
            {
                ViewBag.Message = "查询失败！";
            }

            return View(vm);
        }
        [HttpGet]
        public ActionResult Create(string id, WMS_CustomerModel vm, int? ViewType, long? customerid)
        {
            IEnumerable<SelectListItem> cuslist = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                                      .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            WMS_CustomerService service = new WMS_CustomerService();
            vm.StorerID = cuslist;
            vm.ViewType = ViewType != null ? (int)ViewType : 2;
            vm.StorerKey = "必填";
            vm.City = "必填";
            vm.Company = "必填";
            vm.CompanyCode = "必填";
            vm.UserDef10 = "必填";
            vm.AddressLine1 = "必填";
            vm.Contact1 = "必填";
            vm.PhoneNum1 = "必填";
            bool isShowUserDef2 = true;
            if (id != null)
            {
                WMS_CustomerService customer = new WMS_CustomerService();
                WMS_Customer cus = customer.selectCustomer(id.ToString(), customerid.ToString());
                vm.ConvertDesc(cus);
                vm.CustomerID = customerid;
                //  vm.ViewType = 0;
                isShowUserDef2 = false;
            }
            ViewBag.isWrite = isShowUserDef2;

            return View(vm);
        }
        [HttpPost]
        public ActionResult Create(WMS_CustomerModel vm)
        {
            IEnumerable<SelectListItem> cuslist = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                                       .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            WMS_CustomerService service = new WMS_CustomerService();
            vm.StorerID = cuslist;
            IList<WMS_Customer> customer = new List<WMS_Customer>();
            //var segments = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true }).Result;
            //vm.Segments = segments.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name + "------>>详情>>" + s.Description });
            //vm.Types = vm.StoreStatus > 0 ? 1 : 0;
            customer.Add(vm.Convert());
            Response<IEnumerable<WMS_Customer>> response = service.AddCustomer(new AddWMS_CustomerRequest() { customers = customer });
            {
                WMS_Customer cus = service.selectCustomer(vm.StorerKey, vm.CustomerID.ToString());
                vm.ConvertDesc(cus);

                if (response.IsSuccess)
                {
                    //刷新缓存
                    //ApplicationConfigHelper.RefreshApplicationWMS_Customers();
                    //ApplicationConfigHelper.RefreshGetApplicationWMS_Customer();
                    vm.ViewType = 0;
                    ViewBag.Message = "0";
                }
                return View(vm);
            }
        }
        /// <summary>
        /// 批量导入店铺信息
        /// </summary>
        /// <param name="StorerID"></param>
        /// <returns></returns>
        [HttpPost]
        public string ExeclStorer(int StorerID)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    IList<WMS_Customer> Info = new List<WMS_Customer>();
                    if (ds != null && ds.Tables[0] != null)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Info.Add(new WMS_Customer()
                            {
                                StorerKey = ds.Tables[0].Rows[i]["StorerKey"].ToString().Trim(),
                                Active = true,//1
                                Status = ds.Tables[0].Rows[i]["状态"].ToString().Trim(),
                                Company = ds.Tables[0].Rows[i]["店铺名称"].ToString().Trim(),
                                ReceiptPrefix = "",
                                OrderPrefix = "",
                                CompanyCode = ds.Tables[0].Rows[i]["SAP代码"].ToString().Trim(),
                                Type = ds.Tables[0].Rows[i]["类型"].ToString().Trim(),//SELLER
                                AddressLine1 = ds.Tables[0].Rows[i]["地址1"].ToString().Trim(),
                                AddressLine2 = ds.Tables[0].Rows[i]["地址2"].ToString().Trim(),
                                AddressLine3 = ds.Tables[0].Rows[i]["地址3"].ToString().Trim(),
                                AddressLine4 = ds.Tables[0].Rows[i]["地址4"].ToString().Trim(),
                                City = ds.Tables[0].Rows[i]["城市"].ToString().Trim(),
                                State = ds.Tables[0].Rows[i]["州"].ToString().Trim(),
                                PostCode = ds.Tables[0].Rows[i]["邮编"].ToString().Trim(),
                                Country = ds.Tables[0].Rows[i]["国家"].ToString().Trim(),
                                CountryCode = ds.Tables[0].Rows[i]["国家代码"].ToString().Trim(),//CN
                                Contact1 = ds.Tables[0].Rows[i]["联系人1"].ToString().Trim(),
                                Contact2 = ds.Tables[0].Rows[i]["联系人2"].ToString().Trim(),
                                PhoneNum1 = ds.Tables[0].Rows[i]["联系电话1"].ToString().Trim(),
                                PhoneNum2 = ds.Tables[0].Rows[i]["联系电话2"].ToString().Trim(),
                                FaxNum1 = ds.Tables[0].Rows[i]["传真1"].ToString().Trim(),
                                FaxNum2 = ds.Tables[0].Rows[i]["传真2"].ToString().Trim(),
                                Email1 = ds.Tables[0].Rows[i]["邮箱1"].ToString().Trim(),
                                Email2 = ds.Tables[0].Rows[i]["邮箱2"].ToString().Trim(),
                                TriggerDog = false,//0
                                CreateUser = ds.Tables[0].Rows[i]["创建人"].ToString().Trim(),
                                CreateDate = DateTime.Now,
                                UpdateUser = ds.Tables[0].Rows[i]["更新人"].ToString().Trim(),
                                UpdateDate = DateTime.Now,
                                CustomerID = StorerID
                            });
                        }
                    }
                    var groupedInfos = from g in Info
                            select new WMS_Customer()
                            {
                                StorerKey = g.StorerKey,
                                Active = g.Active,
                                Status = g.Status,
                                Company = g.Company,
                                ReceiptPrefix = g.ReceiptPrefix,
                                OrderPrefix = g.OrderPrefix,
                                CompanyCode = g.CompanyCode,
                                Type = g.Type,
                                AddressLine1 = g.AddressLine1,
                                AddressLine2 = g.AddressLine2,
                                AddressLine3 = g.AddressLine3,
                                AddressLine4 = g.AddressLine3,
                                City = g.City,
                                State = g.State,
                                PostCode = g.PostCode,
                                Country = g.Country,
                                CountryCode = g.CountryCode,
                                Contact1 = g.Contact1,
                                Contact2 = g.Contact2,
                                PhoneNum1 = g.PhoneNum1,
                                PhoneNum2 = g.PhoneNum2,
                                FaxNum1 = g.FaxNum1,
                                FaxNum2 = g.FaxNum2,
                                Email1 = g.Email1,
                                Email2 = g.Email2,
                                TriggerDog = g.TriggerDog,
                                CreateUser = g.CreateUser,
                                CreateDate = g.CreateDate,
                                UpdateUser = g.UpdateUser,
                                UpdateDate = g.UpdateDate,
                                CustomerID = g.CustomerID
                                       };
                    var repeat = groupedInfos.GroupBy(a => a.StorerKey).Where(g => g.Count() >= 2);
                    if (repeat.Count() > 0)
                    {
                        return new { result = "有重复店铺！", IsSuccess = false }.ToJsonString();
                    }
                    WMS_CustomerService service = new WMS_CustomerService();
                    Response<IEnumerable<WMS_Customer>> response = service.AddCustomer(new AddWMS_CustomerRequest() { customers = groupedInfos });
                    if (response.IsSuccess)
                    {
                        if (response.Result.Count() > 0)
                        {
                            //ApplicationConfigHelper.RefreshGetProductStorerList(StorerID.ToString());
                            return new { result = "导入成功！", IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                        }
                    }
                    return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToString();
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        public void StorerDemoExecl()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtDetail = new DataTable();
            //主表信息
            dt.Columns.Add("StorerKey", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("店铺名称", typeof(string));
            dt.Columns.Add("SAP代码", typeof(string));
            dt.Columns.Add("类型", typeof(string));
            dt.Columns.Add("地址1", typeof(string));
            dt.Columns.Add("地址2", typeof(string));
            dt.Columns.Add("地址3", typeof(string));
            dt.Columns.Add("地址4", typeof(string));
            dt.Columns.Add("城市", typeof(string));
            dt.Columns.Add("州", typeof(string));
            dt.Columns.Add("邮编", typeof(string));
            dt.Columns.Add("国家", typeof(string));
            dt.Columns.Add("国家代码", typeof(string));
            dt.Columns.Add("联系人1", typeof(string));
            dt.Columns.Add("联系人2", typeof(string));
            dt.Columns.Add("联系电话1", typeof(string));
            dt.Columns.Add("联系电话2", typeof(string));
            dt.Columns.Add("传真1", typeof(string));
            dt.Columns.Add("传真2", typeof(string));
            dt.Columns.Add("邮箱1", typeof(string));
            dt.Columns.Add("邮箱2", typeof(string));
            dt.Columns.Add("创建人", typeof(string));
            dt.Columns.Add("更新人", typeof(string));
            DataRow dr1 = dt.NewRow();
            dr1["StorerKey"] = "3602";
            dr1["店铺名称"] = "耐克成都双流换季优惠店";
            dr1["SAP代码"] = "8006902";
            dr1["类型"] = "SELLER";
            dr1["地址1"] = "成都市双流区东升街道双楠大道中段633号时代奥特莱斯S-1184,1185,S-2189,2190";
            dr1["国家代码"] = "CN";
            dr1["联系人1"] = "刘力";
            dr1["联系电话1"] = "028-61915601";
            dr1["创建人"] = "NIKENFSCD";
            dt.Rows.Add(dr1);
            ds.Tables.Add(dt);
            //明细信息
            //dtDetail.Columns.Add("SKU", typeof(string));
            //dtDetail.Columns.Add("UPC", typeof(string));
            //dtDetail.Columns.Add("UPC名称", typeof(string));
            //dtDetail.Columns.Add("UPC类型", typeof(string));
            //dtDetail.Columns.Add("UPC数量", typeof(string));
            //ds.Tables.Add(dtDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "店铺导入模板.xls");//生成Excel
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "店铺导入模板");

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
        /// <summary>
        /// 验证客户名称唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckName(string Name, int? Id, bool IsEdit)
        {
            WMS_CustomerService customer = new WMS_CustomerService();
            return customer.CheckNameIsExist(Name.Trim(), Id, base.UserInfo.ProjectID.ToString(), IsEdit);
        }

        public ActionResult Edit(string ID, string CustomerID)
        {
            WMS_CustomerService customer = new WMS_CustomerService();
            WMS_Customer cus = customer.selectCustomer(ID, CustomerID);
            CustomerModel c = new CustomerModel();
            //ID, Code, Name, Description, State, CreateDate, Email, LawPerson, PostCode, 

            c.PostCode = cus.PostCode;



            //Address1, Address2, Bank, Account, TaxID, InvoiceTitle, Contactor1, Title1, Phone1,


            return View(c);
        }
        //[HttpPost]
        //public ActionResult Edit(CustomerModel vm)
        //{
        //    vm.ResponseCustomer = new WMS_CustomerService().UpdateCustomer(new AddCustomerRequest() { Customer = vm.Convert() });
        //    if (vm.ResponseCustomer.IsSuccess)
        //    {
        //        ViewBag.Message = "编辑成功!";

        //    }
        //    ApplicationConfigHelper.RefreshProjectUserCustomers();
        //    ApplicationConfigHelper.RefreshApplicationCustomers();
        //    return RedirectToAction("List");

        //}
        [HttpPost]
        public string Delete(string StorerKey,string CustomerID)
        {
            WMS_CustomerService service = new WMS_CustomerService();
            Response<WMS_Customer> response = new Response<WMS_Customer>();
            try
            {

                response = service.DeleteWMS_Customer(StorerKey, CustomerID);
                if (response.IsSuccess)
                {
                    //ApplicationConfigHelper.RefreshProjectUserCustomers();
                    //ApplicationConfigHelper.RefreshApplicationCustomers();
                }
            }
            catch
            {

            }
            return response.SuccessMessage;
        }
    }
}
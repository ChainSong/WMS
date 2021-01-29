using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Product;
using Runbow.TWS.Web.Areas.WMS.Models.Product;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using System.Web.Script.Serialization;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using Runbow.ImportPrice;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Common.Layui;
using Runbow.TWS.Common.Util;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class ProductController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int? flag, long? customerID)
        {
            Session["searchFlag"] = flag;
            ProductModel vm = new ProductModel();
            ProductSearchCondition pc = new ProductSearchCondition();
            if (customerID == null)
            {
                vm.StorerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                    .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else
            {
                vm.StorerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                    .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName }).Where(m => m.Value == customerID.ToString());
            }
            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.StorerID)
            {
                sb.Append("" + i.Value + ",");
            }
            if (sb.Length > 1)
            {
                pc.StorerID = sb.Remove(sb.Length - 1, 1).ToString();
            }
            vm.ProductSKU = pc;
            var response = new ProductService().QuerySKUProduct(new GetProductByConditionRequest()
            {
                SearchCondition = pc,
                PageIndex = 0,
                PageSize = 8,
            });
            if (response.IsSuccess)
            {
                vm.IEnumerableSearchCondition = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.RowCount = response.Result.RowCount;
            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(ProductModel vm, int? PageIndex)
        {
            //ProductSearchCondition pc = new ProductSearchCondition(); 
            vm.StorerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (vm.ProductSKU.StorerID == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in vm.StorerID)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.ProductSKU.StorerID = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            var response = new ProductService().QuerySKUProduct(new GetProductByConditionRequest()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = 8,
                SearchCondition = vm.ProductSKU,
            });
            if (response.IsSuccess)
            {
                vm.IEnumerableSearchCondition = response.Result.SearchCondition;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
                vm.RowCount = response.Result.RowCount;
                // vm.storer = response.Result.Storer;
            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        public JsonResult GetSKUProductInfo(CommonRequest request)
        {
            ResponseData res = new ResponseData();
            ProductSearchCondition cond = new ProductSearchCondition();
            IEnumerable<ProductStorer> list = null;
            res.code = 401;
            try
            {
                if (request.requestData != null)
                    cond = JsonHelper.Deserialize<ProductSearchCondition>(request.requestData);
            }
            catch (Exception ex)
            {
                res.code = 402;
                res.msg = "JSON字符串转数组对象错误" + ex.Message;
                return Json(res);
            }
            try
            {
                var storelist = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                if (string.IsNullOrEmpty(cond.StorerID))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var i in storelist)
                    {
                        sb.Append("" + i.Value + ",");
                    }
                    if (sb.Length > 1)
                    {
                        cond.StorerID = sb.Remove(sb.Length - 1, 1).ToString();
                    }
                }
                var response = new ProductService().QuerySKUProduct(new GetProductByConditionRequest()
                {
                    PageIndex = request.page - 1,
                    PageSize = request.limit,
                    SearchCondition = cond
                });
                if (response.IsSuccess)
                {
                    list = response.Result.SearchCondition;
                    res.count = response.Result.RowCount;
                }
                res.data = list;
                res.code = list != null && list.Any() ? 0 : 0;
                res.msg = list != null && list.Any() ? "200" : "无数据";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }
            return Json(res);
        }

        // 1是添加,2 是查看 ，3是编辑 
        [HttpGet]
        public ActionResult AddProduct(string ID, int typeid, long? CustomerId)
        {
            QueryProductModel vm = new QueryProductModel();
            ViewBag.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ProductStorerInfo productSearchCondition = new ProductStorerInfo();
            productSearchCondition.Status = 1;
            if (CustomerId != null)
            {
                productSearchCondition.StorerID = Convert.ToInt64(CustomerId);
            }
            //新改
            vm.GetStorerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            string customername = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3) && (c.CustomerID == Convert.ToInt64(CustomerId)))
                .Select(c => c.Code).First();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type_" + customername);
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.Get_Type = st;

            if (typeid == 1)
            {
                vm.type = 0;
                productSearchCondition.GoodsType = 1;
                productSearchCondition.SKUClassification = "1";
                productSearchCondition.SKUGroup = "1";
                vm.productStorerInfo = productSearchCondition;
            }
            else if (typeid == 2)
            {
                var response = new ProductService().GetSKUProduct(ID);
                if (response.IsSuccess)
                {
                    vm.type = 1;
                    vm.productStorerInfo = response.Result.ProductStorerInfo;
                    vm.productDetail = response.Result.InfoDetail;
                }
            }
            else
            {
                var response = new ProductService().GetSKUProduct(ID);
                if (response.IsSuccess)
                {
                    vm.type = 3;
                    vm.productStorerInfo = response.Result.ProductStorerInfo;
                }
            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }
        // 1是添加 ，3是编辑 
        [HttpPost]
        public ActionResult AddProduct(QueryProductModel vm)
        {
            EditSKUProductRequest SearchCondition = new EditSKUProductRequest();
            SearchCondition.Info = vm.productStorerInfo;
            SearchCondition.Info.IsQcEligible = vm.IsQcEligible ? 1 : 0;

            //新增的时候Articel，Size  20170717
            SearchCondition.Info.Str9 = vm.productStorerInfo.Str9;
            SearchCondition.Info.Str10 = vm.productStorerInfo.Str10;
            //新加
            //string customername = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3) && (c.CustomerID == Convert.ToInt64(CustomerId)))
            //                .Select(c => c.Code).First();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type_" + vm.SearchCondition.CustomerName);
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.Get_Type = st;

            var response = new ProductService().EditSKUProduct(SearchCondition);
            if (response.IsSuccess)
            {
                if (response.Result.Info.SKU == null)
                {
                    ViewBag.Tishi = "添加失败";
                    vm.type = 0;
                    return View(vm);
                }
                vm.type = 1;
                vm.IsQcEligible = response.Result.Info.IsQcEligible == 1 ? true : false;
                vm.productStorerInfo = response.Result.Info;
                ApplicationConfigHelper.RefreshGetProductStorerList(SearchCondition.Info.StorerID.ToString());
            }
            else
            {
                ViewBag.Tishi = "添加失败";
            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        public JsonResult DelProduct(string ID, string CustomerID)
        {
            bool response = new ProductService().DelSKUProduct(ID);

            if (response)
            {
                ApplicationConfigHelper.RefreshGetProductStorerList(CustomerID);
                return Json(new { Message = "删除SUK成功！", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除SUK失败！", IsSuccess = false });
            }

        }
        [HttpGet]
        public ActionResult ExeclProduct(int? id)
        {
            return View();
        }
        [HttpPost]
        public string ExeclProduct(int StorerID)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    IList<ProductStorerInfo> Info = new List<ProductStorerInfo>();
                    if (ds != null && ds.Tables[0] != null)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //int StatusTable = ds.Tables[0].Rows[i]["状态"].ToString().Trim() == "可用" ? 1 : 0;
                            int TypeTable = 0;
                            string customername = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3) && (c.CustomerID == Convert.ToInt64(StorerID)))
                                .Select(c => c.Code).First();
                            IEnumerable<WMSConfig> wms = null;
                            try
                            {
                                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type_" + customername);
                            }
                            catch (Exception)
                            {
                            }
                            if (wms == null)
                            {
                                wms = ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
                            }
                            //var dasda = wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault();
                            TypeTable = Convert.ToInt32(wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault());
                            int IsQcEligibleTable = ds.Tables[0].Rows[i]["质检是否合格"].ToString().Trim() == "否" ? 0 : 1;
                            //int StorerID = Convert.ToInt32(ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.Name.Trim() == ds.Tables[0].Rows[i]["货主"].ToString().Trim())
                            //.Select(q => q.ID).ToList().FirstOrDefault());
                            //ds.Tables[0].Rows[i]["货主"].ObjectToInt64();
                            Info.Add(new ProductStorerInfo()
                            {
                                //ID = Convert.ToInt64(ds.Tables[0].Rows[i]["ID"].ToString().Trim()),
                                StorerID = StorerID,
                                SKU = ds.Tables[0].Rows[i]["SKU"].ToString().Trim(),
                                Status = 1,//StatusTable,
                                GoodsName = ds.Tables[0].Rows[i]["产品名称"].ToString().Trim(),
                                GoodsType = TypeTable,
                                SKUClassification = ds.Tables[0].Rows[i]["SKU分类"].ToString().Trim(),
                                SKUGroup = ds.Tables[0].Rows[i]["SKU分组"].ToString().Trim(),
                                ManufacturerSKU = ds.Tables[0].Rows[i]["制造商SKU"].ToString().Trim(),
                                RetailSKU = ds.Tables[0].Rows[i]["零售商SKU"].ToString().Trim(),
                                ReplaceSKU = ds.Tables[0].Rows[i]["可替换SKU"].ToString().Trim(),
                                BoxGroup = ds.Tables[0].Rows[i]["箱组"].ToString().Trim(),
                                Packing = ds.Tables[0].Rows[i]["包装"].ToString().Trim(),
                                Grade = ds.Tables[0].Rows[i]["ABC分类"].ToString().Trim(),
                                Country = ds.Tables[0].Rows[i]["国家"].ToString().Trim(),
                                Manufacturer = ds.Tables[0].Rows[i]["制造商"].ToString().Trim(),
                                DangerCode = ds.Tables[0].Rows[i]["危险代码"].ToString().Trim(),
                                Remark = ds.Tables[0].Rows[i]["描述"].ToString().Trim(),
                                Volume = ds.Tables[0].Rows[i]["容积"].ToString().Trim(),
                                StandardVolume = ds.Tables[0].Rows[i]["标准容积"].ToString().Trim(),
                                Weight = ds.Tables[0].Rows[i]["重量"].ToString().Trim(),
                                StandardWeight = ds.Tables[0].Rows[i]["标准重量"].ToString().Trim(),
                                NetWeight = ds.Tables[0].Rows[i]["净重"].ToString().Trim(),
                                StandardNetWeight = ds.Tables[0].Rows[i]["标准净重"].ToString().Trim(),
                                Price = decimal.Parse(ds.Tables[0].Rows[i]["单价"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["单价"].ToString().Trim()),
                                ActualPrice = decimal.Parse(ds.Tables[0].Rows[i]["实际单价"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["实际单价"].ToString().Trim()),
                                Cost = ds.Tables[0].Rows[i]["成本"].ToString().Trim(),
                                ActualCost = ds.Tables[0].Rows[i]["实际成本"].ToString().Trim(),
                                StandardOrderingCost = ds.Tables[0].Rows[i]["标准订货成本"].ToString().Trim(),
                                ShipmentCost = ds.Tables[0].Rows[i]["运输成本"].ToString().Trim(),
                                QcInSpectionLoc = ds.Tables[0].Rows[i]["质检地"].ToString().Trim(),
                                QCPercentage = ds.Tables[0].Rows[i]["质检合格率"].ToString().Trim(),
                                ReceiptQcUom = ds.Tables[0].Rows[i]["质检Uom"].ToString().Trim(),
                                IsQcEligible = IsQcEligibleTable,
                                PutArea = ds.Tables[0].Rows[i]["放货区域"].ToString().Trim(),
                                PutCode = ds.Tables[0].Rows[i]["放货代码"].ToString().Trim(),
                                PutRule = ds.Tables[0].Rows[i]["放置规则"].ToString().Trim(),
                                PutStrategy = ds.Tables[0].Rows[i]["策略"].ToString().Trim(),
                                AllocateRule = ds.Tables[0].Rows[i]["分配规则"].ToString().Trim(),
                                PickedCode = ds.Tables[0].Rows[i]["拣货代码"].ToString().Trim(),
                                SKUType = ds.Tables[0].Rows[i]["品名类型"].ToString().Trim(),
                                Color = ds.Tables[0].Rows[i]["货品颜色"].ToString().Trim(),
                                Size = ds.Tables[0].Rows[i]["货品尺寸"].ToString().Trim(),
                                //Remark = ds.Tables[0].Rows[i]["Remark"].ToString().Trim(),
                                //Remark = ds.Tables[0].Rows[i]["Remark"].ToString().Trim(),
                                Int1 = ds.Tables[0].Rows[i]["有效期"].ObjectToInt32(),
                                Str9 = ds.Tables[0].Rows[i]["Size"].ToString().Trim(),//新增两列Atricle，Szie 20170717
                                Str10 = ds.Tables[0].Rows[i]["Article"].ToString().Trim(),

                                Str11=ds.Tables[0].Rows[i]["长度"].ToString().Trim(),
                                Str12=ds.Tables[0].Rows[i]["宽度"].ToString().Trim(),
                                Str13=ds.Tables[0].Rows[i]["高度"].ToString().Trim(),
                                Str14=ds.Tables[0].Rows[i]["安全库存"].ToString().Trim(),
                                Str15=ds.Tables[0].Rows[i]["箱规格1"].ToString().Trim(),
                                Str16=ds.Tables[0].Rows[i]["箱规格2"].ToString().Trim(),
                                Str17=ds.Tables[0].Rows[i]["箱规格3"].ToString().Trim(),
                                Str18=ds.Tables[0].Rows[i]["箱规格4"].ToString().Trim(),
                                Str19=ds.Tables[0].Rows[i]["箱规格5"].ToString().Trim()
                            });
                        }
                    }
                    IList<ProductDetail> pd = new List<ProductDetail>();
                    if (ds != null && ds.Tables[1] != null)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            //int StatusTable = ds.Tables[0].Rows[i]["状态"].ToString().Trim() == "可用" ? 1 : 0;
                            //int TypeTable = 0;
                            //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
                            //var dasda = wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault();
                            //TypeTable = Convert.ToInt32(wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault());
                            //Convert.ToInt32 (from q in wms  where q.Name==ds.Tables[0].Rows[i]["货品种类"].ToString().Trim() select q.Code.First());
                            //switch (ds.Tables[0].Rows[i]["货品种类"].ToString().Trim())
                            //{
                            //    case "A类":
                            //        TypeTable = 1;
                            //        break;
                            //    case "B类":
                            //        TypeTable = 2;
                            //        break;
                            //    case "C类":
                            //        TypeTable = 3;
                            //        break;
                            //    default:
                            //        TypeTable = 0;
                            //        break;
                            //}
                            // TypeTable = ds.Tables[0].Rows[i]["货品种类"].ToString().Trim() != "" ? Convert.ToInt32(ds.Tables[0].Rows[i]["货品种类"]) : 0;
                            //int IsQcEligibleTable = ds.Tables[0].Rows[i]["质检是否合格"].ToString().Trim() == "否" ? 0 : 1;
                            //int StorerID = Convert.ToInt32(ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.Name.Trim() == ds.Tables[0].Rows[i]["货主"].ToString().Trim())
                            //.Select(q => q.ID).ToList().FirstOrDefault());
                            //ds.Tables[0].Rows[i]["货主"].ObjectToInt64();
                            pd.Add(new ProductDetail()
                            {
                                // ID = Convert.ToInt64(ds.Tables[0].Rows[i]["ID"].ToString().Trim()),
                                //StorerID = StorerID,
                                SKU = ds.Tables[1].Rows[i]["SKU"].ToString().Trim(),
                                UPC = ds.Tables[1].Rows[i]["UPC"].ToString().Trim(),
                                UPCName = ds.Tables[1].Rows[i]["UPC名称"].ToString().Trim(),
                                UPCType = ds.Tables[1].Rows[i]["UPC类型"].ToString().Trim(),
                                UPCNumber = string.IsNullOrEmpty(ds.Tables[1].Rows[i]["UPC数量"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[i]["UPC数量"].ToString().Trim()),
                                //Status = 1,//StatusTable,
                                //GoodsName = ds.Tables[0].Rows[i]["产品名称"].ToString().Trim(),
                                //GoodsType = TypeTable,
                                //SKUClassification = ds.Tables[0].Rows[i]["SKU分类"].ToString().Trim(),
                                //SKUGroup = ds.Tables[0].Rows[i]["SKU分组"].ToString().Trim(),
                                //ManufacturerSKU = ds.Tables[0].Rows[i]["制造商SKU"].ToString().Trim(),
                                //RetailSKU = ds.Tables[0].Rows[i]["零售商SKU"].ToString().Trim(),
                                //ReplaceSKU = ds.Tables[0].Rows[i]["可替换SKU"].ToString().Trim(),
                                //BoxGroup = ds.Tables[0].Rows[i]["箱组"].ToString().Trim(),
                                //Packing = ds.Tables[0].Rows[i]["包装"].ToString().Trim(),
                                //Grade = ds.Tables[0].Rows[i]["ABC分类"].ToString().Trim(),
                                //Country = ds.Tables[0].Rows[i]["国家"].ToString().Trim(),
                                //Manufacturer = ds.Tables[0].Rows[i]["制造商"].ToString().Trim(),
                                //DangerCode = ds.Tables[0].Rows[i]["危险代码"].ToString().Trim(),
                                //Remark = ds.Tables[0].Rows[i]["描述"].ToString().Trim(),
                                //Volume = ds.Tables[0].Rows[i]["容积"].ToString().Trim(),
                                //StandardVolume = ds.Tables[0].Rows[i]["标准容积"].ToString().Trim(),
                                //Weight = ds.Tables[0].Rows[i]["重量"].ToString().Trim(),
                                //StandardWeight = ds.Tables[0].Rows[i]["标准重量"].ToString().Trim(),
                                //NetWeight = ds.Tables[0].Rows[i]["净重"].ToString().Trim(),
                                //StandardNetWeight = ds.Tables[0].Rows[i]["标准净重"].ToString().Trim(),
                                //Price = decimal.Parse(ds.Tables[0].Rows[i]["单价"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["单价"].ToString().Trim()),
                                //ActualPrice = decimal.Parse(ds.Tables[0].Rows[i]["实际单价"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["实际单价"].ToString().Trim()),
                                //Cost = ds.Tables[0].Rows[i]["成本"].ToString().Trim(),
                                //ActualCost = ds.Tables[0].Rows[i]["实际成本"].ToString().Trim(),
                                //StandardOrderingCost = ds.Tables[0].Rows[i]["标准订货成本"].ToString().Trim(),
                                //ShipmentCost = ds.Tables[0].Rows[i]["运输成本"].ToString().Trim(),
                                //QcInSpectionLoc = ds.Tables[0].Rows[i]["质检地"].ToString().Trim(),
                                //QCPercentage = ds.Tables[0].Rows[i]["质检合格率"].ToString().Trim(),
                                //ReceiptQcUom = ds.Tables[0].Rows[i]["质检Uom"].ToString().Trim(),
                                //IsQcEligible = IsQcEligibleTable,
                                //PutArea = ds.Tables[0].Rows[i]["放货区域"].ToString().Trim(),
                                //PutCode = ds.Tables[0].Rows[i]["放货代码"].ToString().Trim(),
                                //PutRule = ds.Tables[0].Rows[i]["放置规则"].ToString().Trim(),
                                //PutStrategy = ds.Tables[0].Rows[i]["策略"].ToString().Trim(),
                                //AllocateRule = ds.Tables[0].Rows[i]["分配规则"].ToString().Trim(),
                                //PickedCode = ds.Tables[0].Rows[i]["拣货代码"].ToString().Trim(),
                                //SKUType = ds.Tables[0].Rows[i]["品名类型"].ToString().Trim(),
                                //Color = ds.Tables[0].Rows[i]["货品颜色"].ToString().Trim(),
                                //Size = ds.Tables[0].Rows[i]["货品尺寸"].ToString().Trim(),
                                ////Remark = ds.Tables[0].Rows[i]["Remark"].ToString().Trim(),
                                ////Remark = ds.Tables[0].Rows[i]["Remark"].ToString().Trim(),
                                //Int1 = ds.Tables[0].Rows[i]["有效期"].ObjectToInt32()
                            });
                        }
                    }
                    var groupedInfos = from g in Info
                                       select new ProductStorerInfo()
                                       {
                                           StorerID = g.StorerID,
                                           ID = g.ID,
                                           SKU = g.SKU,
                                           Status = g.Status,
                                           GoodsType = g.GoodsType,
                                           GoodsName = g.GoodsName,
                                           SKUClassification = g.SKUClassification,
                                           SKUGroup = g.SKUGroup,
                                           ManufacturerSKU = g.ManufacturerSKU,
                                           RetailSKU = g.RetailSKU,
                                           ReplaceSKU = g.ReplaceSKU,
                                           BoxGroup = g.BoxGroup,
                                           Packing = g.Packing,
                                           Grade = g.Grade,
                                           Country = g.Country,
                                           Manufacturer = g.Manufacturer,
                                           DangerCode = g.DangerCode,
                                           Remark = g.Remark,
                                           Volume = g.Volume,
                                           StandardVolume = g.StandardVolume,
                                           Weight = g.Weight,
                                           StandardWeight = g.StandardWeight,
                                           NetWeight = g.NetWeight,
                                           StandardNetWeight = g.StandardNetWeight,
                                           Price = g.Price,
                                           ActualPrice = g.ActualPrice,
                                           Cost = g.Cost,
                                           ActualCost = g.ActualCost,
                                           StandardOrderingCost = g.StandardOrderingCost,
                                           ShipmentCost = g.ShipmentCost,
                                           QcInSpectionLoc = g.QcInSpectionLoc,
                                           QCPercentage = g.QCPercentage,
                                           ReceiptQcUom = g.ReceiptQcUom,
                                           IsQcEligible = g.IsQcEligible,
                                           PutArea = g.PutArea,
                                           PutCode = g.PutCode,
                                           PutRule = g.PutRule,
                                           PutStrategy = g.PutStrategy,
                                           AllocateRule = g.AllocateRule,
                                           PickedCode = g.PickedCode,
                                           SKUType = g.SKUType,
                                           Color = g.Color,
                                           Size = g.Size,
                                           Int1 = g.Int1,
                                           Str9 = g.Str9,//article,size
                                           Str10 = g.Str10,

                                           Str11 = g.Str11,
                                           Str12 = g.Str12,
                                           Str13 = g.Str13,
                                           Str14 = g.Str14,
                                           Str15 = g.Str15,
                                           Str16 = g.Str16,
                                           Str17 = g.Str17,
                                           Str18 = g.Str18,
                                           Str19 = g.Str19
                                       };
                    var repeat = groupedInfos.GroupBy(a => a.SKU).Where(g => g.Count() >= 2);
                    if (repeat.Count() > 0)
                    {
                        return new { result = "有重复SKU！", IsSuccess = false }.ToJsonString();
                    }
                    var response = new ProductService().EditSKUProductExecl(new AddProductExeclRequest() { Info = groupedInfos, InfoDetail = pd });

                    if (response.IsSuccess)
                    {
                        if (response.Result.Info.Count() > 0)
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
        [HttpPost]
        public string ImportPrice(int StorerID, string pathFile)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    List<Import> listImport = new List<Import>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Import im = new Import();
                        im.item_no = ds.Tables[0].Rows[i]["item_no"].ToString();
                        im.style_code = ds.Tables[0].Rows[i]["style_code"].ToString();
                        im.color_code = ds.Tables[0].Rows[i]["color_code"].ToString();
                        im.size_name = ds.Tables[0].Rows[i]["size_name"].ToString();
                        im.cur_unit_price = ds.Tables[0].Rows[i]["cur_unit_price"].ToString();
                        listImport.Add(im);
                    }
                    SqlParameter[] param = new SqlParameter[2];
                    //拼接xml             
                    string data = XMLHelper.ListToXML<Import>("SKUList", listImport).InnerXml.Replace("encoding=\"utf-8\"", "");
                    param[0] = new SqlParameter("@Data", data);
                    param[1] = new SqlParameter("@CustomerID", StorerID);
                    object o = SqlHelper.ExecuteScalar(ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString(), CommandType.StoredProcedure, "Pro_wms_UpdateSKUPrice_New", param);
                    if (o == null)
                    {
                        SqlParameter[] paramp = new SqlParameter[1];
                        paramp[0] = new SqlParameter("@CustomerID", StorerID);
                        object p = SqlHelper.ExecuteScalar(ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString(), CommandType.StoredProcedure, "pro_wms_UpdateSafeLock_New", paramp);
                        return new { result = "", IsSuccess = true }.ToJsonString();
                    }
                    else
                    {
                        return new { result = o.ToString() + " 有重复或差异，请检查！", IsSuccess = false }.ToJsonString();
                    }
                }
                return new { result = "文件内容为空！", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件！", IsSuccess = false }.ToJsonString();
        }
        [HttpPost]
        public string importDownCoat(int StorerID, string pathFile)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    List<Import> listImport = new List<Import>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Import im = new Import();
                        im.item_no = ds.Tables[0].Rows[i]["Style Color Code"].ToString();
                        listImport.Add(im);
                    }
                    SqlParameter[] param = new SqlParameter[2];
                    //拼接xml             
                    string data = XMLHelper.ListToXML<Import>("SKUList", listImport).InnerXml.Replace("encoding=\"utf-8\"", "");
                    param[0] = new SqlParameter("@Data", data);
                    param[1] = new SqlParameter("@CustomerID", StorerID);
                    object o = SqlHelper.ExecuteScalar(ConfigurationManager.ConnectionStrings["TWS"].ConnectionString.ToString(), CommandType.StoredProcedure, "Pro_wms_UpdateDownCoat", param);
                    if (o == null)
                    {
                        return new { result = "", IsSuccess = true }.ToJsonString();
                    }
                    else
                    {
                        return new { result = o.ToString() + " 有重复或差异，请检查！", IsSuccess = false }.ToJsonString();    
                    }
                }
                return new { result = "文件内容为空！", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件！", IsSuccess = false }.ToJsonString();
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

        public void ProductdemoExecl(long? CustomerId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtDetail = new DataTable();
            dt.Columns.Add("SKU", typeof(string));
            //dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("产品名称", typeof(string));
            dt.Columns.Add("Article", typeof(string));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("货品种类", typeof(string));
            dt.Columns.Add("SKU分类", typeof(string));
            dt.Columns.Add("SKU分组", typeof(string));
            dt.Columns.Add("有效期", typeof(string));
            dt.Columns.Add("制造商SKU", typeof(string));
            dt.Columns.Add("零售商SKU", typeof(string));
            dt.Columns.Add("可替换SKU", typeof(string));
            dt.Columns.Add("箱组", typeof(string));
            dt.Columns.Add("包装", typeof(string));
            dt.Columns.Add("ABC分类", typeof(string));
            dt.Columns.Add("国家", typeof(string));
            dt.Columns.Add("制造商", typeof(string));
            dt.Columns.Add("危险代码", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("容积", typeof(string));
            dt.Columns.Add("标准容积", typeof(string));
            dt.Columns.Add("重量", typeof(string));
            dt.Columns.Add("标准重量", typeof(string));
            dt.Columns.Add("净重", typeof(string));
            dt.Columns.Add("标准净重", typeof(string));
            dt.Columns.Add("单价", typeof(string));
            dt.Columns.Add("实际单价", typeof(string));
            dt.Columns.Add("成本", typeof(string));
            dt.Columns.Add("实际成本", typeof(string));
            dt.Columns.Add("标准订货成本", typeof(string));
            dt.Columns.Add("运输成本", typeof(string));
            dt.Columns.Add("质检地", typeof(string));
            dt.Columns.Add("质检合格率", typeof(string));
            dt.Columns.Add("质检Uom", typeof(string));
            dt.Columns.Add("质检是否合格", typeof(string));
            dt.Columns.Add("放货区域", typeof(string));
            dt.Columns.Add("放货代码", typeof(string));
            dt.Columns.Add("放置规则", typeof(string));
            dt.Columns.Add("策略", typeof(string));
            dt.Columns.Add("分配规则", typeof(string));
            dt.Columns.Add("拣货代码", typeof(string));
            dt.Columns.Add("品名类型", typeof(string));
            dt.Columns.Add("货品颜色", typeof(string));
            dt.Columns.Add("货品尺寸", typeof(string));
            //dt.Columns.Add("Article", typeof(string));
            //dt.Columns.Add("Size", typeof(string));

            dt.Columns.Add("长度", typeof(string));
            dt.Columns.Add("宽度", typeof(string));
            dt.Columns.Add("高度", typeof(string));
            dt.Columns.Add("安全库存", typeof(string));
            dt.Columns.Add("箱规格1", typeof(string));
            dt.Columns.Add("箱规格2", typeof(string));
            dt.Columns.Add("箱规格3", typeof(string));
            dt.Columns.Add("箱规格4", typeof(string));
            dt.Columns.Add("箱规格5", typeof(string));

            DataRow dr1 = dt.NewRow();
            dr1["SKU"] = "1000000";
            dr1["产品名称"] = "润滑油";
            if (CustomerId == 35)
            {
                dr1["货品种类"] = "固体";
            }
            else
            {
                dr1["货品种类"] = "种类1";
            }
            dr1["SKU分类"] = "类型1";
            dr1["SKU分组"] = "组1";
            dr1["有效期"] = 1;
            dt.Rows.Add(dr1);
            ds.Tables.Add(dt);
            dtDetail.Columns.Add("SKU", typeof(string));
            dtDetail.Columns.Add("UPC", typeof(string));
            dtDetail.Columns.Add("UPC名称", typeof(string));
            dtDetail.Columns.Add("UPC类型", typeof(string));
            dtDetail.Columns.Add("UPC数量", typeof(string));
            ds.Tables.Add(dtDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "产品导入模板.xls");  //生成Excel
            EPPlusOperation.ExportDataSetByEPPlus(ds, "产品导入模板");
        }

        public void demoExecl()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("公司名称", typeof(string));
            dt.Columns.Add("业务大类", typeof(string));
            dt.Columns.Add("合同起始日期", typeof(string));
            dt.Columns.Add("合同内容", typeof(string));
            dt.Columns.Add("业务对方名称", typeof(string));
            dt.Columns.Add("是否顺延", typeof(string));
            dt.Columns.Add("实际到期日期", typeof(string));
            dt.Columns.Add("是否含印花税", typeof(string));
            this.WriteExcel(dt, "Contract_" + UserInfo.Name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");  //生成Excel
        }
        /// <summary>
        /// 根据datatable生成Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="path"></param>
        public void WriteExcel(DataTable dt, string fileName)
        {
            try
            {
                var sbHtml = new StringBuilder();
                sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                sbHtml.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
                }

                sbHtml.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbHtml.Append("<tr>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                    }
                    sbHtml.Append("</tr>");
                }

                sbHtml.Append("</table>");
                Response.Charset = "UTF-8";
                Response.HeaderEncoding = Encoding.UTF8;
                Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }
        //自动检索SKU
        [HttpPost]
        public ActionResult GetSKUlist(string sku, long? CustomerID)
        {
            var Product = ApplicationConfigHelper.GetProductStorerList(CustomerID).Where(a => a.SKU.Contains(sku) && a.StorerID == CustomerID);
            if (Product != null && Product.Any())
            {
                return Json(Product.GroupBy(a => new { a.SKU, a.GoodsName }).Distinct().Select(t => new { Value = t.Key.SKU, Text = t.Key.GoodsName, GoodsType = string.Empty }), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Value = string.Empty, Text = string.Empty, GoodsType = string.Empty }, JsonRequestBehavior.AllowGet);
            //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");

            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new
            //{ Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b=>b.Code==t.GoodsType.ToString()).Select(b=>b.Name).First() }), JsonRequestBehavior.AllowGet);
            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b => b.Code == t.GoodsType.ToString()).Select(b => b.Name).First() }), JsonRequestBehavior.AllowGet);
        }
        //检索SKU
        [HttpPost]
        public ActionResult GetSKUlists(string sku, long CustomerID)
        {
            var Product = ApplicationConfigHelper.GetSKUListBySKU(CustomerID, sku);
            if (Product != null && Product.Any())
            {
                return Json(Product.GroupBy(a => new { a.SKU, a.GoodsName }).Distinct().Select(t => new { Value = t.Key.SKU, Text = t.Key.GoodsName, GoodsType = string.Empty }), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Value = string.Empty, Text = string.Empty, GoodsType = string.Empty }, JsonRequestBehavior.AllowGet);
            //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");

            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new
            //{ Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b=>b.Code==t.GoodsType.ToString()).Select(b=>b.Name).First() }), JsonRequestBehavior.AllowGet);
            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b => b.Code == t.GoodsType.ToString()).Select(b => b.Name).First() }), JsonRequestBehavior.AllowGet);
        }
        //自动检索UPC
        [HttpPost]
        public ActionResult GetUPClist(string UPC, long? CustomerID, string SKU)
        {
            var Product = ApplicationConfigHelper.GetProductStorerList(CustomerID).Where(a => a.SKU.Contains(SKU) && a.UPC.Contains(UPC) && a.StorerID == CustomerID);
            if (Product != null && Product.Any())
            {
                return Json(Product.Select(t => new { Value = t.GoodsName, Text = UPC, SKUText = t.SKU, GoodsType = string.Empty }), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Value = string.Empty, Text = string.Empty, SKUText = string.Empty, GoodsType = string.Empty }, JsonRequestBehavior.AllowGet);
            //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");

            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new
            //{ Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b=>b.Code==t.GoodsType.ToString()).Select(b=>b.Name).First() }), JsonRequestBehavior.AllowGet);
            //return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.GoodsName, Text = t.SKU, GoodsType = wms.Where(b => b.Code == t.GoodsType.ToString()).Select(b => b.Name).First() }), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ProductDetail(string ID)
        {
            QueryProductModel vm = new QueryProductModel();
            //ViewBag.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
            //                          .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            var response = new ProductService().GetSKUProduct(ID);
            if (response.IsSuccess)
            {
                vm.GetStorerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                                  .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                vm.type = 1;
                vm.productStorerInfo = response.Result.ProductStorerInfo;
            }


            return View(vm);

        }
        [HttpPost]
        public JsonResult ProductDetail(string Jaonstr, string ID)
        {
            //bool IsSuccess = false;
            try
            {
                var InfoDetail = JSONStringToList<ProductDetail>(Jaonstr);
                var response = new ProductService().AddProductDetail(new AddProductExeclRequest()
                {
                    InfoDetail = InfoDetail,
                    UserName = base.UserInfo.Name,
                });
                if (response)
                {
                    return Json(new { Code = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
        }

        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        public JsonResult DelProductDetail(string ID)
        {
            try
            {

                var response = new ProductService().DelProductDetail(ID);
                if (response)
                {
                    return Json(new { Code = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}

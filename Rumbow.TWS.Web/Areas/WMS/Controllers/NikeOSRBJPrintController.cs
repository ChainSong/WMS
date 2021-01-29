using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WebApi.Express;
using Runbow.TWS.Web.Areas.WMS.Models.OrderManagement;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using SW = System.Web;
using UtilConstants = Runbow.TWS.Common.Constants;
using Newtonsoft.Json;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class NikeOSRBJPrintController : BaseController
    {
        // GET: /WMS/PrintBoxm/
        public ActionResult Index(string id, string type)
        {
            if (type == "1")
            {
                GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;

                //PrintBoxmModel vm = new PrintBoxmModel();

                //PodSearchCondition SearchCondition = new PodSearchCondition();
                //SearchCondition.SystemNumber = SystemNumber;
                //vm.SearchCondition = SearchCondition;

                //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
                //vm.PodCollection = results.PodCollections;
                //vm.PageIndex = results.PageIndex;
                //vm.PageCount = results.PageCount;
                //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
                //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

                //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
                //return View(vm);
                //return jsonStr;
                return View(PackageModel);
            }
            else if (type == "0")
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 箱唛打印(OSR+NFS)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxm(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel PackageModel = new PrintBoxModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton");
            }

            //GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            //PrintBoxmModel vm = new PrintBoxmModel();

            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;

            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(PackageModel);
        }

        /// <summary>
        /// 箱唛打印(CD NFS)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxCD(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            PrintBoxModel PackageModel = new PrintBoxModel();

            //GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);
            //PrintBoxmModel vm = new PrintBoxmModel();

            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;

            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            return View(PackageModel2);
        }

        /// <summary>
        /// 打印快递面单
        /// </summary>
        /// <param name="id">箱号</param>
        /// <param name="type">A:批量；B:单条</param>
        /// <param name="OrderID">订单ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintExpressBillJite(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd, string express)
        {
            ApiRequest ar = new ApiRequest()
            {
                PackageNumber = id,
                OrderType = type,
                ExpressCompany = express
            };
            string arSer = JsonConvert.SerializeObject(ar);
            string res = "";//申请快递成功与否
            string url = UtilConstants.ExpressApiAddress;//申请快递url
            
            try
            {
                //接口会验证，有快递就返回，没有就去申请快递，先后台获取申请快递信息
                res = this.HTTPPost(url, arSer);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                return Json(new { Code = 999, Result = res });
            }

            ApiResponse apiResponse = new ApiResponse();
            apiResponse = JsonConvert.DeserializeObject<ApiResponse>(res);
            //202和1000德邦，200圆通韵达
            if (apiResponse.resultCode.Equals("202") || apiResponse.resultCode.Equals("1000"))
            {
                //调一次前面方法
                PrintBoxModel Package = new PrintBoxModel();
                Package.EnumerableExpressInfo = new OrderManagementService().GetPackageBoxCartonJite(id.ToString(), type).EnumerableExpressListInfo.OrderBy(t => t.PackageNumber);

                return Json(new { Code = 1, Result = Package.EnumerableExpressInfo });
            }
            else
            {
                return Json(new { Code = 0, Result = res });
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult Print(string id, string type, string OrderID, string packageNumber, string packageNumBegin, string packageNumEnd, IEnumerable<PrintExpressJite> printBoxInfos)
        {
            PrintBoxModel PackageModel = new PrintBoxModel();

            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);
            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            return View(PackageModel2);
        }

        /// <summary>
        /// 箱唛打印(SH NFS)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxSH(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd, string Flag = "0")
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            ViewBag.PrintFlag = Flag;
            PrintBoxModel PackageModel = new PrintBoxModel();
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);
            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            return View(PackageModel2);
        }

        /// <summary>
        /// 箱唛打印(SH NFS) CO和RTW类型的订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxSHCORTW(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd, string Flag = "0")
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            ViewBag.PrintFlag = Flag;
            PrintBoxModel PackageModel = new PrintBoxModel();
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);
            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            return View(PackageModel2);
        }

        ///// <summary>
        ///// 箱唛打印(WH NFS)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="type"></param>
        ///// <param name="OrderID"></param>
        ///// <returns></returns>
        //public ActionResult PrintBoxWH(string id, string type, string OrderID)
        //{
        //    ViewBag.OrderID = OrderID;
        //    ViewBag.Type = type;
        //    PrintBoxModel PackageModel = new PrintBoxModel();
        //    //GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
        //    PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo;
        //    //PrintBoxmModel vm = new PrintBoxmModel();
        //    //PodSearchCondition SearchCondition = new PodSearchCondition();
        //    //SearchCondition.SystemNumber = SystemNumber;
        //    //vm.SearchCondition = SearchCondition;
        //    //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
        //    //vm.PodCollection = results.PodCollections;
        //    //vm.PageIndex = results.PageIndex;
        //    //vm.PageCount = results.PageCount;
        //    //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
        //    //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //    ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
        //    //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        //    //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
        //    //return View(vm);
        //    //return jsonStr;
        //    return View(PackageModel);
        //}

        /// <summary>
        /// 箱唛打印(WH NFS)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxWH(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            PrintBoxModel PackageModel = new PrintBoxModel();

            //GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);

            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            //PrintBoxmModel vm = new PrintBoxmModel();
            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;
            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            return View(PackageModel2);
        }

        /// <summary>
        /// 箱唛打印(XA NFS)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxXA(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            PrintBoxModel PackageModel = new PrintBoxModel();

            //GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);

            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            //PrintBoxmModel vm = new PrintBoxmModel();
            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;
            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            return View(PackageModel2);
        }

        public ActionResult PrintBox(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList");
            }
            if (type == "1")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;

                //PrintBoxmModel vm = new PrintBoxmModel();

                //PodSearchCondition SearchCondition = new PodSearchCondition();
                //SearchCondition.SystemNumber = SystemNumber;
                //vm.SearchCondition = SearchCondition;

                //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
                //vm.PodCollection = results.PodCollections;
                //vm.PageIndex = results.PageIndex;
                //vm.PageCount = results.PageCount;
                //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
                //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

                //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
                //return View(vm);
                //return jsonStr;
            }
            else if (type == "0")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            return View(print);
        }

        /// <summary>
        /// 托运单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintPod(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            //20170214  托运单不需要暗箱打印，会出现总数是一箱的数量，只需要打印一张即可
            //if (type == "1")
            //{
            PrintPodModel print = new PrintPodModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintPodCondition(id.ToString(), type).EnumerableBoxListinfo;
            //PrintBoxmModel vm = new PrintBoxmModel();

            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;

            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            return View(print);
            //}
            //else if (type == "0")
            //{
            //    PrintPodModel print = new PrintPodModel();
            //    print.EnumerableCustomerInfo = new OrderManagementService().GetPrintPodCondition(long.Parse(id.ToString()), type).EnumerableBoxListinfo;

            //    return View(print);
            //}
            //else
            //{
            //    return View();
            //}
        }

        public ActionResult PrintPick(string id, string type)
        {
            if (type == "1")
            {
                GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;

                //PrintBoxmModel vm = new PrintBoxmModel();

                //PodSearchCondition SearchCondition = new PodSearchCondition();
                //SearchCondition.SystemNumber = SystemNumber;
                //vm.SearchCondition = SearchCondition;

                //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
                //vm.PodCollection = results.PodCollections;
                //vm.PageIndex = results.PageIndex;
                //vm.PageCount = results.PageCount;
                //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
                //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

                //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
                //return View(vm);
                //return jsonStr;
                return View(PackageModel);
            }
            else if (type == "0")
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        //导出箱明细
        public ActionResult ExportBoxDetails(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();
            DataSet ds = new OrderManagementService().ExportBoxDetails(id.ToString());
            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");

            sb.Append("<tr><td>" + dt.Rows[0]["OrderNumber"].ToString() + "</td>");
            sb.Append("<td>" + dt.Rows[0]["ShipmentNo"].ToString() + "</td>");
            sb.Append("<td>" + dt.Rows[0]["Company"].ToString() + "</td>");
            sb.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
            sb.Append("<tr><td>PackageKey</td><td>BIN</td><td>Material</td><td>Size</td><td>Product</td><td>Quantity</td><td>Gender</td><td>Category</td><td>Description</td></tr>");
            string PackageKey = string.Empty;
            int PackageSumQty = 0;
            int TotalQty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                TotalQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
                if (i == 0)
                {
                    PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                    sb.Append("<tr><td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Size"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Gender"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Category"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["MaterialDesc"].ToString() + "</td></tr>");
                }
                else
                {
                    if (PackageKey != dt.Rows[i]["PackageNumber"].ToString())
                    {
                        sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                        PackageSumQty = 0;
                        PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                    }
                    sb.Append("<tr><td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Size"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Gender"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Category"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["MaterialDesc"].ToString() + "</td></tr>");
                }
                PackageSumQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
            }
            sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            sb.Append("<tr><td>总箱数：</td><td>" + dt.Rows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            sb.Append("</table>");
            SW.HttpResponse Response;
            Response = SW.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;

            Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("BoxListDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
        //批量导出箱明细
        public ActionResult ExportBoxDetailsPL(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();
            DataSet ds = new OrderManagementService().ExportBoxDetails(id.ToString());
            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            string OrderNumber = string.Empty;
            sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j == 0)
                {
                    OrderNumber = dt.Rows[j]["OrderNumber"].ToString();
                    sb.Append("<tr><td>" + dt.Rows[0]["OrderNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[0]["ShipmentNo"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[0]["Company"].ToString() + "</td>");
                    sb.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr><td>PackageKey</td><td>BIN</td><td>Material</td><td>Size</td><td>Product</td><td>Quantity</td><td>Gender</td><td>Category</td><td>Description</td></tr>");
                    string PackageKey = string.Empty;
                    int PackageSumQty = 0;
                    int TotalQty = 0;
                    string expression;
                    expression = "OrderNumber ='" + dt.Rows[j]["OrderNumber"].ToString() + "'";
                    //使用选择方法来找到匹配的所有行。
                    DataRow[] foundRows;
                    foundRows = dt.Select(expression);
                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        TotalQty += Convert.ToInt32(foundRows[i]["Qty"]);
                        if (i == 0)
                        {
                            PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                            sb.Append("<tr><td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                            sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Size"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Gender"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Category"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["MaterialDesc"].ToString() + "</td></tr>");
                        }
                        else
                        {
                            if (PackageKey != foundRows[i]["PackageNumber"].ToString())
                            {
                                sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                                PackageSumQty = 0;
                                PackageKey = foundRows[i]["PackageNumber"].ToString();
                            }
                            sb.Append("<tr><td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                            sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Size"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Gender"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Category"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["MaterialDesc"].ToString() + "</td></tr>");
                        }
                        PackageSumQty += Convert.ToInt32(foundRows[i]["Qty"]);
                    }
                    sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr><td>总箱数：</td><td>" + foundRows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                else
                {
                    if (OrderNumber != dt.Rows[j]["OrderNumber"].ToString())
                    {
                        sb.Append("<tr><td>" + dt.Rows[j]["OrderNumber"].ToString() + "</td>");
                        sb.Append("<td>" + dt.Rows[j]["ShipmentNo"].ToString() + "</td>");
                        sb.Append("<td>" + dt.Rows[j]["Company"].ToString() + "</td>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
                        sb.Append("<tr><td>PackageKey</td><td>BIN</td><td>Material</td><td>Size</td><td>Product</td><td>Quantity</td><td>Gender</td><td>Category</td><td>Description</td></tr>");
                        string PackageKey = string.Empty;
                        int PackageSumQty = 0;
                        int TotalQty = 0;
                        string expression;
                        expression = "OrderNumber ='" + dt.Rows[j]["OrderNumber"].ToString() + "'";
                        //使用选择方法来找到匹配的所有行。
                        DataRow[] foundRows;
                        foundRows = dt.Select(expression);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            TotalQty += Convert.ToInt32(foundRows[i]["Qty"]);
                            if (i == 0)
                            {
                                PackageKey = foundRows[i]["PackageNumber"].ToString();
                                sb.Append("<tr><td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                                sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Size"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Gender"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Category"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["MaterialDesc"].ToString() + "</td></tr>");
                            }
                            else
                            {
                                if (PackageKey != foundRows[i]["PackageNumber"].ToString())
                                {
                                    sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                                    PackageSumQty = 0;
                                    PackageKey = foundRows[i]["PackageNumber"].ToString();
                                }
                                sb.Append("<tr><td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                                sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Size"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Gender"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Category"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["MaterialDesc"].ToString() + "</td></tr>");
                            }
                            PackageSumQty += Convert.ToInt32(foundRows[i]["Qty"]);
                        }
                        sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        sb.Append("<tr><td>总箱数：</td><td>" + foundRows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        OrderNumber = dt.Rows[j]["OrderNumber"].ToString();
                    }

                }
            }
            //sb.Append("<tr><td>" + dt.Rows[0]["OrderNumber"].ToString() + "</td><td></td>");
            //sb.Append("<td>" + dt.Rows[0]["ShipmentNo"].ToString() + "</td>");
            //sb.Append("<td>" + dt.Rows[0]["Company"].ToString() + "</td>");
            //sb.Append("<td></td><td></td><td></td><td></td></tr>");
            //sb.Append("<tr><td>PackageKey</td><td>Material</td><td>Size</td><td>Product</td><td>Quantity</td><td>Gender</td><td>Category</td><td>Description</td></tr>");
            //string PackageKey = string.Empty;
            //int PackageSumQty = 0;
            //int TotalQty = 0;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    TotalQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
            //    if (i == 0)
            //    {
            //        PackageKey = dt.Rows[i]["PackageNumber"].ToString();
            //        sb.Append("<tr><td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Size"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Gender"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Category"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["MaterialDesc"].ToString() + "</td></tr>");
            //    }
            //    else
            //    {
            //        if (PackageKey != dt.Rows[i]["PackageNumber"].ToString())
            //        {
            //            sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            //            PackageSumQty = 0;
            //            PackageKey = dt.Rows[i]["PackageNumber"].ToString();
            //        }
            //        sb.Append("<tr><td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Size"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Gender"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["Category"].ToString() + "</td>");
            //        sb.Append("<td>" + dt.Rows[i]["MaterialDesc"].ToString() + "</td></tr>");
            //    }
            //    PackageSumQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
            //}
            //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            //sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            //sb.Append("<tr><td>总箱数：</td><td>" + dt.Rows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            sb.Append("</table>");
            SW.HttpResponse Response;
            Response = SW.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;

            Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("BoxListDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        /// <summary>
        /// 退货仓批量导出箱清单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult ExportBoxDetailsPL_TH(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();
            DataSet ds = new OrderManagementService().ExportBoxDetails_TH(id.ToString());
            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            string OrderNumber = string.Empty;
            sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j == 0)
                {
                    OrderNumber = dt.Rows[j]["OrderNumber"].ToString();
                    //sb.Append("<tr><td>" + dt.Rows[0]["OrderNumber"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[0]["ShipmentNo"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[0]["Company"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[0]["PLNO"].ToString() + "</td>");
                    //sb.Append("<td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr><td>单号</td><td>OR单号</td><td>箱序号</td><td>箱号</td><td>SKU</td><td>BU</td><td>Qty</td></tr>");
                    string PackageKey = string.Empty;
                    int PackageSumQty = 0;
                    int TotalQty = 0;
                    string expression;
                    expression = "OrderNumber ='" + dt.Rows[j]["OrderNumber"].ToString() + "'";
                    //使用选择方法来找到匹配的所有行。
                    DataRow[] foundRows;
                    foundRows = dt.Select(expression);
                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        TotalQty += Convert.ToInt32(foundRows[i]["Qty"]);
                        if (i == 0)
                        {
                            PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                            sb.Append("<tr><td>" + foundRows[i]["PLNO"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["OrderNumber"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["str19"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + foundRows[i]["Size"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                        }
                        else
                        {
                            if (PackageKey != foundRows[i]["PackageNumber"].ToString())
                            {
                                //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                                PackageSumQty = 0;
                                PackageKey = foundRows[i]["PackageNumber"].ToString();
                            }
                            sb.Append("<tr><td>" + foundRows[i]["PLNO"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["OrderNumber"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["str19"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + foundRows[i]["Size"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                            sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                        }
                        PackageSumQty += Convert.ToInt32(foundRows[i]["Qty"]);
                    }
                    //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    //sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    //sb.Append("<tr><td>总箱数：</td><td>" + foundRows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                else
                {
                    if (OrderNumber != dt.Rows[j]["OrderNumber"].ToString())
                    {
                        //sb.Append("<tr><td>" + dt.Rows[j]["OrderNumber"].ToString() + "</td>");
                        //sb.Append("<td>" + dt.Rows[j]["ShipmentNo"].ToString() + "</td>");
                        //sb.Append("<td>" + dt.Rows[j]["Company"].ToString() + "</td>");
                        //sb.Append("<td>" + dt.Rows[0]["PLNO"].ToString() + "</td>");
                        //sb.Append("<td></td><td></td><td></td><td></td></tr>");
                        //sb.Append("<tr><td>单号</td><td>OR单号</td><td>箱序号</td><td>箱号</td><td>SKU</td><td>BU</td><td>Qty</td></tr>");
                        string PackageKey = string.Empty;
                        int PackageSumQty = 0;
                        int TotalQty = 0;
                        string expression;
                        expression = "OrderNumber ='" + dt.Rows[j]["OrderNumber"].ToString() + "'";
                        //使用选择方法来找到匹配的所有行。
                        DataRow[] foundRows;
                        foundRows = dt.Select(expression);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            TotalQty += Convert.ToInt32(foundRows[i]["Qty"]);
                            if (i == 0)
                            {
                                PackageKey = foundRows[i]["PackageNumber"].ToString();
                                sb.Append("<tr><td>" + foundRows[i]["PLNO"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["OrderNumber"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["str19"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + foundRows[i]["Size"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                            }
                            else
                            {
                                if (PackageKey != foundRows[i]["PackageNumber"].ToString())
                                {
                                    //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                                    PackageSumQty = 0;
                                    PackageKey = foundRows[i]["PackageNumber"].ToString();
                                }
                                sb.Append("<tr><td>" + foundRows[i]["PLNO"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["OrderNumber"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["str19"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["PackageNumber"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Atrcle"].ToString() + foundRows[i]["Size"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["BU"].ToString() + "</td>");
                                sb.Append("<td>" + foundRows[i]["Qty"].ToString() + "</td>");
                            }
                            PackageSumQty += Convert.ToInt32(foundRows[i]["Qty"]);
                        }
                        //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        //sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        //sb.Append("<tr><td>总箱数：</td><td>" + foundRows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        OrderNumber = dt.Rows[j]["OrderNumber"].ToString();
                    }

                }
            }

            sb.Append("</table>");
            SW.HttpResponse Response;
            Response = SW.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;

            Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("BoxListDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        /// <summary>
        /// 退货仓导出箱明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult ExportBoxDetails_TH(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();
            DataSet ds = new OrderManagementService().ExportBoxDetails_TH(id.ToString());
            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");

            //sb.Append("<tr><td>" + dt.Rows[0]["OrderNumber"].ToString() + "</td>");
            //sb.Append("<td>" + dt.Rows[0]["ShipmentNo"].ToString() + "</td>");
            //sb.Append("<td>" + dt.Rows[0]["Company"].ToString() + "</td>");
            //sb.Append("<td>" + dt.Rows[0]["PLNO"].ToString() + "</td>");
            //sb.Append("<td></td><td></td><td></td><td></td></tr>");
            sb.Append("<tr><td>单号</td><td>OR单号</td><td>箱序号</td><td>箱号</td><td>SKU</td><td>BU</td><td>Qty</td></tr>");
            string PackageKey = string.Empty;
            int PackageSumQty = 0;
            int TotalQty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TotalQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
                if (i == 0)
                {
                    PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                    sb.Append("<tr><td>" + dt.Rows[i]["PLNO"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["OrderNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["str19"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + dt.Rows[i]["Size"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");

                    //sb.Append("<tr><td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["str5"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["Size"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["Gender"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["Category"].ToString() + "</td>");
                    //sb.Append("<td>" + dt.Rows[i]["MaterialDesc"].ToString() + "</td></tr>");
                }
                else
                {
                    //if (PackageKey != dt.Rows[i]["PackageNumber"].ToString())
                    //{
                    //    sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                    //    PackageSumQty = 0;
                    //    PackageKey = dt.Rows[i]["PackageNumber"].ToString();
                    //}
                    sb.Append("<tr><td>" + dt.Rows[i]["PLNO"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["OrderNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["str19"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["PackageNumber"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Atrcle"].ToString() + dt.Rows[i]["Size"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["BU"].ToString() + "</td>");
                    sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");
                }
                PackageSumQty += Convert.ToInt32(dt.Rows[i]["Qty"]);
            }
            //sb.Append("<tr><td>页合计：</td><td>" + PackageSumQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            //sb.Append("<tr><td>总合计：</td><td>" + TotalQty + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            //sb.Append("<tr><td>总箱数：</td><td>" + dt.Rows[0]["boxTotal"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            sb.Append("</table>");
            SW.HttpResponse Response;
            Response = SW.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;

            Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("BoxListDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        /// <summary>
        /// 退货仓箱唛打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBoxm_TH(string id, string type, string OrderID, string packageNumBegin, string packageNumEnd, string Flag = "0")
        {

            ViewBag.OrderID = OrderID;
            ViewBag.Type = type;
            ViewBag.PrintFlag = Flag;
            PrintBoxModel PackageModel = new PrintBoxModel();
            //动态读取存储过程，lrg
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton");
            }
            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo.OrderBy(t => t.PackageNumber);
            if (type == "0")// 0 单个打印 1 部分打印
            {
                return View(PackageModel);
            }
            //把packageNumber列取出来
            List<string> packageNumberList = new List<string>();

            foreach (PrintBoxInfo pbi in PackageModel.EnumerableCustomerInfo)
            {
                packageNumberList.Add(pbi.PackageNumber);
            }
            //去重
            for (int i = 0; i < packageNumberList.Count; i++)
            {
                for (int j = packageNumberList.Count - 1; j > i; j--)
                {
                    if (packageNumberList[i] == packageNumberList[j])
                    {
                        packageNumberList.RemoveAt(j);
                    }
                }
            }
            //选取部分箱
            List<PrintBoxInfo> PrintBoxInfoList = new List<PrintBoxInfo>();

            for (int i = int.Parse(packageNumBegin) - 1; i <= int.Parse(packageNumEnd) - 1; i++)
            {
                var temp = (PackageModel.EnumerableCustomerInfo).Where(t => t.PackageNumber == packageNumberList[i]);
                foreach (PrintBoxInfo ptemp in temp)
                {
                    PrintBoxInfoList.Add(ptemp);
                }
            }
            PrintBoxModel PackageModel2 = new PrintBoxModel();
            PackageModel2.EnumerableCustomerInfo = PrintBoxInfoList;
            return View(PackageModel2);





            //ViewBag.OrderID = OrderID;
            //PrintBoxModel PackageModel = new PrintBoxModel();
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton_" + base.UserInfo.ProjectName);
            //}
            //catch
            //{

            //}
            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxCarton_TH");
            //}

            ////GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(long.Parse(id.ToString())).Result;
            //PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            ////PrintBoxmModel vm = new PrintBoxmModel();

            ////PodSearchCondition SearchCondition = new PodSearchCondition();
            ////SearchCondition.SystemNumber = SystemNumber;
            ////vm.SearchCondition = SearchCondition;

            ////var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            ////vm.PodCollection = results.PodCollections;
            ////vm.PageIndex = results.PageIndex;
            ////vm.PageCount = results.PageCount;
            ////IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            ////JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            //////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            ////timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            ////string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            ////return View(vm);
            ////return jsonStr;
            //#region 页面customerid读取
            //IEnumerable<WMS_Config_Type> ctype = null;
            //ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            //ViewBag.ctype = ctype;
            //#endregion
            //return View(PackageModel);
        }

        /// <summary>
        /// 退货仓箱清单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintBox_TH(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList_TH");
            }
            if (type == "1")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;

            }
            else if (type == "0")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            return View(print);
        }

        /// <summary>
        /// 退货仓托运单打印(包装界面)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult PrintPod_TH(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            //20170214  托运单不需要暗箱打印，会出现总数是一箱的数量，只需要打印一张即可
            //if (type == "1")
            //{
            PrintPodModel print = new PrintPodModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintPodCondition_TH(id.ToString(), type).EnumerableBoxListinfo;
            //PrintBoxmModel vm = new PrintBoxmModel();

            //PodSearchCondition SearchCondition = new PodSearchCondition();
            //SearchCondition.SystemNumber = SystemNumber;
            //vm.SearchCondition = SearchCondition;

            //var results = new OrderManagementService().(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            //vm.PodCollection = results.PodCollections;
            //vm.PageIndex = results.PageIndex;
            //vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            //return View(vm);
            //return jsonStr;
            return View(print);
            //}
            //else if (type == "0")
            //{
            //    PrintPodModel print = new PrintPodModel();
            //    print.EnumerableCustomerInfo = new OrderManagementService().GetPrintPodCondition(long.Parse(id.ToString()), type).EnumerableBoxListinfo;

            //    return View(print);
            //}
            //else
            //{
            //    return View();
            //}
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
    }
}

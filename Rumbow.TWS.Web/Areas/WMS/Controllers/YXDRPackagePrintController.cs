using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using SW = System.Web;
using Runbow.TWS.Web.Areas.WMS.Models.OrderManagement;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System.Data;
using Runbow.TWS.Entity.WMS;
namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class YXDRPackagePrintController : BaseController
    {
        //
        // GET: /WMS/YXDRPackagePrint/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 打印报关箱唛
        /// </summary>
        /// <param name="id">箱号</param>
        /// <param name="type">打印类型(1=批量,0=单箱)</param>
        /// <param name="OrderID">订单号</param>
        /// <returns></returns>
        public ActionResult PrintCustomsCarton(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            PrintBoxModel print = new PrintBoxModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPackageCustomerCarton(id, type).EnumerableBoxListinfo;
            return View(print);
        }
        /// <summary>
        /// 打印箱唛
        /// </summary>
        /// <param name="id">箱号</param>
        /// <param name="type">打印类型(1=批量,0=单箱)</param>
        /// <param name="OrderID">订单号</param>
        /// <returns></returns>
        public ActionResult PrintCarton(string id, string type, string OrderID)
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

            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetPackageBoxCarton(id, type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;

            return View(PackageModel);
        }
        /// <summary>
        /// 打印箱清单
        /// </summary>
        /// <param name="id">箱号</param>
        /// <param name="type">打印类型(1=批量,0=单箱)</param>
        /// <param name="OrderID">订单号</param>
        /// <returns></returns>
        public ActionResult PrintBoxList(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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
            if (type == "1") //批量打印//str17存放的是箱唛号外部单号后七位+箱号后三位
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            else if (type == "0")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }

            //PrintBoxModel print = new PrintBoxModel();
            //print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type).EnumerableBoxListinfo;
            return View(print);
        }

        /// <summary>
        /// 爱库存打印箱清单
        /// </summary>
        /// <param name="id">箱号</param>
        /// <param name="type">打印类型(1=批量,0=单箱)</param>
        /// <param name="OrderID">订单号</param>
        /// <returns></returns>
        public ActionResult PrintBoxListAKC(string id, string type, string OrderID)
        {

            ViewBag.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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
            if (type == "1") //批量打印//str17存放的是箱唛号外部单号后七位+箱号后三位
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            else if (type == "0")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
        
            //PrintBoxModel print = new PrintBoxModel();
            //print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type).EnumerableBoxListinfo;
            return View(print);
        }


        /// <summary>
        /// 打印总箱单
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="type">打印类型</param>
        /// <param name="OrderID">返回orderid</param>
        /// <returns></returns>
        public ActionResult PrintTotalBoxList(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            PrintBoxModel print = new PrintBoxModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintTotalBoxListCondition(id.ToString(), type).EnumerableBoxListinfo;
            return View(print);
        }



        /// <summary>
        /// 打印POD
        /// </summary>
        /// <param name="id">出库单ID或箱号</param>
        /// <param name="type">打印类型(1=单个订单POD)</param>
        /// <param name="OrderID">订单号默认0</param>
        /// <returns></returns>
        public ActionResult PrintPod(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            YXDRBJPrintPodModel print = new YXDRBJPrintPodModel();
            print.EnumerableYXDRPodInfo = new OrderManagementService().GetYXDRPrintPodCondition(id, type).EnumerableYXDRPodInfo;

            return View(print);
        }

        /// <summary>
        /// 批量打印POD
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult PrintAllPod(string ids)
        {
            YXDRBJPrintPodModel print = new YXDRBJPrintPodModel();
            print.EnumerableYXDRPodInfo = new OrderManagementService().GetYXDRPrintAllPodCondition(ids).EnumerableYXDRPodInfo;
            return View(print);
        }
        /// <summary>
        /// 导出箱清单
        /// </summary>
        /// <param name="id">orderid</param>
        /// <param name="type">1</param>
        /// <param name="OrderID">0</param>
        public void ExportBoxDetail(string id, string type, string OrderID)
        {

            DataSet ds = new OrderManagementService().ExportBoxDetailsYXDR(id.ToString());
            DataTable dt = ds.Tables[0];
            //这段代码我承认写的很lj
            if (dt.Rows.Count > 0)
            {
                DataTable dtheader = new DataTable();//订单主信息
                DataTable dtBoxdetail = new DataTable();//箱明细信息

                dtheader.Columns.Add("dc1", typeof(string));
                dtheader.Columns.Add("dc2", typeof(string));
                dtheader.Columns.Add("dc3", typeof(string));
                dtheader.Columns.Add("dc4", typeof(string));
                dtheader.Columns.Add("dc5", typeof(string));
                dtheader.Columns.Add("dc6", typeof(string));
                dtheader.Columns.Add("dc7", typeof(string));
                dtheader.Columns.Add("dc8", typeof(string));
                dtheader.Columns.Add("dc9", typeof(string));
                dtheader.Columns.Add("dc10", typeof(string));
                dtheader.Columns.Add("dc11", typeof(string));
                dtheader.Columns.Add("dc12", typeof(string));
                dtheader.Columns.Add("dc13", typeof(string));

                #region 先把一单的头信息加上
                //第一行
                DataRow dr = dtheader.NewRow();
                dr["dc1"] = "箱清单";
                dr["dc2"] = "订单号:";
                dr["dc3"] = dt.Rows[0]["ExternOrderNumber"].ToString().Trim();
                dr["dc4"] = "货主:";
                dr["dc5"] = "Haddad";
                dr["dc6"] = "门店代码:";
                dr["dc7"] = dt.Rows[0]["str4"].ToString().Trim();
                dr["dc8"] = "门店名称:";
                dr["dc9"] = dt.Rows[0]["Company"].ToString();
                dr["dc10"] = "门店地址:";
                dr["dc11"] = dt.Rows[0]["AddressLine1"].ToString();
                dr["dc12"] = "出货日期:";
                dr["dc13"] = DateTime.Now;
                dtheader.Rows.Add(dr);
                //第二行明细列
                DataRow dr2 = dtheader.NewRow();
                dr2["dc1"] = "箱唛号";
                dr2["dc2"] = "箱号";
                dr2["dc3"] = "款号";
                dr2["dc4"] = "描述";
                dr2["dc5"] = "尺码";
                dr2["dc6"] = "数量";
                dr2["dc7"] = "订单号";
                dr2["dc8"] = "条形码";
                dr2["dc9"] = "";
                dr2["dc10"] = "";
                dr2["dc11"] = "";
                dtheader.Rows.Add(dr2);
                #endregion

                List<string> list = new List<string>();
                int boxnumber = 1;//箱号
                int packageSumQty = 0;//每箱总数
                string packagenumber = "";//箱唛号


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr3 = dtheader.NewRow();
                    if (i == 0)
                    {
                        packagenumber = dt.Rows[i]["PackageNumber"].ToString();
                        dr3["dc1"] = dt.Rows[i]["ShippingMark"].ToString();
                        dr3["dc2"] = boxnumber.ToString();
                        dr3["dc3"] = dt.Rows[i]["Atrcle"].ToString();
                        dr3["dc4"] = dt.Rows[i]["GoodsName"].ToString();
                        dr3["dc5"] = dt.Rows[i]["Size"].ToString();
                        dr3["dc6"] = dt.Rows[i]["Qty"].ToString();
                        dr3["dc7"] = dt.Rows[i]["ExternOrderNumber"].ToString();
                        dr3["dc8"] = dt.Rows[i]["SKU"].ToString();
                    }
                    else
                    {
                        if (packagenumber != dt.Rows[i]["PackageNumber"].ToString())
                        {
                            DataRow dr4 = dtheader.NewRow();
                            dr4["dc1"] = "箱总数:";
                            dr4["dc2"] = packageSumQty.ToString(".00");
                            dr4["dc3"] = "";
                            dr4["dc4"] = "";
                            dr4["dc5"] = "";
                            dr4["dc6"] = "";
                            dr4["dc7"] = "";
                            dr4["dc8"] = "";
                            dtheader.Rows.Add(dr4);
                            packagenumber = dt.Rows[i]["PackageNumber"].ToString();
                            packageSumQty = 0;
                            boxnumber++;
                        }

                        dr3["dc1"] = dt.Rows[i]["ShippingMark"].ToString();
                        dr3["dc2"] = boxnumber;
                        dr3["dc3"] = dt.Rows[i]["Atrcle"].ToString();
                        dr3["dc4"] = dt.Rows[i]["GoodsName"].ToString();
                        dr3["dc5"] = dt.Rows[i]["Size"].ToString();
                        dr3["dc6"] = dt.Rows[i]["Qty"].ToString();
                        dr3["dc7"] = dt.Rows[i]["ExternOrderNumber"].ToString();
                        dr3["dc8"] = dt.Rows[i]["SKU"].ToString();


                    }
                    dtheader.Rows.Add(dr3);
                    packageSumQty += Convert.ToInt32(Convert.ToDouble(dt.Rows[i]["Qty"].ToString()));
                }
                DataRow dr5 = dtheader.NewRow();
                dr5["dc1"] = "箱总数:";
                dr5["dc2"] = packageSumQty.ToString(".00");
                dr5["dc3"] = "";
                dr5["dc4"] = "";
                dr5["dc5"] = "";
                dr5["dc6"] = "";
                dr5["dc7"] = "";
                dr5["dc8"] = "";
                dtheader.Rows.Add(dr5);
                //ExportDataToExcelHelper.ExportDataSetToExcel(dtheader, "箱清单" + DateTime.Now.ToString("yyyy-MM-dd"), "");
                EPPlusOperation.ExportByEPPlus(dtheader, "箱清单" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                //ExportDataToExcelHelper.ExportDataSetToExcel(dt, "箱清单" + DateTime.Now.ToString("yyyy-MM-dd"), "");
                EPPlusOperation.ExportByEPPlus(dt, "箱清单" + DateTime.Now.ToString("yyyy-MM-dd"));
            }

        }


    }
}

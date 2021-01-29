using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Areas.ForecastWarehouse.Models;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Common;
using System.Text;
using Runbow.TWS.Entity;
using MyFile = System.IO.File;
using System.Data;
using System.IO;


namespace Runbow.TWS.Web.Areas.ForecastWarehouse.Controllers
{
    public class ForecastWarehouseController : BaseController
    { 
        public ActionResult Index(int? i ,int? PageIndex)
        { 
            ForecastOrdersModel vm = new ForecastOrdersModel();
            if (i != null) {

                if (i == 1)
                {

                    vm.Message = "修改成功!";
                }
                else if (i == 2)
                {

                    vm.Message = "修改失败!";

                }
                else if (i == 3)
                {

                    vm.Message = "状态为待发货,方可解锁!";
                }
               
                //else {

                //    vm.Message = "状态为待发货,方可确认收货!";
                //}

            }
            else
            {
                //
                var Result = new ForecastWarehouseService().GetCRMInfo2(new GetForecastWarehouseRequest() { PageSize =UtilConstants.PAGESIZE , PageIndex = PageIndex ?? 0 }).Result;
                vm.IEnumerableForecastOrders = Result.IEnumerableForecastOrders;
                vm.PageIndex = Result.PageIndex;
                vm.PageSize = Result.PageSize;
                vm.PageCount = Result.PageCount;
               
            }
           
            return View(vm);
        }


        public ActionResult Index2( int? PageIndex)
        {

            //CrmTrackViewModel track=new CrmTrackViewModel();
            //id = track.typeid;
            // id = id1;
           // ForecastOrdersModel Model = new ForecastOrdersModel();
            ForecastOrdersModel vm = new ForecastOrdersModel();
            var Result = new ForecastWarehouseService().GetCRMInfo2(new GetForecastWarehouseRequest() {  PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            vm.IEnumerableForecastOrders = Result.IEnumerableForecastOrders;
            vm.PageIndex = Result.PageIndex;
            vm.PageSize = Result.PageSize;
            vm.PageCount = Result.PageCount;

            return View(vm);

        }
        public ActionResult SpecifiedDeliveryDate( ForecastOrdersModel vm)
        {
            //= new ForecastOrdersModel();
            string a = Request["State2"].ToString();
            vm.States = a;
            string ware = Request["waveId"].ToString();
            vm.WID = ware;
            return View(vm);



        }
        [HttpPost]
        public ActionResult SpecifiedDeliveryDate(string id, string state, string PickTime)
        {
            ForecastOrdersModel vm = new ForecastOrdersModel();

            ForecastOrdersModel Model = new ForecastOrdersModel();
            ////string ware = Request["waveId"].ToString();
            ////string C = ware;
            ////string a = Request["State2"].ToString();
            ////C = a;
           // string l = Model.PickTime.ToString();
            string l = vm.PickTime.ToString(); ;
            //if (vm.States == "待发货")
            //{

            int c = new ForecastWarehouseService().appointed(PickTime, id);
            return Json(new { IsSuccess = true });
        }
        public ActionResult SpecifiedDeliveryDate2(ForecastOrdersModel vm)
        {
            // = new ForecastOrdersModel();
            string a = Request["State2"].ToString();
            vm.States = a;
            string ware = Request["waveId"].ToString();
            vm.WID = ware;
            return View(vm);



        }

        public ActionResult waveList()
        {
            string a = Request["waveList"].ToString();
            ForecastOrdersModel vm = new ForecastOrdersModel();
            vm.IEnumerableForecastOrders = new ForecastWarehouseService().waveList(Request["waveList"].ToString());


            return View(vm);



        }
        
        [HttpPost]
        public ActionResult SpecifiedDeliveryDate2(string id, string state, string PickTime)
        {
            ForecastOrdersModel vm = new ForecastOrdersModel();
            ForecastOrdersModel Model = new ForecastOrdersModel();
         
            string l = vm.PickTime.ToString(); ;
            //if (vm.States == "待发货")
            //{

            int c = new ForecastWarehouseService().require(PickTime, id);
            return Json(new { IsSuccess = true });
            //}
            //else
            //{

            //    Response.Write("<script>alert('状态为待发货,方可执行此操作!');</script>");
            //}
          
        }
        [HttpPost]
        public ActionResult Index(ForecastOrdersModel vm, int? PageIndex, int? id)
        {
           
            //CrmTrackViewModel track=new CrmTrackViewModel();
            //id = track.typeid;
            // id = id1;
            ForecastOrdersModel Model = new ForecastOrdersModel();

            var Result = new ForecastWarehouseService().GetCRMInfo(new GetForecastWarehouseRequest() { ForecastOrders = vm.ForecastOrders, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            vm.IEnumerableForecastOrders = Result.IEnumerableForecastOrders;
            vm.PageIndex = Result.PageIndex;
            vm.PageSize = Result.PageSize;
            vm.PageCount = Result.PageCount;
       
            return View(vm);

        }
      
         
        public ActionResult deblocking()
        { 
            //ForecastOrdersModel Model = new ForecastOrdersModel();
            string ware = Request["waveId"].ToString();
            string C = ware;
            string a = Request["State2"].ToString();
             C = a;
            //string Result = new ForecastWarehouseService().deblocking(ware);
            if ( Request["State2"].ToString() == "待发货")
            {

                int panduan = new ForecastWarehouseService().deblocking2(ware);
                if (panduan > 0)
                {
                    //

                    return RedirectToAction("Index", new { i = 1, PageIndex=1});
                }
                else
                {
                    return RedirectToAction("Index", new { i = 2, PageIndex =1});
                 
                    //Model.Message = "修改失败!";
                }
            }
            else
            {
                return RedirectToAction("Index", new { i = 3, PageIndex=1 });
                //Model.Message = "修改失败!";
                // 
                // Response.Write("<script language = javascript>alert('状态为已锁定,方可解锁!')</script>");

            }

             
           
             
        }
        
         public ActionResult cancellation()
        { 
            
            string ware = Request["waveId"].ToString();
            string C = ware;
            string a = Request["State2"].ToString();
             C = a;



             int panduan = new ForecastWarehouseService().cancellation(ware);
                if (panduan > 0)
                    if (panduan > 0)
                    {
                        return RedirectToAction("Index", new { i = 1, PageIndex=1 });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { i = 2, PageIndex=1 });
                    }
          return   RedirectToAction("Index");
        
            }
         public ActionResult confirmation()
        { 
            //ForecastOrdersModel Model = new ForecastOrdersModel();
            string ware = Request["waveId"].ToString();
            string C = ware;
            string a = Request["State2"].ToString();
             C = a;
            //string Result = new ForecastWarehouseService().deblocking(ware);
            //if ( Request["State2"].ToString() == "待发货")
            //{

                int panduan = new ForecastWarehouseService().confirmation(ware);
                if (panduan > 0)
                {
                    //

                    return RedirectToAction("Index", new { i = 1 , PageIndex=1});
                }
                else
                {
                    return RedirectToAction("Index", new { i =2, PageIndex=1 });
                 
                    //Model.Message = "修改失败!";
                }
            //}
            //else
            //{
            //    return RedirectToAction("Index", new { i = 4 });
            //    //Model.Message = "修改失败!";
            //    // 
            //    // Response.Write("<script language = javascript>alert('状态为已锁定,方可解锁!')</script>");

            //}

             
           
             
        }

         public ActionResult carriers()
         {
             ForecastOrdersModel vm = new ForecastOrdersModel();
             string name = base.UserInfo.Name;
             StringBuilder sb = new StringBuilder();

             //vm.WaveReleaseTime3 = vm.WaveReleaseTime;

             sb.Append("AND State in('正在发货中' , '待发货') ");
             vm.IEnumerableForecastOrders = new ForecastWarehouseService().carriers(name, sb.ToString());

             return View(vm);
         }
         public ActionResult carrierslist(string ShipToSity, string WaveReleaseTime)
         {
             ForecastOrdersModel vm = new ForecastOrdersModel();
             vm.ShipToSity = ShipToSity;
             vm.WaveReleaseTime = WaveReleaseTime;
             StringBuilder sb = new StringBuilder();
         // string ShipToSity2=Session["ShipToSity"].ToString();
             string name = base.UserInfo.Name;
             vm.IEnumerableForecastOrders = new ForecastWarehouseService().carrierslist(name, ShipToSity, WaveReleaseTime,sb.ToString());
        
            

             return View(vm);
         }
          [HttpPost]
         public ActionResult carrierslist(string ShipToSity, string WaveReleaseTime, ForecastOrdersModel vm)
         {
            // ForecastOrdersModel vm = new ForecastOrdersModel();
             StringBuilder sb = new StringBuilder();
             ShipToSity = vm.ShipToSity;
             WaveReleaseTime = vm.WaveReleaseTime;
             if (vm.States != "请选择")
             {
                 sb.Append("AND State='" + vm.States + "'");
             }

             
             // string ShipToSity2=Session["ShipToSity"].ToString();
             string name = base.UserInfo.Name;
             vm.IEnumerableForecastOrders = new ForecastWarehouseService().carrierslist(name, ShipToSity, WaveReleaseTime,sb.ToString());



             return View(vm);
         }
         public ActionResult xiangxi()
         {
             ForecastOrdersModel vm = new ForecastOrdersModel();

             StringBuilder sb = new StringBuilder();
             // string ShipToSity2=Session["ShipToSity"].ToString();
             string name = base.UserInfo.Name;
             vm.IEnumerableForecastOrders = new ForecastWarehouseService().xiangxi(name);



             return View(vm);
         }
        
         [HttpPost]
         public ActionResult carriers(  ForecastOrdersModel vm)
         {
            // string a = Request["DisplayName"].ToString();
             StringBuilder sb = new StringBuilder();
             if (!string.IsNullOrEmpty(vm.WaveReleaseTime))
             {
               
                 sb.Append("AND CONVERT(varchar, Mail.WaveReleaseTime,112)>=CONVERT(varchar, dateadd(day,0,'" + vm.WaveReleaseTime + "'),112)");
             }
             
             if (!string.IsNullOrEmpty(vm.WaveReleaseTime2))
             {
               
                 sb.Append("AND CONVERT(varchar, Mail.WaveReleaseTime,112)<=CONVERT(varchar, dateadd(day,0,'" + vm.WaveReleaseTime2 + "'),112)");
             }
             if (vm.States != "请选择")
             {
                 sb.Append("AND State='" + vm.States + "'");
             }

            
             string name = base.UserInfo.Name;
             
             vm.IEnumerableForecastOrders = new ForecastWarehouseService().carriers(name,sb.ToString());


             return View(vm);
         }
         public void Excel()
         {
             //CrmManageViewModel vm = new CrmManageViewModel();
             // List<CrmBaseViewModel> entityList = new List<CrmBaseViewModel>();


             //获取所有用户信息
             //

             CRMService crm = new CRMService();
             StringBuilder sb = new StringBuilder();
             ForecastOrdersModel vm = new ForecastOrdersModel();
             if (!string.IsNullOrEmpty(Session["WaveReleaseTime"].ToString()))
             {
                 sb.Append("AND CONVERT(varchar, Wave.shipments,112)>=CONVERT(varchar, dateadd(day,0,'" + Session["WaveReleaseTime"].ToString() + "'),112)");
             }

             if (!string.IsNullOrEmpty(Session["WaveReleaseTime2"].ToString()))
             {
                 sb.Append("AND CONVERT(varchar, Wave.shipments,112)<=CONVERT(varchar, dateadd(day,0,'" + Session["WaveReleaseTime2"].ToString() + "'),112)");
             }
             ForecastWarehouseService f=new ForecastWarehouseService();
             DataTable dt = f.export2(sb.ToString());
             DataTable dt2 = new DataTable();
          
             dt2.Columns.Add("Shipment");
             dt2.Columns.Add("ThreePL");
             dt2.Columns.Add("ShipToCode");
             dt2.Columns.Add("ShipToName");
             dt2.Columns.Add("Pieces");
             dt2.Columns.Add("ShipToSity");
             dt2.Columns.Add("Cartons");
             dt2.Columns.Add("完成发货时间");
            

             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 dt2.Rows.Add(dt2.NewRow());
                 for (int j = 0; j < dt.Columns.Count; j++)
                 {

                     dt2.Rows[i][j] = dt.Rows[i][j];

                 }
             }


             string targetPath = Server.MapPath("~/DownLoadExcel");
             NewExcelHelper excelHelper = new NewExcelHelper();
             string execlname = DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
             string fileFullPath = Path.Combine(targetPath, execlname);
             excelHelper.CreateExcelByDataTable(fileFullPath, dt2);
             excelHelper.Dispose();
             FileDownLoad(fileFullPath);

             //if (fileFullPath != "")
             //{

             //    File.Delete(fileFullPath);
             //}


         }

         private void FileDownLoad(string filename)
         {
             string destFileName = filename;
             //destFileName = Server.MapPath("./") + destFileName;

             destFileName = Server.UrlDecode(destFileName);

             if (MyFile.Exists(destFileName))
             {
                 FileInfo fi = new FileInfo(filename);
                 Response.Clear();
                 Response.ClearHeaders();
                 Response.Buffer = true;
                 Response.Charset = "GB2312";

                 //添加头信息，为 "文件下载/另存为 "对话框指定默认文件名  
                 Response.AppendHeader("Content-Disposition", "attachment;filename="
                 + HttpUtility.UrlEncode(Path.GetFileName(destFileName),
               Encoding.UTF8));
                 Response.AppendHeader("Content-Length", fi.Length.ToString());
                 Response.ContentType = "text/plain";
                 Response.Filter.Close();
                 Response.WriteFile(destFileName);
                 Response.Flush();
                 Response.End();
             }
             else
             {
                 Response.Write("<script language = javascript>alert('下载出错')</script>");
             }
         }
         [HttpPost]
         public ActionResult export(ForecastOrdersModel vm)
         {

             StringBuilder sb = new StringBuilder();

             if (!string.IsNullOrEmpty(vm.WaveReleaseTime))
             {

                 //vm.WaveReleaseTime3 = vm.WaveReleaseTime;
                 Session["WaveReleaseTime"] = vm.WaveReleaseTime;
                 sb.Append("AND CONVERT(varchar, Wave.shipments,112)>=CONVERT(varchar, dateadd(day,0,'" + vm.WaveReleaseTime + "'),112)");
             }

             if (!string.IsNullOrEmpty(vm.WaveReleaseTime2))
             {
                // vm.WaveReleaseTime4 = vm.WaveReleaseTime2;
                 Session["WaveReleaseTime2"] = vm.WaveReleaseTime2;
                 sb.Append("AND CONVERT(varchar, Wave.shipments,112)<=CONVERT(varchar, dateadd(day,0,'" + vm.WaveReleaseTime2 + "'),112)");
             }

             vm.IEnumerableForecastOrders = new ForecastWarehouseService().export(sb.ToString());
             return View(vm);
         }
         public ActionResult export()
         {
             ForecastOrdersModel vm = new ForecastOrdersModel();
            
          
             return View(vm);
         }
    }
}

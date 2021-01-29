using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Dao.RabbitMQ;
using Runbow.TWS.Entity.RabbitMQ;
using Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse;
using Runbow.TWS.Web.Areas.WMS.Models.IntelligentOperation;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using aaaa = System.IO.File;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class IntelligentOperationController : BaseController
    {
        //
        // GET: /WMS/IntelligentOperation/
        private RabbitReceiver r2 = new RabbitReceiver();
        public ActionResult Index()
        {

            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.UserID == base.UserInfo.ID).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            ViewBag.WorkStation = WorkStation;
            return View();
        }
        public ActionResult ReceiptIndex()
        {

            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.UserID == base.UserInfo.ID).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            ViewBag.WorkStation = WorkStation;
            return View();
        }
        public JsonResult GetMessageMQ(string WorkStationId)
        {
            try
            {   
                var goodsShelve = Read("VGA_1_IN").ToString();    
                var response = new IntelligentOperationService().ShelvesPanel(goodsShelve, WorkStationId).Result;
                if (response.instructions.Count() > 0)
                {
                    return Json(new { Code = 1, instructions = response.instructions, shelvesPanel = response.shelvesPanel });
                }
            }
            catch (Exception)
            {
                return Json(new { Code = 0 });
            }
            return Json(new { Code = 0 });

            //try
            //{
            //    if (Session["BasicDeliverEventArgs"] == null)
            //    {
            //        var e = r2.Consume(new RabbitRecOption("VGA_" + WorkStationId + "_IN"));
            //        //receiver.Handle(Func);
            //        //Thread.Sleep(2000);

            //        Session["BasicDeliverEventArgs"] = e.DeliveryTag;
            //        //var e1 = r2.Consume1(new RabbitRecOption("aaa6"));
            //        //Session["BasicDeliverEventArgs1"] = e1.DeliveryTag;
            //        var body = e.Body;
            //        var content = Encoding.UTF8.GetString(body);
            //        if (content.Length > 0)
            //        {
            //            //VGA_[操作台ID]_IN
            //            var contentBody = JSONStringTo<InstructionInfo>(content);
            //            var response = new IntelligentOperationService().ShelvesPanel(contentBody.goodsShelve, WorkStationId).Result;
            //            if (response.instructions.Count() > 0)
            //            {
            //                return Json(new { Code = 1, instructions = response.instructions, shelvesPanel = response.shelvesPanel });
            //            }
            //            else
            //            {
            //                //当有货架过来 但是没有需要处理的指令  直接ack这条信息 让货架离开
            //                if (AckMq(WorkStationId, contentBody.goodsShelve))
            //                {

            //                }

            //            }
            //        }
            //    }
            //    else
            //    {
            //        //var e = ulong.Parse(Session["BasicDeliverEventArgs"].ToString());
            //        //r2.backAck("VGA_" + WorkStationId + "_IN", e);
            //        r2.CloseChannel("VGA_" + WorkStationId + "_IN");
            //        //r2.backAck(e1);
            //        //receiver.Handle(Func);
            //        //receiver.backAck(e);   
            //        Session["BasicDeliverEventArgs"] = null;
            //        return Json(new { Code = 0 });
            //    }
            //}
            //catch (Exception e)
            //{
            //    return Json(new { Code = 0 });
            //}
            //return Json(new { Code = 0 });

        }
        public JsonResult GetMessageMQ_Receipt(string WorkStationId)
        {
            //return await Task.Factory.StartNew(() =>
            //{
            try
            {
                if (Session["BasicDeliverEventArgs_Receipt"] == null)
                {
                    var e = r2.Consume(new RabbitRecOption("VGA_" + WorkStationId + "_IN_Receipt"));
                    //receiver.Handle(Func);
                    //Thread.Sleep(2000);

                    Session["BasicDeliverEventArgs_Receipt"] = e.DeliveryTag;
                    //var e1 = r2.Consume1(new RabbitRecOption("aaa6"));
                    //Session["BasicDeliverEventArgs1"] = e1.DeliveryTag;
                    var body = e.Body;
                    var content = Encoding.UTF8.GetString(body);
                    if (content.Length > 0)
                    {
                        //VGA_[操作台ID]_IN
                        //var contentBody = JSONStringTo<InstructionInfo>(content);
                        var response = new IntelligentOperationService().ShelvesPanel_Receipt(content, WorkStationId).Result;
                        if (response.instructions.Count() > 0)
                        {
                            return Json(new { Code = 1, instructions = response.instructions, shelvesPanel = response.shelvesPanel });
                        }
                        else
                        {
                            //当有货架过来 但是没有需要处理的指令  直接ack这条信息 让货架离开
                            if (AckMq(WorkStationId, content))
                            {

                            }

                        }
                    }
                }
                else
                {
                    //var e = ulong.Parse(Session["BasicDeliverEventArgs"].ToString());
                    //r2.backAck("VGA_" + WorkStationId + "_IN", e);
                    r2.CloseChannel("VGA_" + WorkStationId + "_IN_Receipt");
                    //r2.backAck(e1);
                    //receiver.Handle(Func);
                    //receiver.backAck(e);   
                    Session["BasicDeliverEventArgs_Receipt"] = null;
                    return Json(new { Code = 0 });
                }
            }
            catch (Exception e)
            {
                return Json(new { Code = 0 });
            }
            return Json(new { Code = 0 });
            //return strReturn;
            //});
            //var receiver = new RabbitReceiver(new RabbitRecOption("test"));
            //receiver.Handle(Func);

            //return Json(new { Code = 1 });
        }
        public JsonResult SubmitData(long ID, long RePickWallDetailId, int ActualQty = 0)
        {
            var response = new IntelligentOperationService().SubmitData(ID, RePickWallDetailId, ActualQty);
            if (response.IsSuccess)
            {
                return Json(new { Code = 1, Body = "成功" });
            }
            return Json(new { Code = 0 });
        }

        public JsonResult SubmitData_Receipt(long ID, int ActualQty = 0)
        {
            var response = new IntelligentOperationService().SubmitData_Receipt(ID, ActualQty);
            if (response.IsSuccess)
            {
                return Json(new { Code = 1, Body = "成功" });
            }
            return Json(new { Code = 0 });
        }
        public static T JSONStringTo<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            T objs = Serializer.Deserialize<T>(JsonStr);
            return objs;
        }
        /// <summary>
        /// ack
        /// </summary>
        /// <param name="WorkStationId"></param>
        /// <param name="GoodsShelve"></param>
        /// <returns></returns>
        public JsonResult ValidateMQ(string WorkStationId, string GoodsShelve)
        {
            del("VGA_1_IN");
            if (true)
            {
                return Json(new { Code = 1, Body = "成功" });
            }
            else
            {
                return Json(new { Code = 0, Body = "失败" });
            }
            //if (AckMq(WorkStationId, GoodsShelve))
            //{
            //    return Json(new { Code = 1, Body = "成功" });
            //}
            //else
            //{
            //    return Json(new { Code = 0, Body = "失败" });
            //}
        }

        private bool AckMq(string WorkStationId, string GoodsShelve)
        {
            try
            {
                MQTxt("VGA_1_OUT", GoodsShelve);
                //var e = ulong.Parse(Session["BasicDeliverEventArgs"].ToString());

                //RabbitSender rs = new RabbitSender(new RabbitSenderOption(new { GoodsShelve = GoodsShelve }, "VGA_" + WorkStationId + "_OUT", "VGA_" + WorkStationId + "_OUT", "VGA_" + WorkStationId + "_OUT", "VGA_" + WorkStationId + "_OUT", Convert.ToByte(0)));
                ////WriterLogFile("开始发送货架：" + i.GoodsShelve + "到操作台" + i.OperatingArea);
                //rs.Send();
                ////var e1 = ulong.Parse(Session["BasicDeliverEventArgs1"].ToString());
                //if (e != 0)
                //{
                //    r2.backAck("VGA_" + WorkStationId + "_IN", e);
                //    //r2.backAck(e1);
                //    //receiver.Handle(Func);
                //    //receiver.backAck(e);
                //}
                //Session["BasicDeliverEventArgs"] = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="WorkStationId"></param>
        /// <returns></returns>
        public JsonResult CloseChannel(string WorkStationId)
        {

            //r2.CloseChannel("VGA_" + WorkStationId + "_IN");
            ////r2.backAck(e1);
            ////receiver.Handle(Func);
            ////receiver.backAck(e);

            //Session["BasicDeliverEventArgs"] = null;
            return Json(new { Code = 1, Body = "成功" });
        }
        public JsonResult CloseChannel_Receipt(string WorkStationId)
        {

            r2.CloseChannel("VGA_" + WorkStationId + "_IN_Receipt");
            //r2.backAck(e1);
            //receiver.Handle(Func);
            //receiver.backAck(e);

            Session["BasicDeliverEventArgs_Receipt"] = null;
            return Json(new { Code = 1, Body = "成功" });
        }
        public JsonResult GetPickUpGoodsWall(long WorkStationId)
        {
            var response = new IntelligentOperationService().GetPickUpGoodsWall(WorkStationId);
            if (response.IsSuccess)
            {
                var rows = response.Result.pickUpGoodsWall.Select(a => a.Rows).FirstOrDefault();
                var cells = response.Result.pickUpGoodsWall.Select(a => a.Cells).FirstOrDefault();
                return Json(new { Code = 1, Rows = rows, Cells = cells, pickUpGoodsWall = response.Result.pickUpGoodsWall, mapping = response.Result.mapping });      //, mapping = response.Result.mapping
            }
            return Json(new { Code = 0 });

        }
        private void Func(BasicDeliverEventArgs e)
        {
            if (e != null)
            {
                var body = e.Body;
                var content = Encoding.UTF8.GetString(body);
                //Label1.Text = content.ToString();
                //return true;
                //this.basucack = e;
                Session["BasicDeliverEventArgs"] = e;
            }
            //return false;
        }
        [HttpGet]
        public ActionResult GoodsManagement()
        {
            IntelligentOperation io = new IntelligentOperation();
            io.response = new PickUpGoodsManagementResponse();
            IntelligentOperationService ios = new IntelligentOperationService();
            io.request = new PickUpGoodsManagementRequest();

            io.request.PageIndex = 0;
            io.request.PageSize = UtilConstants.PAGESIZE;
            //if (string.IsNullOrEmpty(WorkStationId))
            //{
            //    var WorkStationList = ApplicationConfigHelper.GetCacheInfo().Where(a => a.UserID == base.UserInfo.ID).Select(a => a.OperationAreaID);
            //    if (WorkStationList.Count() == 1)
            //    {
            //        io.request.WorkStationId = WorkStationList.First().ToString();
            //    }
            //}
            //else
            //{
            //    io.request.WorkStationId = WorkStationId;
            //}
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            io.CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerName.ToString(), Text = c.CustomerName });
            io.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                 .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            if (io.CustomerList.Count() == 1)
            {
                io.request.CustomerName = io.CustomerList.First().Text;
            }
            if (io.WarehouseList.Count() == 1)
            {
                io.request.Warehouse = io.WarehouseList.First().Text;
            }
            //if (!string.IsNullOrEmpty(io.request.WorkStationId))
            //{
            //    io.request.Warehouse = ApplicationConfigHelper.GetCacheInfo().Where(a => a.OperationAreaID == Convert.ToInt32(io.request.WorkStationId)).Select(a => a.WarehouseName).FirstOrDefault().ToString();
            //    io.request.CustomerName = ApplicationConfigHelper.GetCacheInfo().Where(a => a.OperationAreaID == Convert.ToInt32(io.request.WorkStationId)).Select(a => a.CustomerName).FirstOrDefault().ToString();
            //}
            //else
            //{
            //}

            var response = ios.GetGoodsManagement(io.request);
            if (response.IsSuccess)
            {
                io.response.instructions = response.Result.instructions;
            }
            return View(io);
        }
        [HttpPost]
        public ActionResult GoodsManagement(IntelligentOperation io, int? PageIndex)
        {
            io.response = new PickUpGoodsManagementResponse();
            IntelligentOperationService ios = new IntelligentOperationService();

            var OperationAreaList = ApplicationConfigHelper.GetCacheInfo().Where(a => a.UserID == base.UserInfo.ID).Select(a => a.OperationAreaID);

            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            io.CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerName.ToString(), Text = c.CustomerName });
            io.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                 .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            if (io.CustomerList.Count() == 1)
            {
                io.request.CustomerName = io.CustomerList.First().Text;
            }
            if (io.WarehouseList.Count() == 1)
            {
                io.request.Warehouse = io.WarehouseList.First().Text;
            }
            //if (!string.IsNullOrEmpty(io.request.WorkStationName))
            //{
            //    io.request.Warehouse = ApplicationConfigHelper.GetCacheInfo().Where(a => a.Operation == io.request.WorkStationName).Select(a => a.WarehouseName).FirstOrDefault().ToString();
            //    io.request.CustomerName = ApplicationConfigHelper.GetCacheInfo().Where(a => a.Operation == io.request.WorkStationName).Select(a => a.CustomerName).FirstOrDefault().ToString();
            //}
            //else
            //{

            //}
            io.request.PageIndex = PageIndex ?? 0;
            io.request.PageSize = UtilConstants.PAGESIZE;
            var response = ios.GetGoodsManagement(io.request);
            if (response.IsSuccess)
            {
                io.response.instructions = response.Result.instructions;
                io.PageCount = response.Result.PageCount;
                io.PageIndex = response.Result.PageIndex;
            }
            return View(io);
        }


        byte[] byData = new byte[100];
        char[] charData = new char[1000];
        public string Read(string Name)
        {
            string path = "//192.168.10.200/cz/Order/" + Name + ".txt";
            //FileStream file = new FileStream(path + Name + ".txt", FileMode.Open);
            FileInfo fi = new FileInfo((path));
            StreamReader sr = fi.OpenText();
            string str = sr.ReadToEnd();
            sr.Close();
            return str.Trim();
            //try
            //{
            //    byte[] byData = new byte[100];
            //    char[] charData = new char[1000];
            //    string path = "//192.168.10.200/cz/Order/";
            //    FileStream file = new FileStream(path + Name + ".txt", FileMode.Open);
            //    file.Seek(0, SeekOrigin.Begin);
            //    file.Read(byData, 0, 100); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
            //    Decoder d = Encoding.Default.GetDecoder();
            //    d.GetChars(byData, 0, byData.Length, charData, 0);
            //    file.Close();
            //    return Encoding.Default.GetString(byData);
            //}
            //catch (IOException e)
            //{
            //    return "";
            //}
        }

        public static void del(string Name)
        {
            string path = "//192.168.10.200/cz/Order";
            if (aaaa.Exists(@path + Name + ".dat"))
            {
                //如果存在则删除
                aaaa.Delete(@path + Name + ".dat");
            }
        }
        public static void MQTxt(string Name, string fileInfotxt)
        {
            try
            {
                string path = "//192.168.10.200/cz";
                //如果不存在，则创建目录
                if (!Directory.Exists(path + "Order/"))
                {
                    Directory.CreateDirectory(path + "\\Order/");
                }
                FileStream fs = new FileStream(path + "\\Order/" + Name + ".txt", FileMode.Append);

                StreamWriter streamWriter = new StreamWriter(fs);              //, Encoding.UTF8
                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.WriteLine(fileInfotxt);
                streamWriter.Flush();
                streamWriter.Close();
                fs.Close();
            }
            catch { }
        }
        //public JsonResult ShelvesPanel(string id)
        //{
        //    var response = new IntelligentOperationService().ShelvesPanel(id);
        //    if (response.IsSuccess)
        //    {
        //        return Json(new { Code = 1, response = response });
        //    }
        //    return Json(new { Code = 0 });

        //}
    }
}

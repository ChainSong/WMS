using Runbow.TWS.Biz.POD;
using Runbow.TWS.MessageContracts.POD.MaryKay;
using Runbow.TWS.Web.Areas.POD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.POD.MaryKay;
using Runbow.TWS.Web.Common;
using Runbow.TWS.MessageContracts;
using System.Collections;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Data.SqlClient;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class MaryKayController : BaseController
    {
        //
        // GET: /POD/MaryKay/

        public ActionResult MaryKayOrderNoIssued(MaryKayOrderNoIssuedModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult MaryKayOrderNoIssued(MaryKayOrderNoIssuedModel model, int? PageIndex)
        {
            //QueryDMSPODRequest request = new QueryDMSPODRequest()
            //{
            //    PageIndex = model.PageIndex,
            //    PageSize = model.PageSize,
            //    PageCount = model.PageCount,
            //    SystemOrderNo = model.SystemOrderNo,
            //    MkOrderNo = model.MkOrderNo,
            //    BeginOrderNoDateTime = model.BeginOrderNoDateTime,
            //    EndOrderNoDateTime = model.EndOrderNoDateTime,
            //    EndCity = model.EndCity,
            //    EndCityID = model.EndCityID,
            //    ShipperID = model.ShipperID,
            //    ShipperName = model.ShipperName,
            //};
            //var Result = new MaryKayService().QueryDMSPOD(request).Result;

            //model.PageIndex = Result.PageIndex;
            //model.PageSize = Result.PageSize;
            //model.PageCount = Result.PageCount;

            var Result = new MaryKayService().GetMaryKayGetOrderIssued(new GetMaryKayOrderIssuedRequest() { SqlWhere = this.SqlWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            model.OrderNoIssuedTable = Result.OrderNoIssuedTable;
            model.PageIndex = Result.PageIndex;
            model.PageSize = Result.PageSize;
            model.PageCount = Result.PageCount;          
            
            return View(model);
        }

        public string SqlWhere(MaryKayOrderNoIssuedModel model) 
        {
            string sql = "";

            if (!string.IsNullOrEmpty(model.ShipperID))
            {
                sql += " AND POD.ShipperID='" + model.ShipperID + "'";
            }


            if(!string.IsNullOrEmpty(model.MkOrderNo))
            {
                sql += " AND POD.CustomerOrderNumber='" + model.MkOrderNo + "'";
            }


            if (!string.IsNullOrEmpty(model.SystemOrderNo))
            {
                sql += " AND POD.SystemNumber='" + model.MkOrderNo + "'";
            }

            if(!string.IsNullOrEmpty(model.BeginOrderNoDateTime.ToString()))
            {
                sql += " AND POD.Str1>='" + Convert.ToDateTime(model.BeginOrderNoDateTime).ToString("yyyy-MM-dd") + "'";
            }

            if (!string.IsNullOrEmpty(model.EndOrderNoDateTime.ToString()))
            {
                sql += " AND POD.Str1<='" + Convert.ToDateTime(model.EndOrderNoDateTime.ToString()).ToString("yyyy-MM-dd") + "'";
            }


            if (model.EndCity != null && Convert.ToInt32(model.EndCityID) != 0)
            {
                sql += " and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + model.EndCityID + "))";
            }



            if (!string.IsNullOrEmpty(model.IssuedStatusID))
            {
                if (model.IssuedStatusID == "未同步")
                {
                    sql += " AND POD.Str40 IS NULL";
                }
                else
                {
                    sql += " AND POD.Str40='" + model.IssuedStatusID + "'";
                }
            }
           


            return sql;
        }

        public string JsonToTable(string jsonStr)
        {
            List<MaryKayToJson> ToJosnList = new List<MaryKayToJson>();
            JavaScriptSerializer json = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            List<MaryKayToJson> jsonlist = json.Deserialize<List<MaryKayToJson>>(jsonStr);    //将json数据转化为对象类型并赋值给list

            string value = "";
            for (int i = 0; i < jsonlist.Count; i++)
            {
                value += jsonlist[i].PODID.ToString()+",";
            }
            return value;
        }

        /// <summary>
        /// 获取物流信息详情
        /// </summary>
        /// <param name="Id">订单号Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MaryKayLoginsticsTrackDetail(string Id)
        {
            MaryKayTrackInfoModel model = new MaryKayTrackInfoModel();
            var Result = new MaryKayService().GetGetMaryKayGetTrackDetail(new GetMaryKayTrackInfoRequest() { SqlWhere = "SELECT * FROM tbl_PODTrack WHERE colCustOrderNo='" + Id + "' ORDER BY colTrackTime DESC,colSignTime DESC " }).Result;
            model.TrackInfoTable = Result.TrackInfoTable;
            model.SystemOrderNo = Id;
            model.TrackInfo = Result.TrackInfoTable.Rows[0]["colSignTime"].ToString().Substring(0,4)+"年"; //时间(年)
            model.MkOrderNo = Result.TrackInfoTable.Rows[0]["colTrackInfo"].ToString().ToUpper() == "SIGNED"?("已签收 签收人："+Result.TrackInfoTable.Rows[0]["colSignName"].ToString()+""):"运输中";//最新的物流信息状态 不等于SIGNED 就还在运输
            return View(model);
        }

        /// <summary>
        /// 物流跟踪信息查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MaryKayLoginsticsTrackInfo(MaryKayTrackInfoModel model)
        {
          
            var Result = new MaryKayService().GetMaryKayGetTrackInfo(new GetMaryKayTrackInfoRequest() { SqlWhere = GetTrackConditionSqlWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = 0 }).Result;
            model.TrackInfoTable = Result.TrackInfoTable;
            model.PageIndex = Result.PageIndex;
            model.PageSize = Result.PageSize;
            model.PageCount = Result.PageCount;
            return View(model);
        }

        /// <summary>
        /// 物流跟踪信息查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MaryKayLoginsticsTrackInfo(MaryKayTrackInfoModel model, int? PageIndex)
        {
            //导出Excel
            if (model.IsExport)
            {
                return MaryKayTrackExport(model);
            }
            else
            {
                var Result = new MaryKayService().GetMaryKayGetTrackInfo(new GetMaryKayTrackInfoRequest() { SqlWhere = GetTrackConditionSqlWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
                model.TrackInfoTable = Result.TrackInfoTable;
                model.PageIndex = Result.PageIndex;
                model.PageSize = Result.PageSize;
                model.PageCount = Result.PageCount;
                return View(model);
            }
        }

           
        /// <summary>
        /// 导出玫琳凯物流跟踪信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult MaryKayTrackExport(MaryKayTrackInfoModel model)
        {
            string SQL = this.GetTrackConditionSqlWhere(model);
            string ReportName = "MaryKayTrank" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            DataTable Exprottable = new MaryKayService().MaryKayExportTrackInfo(new GetMaryKayTrackInfoRequest() { SqlWhere = SQL }).Result.TrackInfoTable;
          

            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            //生成Excel 
            //excelHelper.ExportExcel2(Exprottable, fileFullPath); //速度很慢  方便导入
            //excelHelper.WriteExcel(Exprottable, fileFullPath);   //速度最快 导出后不能再导入
            //NopiExcelHelper.x2007.TableToExcelForXLSX(Exprottable, fileFullPath); //速度适中 超过10万以上会内存溢出
            ExportDataToExcelHelper.ExportDataSetToExcel(Exprottable, "MaryLayExportPods", "");

            string mimeType = "application/msexcel";
           // FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return new EmptyResult();
        }

        /// <summary>
        /// 物流跟踪信息查询条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetTrackConditionSqlWhere(MaryKayTrackInfoModel model)
        {
            string sql = "";

            //MK订单号 支持多个查询 以逗号隔开
            if (!string.IsNullOrEmpty(model.MkOrderNo))
            {
                sql += " AND A.CustomerOrderNo IN (SELECT col FROM   dbo.f_splitSTR('" + model.MkOrderNo.Replace(" ", "") + "',CHAR(13)+CHAR(10))) ";
            }

            //快递单号（运单号）
            if (!string.IsNullOrEmpty(model.ExpressOrderNo))
            {
                sql += " AND A.DeliveryNo IN ( SELECT col FROM  dbo.f_splitSTR('" + model.ExpressOrderNo.Replace(" ", "") + "',CHAR(13)+CHAR(10)))";
            }

            //订单创建时间时间
            if (!string.IsNullOrEmpty(model.BeginTrackDateTime.ToString()))
            {
                sql += " AND A.colCreateTime>='" + model.BeginTrackDateTime.DateTimeToString() + "'";
            }

            if (!string.IsNullOrEmpty(model.EndTrackDateTime.ToString()))
            {
                sql += " AND  A.colCreateTime<='" + model.EndTrackDateTime.DateTimeToString() + " 23:59:59'";
            }

            if (!string.IsNullOrEmpty(model.TrackInfoTypeID))
            {
                sql += " AND B.colTrackInfo='" + model.TrackInfoTypeID + "'";
            }

            return sql;
        }

        [HttpGet]
        public ActionResult MaryKaySynchroLogisticInfo(int? id)
        {
            return View();
        }

        /// <summary>
        /// 更新物流跟踪信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MaryKayLogisticTrancUpdate(int id)
        {
            ViewBag.Message = "修改物流跟踪信息";
            MaryKayLoginsticsTrackInfoModel myModel = new MaryKayLoginsticsTrackInfoModel();
            var Result = new MaryKayService().GetGetMaryKayGetTrackDetail(new GetMaryKayTrackInfoRequest() { SqlWhere = "SELECT * FROM tbl_PODTrack WHERE colID='" + id + "'" }).Result;
            DataTable myDt = Result.TrackInfoTable;
            var type = new List<SelectListItem>();
            if (myDt != null && myDt.Rows.Count > 0)
            {
                foreach (DataRow dr in myDt.Rows)
                {
                    myModel.colID = dr["colID"].ToString();   
                    myModel.colOrderNo = dr["colOrderNo"].ToString(); //系统订单号
                    myModel.colIsNormal = dr["colIsNormal"].ToString(); //是否异常
                    myModel.colProvince = dr["colProvince"].ToString(); //所在省份
                    myModel.colResponsibilityOwner = dr["colResponsibilityOwner"].ToString(); //责任归属
                    myModel.colSignName = dr["colSignName"].ToString(); //签收人
                    myModel.colSignTime = dr["colSignTime"].ToString(); //签收时间
                    myModel.colTrackTime = dr["colTrackTime"].ToString(); //跟踪时间
                    myModel.colTrackComment = dr["colTrackComment"].ToString(); //跟踪备注
                    myModel.colTrackInfo = dr["colTrackInfo"].ToString().ToUpper().Trim();//跟踪信息
                    

                    myModel.colTransStatus = dr["colTransStatus"].ToString(); //跟踪状态
                    myModel.colUpdater = dr["colUpdater"].ToString(); //更新人
                    myModel.colUpdateTime = dr["colUpdateTime"].ToString(); //更新时间
                    myModel.colGoodsStatus = dr["colGoodsStatus"].ToString();
                    myModel.colDelivery = dr["colDelivery"].ToString(); //快递运单号
                    myModel.colCustOrderNo = dr["colCustOrderNo"].ToString(); //MK订单号
                    myModel.colCreateTime = dr["colCreateTime"].ToString();//系统创建时间
                    myModel.colCreater = dr["colCreater"].ToString(); //创建人
                    myModel.colCity = dr["colCity"].ToString();//所在城市
                    myModel.colActualNoReceive = dr["colActualNoReceive"].ToString();

                }
                
            }

            return View(myModel);
        }

        /// <summary>
        /// 更新物流跟踪信息
        /// </summary>
        /// <param name="myModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MaryKayLogisticTrancUpdate(MaryKayLoginsticsTrackInfoModel myModel)
        {     
            try
            {
                var Result = new MaryKayService().UpdateMaryKayGetTrackInfo(new GetMaryKayTrackInfoRequest() { SqlWhere = "UPDATE tbl_PODTrack SET colTrackTime='" + myModel.colTrackTime + "',colCreateTime='" + myModel.colCreateTime + "',colSignTime='" + myModel.colSignTime + "',colTrackInfo='" + myModel.colTrackInfo + "',colTrackComment='" + myModel.colTrackComment + "',colResponsibilityOwner='" + myModel.colResponsibilityOwner + "' WHERE colID='" + myModel.colID + "'" }); 
                if(Result.IsSuccess)
                    ViewBag.Message = "更新成功";
                else
                    ViewBag.Message = "更新失败";
            }
            catch 
            {
                ViewBag.Message = "更新失败";
            }
            return View(myModel);
        }

        /// <summary>
        /// 玫琳凯跟踪信息导入覆盖更新
        /// </summary>
        /// <returns></returns>
         [HttpGet]
        public ActionResult MaryKayLogisticImport(int? id)
        {
            return View();
        }
  

        /// <summary>
        /// 玫琳凯跟踪信息导入覆盖更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
         public string MaryKayLogisticImport()
        {
            StringBuilder strSql = new StringBuilder();
            MaryKayService mks = new MaryKayService();
            ExcelHelper excelHelper = new ExcelHelper();
            int SuccessCount = 0;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataTable dt = GetDataFromExcel(hpf).Tables[0];

                    if (dt != null&&dt.Rows.Count>0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            strSql.Append("  UPDATE tbl_PODTrack SET colTrackTime='" + dr["跟踪时间"].ToString() + "',colCreateTime='" + dr["系统创建时间"].ToString() + "',colSignTime='" + dr["签收时间"].ToString() + "',colTrackInfo='" + dr["跟踪信息"].ToString() + "',colTrackComment='" + dr["跟踪备注"].ToString() + "',colResponsibilityOwner='" + dr["责任归属"].ToString() + "' WHERE colID='" + dr["物流信息表Id"].ToString() + "'   \n      ");
                        }
                         //执行更新sql
                        SuccessCount = mks.SynchroLogisticInfo(strSql.ToString());
                        return new { result = "共同步数据 <span style='color:red;' >" + dt.Rows.Count + "</span>条数据 </br> 同步成功 <span style='color:red;' >" + SuccessCount + "</span>条", IsSuccess = true }.ToJsonString();
                    }
                    return new { result = "数据库存储出错！", IsSuccess = false }.ToJsonString();
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

     
        /// <summary>
        /// 玫琳凯运单信息导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string MaryKaySynchroLogisticInfo()
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder successSB = new StringBuilder();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null)
                    {
                        successSB.Append("<table><thead>")
                                 .Append("<tr><th>运单号")
                                 .Append("</th><th>订单号")
                                 .Append("</th></tr></thead><tbody>");
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count>0)
                            {
                                foreach (DataRow dr in ds.Tables[i].Rows)
                                {
                                    //排除为空的行
                                    if (dr["运单号"].ToString().Trim().Length > 0 && dr["订单号"].ToString().Trim().Length > 0)
                                    {
                                        successSB.Append(" <tr><td> " + dr["运单号"].ToString().Trim() + " </td><td> " + dr["订单号"].ToString().Trim() + " </td></tr> ");
                                        //将运单号根据订单号分别更新到表 tbl_POD 和 tbl_PODTrack 
                                        strSql.Append("   UPDATE  dbo.tbl_POD  SET DeliveryNo='" + dr["运单号"].ToString().Trim() + "',colUpdateTime=GETDATE() WHERE CustomerOrderNo='" + dr["订单号"].ToString().Trim() + "'   \n      ");
                                        //strSql.Append("   UPDATE  dbo.tbl_PODTrack  SET colDelivery='" + dr["运单号"].ToString().Trim() + "' WHERE colCustOrderNo='" + dr["订单号"].ToString().Trim() + "'   \n      ");
                                    }
                                }
                            }
                        }
                        successSB.Append("</tbody></table>");

                        int success = 0;
                        if (strSql.ToString().Trim().Length > 0)
                        {
                            MaryKayService mks = new MaryKayService();
                            success=mks.SynchroLogisticInfo(strSql.ToString());
                        }

                        return new { result = successSB.ToString(), IsSuccess = true, count = success }.ToJsonString();
                    }
                    return new { result = "数据库存储出错！", IsSuccess = false }.ToJsonString();
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        /// <summary>
        /// 删除物流跟踪信息
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteOrderNo(string Ids)
        {
            MaryKayService myMaryKayService = new MaryKayService();
            bool value;
            try
            {
                value = myMaryKayService.DeleteOrderNo(new GetMaryKayTrackInfoRequest() { ID = Ids.Replace("\"", "") });
            }
            catch
            {
                value = false;
            }
            return value;
        }

        /// <summary>
        /// 获取上传Excel文件 转为DataSet
        /// </summary>
        /// <param name="hpf"></param>
        /// <returns></returns>
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
        /// 元智HTTP接口
        /// </summary>
        /// <param name="CustomerOrderNo"></param>
        /// <returns>String_POD</returns>
        public string GetPODInfoStr_HTTP(int id)
        {
            StringBuilder strPOD = new StringBuilder();
            //List<tbl_PODInfo> listpod = dal.GetPODInfo_HTTP(CustomerOrderNo);

            GetMaryKayOrderIssuedRequest request = new GetMaryKayOrderIssuedRequest() { ID = id };
            DataTable Table = new MaryKayService().GetHttpYZ(request).Result.YZTable;

            strPOD.Append("<RequestOrder>");
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                strPOD.Append("<MKOrderDate>" + Table.Rows[i]["签收时间"].ToString() + "</MKOrderDate>");
                strPOD.Append("<Lot>" + Table.Rows[i]["批次号"].ToString() + "</Lot>");
                strPOD.Append("<OPPCode>" + Table.Rows[i]["OOP编号"].ToString() + "</OPPCode>");
                strPOD.Append("<OPPName>" + Table.Rows[i]["OPP名称"].ToString() + "</OPPName>");
                strPOD.Append("<Province>" + Table.Rows[i]["省"].ToString() + "</Province>");
                strPOD.Append("<City>" + Table.Rows[i]["市"].ToString() + "</City>");
                strPOD.Append("<PostCode>" + Table.Rows[i]["邮政编码"].ToString() + "</PostCode>");
                strPOD.Append("<Address>" + Table.Rows[i]["地址"].ToString() + "</Address>");
                strPOD.Append("<Telephone>" + Table.Rows[i]["电话"].ToString() + "</Telephone>");
                strPOD.Append("<DeiveryComment>" + Table.Rows[i]["备注"].ToString() + "</DeiveryComment>");
                strPOD.Append("<OrderNo>" + Table.Rows[i]["客户运单号"].ToString() + "</OrderNo>");
                strPOD.Append("<MKWebOrderNo>" + Table.Rows[i]["网上订单号"].ToString() + "</MKWebOrderNo>");
                strPOD.Append("<MKConsultID>" + Table.Rows[i]["顾问编号"].ToString() + "</MKConsultID>");
                strPOD.Append("<MKConsultName>" + Table.Rows[i]["顾问名称"].ToString() + "</MKConsultName>");
                strPOD.Append("<Name>" + Table.Rows[i]["收件人姓名"].ToString() + "</Name>");
                strPOD.Append("<IDCardType>" + Table.Rows[i]["收件人证件类型"].ToString() + "</IDCardType>");
                strPOD.Append("<IDCardNo>" + Table.Rows[i]["收件人证件号码"].ToString() + "</IDCardNo>");
                strPOD.Append("<NetWeight>" + Table.Rows[i]["订单重量(克)"].ToString() + "</NetWeight>");
                strPOD.Append("<OrderAmount>" + Table.Rows[i]["订单金额"].ToString() + "</OrderAmount>");
                strPOD.Append("<TotalCartonQty>" + Table.Rows[i]["小箱个数"].ToString() + "</TotalCartonQty>");
                strPOD.Append("<MKBigCartonNo>" + Table.Rows[i]["大箱号"].ToString() + "</MKBigCartonNo>");
            }
            strPOD.Append("</RequestOrder>");
            return strPOD.ToString();

        }
        /// <summary>
        /// 韵达接口
        /// </summary>
        /// <param name="CustomerOrderNo"></param>
        /// <returns>String_POD</returns>
        public string getxmldate(int  id)
        {
            StringBuilder strPOD = new StringBuilder();
            //List<tbl_PODInfo> listpod = dal.GetPODInfo_HTTP(CustomerOrderNo);
            GetMaryKayOrderIssuedRequest request = new GetMaryKayOrderIssuedRequest() { ID = id };
            DataTable Table = new MaryKayService().GetHttpYD(request).Result.YDTable;
            strPOD.Append("<orders>");
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                strPOD.Append("<order>");
                strPOD.Append("<orderid>" + Table.Rows[i]["发货单号"].ToString() + "</orderid>");
                strPOD.Append("<callback></callback>");
                strPOD.Append("<customerid>MK-沈阳</customerid>");
                strPOD.Append("<mailno></mailno>");
                strPOD.Append("<sender><name>物流</name>");
                strPOD.Append("<company>物流</company>");
                strPOD.Append("<city>沈阳市,苏家屯区</city>");
                strPOD.Append("<address>沙河街道,吴家屯西,鲍家村,中储物流中心院内3号库,万络物流,申通对面</address>");
                strPOD.Append("<postcode></postcode>");
                strPOD.Append("<phone></phone>");
                strPOD.Append("<mobile>13998380156</mobile></sender>");//发件人
                strPOD.Append("<receiver><name>" + Table.Rows[i]["顾问名称"].ToString() + "</name>");
                strPOD.Append("<company>" + Table.Rows[i]["收件人姓名"].ToString() + "</company>");
                strPOD.Append("<city>" + Table.Rows[i]["目的城市"].ToString() + "</city>");
                strPOD.Append("<address>" + Table.Rows[i]["地址"].ToString() + "</address>");
                strPOD.Append("<postcode></postcode>");
                strPOD.Append("<phone></phone>");
                strPOD.Append("<mobile>" + Table.Rows[i]["电话"].ToString() + "</mobile></receiver>");
                strPOD.Append("<sendstarttime>" + Table.Rows[i]["创建时间"].ToString() + "</sendstarttime>");
                //strPOD.Append("<sendendtime></sendendtime>");
                //strPOD.Append("<weight>" + Convert.ToInt32(Convert.ToInt32(listpod[i].TotalGrossWeight) / 1000).ToString() + "</weight>");
                //strPOD.Append("<size></size>");
                //strPOD.Append("<value></value>");
                //strPOD.Append("<freight></freight>");
                //strPOD.Append("<premium></premium>");
                //strPOD.Append("<other_charges></other_charges>");
                //strPOD.Append("<collection_currency></collection_currency>");
                //strPOD.Append("<collection_value></collection_value>");
                //strPOD.Append("<special>玫琳凯</special>");
                //strPOD.Append("<items><item><name></name>");
                //strPOD.Append("<number></number>");
                //strPOD.Append("<remark></remark> </item></items>");
                //strPOD.Append("<remark></remark>");
                strPOD.Append("</order>");
            }
            strPOD.Append("</orders>");
            return strPOD.ToString();

        }

        public string GetYundaError(string c) 
        {
            string value = "";
                                             if (c == "s00")
                                             {
                                                value = "未知错误";
                                             }
                                             else if (c == "s01")
                                             {
                                                value = "非法的合作商账户";
                                             }
                                            else if (c == "s02")
                                            {
                                                value = "非法的主机来源";
                                                
                                            }
                                            else if (c == "s03")
                                            {
                                                value = "非法的数据签名";
                                                
                                            }
                                            else if (c == "s04")
                                            {
                                                value = "非法的请求类型";
                                                
                                            }
                                            else if (c == "s05")
                                            {
                                                value = "非法的XML格式";
                                            }
                                            else if (c == "s06")
                                            {
                                                value = "非法的订单号";
                                            }
                                            else if (c == "s07")
                                            {
                                                value = "账户不具有访问本功能的权限";
                                               
                                            }
                                            else if (c == "s11")
                                            {
                                                value = "无效的指令操作";
                                               
                                            }
                                            else if (c == "s12")
                                            {
                                                value = "没有指定有效的查询条件";
                                              
                                            }
                                            else if (c == "s21")
                                            {
                                                value = "可更新的字段中数据内容一致，忽略更新";
                                               
                                            }
                                            else if (c == "s22")
                                            {
                                                value = "订单当前状态下不再允许取消";
                                                
                                            }
                                            else if (c == "s23")
                                            {
                                                value = "订单当前状态下不再允许修改";
                                               
                                            }
                                            else if (c == "s51")
                                            {
                                                value = "发件人信息不完整";
                                              
                                            }
                                            else if (c == "s52")
                                            {
                                                value = "收件人信息不完整";
                                               
                                            }
                                            else if (c == "s71")
                                            {
                                                value = "发件人所在地区服务已关闭";
                                               
                                            }
                                            else if (c == "s72")
                                            {
                                                value = "收件人所在地区服务已关闭";
                                                
                                            }
                                            else if (c == "s91")
                                            {
                                                value = "信息不完整";
                                               
                                            }
                                            else if (c == "s97")
                                            {
                                                value = "数据更新失败";
                                               
                                            }
                                            else if (c == "s98")
                                            {
                                                value = "数据保存失败";
                                              
                                            }
                                            else if (c == "s99")
                                            {
                                                value = "服务器错误";
                                                
                                            }
                                            else if (c == "e01")
                                            {
                                                value = "用户取消投递";
                                               
                                            }
                                            else if (c == "e02")
                                            {
                                                value = "用户恶意下单";
                                               
                                            }
                                            else if (c == "e03")
                                            {
                                                value = "黑名单客户";
                                                
                                            }
                                            else if (c == "e11")
                                            {
                                                value = "揽收地超服务范围";
                                              
                                            }
                                            else if (c == "e12")
                                            {
                                                value = "派送地超服务范围";
                                                
                                            }
                                            else if (c == "e13")
                                            {
                                                value = "揽收预约时间超范围，无法协商";
                                                
                                            }
                                            else if (c == "e14")
                                            {
                                                value = "揽收地址错误或不详";
                                               
                                            }
                                            else if (c == "e15")
                                            {
                                                value = "派送地址错误或不详";
                                                
                                            }
                                            else if (c == "e16")
                                            {
                                                value = "多次联系，无法联系上发货方";
                                              
                                            }
                                            else if (c == "e17")
                                            {
                                                value = "上门后用户不接受价格";
                                               
                                            }
                                            else if (c == "e18")
                                             {
                                                 value = "虚假揽货电话（客户电话与联系人不符）";
                                               
                                            }
                                            else if (c == "e19")
                                            {
                                                value = "托寄物品为禁限寄品";
                                              
                                            }
                                            else if (c == "e20")
                                            {
                                                value = "托寄物品超规格";
                                              
                                            }
                                            else if (c == "e21")
                                            {
                                                value = "无法联系上收件人";
                                               
                                            }
                                            else if (c == "e22")
                                            {
                                                value = "用户包装问题，取消投递";
                                                
                                            }
                                            else if (c == "e52")
                                            {
                                                value = "错误收件人联系方式及地址";

                                            }
                                            else if (c == "e53")
                                            {
                                                value = "收件人拒收（未验货）";
                                               
                                            }
                                            else if (c == "e54")
                                            {
                                                value = "收件人拒收（验货，货不对款）";
                                           
                                            }
                                            else if (c == "e55")
                                            {
                                                value = "收件人拒收（因拖寄物品破损）";
                                                
                                            }
                                            else if (c == "e56")
                                            {
                                              
                                                value = "收件人拒收（代收货款价格不对）";
                                            }
                                            else if (c == "e57")
                                            {
                                                value = "收件人拒付或仅愿意部分支付";
                                              
                                            }
                                            else if (c == "e81")
                                            {
                                                value = "托寄物品丢失";
                                               
                                            }
                                           
                                            else if (c == "e99")
                                            {
                                                value = "其他原因";
                                             
                                            }
                                             return value;
                                          
        }

        public string HttpPostToYz(string xmlString, string pInfo)
        {
            try
            {
                WebRequest req = WebRequest.Create(pInfo);

                string xmlValue = HttpUtility.UrlEncode(xmlString, Encoding.UTF8);
                //   string signValue = HttpUtility.UrlEncode(signXml, Encoding.UTF8);
                byte[] bytes = Encoding.ASCII.GetBytes(xmlString);
                //                req.Headers.Add("Accept-Encoding:gzip, deflate");
                req.Headers.Add("Accept-Language:zh-cn,en-us");
                req.Timeout = 30000;
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded;charset=UTF8";
                req.ContentLength = bytes.Length;

                Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();


                WebResponse resp = req.GetResponse();
                if (resp == null)
                    return null;
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        /// <summary>
        /// 订单下发
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PODISsued(string array)
        {
            Pod pi = new Pod();
            StringBuilder sb = new StringBuilder();
            MaryKayService marykayservice = new MaryKayService();
            GetMaryKayOrderIssuedRequest request = new GetMaryKayOrderIssuedRequest();
            List<long> IDs = new List<long>();
            string req = "";
            string existence = "";
            string xmlError = "";
            string TimeNow = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int TrueCount = 0;
            request.InterfaceLog = new MaryKay_InterfaceLog();
            //首先获取所有被选中的Checkbox的ID值
            string sIDList = JsonToTable(array);
            if (sIDList == "")
            {
                return Json(new { Message = "请选择要下发的订单", IsSuccess = false });
               
            }
            else
            {
                string[] sid = sIDList.Split(',');
                //循环所获得的Checkbox
                for (int i = 0; i < sid.Length; i++)
                {
                    try
                    {
                        if (sid[i] != "")
                        {
                            request.ID = Convert.ToInt32(sid[i]);
                            pi = new MaryKayService().GetOrderNoIssuedInfoByID(request).Result.PODEntity;
                            string CustomerOrderNo = pi.CustomerOrderNumber.Trim();
                            string postData = this.GetPODInfoStr_HTTP(request.ID);
                            string ShipperName = pi.ShipperName.ToString().Trim();

                            #region 山东海虹物流
                            //if (ShipperName.Trim() == "山东海虹物流")
                            //{
                            //    request.InterfaceLog.EventType = "POSTHWPOD";
                            //    string tValue = "lcdata=" + HttpUtility.UrlEncode(postData) + "&sdata=" + "qdhh";
                            //    //req = podbll.HttpPostToYz(tValue, "http://218.59.33.74:5555/srpartner/MK/Order.aspx");
                            //    req = this.HttpPostToYz(tValue, "http://119.167.223.86:6688/MK/Order.aspx");
                            //    //req = podbll.SendMessageToYZ("http://www.yjkd.com/hdweb/yjk.aspx", tValue);
                            //    //string strRequst = req.Substring(req.LastIndexOf("<Success>"), req.IndexOf("</Success>"));
                            //    XmlDocument xd = new XmlDocument();
                            //    xd.LoadXml(req);
                            //    string Operate = xd.SelectSingleNode("OrderResponse").SelectSingleNode("Operate").InnerText;
                            //    if (Operate.ToLower().Trim() == "p1")
                            //    {
                            //        request.OrderNoIssuedStatus = "同步成功";
                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //        request.InterfaceLog.LogDetails = "订单下发成功:" + postData;
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);
                            //        IDs.Add(request.ID);
                            //        TrueCount++;
                            //    }
                            //    if (Operate.ToLower().Trim() == "p2")
                            //    {
                            //        request.OrderNoIssuedStatus = "同步失败";
                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //        request.InterfaceLog.LogDetails = "订单下发失败:" + postData;
                            //        request.InterfaceLog.EventType = "POSTHWPOD_Error";
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request); 
                            //    }
                            //    if (Operate.ToLower().Trim() == "p3")
                            //    {
                            //        request.OrderNoIssuedStatus = "同步失败";
                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //        existence = existence + CustomerOrderNo + ",";
                            //        request.InterfaceLog.LogDetails = "订单已存在:" + CustomerOrderNo + postData;
                            //        request.InterfaceLog.EventType = "POSTHWPOD_Error";
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);
                            //    }
                            //    if (Operate.ToLower().Trim() == "p0")
                            //    {
                            //        request.OrderNoIssuedStatus = "同步失败";

                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);

                            //        xmlError = xmlError + CustomerOrderNo + ",";
                            //        request.InterfaceLog.LogDetails = "XML错误:" + CustomerOrderNo + postData;
                            //        request.InterfaceLog.EventType = "POSTHWPOD_Error";
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);


                            //    }
                            //    if (Operate.ToLower().Trim() != "p0" && Operate.ToLower().Trim() != "p1" 
                            //        && Operate.ToLower().Trim() != "p2" && Operate.ToLower().Trim() != "p3")
                            //    {
                            //        request.OrderNoIssuedStatus = "同步失败";
                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //        request.InterfaceLog.LogDetails = "未知错误:" + CustomerOrderNo + postData;
                            //        request.InterfaceLog.EventType = "POSTHWPOD_Error";
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);
                            //    }
                            //} 
                            #endregion

                            #region 韵达快递
                            //if (ShipperName == "韵达快递")
                            //{
                            //    //string URL = "http://orderdev.yundasys.com:10209/join/interface.php";//1

                            //    string URL = "http://join.yundasys.com/interface.php";//1
                            //    WebRequest myHttpWebRequest = WebRequest.Create(URL);
                            //    myHttpWebRequest.Method = "POST";
                            //    //string CustomerOrderNo2 = pi.CustomerOrderNumber.ToString().Trim();
                            //    string xmldate = this.getxmldate(request.ID);
                            //    string newdata = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmldate));
                            //    string md5data = newdata + "crm716566889281";
                            //    string hash = GetMd5Hash(MD5CryptoServiceProvider.Create(), md5data);
                            //    string postData2 = "partnerid=" + HttpUtility.UrlEncode("crm716566") + "&version=" + HttpUtility.UrlEncode("1.0") + "&request=" + HttpUtility.UrlEncode("data") + "&xmldata=" + HttpUtility.UrlEncode(newdata) + "&validation=" + HttpUtility.UrlEncode(hash);
                            //    byte[] byte1 = Encoding.UTF8.GetBytes(postData2);
                            //    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                            //    myHttpWebRequest.ContentLength = byte1.Length;
                            //    Stream newStream = myHttpWebRequest.GetRequestStream();
                            //    newStream.Write(byte1, 0, byte1.Length);
                            //    newStream.Close();
                            //    string retString = "";
                            //    HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                            //    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                            //    {
                            //        retString = sr.ReadToEnd();
                            //        req = retString;
                            //        string s = retString;
                            //        XmlDocument xd = new XmlDocument();
                            //        xd.LoadXml(req);
                            //        string b = xd.SelectSingleNode("response").SelectSingleNode("result").InnerXml;
                            //        //string b = s.Substring(18, 4);
                            //        if (b == "true")
                            //        {
                            //            s = xd.SelectSingleNode("response").SelectSingleNode("orders").SelectSingleNode("order").SelectSingleNode("result").InnerXml;
                            //            //s = s.Replace("<response><result>true</result><orders><order><callback></callback>", "");
                            //            //string s1 = s.Substring(9, s.IndexOf("</orderid>") - 9);
                            //            //string s2 = s.Substring(s.IndexOf("<result>") + 8, s.IndexOf("</result>") - 37);
                            //            if (s == "false")
                            //            {

                            //                //string c = s.Replace("<orderid>" + s1 + "</orderid><result>false</result><remark>", "");
                            //                //c = c.Replace("</remark></order></orders></response>", "");
                            //                string c = xd.SelectSingleNode("response").SelectSingleNode("orders").SelectSingleNode("order").SelectSingleNode("remark").InnerXml;

                            //                string errorvalue = this.GetYundaError(c);
                            //                request.OrderNoIssuedStatus = "同步失败";
                            //                marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //                request.InterfaceLog.LogDetails = errorvalue + ":" + CustomerOrderNo;
                            //                request.InterfaceLog.UserDef1 = xmldate;

                            //                marykayservice.AddMaryKayInterfaceLog(request);


                            //            }
                            //            else
                            //            {
                            //                request.OrderNoIssuedStatus = "同步成功";

                            //                marykayservice.UpdateOrderNoIsSuedStatus(request);

                            //                request.InterfaceLog.LogDetails = "订单下发成功:" + CustomerOrderNo;
                            //                request.InterfaceLog.UserDef1 = xmldate;
                            //                marykayservice.AddMaryKayInterfaceLog(request);
                            //                IDs.Add(request.ID);
                            //                TrueCount++;
                            //            }
                            //        }
                            //        else 
                            //        {
                            //            string n = xd.SelectSingleNode("response").SelectSingleNode("remark").InnerXml;
                            //            string errorvalue = this.GetYundaError(n);
                            //            request.OrderNoIssuedStatus = "同步失败";
                            //            marykayservice.UpdateOrderNoIsSuedStatus(request);
                            //            request.InterfaceLog.LogDetails = errorvalue + ":" + CustomerOrderNo;
                            //            request.InterfaceLog.UserDef1 = xmldate;
                            //            marykayservice.AddMaryKayInterfaceLog(request);
                            //        }
                            //   }
                            //} 
                            #endregion

                            #region 安达信物流
                            //if (ShipperName == "安达信物流")
                            //{
                            //    request.InterfaceLog.EventType = "POSTADX";
                            //    string tValue = HttpUtility.UrlEncode(postData, Encoding.UTF8);
                            //    req = this.HttpPostToYz(tValue, "http://203.171.230.228:8024/api.aspx ");
                            //    XmlDocument xd = new XmlDocument();
                            //    xd.LoadXml(req);
                            //    string Operate = xd.SelectSingleNode("return").SelectSingleNode("Result").InnerText;

                            //    if (Operate.ToLower().Trim() == "true")
                            //    {

                            //        request.OrderNoIssuedStatus = "同步成功";

                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);

                            //        request.InterfaceLog.LogDetails = "订单下发成功:" + postData;

                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);
                            //        IDs.Add(request.ID);
                            //        TrueCount++;
                            //    }

                            //    else
                            //    {

                            //        request.OrderNoIssuedStatus = "同步失败";

                            //        marykayservice.UpdateOrderNoIsSuedStatus(request);

                            //        request.InterfaceLog.LogDetails = "未知错误:" + postData;
                            //        request.InterfaceLog.EventType = "POSTHWPOD_Error";
                            //        request.InterfaceLog.UserDef1 = req;
                            //        marykayservice.AddMaryKayInterfaceLog(request);
                            //    }
                            //} 
                            #endregion

                        }
                    }
                    catch (SqlException e)
                    {
                       
                        return Json(new { Message = "订单下发失败！" + e.ToString(), IsSuccess = false });
                    }

                }

                //IEnumerable<long> IDs = Enumerable.Empty<long>();
               
                
                int Count = sIDList.Split(',').Length - 1;
                if (Count == TrueCount)
                {
                    return Json(new { Message = "订单下发完成！", IsSuccess = true, IDList = IDs });
                }
                else 
                {


                    return Json(new { Message = "订单存在" + (Count - TrueCount).ToString() + "票，下发失败！", IsSuccess = false, IDList = IDs });
                }

            }
        }

        /// <summary>
        /// POST提交 
        /// </summary>
        private string HTTPPost(string url, string postDataStr, string signStr)
        {
            string responseStr = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = @"POST";
            req.ContentType = "application/json;charset=UTF-8";
            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            WebResponse wr = req.GetResponse();
            Stream responseStream = wr.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        public ActionResult MaryKayTrackInfo(MaryKayTrackInfoModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult MaryKayTrackInfo(MaryKayTrackInfoModel model,int? PageIndex)
        {
            if (model.IsExport)
            {
                return GetMaryKayTrackExport(model);
            }
            else { 
            var Result = new MaryKayService().GetMaryKayGetTrack(new GetMaryKayTrackInfoRequest() { SqlWhere = GetTrackSqlWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            model.TrackInfoTable = Result.TrackInfoTable;
            model.PageIndex = Result.PageIndex;
            model.PageSize = Result.PageSize;
            model.PageCount = Result.PageCount;
            }
            return View(model);
            
        }

        public string GetTrackSqlWhere(MaryKayTrackInfoModel model)
        {
            string sql = "";

            


            if (!string.IsNullOrEmpty(model.MkOrderNo))
            {
                sql += " AND POD.CustomerOrderNumber='" + model.MkOrderNo + "'";
            }


            if (!string.IsNullOrEmpty(model.SystemOrderNo))
            {
                sql += " AND POD.SystemNumber='" + model.MkOrderNo + "'";
            }

            if (!string.IsNullOrEmpty(model.BeginTrackDateTime.ToString()))
            {
                sql += " AND PodTrack.DateTime2>='" + model.BeginTrackDateTime.ToString() + "'";
            }

            if (!string.IsNullOrEmpty(model.EndTrackDateTime.ToString()))
            {
                sql += " AND  PodTrack.DateTime2<='" + model.EndTrackDateTime.ToString() + "'";
            }


            if (model.EndCity != null && Convert.ToInt32(model.EndCityID) != 0)
            {
                sql += " and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + model.EndCityID + "))";
            }

            if (!string.IsNullOrEmpty(model.ExpressOrderNo))
            {
                sql += " AND POD.Str19='" + model.ExpressOrderNo + "'";
            }

            if (!string.IsNullOrEmpty(model.UpLoadStatusID))
            {
                if (model.UpLoadStatusID == "未上传")
                {
                    sql += " AND PodTrack.Str6 IS NULL";
                }
                else
                {
                    sql += " AND PodTrack.Str6='" + model.UpLoadStatusID + "'";
                }
            }
            


            if (!string.IsNullOrEmpty(model.TrackInfoTypeID))
            {
                sql += " AND PodTrack.Str3='" + model.TrackInfoTypeID + "'";
            }


            return sql;
        }


        public ActionResult DeleteTrackInfoByID(string array) 
        {
            string[] IDS = JsonToTable(array).Split(',');
            string IdList = "";
            for (int i = 0; i < IDS.Length;i++ )
            {
                if (IDS[i].ToString().Trim()=="")
                {
                    continue;
                }
                IdList +=  IDS[i].ToString().Trim() + ",";
            }


            bool Result = new MaryKayService().DeleteTrackInfoByID(new GetMaryKayTrackInfoRequest() { ID = IdList.Substring(0,IdList.Length-1)});

            if (Result)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else 
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
            

        }


        public ActionResult GetMaryKayTrackExport(MaryKayTrackInfoModel model)
        {
            string SQL = this.GetTrackSqlWhere(model);
            string ReportName ="MaryKay跟踪报表导出"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            DataTable Exprottable = new MaryKayService().GetMaryKayTrackExport(new GetMaryKayTrackInfoRequest() { SqlWhere = SQL}).Result.TrackInfoTable;
            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
            excelHelper.Dispose();

            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType, Url.Encode(ReportName));
        }
        
        public static readonly string MKAccount = ConfigHelper.GetConfigValue("MKAccount");
        public static readonly string MKPassword =  ConfigHelper.GetConfigValue("MKPassword");
        public static readonly string MKDatasecurity = ConfigHelper.GetConfigValue("MKDatasecurity");
        public static readonly string Trasactiontype = ConfigHelper.GetConfigValue("Trasactiontype");

        public ActionResult UpLoadMK(string array)
        {


            string[] IDS = JsonToTable(array).Split(',');
            string IdList = "";
            for (int i = 0; i < IDS.Length; i++)
            {
                if (IDS[i].ToString().Trim() == "")
                {
                    continue;
                }
                IdList += IDS[i].ToString().Trim() + ",";
            }

            if (IdList=="")
            {
                return Json(new { Message = "请先选择订单！", IsSuccess = false });
            }
            GetMaryKayTrackInfoRequest Request = new GetMaryKayTrackInfoRequest() { ID = IdList.Substring(0,IdList.Length-1) };
            DataTable MaryKayUploadMK = new MaryKayService().GetMaryKayTrackListInfoByIDS(Request).Result.TrackInfoTable;

            List<int> listIDS = new List<int>();
            string UpLoadMessage =  UploadToMK(MaryKayUploadMK, Trasactiontype, out listIDS);

            return Json(new { Message = UpLoadMessage, IsSuccess = true, IDS = listIDS });
        }

        /// <summary>
        /// 接收到订单跟踪就上传MK
        /// </summary>
        /// <param name="colCustOrderNo"></param>
        /// <param name="colSignTime"></param>
        /// <param name="strComment"></param>
        /// <param name="strTrasactiontype"></param>
        public string UploadToMK(DataTable UpLoadMKTable, string strTrasactiontype,out List<int> ListIDS)
        {
            MaryKayService marykayservice = new MaryKayService();
            GetMaryKayOrderIssuedRequest IssuedRequest = new GetMaryKayOrderIssuedRequest();
            GetMaryKayTrackInfoRequest UploadRequest = new GetMaryKayTrackInfoRequest();
            int TrueCount = 0;
            string Message = "";
            ListIDS = new List<int>();
            for (int i = 0; i < UpLoadMKTable.Rows.Count; i++)
            {
                CookieContainer mycookie = new CookieContainer();

                

                MaryKayHDtoLIPS.HDToLIPS servicemarykay = new MaryKayHDtoLIPS.HDToLIPS();
                int r;
                servicemarykay.CookieContainer = mycookie;
                int iReturn = servicemarykay.CheckUser(MKAccount, MKPassword);
                if (iReturn == 1)
                {

                    if (UpLoadMKTable.Rows[i]["跟踪信息"].ToString().Trim().ToUpper() == "SIGNED")
                    {
                        r = servicemarykay.HDToLIPSByWebService(UpLoadMKTable.Rows[i]["MK运单号"].ToString(), UpLoadMKTable.Rows[i]["跟踪时间"].ToString(), UpLoadMKTable.Rows[i]["Delivery"].ToString(), strTrasactiontype);
                        
                        
                    }
                    else 
                    {
                        r = servicemarykay.HDToLIPSByWebService(UpLoadMKTable.Rows[i]["MK运单号"].ToString(), UpLoadMKTable.Rows[i]["跟踪时间"].ToString(), UpLoadMKTable.Rows[i]["Delivery"].ToString(), "");
                    }
                    
                    if (r == 1)
                    {

                        UploadRequest.UpLoadMKStatus = "上传成功";
                        UploadRequest.PodTrackID = Convert.ToInt32(UpLoadMKTable.Rows[i]["ID"].ToString());
                        new MaryKayService().UpdateIsNormalByID(UploadRequest);

                        if (UpLoadMKTable.Rows[i]["跟踪信息"].ToString().Trim().ToUpper() == "SIGNED")
                        {
                            UploadRequest.CustomerOrderNumber = UpLoadMKTable.Rows[i]["MK运单号"].ToString().Trim();
                            UploadRequest.PODStatusID = 5;
                            UploadRequest.PODStatusName = "待结案";
                            new MaryKayService().UpdatePODStatusByCustomerOrderNumber(UploadRequest);
                        }

                        IssuedRequest.InterfaceLog = new MaryKay_InterfaceLog();
                        IssuedRequest.InterfaceLog.LogDetails = "上传MK成功:" + UpLoadMKTable.Rows[i]["MK运单号"].ToString() + "-" + UpLoadMKTable.Rows[i]["跟踪信息"].ToString();
                        IssuedRequest.InterfaceLog.EventType = "POSTMKPODTrack";
                        IssuedRequest.InterfaceLog.UserDef1 = "用户状态:" + iReturn + "上传状态:" + r;
                        marykayservice.AddMaryKayInterfaceLog(IssuedRequest);
                        TrueCount++;
                        ListIDS.Add(UploadRequest.PodTrackID);
                    }
                    else
                    {




                        UploadRequest.UpLoadMKStatus = "上传失败";
                        UploadRequest.PodTrackID = Convert.ToInt32(UpLoadMKTable.Rows[i]["ID"].ToString());
                        new MaryKayService().UpdateIsNormalByID(UploadRequest);
                        IssuedRequest.InterfaceLog = new MaryKay_InterfaceLog();
                        IssuedRequest.InterfaceLog.LogDetails = "上传MK失败:" + UpLoadMKTable.Rows[i]["MK运单号"].ToString() + "-" + UpLoadMKTable.Rows[i]["跟踪信息"].ToString();
                        IssuedRequest.InterfaceLog.EventType = "POSTMKPODTrack_false";
                        IssuedRequest.InterfaceLog.UserDef1 = "用户状态:" + iReturn + "上传状态:" + r;
                        marykayservice.AddMaryKayInterfaceLog(IssuedRequest);
                    }
                }
                else
                {


                    UploadRequest.UpLoadMKStatus = "上传失败";
                    UploadRequest.PodTrackID = Convert.ToInt32(UpLoadMKTable.Rows[i]["ID"].ToString());
                    new MaryKayService().UpdateIsNormalByID(UploadRequest);
                    IssuedRequest.InterfaceLog = new MaryKay_InterfaceLog();
                    IssuedRequest.InterfaceLog.LogDetails = "用户验证失败:";
                    IssuedRequest.InterfaceLog.EventType = "POSTMKPODTrack_error";
                    IssuedRequest.InterfaceLog.UserDef1 = "用户状态:" + iReturn;
                    marykayservice.AddMaryKayInterfaceLog(IssuedRequest);
                }
            }
           
            if (UpLoadMKTable.Rows.Count == TrueCount)
            {
                Message += "订单下发完成！";
            }
            else 
            {
                Message += "订单已下发但有" + (UpLoadMKTable.Rows.Count-TrueCount).ToString()+"单上传失败！";
            }
            return Message;
        }

        public ActionResult GetYUNDATrackInfo() 
        {
            try
            {
                DataTable ds = new MaryKayService().GetYundaOrderNoInfo().Result.TrackInfoTable;
                foreach (DataRow dr in ds.Rows)
                {
                    //string URL = "http://orderdev.yundasys.com:10209/join/query/xml.php";
                    string URL = "http://join.yundasys.com/interface.php";
                    WebRequest myHttpWebRequest = WebRequest.Create(URL);
                    myHttpWebRequest.Method = "POST";
                    string postData = "partnerid=crm716566&mailno=" + dr["CustomerOrderNumber"].ToString().Trim();
                    //string postData = "partnerid=test&mailno=1200000000001";
                    byte[] byte1 = Encoding.UTF8.GetBytes(postData);

                    myHttpWebRequest.ContentType = "multipart/form-data";
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    myHttpWebRequest.ContentLength = byte1.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(byte1, 0, byte1.Length);
                    newStream.Close();
                    string retString = "";
                    HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                    {
                        retString = sr.ReadToEnd();

                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(retString);
                        //xDoc.Load(@"E:\韵达接口");
                        string result = xDoc.GetElementsByTagName("result")[0].InnerXml;
                        if (result == "true")
                        {

                            string CustomerOrderNumber = xDoc.GetElementsByTagName("mailno")[0].InnerXml;//作为Mk运单号

                            string time = xDoc.GetElementsByTagName("time")[0].InnerXml;
                            string remark = xDoc.GetElementsByTagName("remark")[0].InnerXml;
                            string status = xDoc.GetElementsByTagName("status")[0].InnerXml;
                            string weight = xDoc.GetElementsByTagName("weight")[0].InnerXml;



                            GetMaryKayTrackInfoRequest trackRequest = new GetMaryKayTrackInfoRequest();

                            if (!string.IsNullOrEmpty(CustomerOrderNumber))
                            {

                                string colResponsibilityOwner = "YUNDA";//约定
                                //tbl_PODTrackBLL bll = new tbl_PODTrackBLL();
                                XmlNodeList xNodeList = xDoc.GetElementsByTagName("steps");

                                foreach (XmlNode xn1 in xNodeList)
                                {
                                    XmlNodeList xnl2 = xn1.ChildNodes;
                                    foreach (XmlNode xn2 in xnl2)
                                    {

                                        string Tracktimes = xn2["time"].InnerXml;//作为跟踪时间
                                        string address = xn2["address"].InnerXml;
                                        string station = xn2["station"].InnerXml;
                                        string station_phone = xn2["station_phone"].InnerXml;
                                        string TrackInfo = xn2["status"].InnerXml;//作为跟踪信息
                                        string TrackRemarks = xn2["remark"].InnerXml;//作为跟踪备注
                                        string next = xn2["next"].InnerXml;
                                        string next_name = xn2["next_name"].InnerXml;


                                        if (TrackInfo == "signed")
                                        {
                                            string signer = xn2["signer"].InnerXml;//作为签收人
                                            trackRequest.CustomerOrderNumber = CustomerOrderNumber;
                                            trackRequest.TrackInfo = TrackInfo;
                                            trackRequest.TrackComment = TrackRemarks;
                                            trackRequest.ResponsibilityOwner = colResponsibilityOwner;
                                            trackRequest.TrackTime = Convert.ToDateTime(Tracktimes);
                                            trackRequest.SignName = signer;
                                            trackRequest.CreateTime = DateTime.Now;
                                            trackRequest.Creator = base.UserInfo.DisplayName;

                                        }
                                        else
                                        {
                                            trackRequest.CustomerOrderNumber = CustomerOrderNumber;
                                            trackRequest.TrackInfo = TrackInfo;
                                            trackRequest.TrackComment = TrackRemarks;
                                            trackRequest.ResponsibilityOwner = colResponsibilityOwner;
                                            trackRequest.TrackTime = Convert.ToDateTime(Tracktimes);
                                            trackRequest.SignName = "";
                                            trackRequest.CreateTime = DateTime.Now;
                                            trackRequest.Creator = base.UserInfo.DisplayName;

                                        }

                                        new MaryKayService().AddYUNDATrackInfo(trackRequest);


                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch(Exception e)
            {
            
            }

            return Json(new { Message = "获取结束！", IsSuccess = true });
        }
    }
}

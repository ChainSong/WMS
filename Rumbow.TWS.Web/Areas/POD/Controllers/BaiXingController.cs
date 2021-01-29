using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MyFile = System.IO.File;
using Runbow.TWS.Common;
using Runbow.TWS.Biz.POD;

using Runbow.TWS.Web.Areas.AMS.Models;
using Runbow.TWS.MessageContracts.AMS;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Entity.POD.Nike;
using Runbow.TWS.MessageContracts.POD.Nike;

namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class BaiXingController : BaseController
    {
        private long _CustomerID = 40;

        //快递订单导入
        [HttpGet]
        public ActionResult PodImport()
        {
            return View();
        }

        [HttpPost]
        public string PodImport(string customer)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(uploadFolderPath, "AMSTempFile");
            string url = string.Empty, actualNameInServer = string.Empty, ext = string.Empty;
            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    ext = Path.GetExtension(hpf.FileName);
                    if (!ext.ToLower().Equals(".xls") && !ext.ToLower().Equals(".xlsx"))
                    {
                        return new { msg = "请选择Excel格式的文件" }.ToJsonString();
                    }
                    actualNameInServer = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    url = Path.Combine(targetPath, actualNameInServer);
                    hpf.SaveAs(url);
                    hpf.InputStream.Close();
                    Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(url);
                    DataSet ds = excelHelper.GetAllDataFromAllSheets();
                    excelHelper.Dispose();
                    MyFile.Delete(url);
                    IEnumerable<Pod> pods = null;
                    List<Pod> pp = new List<Pod>();
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder results = new StringBuilder();
                        BaiXingService bxService = new BaiXingService();
                        string str1, str37, str4, str5, str36, str10, str11, str38, str2, customerOrderNumber;
                        string systemNumber = "";
                        long podID = 0;
                        Double weight = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];
                            //调度运单号,异常收件原因,取件方联系人,取件方联系电话,取件方联系地址,到件方联系人,到件方联系电话,到件方联系地址
                            //到件方联系电话,货物名称,货物重量,订单备注,委托方联系人,委托方客户卡号
                            customerOrderNumber = row["委托方联系人"].ToString().Trim();  //客户订单号
                            if (string.IsNullOrEmpty(customerOrderNumber))
                            {
                                continue;
                            }
                            else
                            {
                                Runbow.TWS.MessageContracts.POD.GetBaiXingRequest bxRequest = new MessageContracts.POD.GetBaiXingRequest() { CustomerOrderNumber = customerOrderNumber };
                                Response<IEnumerable<Pod>> Podresponse = bxService.GetBaiXingPod(bxRequest);
                                if (Podresponse.IsSuccess && Podresponse.Result.Count() > 0)
                                {
                                    #region pod赋值
		                            //快递公司不是顺丰直接过滤
                                    if (!Podresponse.Result.First().Str17.Contains("顺丰"))
                                    {
                                        continue;
                                    }
                                    //对应的快递信息PODID
                                    if (Podresponse.Result.Count() > 1)
                                    {
                                        podID = Podresponse.Result.LastOrDefault().ID;
                                    }

                                    //PodID = Podresponse.Result.First().ID;
                                    systemNumber = Podresponse.Result.First().SystemNumber;
                                    try
                                    {
                                        weight = Convert.ToDouble(row["货物重量"].ToString().Trim());      //重量(公斤)
                                    }
                                    catch
                                    {
                                    }

                                    str1 = row["调度运单号"].ToString().Trim();      //快递单号
                                    str37 = row["异常收件原因"].ToString().Trim();  //异常说明
                                    str4 = row["取件方联系人"].ToString().Trim();      //发货方联系人
                                    str5 = row["取件方联系电话"].ToString().Trim();    //发货方电话
                                    str36 = row["取件方联系地址"].ToString().Trim();    //发货方详细地址
                                    str10 = row["到件方联系人"].ToString().Trim();      //收货方联系人
                                    str11 = row["到件方联系电话"].ToString().Trim();    //收货方电话
                                    str38 = row["到件方联系地址"].ToString().Trim();    //收货方详细地址
                                    str2 = row["货物名称"].ToString().Trim();      //商品品名
                                    //str1 = row["订单备注"].ToString().Trim();      //备注信息
                                    //str1 = row["委托方客户卡号"].ToString().Trim(); //委托方客户卡号
                                    //p.CustomerID = 40;

                                    Pod p = new Pod()
                                    {
                                        ID = podID,
                                        ProjectID = 1,
                                        SystemNumber = systemNumber + "-1",
                                        CustomerOrderNumber = customerOrderNumber,
                                        CustomerID = Podresponse.Result.First().CustomerID, //同一客户，分单机制
                                        CustomerName = Podresponse.Result.First().CustomerName,
                                        ActualDeliveryDate = Podresponse.Result.First().ActualDeliveryDate,
                                        StartCityID = 1,//Podresponse.Result.First().StartCityID,
                                        StartCityName = Podresponse.Result.First().StartCityName,
                                        EndCityID = 2,//Podresponse.Result.First().EndCityID,
                                        EndCityName = Podresponse.Result.First().EndCityName,
                                        PODStateID = Podresponse.Result.First().PODStateID,
                                        PODStateName = Podresponse.Result.First().PODStateName,
                                        ShipperTypeID = 36,
                                        ShipperTypeName = "快递",
                                        BoxNumber = Podresponse.Result.First().BoxNumber,
                                        Weight = weight,
                                        Volume = Podresponse.Result.First().Volume,
                                        GoodsNumber = Podresponse.Result.First().GoodsNumber,
                                        Creator = "bxadmin",
                                        CreateTime = DateTime.Now,
                                        Str1 = str1, //快递运单号
                                        Str2 = str2,//商品品名
                                        Str3 = Podresponse.Result.First().Str3,//交易金额
                                        Str15 = Podresponse.Result.First().Str15,//实际快递费用
                                        Str4 = str4,//发货联系人
                                        Str5 = str5,//发货电话
                                        Str6 = Podresponse.Result.First().Str6,//发货省
                                        Str7 = Podresponse.Result.First().Str7,//发货市
                                        Str8 = Podresponse.Result.First().Str8,//发货区
                                        Str36 = str36,//发货详细地址
                                        Str10 = str10,//收货联系人
                                        Str11 = str11,//收货电话
                                        Str12 = Podresponse.Result.First().Str12,//收货省
                                        Str13 = Podresponse.Result.First().Str13,//收货市
                                        Str14 = Podresponse.Result.First().Str14,//收货区
                                        Str38 = str38,//收货详细地址
                                        Str16 = Podresponse.Result.First().Str16,//投保
                                        Str17 = Podresponse.Result.First().Str17,//podRequest.courier,//快递公司
                                        Str18 = Podresponse.Result.First().Str18,//预估快递费用
                                        Str19 = Podresponse.Result.First().Str19,//来源应用
                                        Str37 = string.IsNullOrEmpty(str37) ? row["订单备注"].ToString().Trim() : str37,//备注信息
                                        PODTypeID = 7,
                                        PODTypeName = "出货运单",
                                        TtlOrTplID = 26,
                                        TtlOrTplName = "LTL",
                                        Type = 1,//分单标识(默认为2)
                                        Str20 = null,
                                    };
	                                #endregion;

                                    pp.Add(p);
                                }                                                           
                            }
                        }
                        var response = new BaiXingService().AddPods_BX(new AddPodsRequest() { Pods = pp });
                        if (response.IsSuccess)
                        {
                            StringBuilder orderNumbers = new StringBuilder();
                            foreach (PodKey pk in response.Result)
                            {
                                orderNumbers.Append(pk.CustomerOrderNumber + ",");
                            }
                            results.Append("成功导入" + response.Result.Count() + "条，百姓运单号：" + orderNumbers.ToString());
                        }
                        else
                        {
                            results.Append("导入失败，请联系管理员.");
                        }
                        return new { result = results.ToString(), IsSuccess = true }.ToJsonString();
                    }
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        //百姓订单查询导出
        [HttpGet]
        public ActionResult Index(int? PageIndex)
        {
            BaiXingViewModel nm = new BaiXingViewModel();
            return View(nm);
        }

        [HttpPost]
        public ActionResult Index(BaiXingViewModel vm, int? PageIndex)
        {
            if (vm.IsForExport)
            {
                var podAlls = new BaiXingService().ExportAll(new QueryPodRequest()
                {
                    SearchCondition = vm.SearchCondition,
                    ProjectID = base.UserInfo.ProjectID,
                    PageIndex = PageIndex ?? 0,
                    PageSize = 10000
                });
                return this.ExportBaiXingPodsToExcel(podAlls);
            }
            vm.SearchCondition.CustomerID = 40;
            var result = new BaiXingService().QueryPod(new QueryPodRequest() { PageSize = 100, PageIndex = PageIndex ?? 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.PodCollection = result.PodCollections;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;

            return View(vm);
        }


        /// <summary>
        /// 百姓网导出报表
        /// </summary>
        private ActionResult ExportBaiXingPodsToExcel(DataTable Pods)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("委托方客户卡号*", typeof(string));
            dtPod.Columns.Add("取件方联系人*", typeof(string));
            dtPod.Columns.Add("取件方联系电话*", typeof(string));
            dtPod.Columns.Add("取件方联系地址*", typeof(string));
            dtPod.Columns.Add("取件方城市代码*", typeof(string));
            dtPod.Columns.Add("到件方联系人*", typeof(string));
            dtPod.Columns.Add("到件方联系电话*", typeof(string));
            dtPod.Columns.Add("到件方联系地址*", typeof(string));
            dtPod.Columns.Add("委托方联系人*", typeof(string));
            dtPod.Columns.Add("委托方手机*", typeof(string));
            dtPod.Columns.Add("委托方联系地址*", typeof(string));
            dtPod.Columns.Add("委托方城市代码*", typeof(string));
            dtPod.Columns.Add("寄件类型*", typeof(string));
            dtPod.Columns.Add("物品名称*", typeof(string));
            dtPod.Columns.Add("付款方式*", typeof(string));
            dtPod.Columns.Add("订单备注", typeof(string));
            dtPod.Columns.Add("短信备注", typeof(string));
            dtPod.Columns.Add("付款方月结卡号", typeof(string));
            dtPod.Columns.Add("付款方公司名称", typeof(string));
            dtPod.Columns.Add("付款方联系人", typeof(string));
            dtPod.Columns.Add("付款方联系电话", typeof(string));
            dtPod.Columns.Add("取件方公司名称", typeof(string));
            dtPod.Columns.Add("取件方客户卡号", typeof(string));
            dtPod.Columns.Add("到件方公司名称", typeof(string));
            dtPod.Columns.Add("到件方客户卡号", typeof(string));
            dtPod.Columns.Add("到件方税号", typeof(string));
            dtPod.Columns.Add("委托方公司名称", typeof(string));
            dtPod.Columns.Add("委托方座机", typeof(string));
            dtPod.Columns.Add("委托方传真", typeof(string));
            dtPod.Columns.Add("委托方E-mail", typeof(string));
            dtPod.Columns.Add("地址是否保密", typeof(string));
            dtPod.Columns.Add("是否保价", typeof(string));
            dtPod.Columns.Add("预约取件时间", typeof(string));
            dtPod.Columns.Add("声明/申报价值", typeof(string));
            dtPod.Columns.Add("币种", typeof(string));
            dtPod.Columns.Add("数量（件）", typeof(string));
            dtPod.Columns.Add("重量（KG）", typeof(string));
            dtPod.Columns.Add("货物规格长（cm）", typeof(string));
            dtPod.Columns.Add("货物规格宽（cm）", typeof(string));
            dtPod.Columns.Add("货物规格高（cm）", typeof(string));

            #endregion

            #region 赋值
            //Pods.PodPod.OrderBy(p => p.ID).Each((i, p) =>
            //{
            //    DataRow dr = dtPod.NewRow();
            //    dr["委托方客户卡号*"] = "70001201606220001"; //月结账号(顺丰提供)
            //    dr["取件方联系人*"] = p.Str4;
            //    dr["取件方联系电话*"] = p.Str5;
            //    dr["取件方联系地址*"] = p.Str6 + p.Str7 + p.Str8 + p.Str36;//p.Str6 + p.Str7 + p.Str8
            //    dr["取件方城市代码*"] = "";//p.Str13
            //    dr["到件方联系人*"] = p.Str10;
            //    dr["到件方联系电话*"] = p.Str11;
            //    dr["到件方联系地址*"] = p.Str12 + p.Str13 + p.Str14 + p.Str38;//p.Str12 + p.Str13 + p.Str14
            //    dr["委托方联系人*"] = p.CustomerOrderNumber; //百姓网订单号
            //    dr["委托方手机*"] = "13524407929"; //客服号码
            //    dr["委托方联系地址*"] = "上海市闵行区七莘路1839号财富108广场南座20楼";//地址
            //    dr["委托方城市代码*"] = "021";
            //    dr["寄件类型*"] = "快件";
            //    dr["物品名称*"] = p.Str2;
            //    dr["付款方式*"] = "转第三方月结";
            //    dr["订单备注"] = "";
            //    dr["短信备注"] = "";
            //    dr["付款方月结卡号"] = "70001201606220001";//月结账号(顺丰提供)
            //    dr["付款方公司名称"] = "上海物流科技股份有限公司";//顺丰提供
            //    dr["付款方联系人"] = "dana.chen";//顺丰提供
            //    dr["付款方联系电话"] = "021-54431003";//顺丰提供
            //    dr["取件方公司名称"] = "";
            //    dr["取件方客户卡号"] = "";
            //    dr["到件方公司名称"] = "";
            //    dr["到件方客户卡号"] = "";
            //    dr["到件方税号"] = "";
            //    dr["委托方公司名称"] = "上海物流科技股份有限公司";
            //    dr["委托方座机"] = "021-54431003";
            //    dr["委托方传真"] = "021-54431002";
            //    dr["委托方E-mail"] = "dana.chen@runbow.com.cn";
            //    dr["地址是否保密"] = "";
            //    dr["是否保价"] = "";
            //    dr["预约取件时间"] = "";
            //    dr["声明/申报价值"] = "";
            //    dr["币种"] = "";
            //    dr["数量（件）"] = "";
            //    dr["重量（KG）"] = "";
            //    dr["货物规格长（cm）"] = "";
            //    dr["货物规格宽（cm）"] = "";
            //    dr["货物规格高（cm）"] = "";


            //    dtPod.Rows.Add(dr);
            //});
            #endregion

            try
            {
                Pods.Columns["CreateTime"].ColumnName = "客户下单日";
                Pods.Columns["CustomerOrderNumber"].ColumnName = "订单号(百姓网)";
                Pods.Columns["SystemNumber"].ColumnName = "订单号()";
                Pods.Columns["Str17"].ColumnName = "快递公司";
                Pods.Columns["Str1"].ColumnName = "快递订单号";
                Pods.Columns["Str2"].ColumnName = "商品品名";
                Pods.Columns["Str7"].ColumnName = "取件城市";
                Pods.Columns["Str13"].ColumnName = "送件城市";
            }
            catch
            { }
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            //string targetPath = Path.Combine(uploadFolderPath, "AMSTempFile");
            ExportDataToExcelHelper.SaveDateSetExportNew(Pods, uploadFolderPath, "BaiXingExportPods");
            return new EmptyResult();
        }
    }
}

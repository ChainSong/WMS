using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Web.Areas.ShipperManagement.Models;
using Runbow.TWS.Common;
using System.Text;
using Runbow.TWS.Web.Common;
using System.Data;
using Runbow.TWS.Biz.ShipperManagement;
using Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement;
using System.Web.Script.Serialization;
using Runbow.TWS.MessageContracts.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement;
using System.IO;
using MyFile = System.IO.File;

namespace Runbow.TWS.Web.Areas.ShipperManagement.Controllers
{
    public class ShipperManagementController : BaseController
    {
        public ActionResult Index(bool? useSession)
        {
            QueryCRMShipperViewModel vm = new QueryCRMShipperViewModel();
          
            vm.PageIndex = 0;
            vm.PageCount = 0;

            ///特殊业务，要求只有特定的人员才能进行修改操作
            if (base.UserInfo.ProjectRoleID == 28)
            {
                vm.ShowEditButton = true;
            }
            else 
            {
                vm.ShowEditButton = false;
            }

            if (useSession.HasValue && useSession.Value)
            {

                if (Session["ShipperCRM_SearchCondition"] != null)
                {
                    vm.SearchCondition = (CRMShipperSearchCondition)Session["ShipperCRM_SearchCondition"];
                    vm.PageIndex = Session["ShipperCRM_PageIndex"] != null ? (int)Session["ShipperCRM_PageIndex"] : 0;
                    
                    //bukan
                    if (!string.IsNullOrEmpty(vm.SearchCondition.TransportMode))
                    {
                        IList<SelectListItem> selectedtransportModeTypes = new List<SelectListItem>();
                        vm.SearchCondition.TransportMode.Split('|').Each((i, s) =>
                        {
                            selectedtransportModeTypes.Add(new SelectListItem() { Text = s, Value = s });
                        });

                        vm.SelectedTransportModes = selectedtransportModeTypes;
                    }

                    if (!string.IsNullOrEmpty(vm.SearchCondition.ProductType))
                    {
                        IList<SelectListItem> selectedProductTypes = new List<SelectListItem>();
                        vm.SearchCondition.ProductType.Split('|').Each((i, s) =>
                        {
                            selectedProductTypes.Add(new SelectListItem() { Text = s, Value = s });
                        });

                        vm.SelectedProductTypes = selectedProductTypes;
                    }
                }
                else
                {
                    vm.SearchCondition = new CRMShipperSearchCondition();
                    vm.PageIndex = 0;
                }

                var getCRMShippersByConditionResponse = new ShipperManagementService().GetCRMShippersByCondition(new GetCRMShippersByConditionRequest()
                {
                    SearchCondition = vm.SearchCondition,
                    PageSize = UtilConstants.PAGESIZE,
                    PageIndex = vm.PageIndex,
                });

                if (getCRMShippersByConditionResponse.IsSuccess)
                {
                    vm.CRMShipperCollection = getCRMShippersByConditionResponse.Result.CRMShipperCollection;
                    vm.PageIndex = getCRMShippersByConditionResponse.Result.PageIndex;
                    vm.PageCount = getCRMShippersByConditionResponse.Result.PageCount;
                }
            }
            else
            {
                vm.SearchCondition = new CRMShipperSearchCondition();
            }
                        
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(QueryCRMShipperViewModel vm, int? PageIndex, string Action)
        {
            if (vm.PostedTransportModes != null && vm.PostedTransportModes.Any())
            {
                StringBuilder transportModeSB = new StringBuilder();
                vm.PostedTransportModes.Each((i, s) =>
                {
                    transportModeSB.Append(s).Append("|");
                });
                transportModeSB.Remove(transportModeSB.Length - 1, 1);
                vm.SearchCondition.TransportMode = transportModeSB.ToString();
            }

            if (vm.PostedProductTypes != null && vm.PostedProductTypes.Any())
            {
                StringBuilder postedProductTypesSB = new StringBuilder();
                vm.PostedProductTypes.Each((i, s) =>
                {
                    postedProductTypesSB.Append(s).Append("|");
                });
                postedProductTypesSB.Remove(postedProductTypesSB.Length - 1, 1);
                vm.SearchCondition.ProductType = postedProductTypesSB.ToString();
            }
            //查询导出
            var getCRMShippersByConditionRequest = new GetCRMShippersByConditionRequest();

            if (Action == "查询" || Action== "Index")
            {
                getCRMShippersByConditionRequest.SearchCondition = vm.SearchCondition;
                getCRMShippersByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getCRMShippersByConditionRequest.PageIndex = PageIndex ?? 0;
            }
            else if (Action == "导出")
            {
                getCRMShippersByConditionRequest.SearchCondition = vm.SearchCondition;
                getCRMShippersByConditionRequest.PageSize = 0;
                getCRMShippersByConditionRequest.PageIndex = 0;
            }

            var getCRMShippersByConditionResponse = new ShipperManagementService().GetCRMShippersByCondition(getCRMShippersByConditionRequest);
           

            if (getCRMShippersByConditionResponse.IsSuccess)
            {
                if (!string.IsNullOrEmpty(vm.SearchCondition.TransportMode))
                {
                    IList<SelectListItem> selectedtransportModeTypes = new List<SelectListItem>();
                    vm.SearchCondition.TransportMode.Split('|').Each((i, s) =>
                    {
                        selectedtransportModeTypes.Add(new SelectListItem() { Text = s, Value = s });
                    });

                    vm.SelectedTransportModes = selectedtransportModeTypes;
                }

                if (!string.IsNullOrEmpty(vm.SearchCondition.ProductType))
                {
                    IList<SelectListItem> selectedProductTypes = new List<SelectListItem>();
                    vm.SearchCondition.ProductType.Split('|').Each((i, s) =>
                    {
                        selectedProductTypes.Add(new SelectListItem() { Text = s, Value = s });
                    });

                    vm.SelectedProductTypes = selectedProductTypes;
                }

                vm.CRMShipperCollection = getCRMShippersByConditionResponse.Result.CRMShipperCollection;
                vm.PageIndex = getCRMShippersByConditionResponse.Result.PageIndex;
                vm.PageCount = getCRMShippersByConditionResponse.Result.PageCount;
                Session["ShipperCRM_SearchCondition"] = vm.SearchCondition;
                Session["ShipperCRM_PageIndex"] = vm.PageIndex;

                if (Action == "导出")
                {
                    return this.Export(getCRMShippersByConditionResponse.Result.CRMShipperCollection);
                }
            }

            return View(vm);
        }
        
        private ActionResult Export(IEnumerable<CRMShipper> crmShippers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("归属区域", typeof(string));
            dt.Columns.Add("合作项目", typeof(string));
            dt.Columns.Add("企业名称", typeof(string));
            dt.Columns.Add("合同起始日期", typeof(string));
            dt.Columns.Add("合同结束日期", typeof(string));
            dt.Columns.Add("实际终止合作日期", typeof(string));
            dt.Columns.Add("是否有保单", typeof(string));
            dt.Columns.Add("投保公司", typeof(string));
            dt.Columns.Add("购买险种", typeof(string));
            dt.Columns.Add("保额", typeof(string));
            dt.Columns.Add("有效期限", typeof(string));
            dt.Columns.Add("六证", typeof(string));
            dt.Columns.Add("承运商注册资金", typeof(string));
            dt.Columns.Add("说明", typeof(string));

            crmShippers.Each((i, s) => {
                DataRow dr = dt.NewRow();
                dr[0] = s.Attribution;
                dr[1] = s.Str2;
                dr[2] = s.Name;
                dr[3] = s.DateTime1.HasValue ? s.DateTime1.Value.DateTimeToString() : "";
                dr[4] = s.DateTime2.HasValue ? s.DateTime2.Value.DateTimeToString() : "";
                dr[5] = s.DateTime3.HasValue ? s.DateTime3.Value.DateTimeToString() : "";
                dr[6] = s.Str4;
                dr[7] = s.InsuranceCompanies;
                dr[8] = s.InsuranceType;
                dr[9] = s.SumInsured;
                dr[10] = s.ValidityPeriod;
                dr[11] = s.SixCard;
                dr[12] = s.RegisteredCapital;
                dr[13] = s.Remark;
                dt.Rows.Add(dr);
            });

            return this.ExportDataTableToExcel(dt, "Shipper.xls");
        }
        //
        private ActionResult ExportDataTableToExcel(DataTable dt, string FileName)
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
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public ActionResult Create(long? id, int? ViewType)
        {
            CRMShipperOperationViewModel vm = new CRMShipperOperationViewModel();

            if (!id.HasValue)
            {
                vm.ViewType = 1;
                vm.CRMShipper = new CRMShipper();
                vm.CRMShipper.AttachmentGroupID = Guid.NewGuid().ToString();
                vm.CRMShipper.Str3 = Guid.NewGuid().ToString();
            }
            else
            {
                var getCRMShipperResponse = new ShipperManagementService().GetCRMShipperInfo(new CRMShipperOperationRequest() { CRMShipperID = id });
                if (getCRMShipperResponse.IsSuccess)
                {
                    vm.CRMShipper = getCRMShipperResponse.Result.CRMShipper;
                    if (vm.CRMShipper == null)
                    {
                        vm.CRMShipper = new CRMShipper();
                    }

                    if (!string.IsNullOrEmpty(vm.CRMShipper.DeliveryOfVehicleType))
                    {
                        IList<SelectListItem> selectedDeliveryOfVehicleTypes = new List<SelectListItem>();
                        vm.CRMShipper.DeliveryOfVehicleType.Split('|').Each((i, s) => {
                            selectedDeliveryOfVehicleTypes.Add(new SelectListItem() { Text = s, Value = s });
                        });
                        vm.SelectedDeliveryOfVehicleTypes = selectedDeliveryOfVehicleTypes;
                    }

                    if (!string.IsNullOrEmpty(vm.CRMShipper.SixCard))
                    {
                        IList<SelectListItem> selectedSixCards = new List<SelectListItem>();
                        vm.CRMShipper.SixCard.Split('|').Each((i, s) =>
                        {
                            selectedSixCards.Add(new SelectListItem() { Text = s, Value = s });
                        });
                        vm.SelectedSixCards = selectedSixCards;
                    }

                    if (!string.IsNullOrEmpty(vm.CRMShipper.TermialOfVehicleType))
                    {
                        IList<SelectListItem> selectedTermialOfVehicleTypes = new List<SelectListItem>();
                        vm.CRMShipper.TermialOfVehicleType.Split('|').Each((i, s) =>
                        {
                            selectedTermialOfVehicleTypes.Add(new SelectListItem() { Text = s, Value = s });
                        });
                        vm.SelectedTermialOfVehicleTypes = selectedTermialOfVehicleTypes;
                    }

                    if (!string.IsNullOrEmpty(vm.CRMShipper.TransportMode))
                    {
                        IList<SelectListItem> selectedTransportModes = new List<SelectListItem>();
                        vm.CRMShipper.TransportMode.Split('|').Each((i, s) =>
                        {
                            selectedTransportModes.Add(new SelectListItem() { Text = s, Value = s });
                        });
                        vm.SelectedTransportModes = selectedTransportModes;
                    }

                    if (!string.IsNullOrEmpty(vm.CRMShipper.TrunkOfVehicleType))
                    {
                        IList<SelectListItem> selectedTrunkOfVehicleTypes = new List<SelectListItem>();
                        vm.CRMShipper.TrunkOfVehicleType.Split('|').Each((i, s) =>
                        {
                            selectedTrunkOfVehicleTypes.Add(new SelectListItem() { Text = s, Value = s });
                        });
                        vm.SelectedTrunkOfVehicleTypes = selectedTrunkOfVehicleTypes;
                    }

                    vm.CRMShipperCooperationCollection = getCRMShipperResponse.Result.CRMShipperCooperationCollection;
                    vm.CRMShipperTransportationLineCollection = getCRMShipperResponse.Result.CRMShipperTransportationLineCollection;
                    vm.CRMShipperTerminalInfoCollection = getCRMShipperResponse.Result.CRMShipperTerminalInfoCollection;

                    if (string.IsNullOrEmpty(vm.CRMShipper.Str3))
                    {
                        vm.CRMShipper.Str3 = Guid.NewGuid().ToString();
                    }

                    if (string.IsNullOrEmpty(vm.CRMShipper.AttachmentGroupID))
                    {
                        vm.CRMShipper.AttachmentGroupID = Guid.NewGuid().ToString();
                    }

                }    

                if (!ViewType.HasValue)
                {
                    vm.ViewType = 0;
                }
                else
                {
                    vm.ViewType = ViewType.Value; 
                }

                if (base.UserInfo.ProjectRoleID == 28)
                {
                    vm.ShowEditButton = true;
                }
                else
                {
                    vm.ShowEditButton = false;
                }
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CRMShipperOperationViewModel vm)
        {
            if (vm.PostedDeliveryOfVehicleTypes != null && vm.PostedDeliveryOfVehicleTypes.Any())
            {
                StringBuilder deliveryOfVehicleSB = new StringBuilder();
                vm.PostedDeliveryOfVehicleTypes.Each((i, s) => {
                    deliveryOfVehicleSB.Append(s).Append("|");
                });
                deliveryOfVehicleSB.Remove(deliveryOfVehicleSB.Length - 1, 1);
                vm.CRMShipper.DeliveryOfVehicleType = deliveryOfVehicleSB.ToString();
            }

            if (vm.PostedSixCards != null && vm.PostedSixCards.Any())
            {
                StringBuilder sixCardsSB = new StringBuilder();
                vm.PostedSixCards.Each((i, s) =>
                {
                    sixCardsSB.Append(s).Append("|");
                });
                sixCardsSB.Remove(sixCardsSB.Length - 1, 1);
                vm.CRMShipper.SixCard = sixCardsSB.ToString();
            }

            if (vm.PostedTermialOfVehicleTypes != null && vm.PostedTermialOfVehicleTypes.Any())
            {
                StringBuilder termialOfVehicleTypeSB = new StringBuilder();
                vm.PostedTermialOfVehicleTypes.Each((i, s) =>
                {
                    termialOfVehicleTypeSB.Append(s).Append("|");
                });
                termialOfVehicleTypeSB.Remove(termialOfVehicleTypeSB.Length - 1, 1);
                vm.CRMShipper.TermialOfVehicleType = termialOfVehicleTypeSB.ToString();
            }

            if (vm.PostedTransportModes != null && vm.PostedTransportModes.Any())
            {
                StringBuilder transportModesSB = new StringBuilder();
                vm.PostedTransportModes.Each((i, s) =>
                {
                    transportModesSB.Append(s).Append("|");
                });
                transportModesSB.Remove(transportModesSB.Length - 1, 1);
                vm.CRMShipper.TransportMode = transportModesSB.ToString();
            }


            if (vm.PostedTrunkOfVehicleTypes != null && vm.PostedTrunkOfVehicleTypes.Any())
            {
                StringBuilder trunkOfVehicleTypesSB = new StringBuilder();
                vm.PostedTrunkOfVehicleTypes.Each((i, s) =>
                {
                    trunkOfVehicleTypesSB.Append(s).Append("|");
                });
                trunkOfVehicleTypesSB.Remove(trunkOfVehicleTypesSB.Length - 1, 1);
                vm.CRMShipper.TrunkOfVehicleType = trunkOfVehicleTypesSB.ToString();
            }
            //创建时间，创建人，更新时间，更新人
            if (vm.ViewType == 1)
            {
                vm.CRMShipper.Creator = base.UserInfo.Name;
                vm.CRMShipper.CreateTime = DateTime.Now;
            }

            vm.CRMShipper.UpdateTime = DateTime.Now;
            vm.CRMShipper.Updator = base.UserInfo.Name;      
            //
            var insertOrUpdateCRMShipperResponse = new ShipperManagementService().AddOrUpdateCRMShippers(new AddOrUpdateCRMShippersRequest()
            {
                CRMShipperCollection = new List<CRMShipper> { vm.CRMShipper }
            });


            // 插入更新结果如果成功
            if (insertOrUpdateCRMShipperResponse.IsSuccess)
            {
                if (insertOrUpdateCRMShipperResponse.Result != null && insertOrUpdateCRMShipperResponse.Result.Any())
                {
                    long id = insertOrUpdateCRMShipperResponse.Result.First();
                    var getCRMShipperResponse = new ShipperManagementService().GetCRMShipperInfo(new CRMShipperOperationRequest() { CRMShipperID = id });
                    if (getCRMShipperResponse.IsSuccess)
                    {
                        vm.ViewType = 0;
                        vm.CRMShipper = getCRMShipperResponse.Result.CRMShipper;
                        vm.CRMShipperCooperationCollection = getCRMShipperResponse.Result.CRMShipperCooperationCollection;
                        vm.CRMShipperTransportationLineCollection = getCRMShipperResponse.Result.CRMShipperTransportationLineCollection;
                        return View(vm);
                    }
                }
            }
            return View(vm);
        }


        //删除 JsonResult
        [HttpPost]
        public JsonResult DeleteCRMShipper(long id)
        {
            var response = new ShipperManagementService().DeleteCRMShipper(new CRMShipperOperationRequest() { CRMShipperID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
               return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }
        // 不看
        [HttpPost]
        public ActionResult DeleteCRMShipperTerminalInfo(long id)
        {
            var response = new ShipperManagementService().DeleteCRMShipperTerminalInfo(new CRMShipperOperationRequest() { CRMShipperTerminalInfoID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }
        //不看
        [HttpPost]
        public ActionResult DeleteCRMShipperTransportationLine(long id)
        {
            var response = new ShipperManagementService().DeleteCRMShipperTransportationLine(new CRMShipperOperationRequest() { CRMShipperTransportationLineID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }

        public ActionResult InsertCRMShipperTransportationLine(long id, string StartPlaceID, string StartPlaceName, string EndPlaceID, string EndPlaceName,string CoverRegionID, string CoverRegionName, int Period)
        {
            var startPlaceIDArray = StartPlaceID.Split(',');
            var startPlaceNameArray = StartPlaceName.Split(',');
            var endPlaceIDArray = EndPlaceID.Split(',');
            var endPlaceNameArray = EndPlaceName.Split(',');
            var coverRegionIDArray = CoverRegionID.Split(',');
            var coverRegionNameArray = CoverRegionName.Split(',');

            IList<CRMShipperTransportationLine> lines = new List<CRMShipperTransportationLine>();
            
            for (int i = 0; i < startPlaceIDArray.Length; i++)
            {
                for (int j = 0; j < endPlaceIDArray.Length; j++)
                {
                    for (int k = 0; k < coverRegionIDArray.Length; k++)
                    {
                        lines.Add(new CRMShipperTransportationLine()
                        {
                            ID = 0,
                            CRMShipperID = id,
                            StartCityID = startPlaceIDArray[i].ObjectToInt64(),
                            StartCityName = startPlaceNameArray[i],
                            EndCityID = endPlaceIDArray[j].ObjectToInt64(),
                            EndCityName = endPlaceNameArray[j],
                            CoverRegionID = coverRegionIDArray[k].ObjectToInt64(),
                            CoverRegionName = coverRegionNameArray[k],
                            Period = Period
                        });
                    }
                }
            }

            var response = new ShipperManagementService().AddOrUpdateCRMShipperTransportationLines(new AddOrUpdateCRMShipperTransportationLineRequest() { CRMShipperTransportationLineCollection = lines });

            if (response.IsSuccess)
            {
                return Json(new { Message = "新增成功", IsSuccess = true, Lines = response.Result.ToJsonString() }); 
            }

            return Json(new { Message = "新增线路失败！", IsSuccess = false });
        }

        public ActionResult CRMShipperTransportationLineManage(long id, int? ViewType)
        {
            CRMShipperTransportationLineManageViewModel vm = new CRMShipperTransportationLineManageViewModel();
            vm.CRMShipperID = id;
            vm.ViewType = ViewType ?? 0;

            var getCRMShipperTransportationLinesResponse = new ShipperManagementService().GetCRMShipperTransportationLines(new CRMShipperOperationRequest() { CRMShipperID = id });

            if (getCRMShipperTransportationLinesResponse.IsSuccess)
            {
                vm.CRMShipperTransportationLineCollection = getCRMShipperTransportationLinesResponse.Result;
            }

            return View(vm);
        }

        public ActionResult CRMShipperCooperationManage(long id, int? ViewType)
        {
            CRMShipperCooperationManageViewModel vm = new CRMShipperCooperationManageViewModel();
            vm.CRMShipperID = id;
            vm.ViewType = ViewType ?? 0;
            vm.AttachmentGroupID = Guid.NewGuid().ToString();

            var getCRMShipperCooperationsResponse = new ShipperManagementService().GetCRMShipperCooperations(new CRMShipperOperationRequest() { CRMShipperID = id });
            if (getCRMShipperCooperationsResponse.IsSuccess)
            {
                vm.CRMShipperCooperationCollection = getCRMShipperCooperationsResponse.Result;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteCRMShipperCooperation(long id)
        {
            var response = new ShipperManagementService().DeleteCRMShipperCooperation(new CRMShipperOperationRequest() { CRMShipperCooperationID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }

        [HttpPost]
        public ActionResult CRMShipperCooperationManage(CRMShipperCooperationManageViewModel vm)
        {
            IList<CRMShipperCooperation> cooperations = new List<CRMShipperCooperation>();
            StringBuilder productTypesSB = new StringBuilder();
            if (vm.PostedProductTypes != null && vm.PostedProductTypes.Any())
            {
                vm.PostedProductTypes.Each((i, p) => { productTypesSB.Append(p).Append("|"); });
                productTypesSB.Remove(productTypesSB.Length - 1, 1);
            }

            cooperations.Add(new CRMShipperCooperation() { CRMShipperID = vm.CRMShipperID, Name = vm.Name, Remark = vm.Remark, AttachmentGroupID = vm.AttachmentGroupID,
                                                           Str1 = vm.Str1,
                                                           Str2 = vm.Str2,
                                                           Str3 = vm.Str3,
                                                           Str4 = vm.Str4,
                                                           Str5 = vm.Str5,
                                                           Str6 = vm.Str6,
                                                           Str7 = productTypesSB.ToString(),
                                                           Str8 = vm.Str8,
                                                           Str9 = vm.Str9,
                                                           Str10 = vm.Str10
            });

            new ShipperManagementService().AddOrUpdateCRMShipperCooperations(new AddOrUpdateCRMShipperCooperationsRequest() { CRMShipperCooperationCollection = cooperations });
            return RedirectToAction("CRMShipperCooperationManage", new { id = vm.CRMShipperID, ViewType = vm.ViewType });      
        }


        public ActionResult CRMShipperTerminalInfoManage(long id, int? ViewType)
        {
            CRMShipperTerminalInfoManageViewModel vm = new CRMShipperTerminalInfoManageViewModel();
            vm.CRMShipperID = id;
            vm.ViewType = ViewType ?? 0;

            var getCRMShipperTerminalInfoResponse = new ShipperManagementService().GetCRMShipperTerminalInfos(new CRMShipperOperationRequest() { CRMShipperID = id });
            if (getCRMShipperTerminalInfoResponse.IsSuccess)
            {
                vm.CRMShipperTerminalInfoCollection = getCRMShipperTerminalInfoResponse.Result;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult CRMShipperTerminalInfoManage(CRMShipperTerminalInfoManageViewModel vm)
        {
            IList<CRMShipperTerminalInfo> terminalInfos = new List<CRMShipperTerminalInfo>();
            terminalInfos.Add(new CRMShipperTerminalInfo()
            {
                CRMShipperID = vm.CRMShipperID,
                TerminalAddress = vm.TerminalAddress,
                IsOwn = vm.IsOwn,
                TerminalOfficeArea = vm.TerminalOfficeArea,
                TerminalWareHouseArea = vm.TerminalWareHouseArea,
                TerminalWareHouseAreaRange = vm.TerminalWareHouseAreaRange,
                TerminalNumberOfEmployees = vm.TerminalNumberOfEmployees,
                TerminalNumberOfCustomerService = vm.TerminalNumberOfCustomerService,
                TerminalNumberOfStevedores = vm.TerminalNumberOfStevedores,
                TerminalForkliftsUsage = vm.TerminalForkliftsUsage,
                TerminalLoadingPlatform = vm.TerminalLoadingPlatform,
                TerminalDeliveryVehicles = vm.TerminalDeliveryVehicles,
                Str1 = vm.Str1,
                Str2 = vm.Str2,
                Str3 = vm.Str3,
                Str4 = vm.Str4,
                Str5 = vm.Str5,
            });

            new ShipperManagementService().AddOrUpdateCRMShipperTerminalInfos(new AddOrUpdateCRMShipperTerminalInfoRequest() { CRMShipperTerminalInfoCollection = terminalInfos });
            return RedirectToAction("CRMShipperTerminalInfoManage", new { id = vm.CRMShipperID, ViewType = vm.ViewType });      
        }


        //承运商批量导入
        [HttpGet]
        public ActionResult ImputShipper()
        {
           
            return View();
        }

        public string ImputShippers()
        {
            if (Request.Files.Count > 0)
            { 
                 HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                 if (hpf.ContentLength > 0)
                 {
                     try
                     {
                         DataSet ds = this.GetDataFromExcel(hpf);
                         for (int j = 0; j < ds.Tables.Count; j++)
                         {
                             if (ds != null && ds.Tables[j] != null)
                             {
                                 IList<InsertShipperExcel> crmshipper = new List<InsertShipperExcel>();
                                 for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                 {
                                     crmshipper.Add(new InsertShipperExcel()
                                     {
                                         Name = ds.Tables[0].Rows[i]["公司名称"].ToString().Trim(),
                                         TransportMode = ds.Tables[0].Rows[i]["运输方式"].ToString().Trim(),
                                         Code = ds.Tables[0].Rows[i]["公司名称"].ToString().Trim(),
                                         Creater = base.UserInfo.Name,
                                         CreateTime = DateTime.Now
                                     });
                                 }
                                 
                                 var response = new ShipperManagementService().InsertCRMShipperExecl(new GetCRMShippersByConditionRequest() { InsertShipper = crmshipper});
                                 if (response)
                                 {
                                     return new { result = "导入成功！", IsSuccess = true }.ToString();
                                 }
                             }
                         }
                     }
                     catch(Exception)
                     {
                         throw;
                     }
                     return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                 }
                 return new { result = "文件内容为空", IsSuccess = false }.ToString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToString();
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





        //所有承运商和车辆 分页
        [HttpGet]
        public ActionResult ShipperToVehicle(string id,int? type)
        {
            ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel()
            {
                Shipper = ApplicationConfigHelper.GetShipperList(),
                Vehicle = ApplicationConfigHelper.GetVehicleList()
            };


            int pagesize = 17;
            var response = new VehicleManagementService().GetAllVehicle(new GetCRMVehicleByConditionRequest() 
            {
                
                PageSize = pagesize,
                PageIndex = sv.PageIndex,
            });


            if (response.IsSuccess)
            {
                sv.Vehicle = response.Result.CRMVehicleCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }

            return View(sv);
        }

   
        //查询
        public string SearchShipperToVehicle(string vehicleNo)//int? Index,  string Action
        {
            ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel();
       
            sv.VehicleNo = vehicleNo;
            var request = new GetCRMVehicleByConditionRequest();

             int pagesize = 17;
             request.PageIndex = sv.PageIndex;
             request.PageSize = pagesize;
             request.vehicleNo = vehicleNo;
            
            var response = new VehicleManagementService().GetAllVehicle(request);

            if (response.IsSuccess)
            {
                sv.Vehicle = response.Result.CRMVehicleCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;
        }


        [HttpPost]
        public string ShipperToVehicle(string shipperName, string vehicleNo, long SID, int? Index)
        {
            ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel();
            sv.PageIndex = Index??0;
            sv.SID = (SID);
            sv.ShipperName = shipperName;
            //sv.VehicleNo = vehicle;
            
            
            var request = new GetCRMVehicleByConditionRequest();

             int pagesize = 17;
             request.PageIndex = sv.PageIndex;
             request.PageSize = pagesize;
             request.vehicleNo = vehicleNo;
            

            var response = new VehicleManagementService().GetAllVehicle(request);

            if (response.IsSuccess)
            {
                sv.Vehicle = response.Result.CRMVehicleCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);


            return js;
        }

        //

        //提交
        public bool AddShipperToVehicle(string shipper, string jsonStr)
        {

            var request = JsonToModel<CRMCar>(jsonStr);
            var response = new VehicleManagementService().AddShipperToVehicle(new ShipperMappingVehicleRequest
            {
                car = request,
                ShipperName = shipper,

                UserName = base.UserInfo.Name
            });
            if (response == "操作成功")
                return true;
            else
                return false;
        }

        public static List<T> JsonToModel<T>(string jsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Deserialize<List<T>>(jsonStr);
        }

        ////提交
        //public bool AddShipperToVehicle(string shippername, string vehicleno)
        //{
        //    ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel();
        //    sv.ShipperName = shippername;
        //    sv.VehicleNo = vehicleno;

        //    var request = new ShipperMappingVehicleRequest();
        //    request.ShipperName = shippername;
        //    request.VehicleNo = vehicleno;
        //    request.UserName = base.UserInfo.Name;

        //    var response = new VehicleManagementService().AddShipperToVehicle(request);

        //    if (response == "操作成功")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        
        //}

         


        //模糊查询显示承运商
        [HttpPost]
        public ActionResult GetAllShippersbySID(string name)
        {
            var shippers = ApplicationConfigHelper.GetShipperList();
            return Json(shippers.Where(s => s.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
        }


        //输入承运商名称，显示车辆列表
        public string GetCRM_ShipperMappingVehicle(string name)
        {
            ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel(); 
 
            var request = new ShipperMappingVehicleRequest();
            request.ShipperName = name;
            
             

            var response = new VehicleManagementService().GetCRM_ShipperMappingVehicle(request);

            if (response.IsSuccess)
            {
                sv.Vehicle = response.Result.CRMVehicleCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }



            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;
             
        }

        //右边表格查询
        public string GetCRMShipperMappingVehicle(string name, string vehicleno)
        {
            ShipperToVehicleViewModel sv = new ShipperToVehicleViewModel();
            sv.ShipperName = name;
            sv.VehicleNo = vehicleno;

            var request = new ShipperMappingVehicleRequest();
            request.ShipperName = name;
            request.VehicleNo = vehicleno;


            var response = new VehicleManagementService().GetCRMShipperMappingVehicle(request);

            if (response.IsSuccess)
            {
                sv.Vehicle = response.Result.CRMVehicleCollection;
            }

            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;
        
        }
    }
}

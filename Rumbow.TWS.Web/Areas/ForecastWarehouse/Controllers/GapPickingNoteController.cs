using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Web.Areas.ForecastWarehouse.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Biz.ForecastWarehouse;
using Runbow.TWS.Dao.ForecastWarehouse;
using Runbow.TWS.MessageContracts.ForecastWarehouse;
using Runbow.TWS.Entity.ForecastWarehouse;

namespace Runbow.TWS.Web.Areas.ForecastWarehouse.Controllers
{
    public class GapPickingNoteController : BaseController
    {
        //
        // GET: /ForecastWarehouse/GapPickingNote/
        [HttpGet]
        public ActionResult PickingNote()
        {
            var user = base.UserInfo.Name;
            GapPickingNoteModel vm = new GapPickingNoteModel();

            vm.ViewType = 0;
            var response = new GapPickingNoteService().GetGapPickingNote(new GapPickingNoteRequest() { User = user });
            vm.Code = response.Result.Code;
            vm.CreatTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            
            #region 服务明细
            List<SelectListItem> item = new List<SelectListItem>();
            #region 注释
            //item.Add(new SelectListItem { Value = "01", Text = "店铺之间的货品转货(Store to store transfer)" });
            //item.Add(new SelectListItem { Value = "02", Text = "正价店铺转货到清仓店，仅大陆地区(Pull-off transfer to liquidation store)" });
            //item.Add(new SelectListItem { Value = "03", Text = "日常运营辅料/GWP买赠商品转货(Non-stock/GWP transfer)" });
            //item.Add(new SelectListItem { Value = "04", Text = "可售商品/Pull-back退货(Saleable/Pull-back Return return to DC)" });
            //item.Add(new SelectListItem { Value = "05", Text = "残次/不可售商品/Write-off退货(Damage&Defective/Write-off return to CDC)" });
            //item.Add(new SelectListItem { Value = "06", Text = "防盗扣退货(Security HT return to DC)" });
            //item.Add(new SelectListItem { Value = "07", Text = "日常运营辅料退货(Non-stock return to SUPPLY DC)" });
            //item.Add(new SelectListItem { Value = "08", Text = "GWP买赠商品退货(GWP return to DC)" });
            //item.Add(new SelectListItem { Value = "09", Text = "收银条退货(Cash Media return to DC)" });
            //item.Add(new SelectListItem { Value = "10", Text = "其他-请备注货品明细(Others-please provide details)" });
            #endregion
            ViewBag.Details = item;
            #endregion
            #region 目的代码
            List<SelectListItem> items = new List<SelectListItem>();
            ViewBag.Code = items;
            #endregion

            Session["CreatTime"] = vm.CreatTime;
            Session["Code"] = vm.Code;

            return View(vm);
        }

        [HttpPost]
        public ActionResult PickingNote(GapPickingNoteModel vm)
        {
            vm.Gappackingnote = new GapPickingNote();
            //GapPickingNoteModel vm = new GapPickingNoteModel();
            vm.Code = Session["Code"] as UserCode;
            vm.CreatTime = DateTime.Parse(Session["CreatTime"].ToString());

            #region
            vm.Gappackingnote.StoreCode = vm.Code.StoreCode;
            vm.Gappackingnote.StoreName = vm.Code.StoreName;
            vm.Gappackingnote.City = vm.Code.City;
            vm.Gappackingnote.Brand = vm.Code.Brand;
            vm.Gappackingnote.TransferorReturn = vm.TransferorReturn;
            vm.Gappackingnote.ServiceDetail = vm.ServiceDetail;
            vm.Gappackingnote.DestinationCode = vm.DestinationCode;
            vm.Gappackingnote.CartonQuantity = vm.CartonQuantity;
            vm.Gappackingnote.ExpectedDeliveryDate = vm.ExpectedDeliveryDate;
            vm.Gappackingnote.ExpectedArrivalDate = vm.ExpectedArrivalDate;
            vm.Gappackingnote.Remark = vm.Remark;
            vm.Gappackingnote.CreatTime = vm.CreatTime;
            #endregion

            Session["Gappackingnote"] = vm.Gappackingnote;

            var response = new GapPickingNoteService().AddGapPickingNote(new GapPickingNoteRequest { GapPickingNote = vm.Gappackingnote });
            
            vm.ViewType = 1;

            return View(vm);
        }



        public ActionResult PickingNotes()
        {
            GapPickingNoteModel vm = new GapPickingNoteModel();
            vm.Gappackingnote = Session["Gappackingnote"] as GapPickingNote;

            return View(vm);
        }

        public ActionResult ChangeTransferOrReturn(string str)
        {
            var temp = new { data1 = new List<SelectListItem>(), data2 = new List<SelectListItem>() };
            #region
            List<SelectListItem> item = new List<SelectListItem>();
            List<SelectListItem> items = new List<SelectListItem>();
            if (str == "转货")
            {
                item.Add(new SelectListItem { Value = "店铺之间的货品转货(Store to store transfer)", Text = "店铺之间的货品转货(Store to store transfer)" });
                item.Add(new SelectListItem { Value = "正价店铺转货到清仓店，仅大陆地区(Pull-off transfer to liquidation store)", Text = "正价店铺转货到清仓店，仅大陆地区(Pull-off transfer to liquidation store)" });
                item.Add(new SelectListItem { Value = "日常运营辅料/GWP买赠商品转货(Non-stock/GWP transfer)", Text = "日常运营辅料/GWP买赠商品转货(Non-stock/GWP transfer)" });

                items.Add(new SelectListItem { Value = "000001", Text = "000001" });
                items.Add(new SelectListItem { Value = "000002", Text = "000002" });
                items.Add(new SelectListItem { Value = "000003", Text = "000003" });
                items.Add(new SelectListItem { Value = "000004", Text = "000004" });
                items.Add(new SelectListItem { Value = "000005", Text = "000005" });
                items.Add(new SelectListItem { Value = "000006", Text = "000006" });
                items.Add(new SelectListItem { Value = "000007", Text = "000007" });
                items.Add(new SelectListItem { Value = "000008", Text = "000008" });
                items.Add(new SelectListItem { Value = "000009", Text = "000009" });
            }
            else if (str == "退货")
            {
                item.Add(new SelectListItem { Value = "可售商品/Pull-back退货(Saleable/Pull-back Return return to DC)", Text = "可售商品/Pull-back退货(Saleable/Pull-back Return return to DC)" });
                item.Add(new SelectListItem { Value = "残次/不可售商品/Write-off退货(Damage&Defective/Write-off return to CDC)", Text = "残次/不可售商品/Write-off退货(Damage&Defective/Write-off return to CDC)" });
                item.Add(new SelectListItem { Value = "防盗扣退货(Security HT return to DC)", Text = "防盗扣退货(Security HT return to DC)" });
                item.Add(new SelectListItem { Value = "日常运营辅料退货(Non-stock return to SUPPLY DC)", Text = "日常运营辅料退货(Non-stock return to SUPPLY DC)" });
                item.Add(new SelectListItem { Value = "GWP买赠商品退货(GWP return to DC)", Text = "GWP买赠商品退货(GWP return to DC)" });
                item.Add(new SelectListItem { Value = "收银条退货(Cash Media return to DC)", Text = "收银条退货(Cash Media return to DC)" });
                item.Add(new SelectListItem { Value = "其他-请备注货品明细(Others-please provide details)", Text = "其他-请备注货品明细(Others-please provide details)" });

                items.Add(new SelectListItem { Value = "CDC:SHD0005", Text = "CDC:SHD0005" });
                items.Add(new SelectListItem { Value = "SUPPLY DC:SHD0038", Text = "SUPPLY DC:SHD0038" });
                items.Add(new SelectListItem { Value = "HK DC:HKD0037", Text = "HK DC:HKD0037" });
                items.Add(new SelectListItem { Value = "TPE POOLER:TW0000", Text = "TPE POOLER:TW0000" });
            }
            else
            {
                item.Add(new SelectListItem { Value = "未选择", Text = "==请选择==" });
                items.Add(new SelectListItem { Value = "未选择", Text = "==请选择==" });
            }
            #endregion
            temp = new { data1 = item, data2 = items };

            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}

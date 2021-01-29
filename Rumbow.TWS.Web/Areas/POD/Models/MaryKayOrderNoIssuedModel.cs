using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class MaryKayOrderNoIssuedModel
    {
        public string SystemOrderNo { get; set; }
        public string MkOrderNo { get; set; }
       
        public DateTime? BeginOrderNoDateTime { get; set; }
        public DateTime? EndOrderNoDateTime { get; set; }
        public string EndCity { get; set; }
        public string EndCityID { get; set; }
        public string ShipperName { get; set; }
        public string ShipperID { get; set; }

        //Response<IEnumerable<AMSUpload>> resams = service.GetAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload });     
        /// <summary>
        /// 下发状态
        /// </summary>
        public string IssuedStatusID { get; set; }
        public IEnumerable<SelectListItem> IssuedStatus
        {  
             get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择",Selected=true }, 
                    new SelectListItem() { Value = "未同步", Text = "未同步" }, 
                    new SelectListItem() { Value = "同步成功", Text = "同步成功" }, 
                    new SelectListItem() { Value = "同步失败", Text = "同步失败" }
                    
                };

            }
        }

        /// <summary>
        /// 承运商
        /// </summary>
        public IEnumerable<SelectListItem> Shipper
        {
            get
            {
                MaryKayService service = new MaryKayService();
                Response<IEnumerable<DMS_Shipper>> resams = service.GetMaryKayShipper();

                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = "", Text = "请选择", Selected = true });
                if (resams.Result != null)
                {
                    foreach (DMS_Shipper a in resams.Result)
                    {
                        list.Add(new SelectListItem() { Value = a.colCode, Text = a.colName });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 省份
        /// </summary>
        public IEnumerable<SelectListItem> Province
        {
            get
            {
                MaryKayService service = new MaryKayService();
                Response<IEnumerable<DMS_Province>> resams = service.GetMaryKayProvince();

                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = "", Text = "请选择", Selected = true });
                if (resams.Result != null)
                {
                    foreach (DMS_Province a in resams.Result)
                    {
                        list.Add(new SelectListItem() { Value = a.colCode, Text = a.colName });
                    }
                }

                return list;
            }
        }

        public DataTable OrderNoIssuedTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

    }


    public struct MaryKayToJson
    {
        public string PODID { get; set; }  //属性的名字，必须与json格式字符串中的"key"值一样。

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodInfoUpdateViewModel
    {
        [Display(Name = "订单编号")]
        public string CustomerOrderNumber { get; set; }
        [Display(Name = "订单日期")]
        public string OrderNoDate { get; set; }
        [Display(Name = "未及时出货原因")]
        public string NotDeliverGoodsRemark { get; set; }


        public bool ISORSUCCESS { get; set; }
        public string ERRORSOURCEVALUE { get; set; }
        public DataTable DeliverGoods { get; set; }
        public string Message { get; set; }

        public DateTime ActualDeliveryDate { get; set; }
    }
}
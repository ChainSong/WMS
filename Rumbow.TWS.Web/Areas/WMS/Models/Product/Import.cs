using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.WMS.Models.Product
{
    public class Import
    {
        public string item_no { get; set; }
        public string style_code { get; set; }
        public string color_code { get; set; }
        public string size_name { get; set; }
        public string cur_unit_price { get; set; }
    }
}
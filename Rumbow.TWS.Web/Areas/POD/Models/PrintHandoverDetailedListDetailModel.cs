using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PrintHandoverDetailedListDetailModel
    {
        public DataTable DataTable { get; set; }
        public DataTable DataTableInvoice { get; set; }
        public DataTable DataTableForCollection { get; set; }
        
        public string TitleText { get; set; }
        public string PageNotText { get; set; }
        public string NumberOfPackages { get; set; }
        public string ShipperNumber { get; set; }





        

    }


    public struct ToJson
    {
        public string ID { get; set; }  //属性的名字，必须与json格式字符串中的"key"值一样。
        public string PackagesNumber { get; set; }
        public string ShippeNO { get; set; }
    }
}
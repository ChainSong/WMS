using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Warehouse
{
    public class WarehouseCheckSearchCondition
    {
        public int ID { get; set; }
        public int CID { get; set; }
        public string CheckNumber { get; set; }
        public string ExternNumber { get; set; }
        public long ? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Warehouse { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string SKU { get; set; }
        public string GoodsType { get; set; }
        public int CheckQty { get; set; }
        public int ActualQty { get; set; }
        public string IS_Difference { get; set; }
        public string IS_Deal { get; set; }
        public string Remark { get; set; }
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public string str5 { get; set; }
        public string Oprer { get; set; }
        public int ViewType { get; set; }
        public DateTime? Checkdate { get; set; }
        public DateTime? StartCheckdate { get; set; }
        public DateTime? EndCheckdate { get; set; }
        public int Type { get; set; }
        public string Type_description { get; set; }

        public string[] ActulQtyargs;
        public string[] Roles;
        public string[] EmptyLocation;//空库位的类型（发货/移动之类的）
        public int PageIndex { get; set; }
        public int PageSize  { get; set; }


        public long WareHouseID { get; set; }//lrg
        public long AreaID { get; set; }
    }
}

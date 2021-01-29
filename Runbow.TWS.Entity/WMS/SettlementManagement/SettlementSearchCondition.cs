using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.SettlementManagement
{
    public class SettlementSearchCondition
    {
        public long ID { get; set; }
        public long WSID { get; set; }
        public string SettlementNumber { get; set; }
        public string ExternNumber { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long WarehouseID { get; set; }
        public string WarehouseName { get; set; }
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
        public DateTime? CompleteDate { get; set; }
        public DateTime? StartCompleteDate { get; set; }
        public DateTime? EndCompleteDate { get; set; }

        public DateTime? Settlementdate { get; set; }
        public DateTime? StartSettlementdate { get; set; }
        public DateTime? EndSettlementdate { get; set; }

        public int Type { get; set; }
        public string Type_description { get; set; }
        public string Month { get; set; }

        public string[] ActulQtyargs;
        public string[] Roles;
        public int PageIndex { get; set; }
        public int PageSize  { get; set; }

        public int CostCategory { get; set; }
    }
}

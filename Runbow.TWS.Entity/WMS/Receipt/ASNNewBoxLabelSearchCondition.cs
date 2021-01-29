namespace Runbow.TWS.Entity.WMS.Receipt
{
    public class ASNNewBoxLabelSearchCondition: ASNNewBoxLabel
    {//NIKE退货仓-
        public string StartCreateTime { get; set; }//开始时间
        public string EndCreateTime { get; set; }//结束时间
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int BoxTotal { get; set; }
    }
}

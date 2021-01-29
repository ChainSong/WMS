using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.BaiduMap
{
    public class PodStatusLogMap
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("PodID", "PodID")]
        public long PodID { get; set; }
        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public string CreateTime { get; set; }
        [EntityPropertyExtension("DriverPhone", "DriverPhone")]
        public string DriverPhone { get; set; }
        [EntityPropertyExtension("ReceivingCustomer", "ReceivingCustomer")]
        public string ReceivingCustomer { get; set; }
        [EntityPropertyExtension("Destination", "Destination")]
        public string Destination{ get; set; }
        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public string GoodsNumber { get; set; }
        [EntityPropertyExtension("CarNo", "CarNo")]
        public string CarNo { get; set; }
        [EntityPropertyExtension("DriverName", "DriverName")]
        public string DriverName { get; set; }
        [EntityPropertyExtension("PODType", "PODType")]
        public string PODType { get; set; }
        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
         [EntityPropertyExtension("EstimatedArrivalDate", "EstimatedArrivalDate")]
        public string EstimatedArrivalDate { get; set; }
        
        [EntityPropertyExtension("Contact", "Contact")]
        public string Contact { get; set; }
        [EntityPropertyExtension("ContactPhone", "ContactPhone")]
        public string ContactPhone { get; set; }
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }
        [EntityPropertyExtension("ActualDepartureTime", "ActualDepartureTime")]
        public string ActualDepartureTime { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public string DateTime2 { get; set; }
        [EntityPropertyExtension("ActualArrivalTime", "ActualArrivalTime")]
        public string ActualArrivalTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class PodReplyDocumentSearchCondition : PodReplyDocumentWithAttachment
    {
        public DateTime? EndCreateDate { get; set; }

        public IEnumerable<long> CustomerIDs { get; set; }

        public long? ShipperID { get; set; }

        public string ShipperName { get; set; }

        public bool IsInnerUser { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public DateTime? EndActualDeliveryDate { get; set; }

        public bool? HasAttachment { get; set; }

        public long? ShipperTypeID { get; set; }

        public long? PODTypeID { get; set; }

        public string StartWareHouse { get; set; }

        public string SalesOrderOrNoneSalesOrder { get; set; }

        public string PodRegion { get; set; }

        public string AuditType { get; set; }

        public string Number103s { get; set; }

        public string CreditNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? EndOrderDate { get; set; }

    }
}

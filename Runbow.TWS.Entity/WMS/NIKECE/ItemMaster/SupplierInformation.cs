using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class SupplierInformation
    {
        [XmlElement("SupplierID")]
        public string SupplierID { get; set; }
        [XmlElement("SupplierRetailSaleUnitCode")]
        public string SupplierRetailSaleUnitCode { get; set; }
        [XmlElement("SupplierLinearMeasureCode")]
        public string SupplierLinearMeasureCode { get; set; }
        [XmlElement("AvailabilityStatus")]
        public string AvailabilityStatus { get; set; }
        [XmlElement("SupplierItemID")]
        public string SupplierItemID { get; set; }
        [XmlElement("Brand")]
        public string Brand { get; set; }
    }
}

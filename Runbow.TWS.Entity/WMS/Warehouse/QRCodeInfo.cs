using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class QRCodeInfo
    {
        [EntityPropertyExtension("ChargingName", "ChargingName")]
        public string ChargingName { get; set; }

        [EntityPropertyExtension("Operation", "Operation")]
        public string Operation { get; set; }

        [EntityPropertyExtension("OperationType", "OperationType")]
        public string OperationType { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }

        [EntityPropertyExtension("Length", "Length")]
        public string Length { get; set; }

        [EntityPropertyExtension("Width", "Width")]
        public string Width { get; set; }

        [EntityPropertyExtension("MapType", "MapType")]
        public string MapType { get; set; }

        [EntityPropertyExtension("MapStatus", "MapStatus")]
        public string MapStatus { get; set; }

        [EntityPropertyExtension("X", "X")]
        public string X { get; set; }
        [EntityPropertyExtension("Y", "Y")]
        public string Y { get; set; }

        [EntityPropertyExtension("MapID", "MapID")]
        public long MapID { get; set; }

        [EntityPropertyExtension("MapDetailID", "MapDetailID")]
        public long MapDetailID { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("WareHouseID", "WareHouseID")]
        public long WareHouseID { get; set; }

        [EntityPropertyExtension("GoodsShelfID", "GoodsShelfID")]
        public string GoodsShelfID { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("QRCode", "QRCode")]
        public string QRCode { get; set; }

       
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

      

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

       
    }
}

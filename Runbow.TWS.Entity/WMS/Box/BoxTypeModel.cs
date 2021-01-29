using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Box
{
    public class BoxTypeModel
    {
        [EntityPropertyExtension("Id", "Id")]
        public int Id { get; set; }

        [EntityPropertyExtension("CustomerId", "CustomerId")]
        public int CustomerId { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseId", "WarehouseId")]
        public int WarehouseId { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }
        [EntityPropertyExtension("BoxType", "BoxType")]
        public string BoxType { get; set; }
        [EntityPropertyExtension("BoxLength", "BoxLength")]
        public double BoxLength { get; set; }
        [EntityPropertyExtension("BoxWidth", "BoxWidth")]
        public double BoxWidth { get; set; }
        [EntityPropertyExtension("BoxHigh", "BoxHigh")]
        public double BoxHigh { get; set; }
        [EntityPropertyExtension("BoxVolume", "BoxVolume")]
        public double BoxVolume { get; set; } 
        [EntityPropertyExtension("BoxWeight", "BoxWeight")]
        public double BoxWeight { get; set; }




    }
}

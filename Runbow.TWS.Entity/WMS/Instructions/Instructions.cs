using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Instructions
{
    public class Instructions
    {
        [EntityPropertyExtension("ID", "ID")]
        public long? ID { get; set; }
        [EntityPropertyExtension("InstructionTime", "InstructionTime")]
        public DateTime? InstructionTime { get; set; }

        [EntityPropertyExtension("WaveNumber", "WaveNumber")]
        public string WaveNumber { get; set; }
        [EntityPropertyExtension("WaveTime", "WaveTime")]
        public DateTime WaveTime { get; set; }
        [EntityPropertyExtension("ReleatedID", "ReleatedID")]
        public long ReleatedID { get; set; }
        [EntityPropertyExtension("ReleatedNumber", "ReleatedNumber")]
        public string ReleatedNumber { get; set; }
        [EntityPropertyExtension("ReleatedType", "ReleatedType")]
        public int ReleatedType { get; set; }
        [EntityPropertyExtension("ReleatedDetailID", "ReleatedDetailID")]
        public long ReleatedDetailID { get; set; }
        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }
        [EntityPropertyExtension("InstructionOrder", "InstructionOrder")]
        public int InstructionOrder { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        [EntityPropertyExtension("QtyExcepted", "QtyExcepted")]
        public decimal QtyExcepted { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }
        [EntityPropertyExtension("Uint", "Uint")]
        public string Uint { get; set; }
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        [EntityPropertyExtension("GoodsShelve", "GoodsShelve")]
        public string GoodsShelve { get; set; }
        [EntityPropertyExtension("GoodsShelveCurrentLocation", "GoodsShelveCurrentLocation")]
        public string GoodsShelveCurrentLocation { get; set; }

        [EntityPropertyExtension("OperatingAreaName", "OperatingAreaName")]
        public string OperatingAreaName { get; set; }

        [EntityPropertyExtension("OperatingArea", "OperatingArea")]
        public string OperatingArea { get; set; }

        [EntityPropertyExtension("IsRobotArrive", "IsRobotArrive")]
        public bool IsRobotArrive { get; set; }
        [EntityPropertyExtension("RobotArriveTime", "RobotArriveTime")]
        public DateTime RobotArriveTime { get; set; }
        [EntityPropertyExtension("SKUActual", "SKUActual")]
        public string SKUActual { get; set; }
        [EntityPropertyExtension("UPCActual", "UPCActual")]
        public string UPCActual { get; set; }
        [EntityPropertyExtension("GoodsNameActual", "GoodsNameActual")]
        public string GoodsNameActual { get; set; }
        [EntityPropertyExtension("GoodsTypeActual", "GoodsTypeActual")]
        public string GoodsTypeActual { get; set; }
        [EntityPropertyExtension("QtyActual", "QtyActual")]
        public decimal QtyActual { get; set; }
        [EntityPropertyExtension("BoxNumberActual", "BoxNumberActual")]
        public string BoxNumberActual { get; set; }
        [EntityPropertyExtension("BatchNumberActual", "BatchNumberActual")]
        public string BatchNumberActual { get; set; }
        [EntityPropertyExtension("UnitActual", "UnitActual")]
        public string UnitActual { get; set; }
        [EntityPropertyExtension("SpecificationsActual", "SpecificationsActual")]
        public string SpecificationsActual { get; set; }
        [EntityPropertyExtension("LocationActual", "LocationActual")]
        public string LocationActual { get; set; }
        [EntityPropertyExtension("IsReturnDone", "IsReturnDone")]
        public bool IsReturnDone { get; set; }
        [EntityPropertyExtension("ReturnDoneTime", "ReturnDoneTime")]
        public DateTime? ReturnDoneTime { get; set; }
        [EntityPropertyExtension("GoodShelveDestinationLocation", "GoodShelveDestinationLocation")]
        public string GoodShelveDestinationLocation { get; set; }
        [EntityPropertyExtension("IsInstructionDone", "IsInstructionDone")]
        public int IsInstructionDone { get; set; }
        [EntityPropertyExtension("InstructionDoneTime", "InstructionDoneTime")]
        public DateTime? InstructionDoneTime { get; set; }
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
        [EntityPropertyExtension("int1", "int1")]
        public int int1 { get; set; }
        [EntityPropertyExtension("Int2", "Int2")]
        public int Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public int Int3 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }
        [EntityPropertyExtension("bit1", "bit1")]
        public bool bit1 { get; set; }
        [EntityPropertyExtension("bit2", "bit2")]
        public bool bit2 { get; set; }
        [EntityPropertyExtension("bit3", "bit3")]
        public bool bit3 { get; set; }
        [EntityPropertyExtension("LevelsNumber", "LevelsNumber")]
        public int LevelsNumber { get; set; }
        [EntityPropertyExtension("SerialNumber", "SerialNumber")]
        public int SerialNumber { get; set; }
        [EntityPropertyExtension("RowNumber", "RowNumber")]
        public int RowNumber { get; set; }
        [EntityPropertyExtension("CellNumber", "CellNumber")]
        public int CellNumber { get; set; }
        [EntityPropertyExtension("RePickWallDetailID", "RePickWallDetailID")]
        public long? RePickWallDetailID { get; set; }



    }
}

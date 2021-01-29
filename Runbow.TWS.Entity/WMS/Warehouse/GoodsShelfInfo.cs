using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class GoodsShelfInfo
    {
        [EntityPropertyExtension("RowNumber", "RowNumber")]
        public int RowNumber { get; set; }

        [EntityPropertyExtension("CellNumber", "CellNumber")]
        public int CellNumber { get; set; }

        [EntityPropertyExtension("LocationID", "LocationID")]
        public long LocationID { get; set; }

        [EntityPropertyExtension("AreaName", "AreaName")]
        public string AreaName { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("LevelsNumber", "LevelsNumber")]
        public long LevelsNumber { get; set; }

        [EntityPropertyExtension("SerialNumber", "SerialNumber")]
        public long SerialNumber { get; set; }

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

        [EntityPropertyExtension("CustomerID2", "CustomerID2")]
        public long CustomerID2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("WareHouseID2", "WareHouseID2")]
        public long WareHouseID2 { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("GoodsShelvesName", "GoodsShelvesName")]
        public string GoodsShelvesName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Levels", "Levels")]
        public string Levels { get; set; }

        /// <summary>
    
        /// </summary>
        [EntityPropertyExtension("Grids", "Grids")]
        public string Grids { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Weights", "Weights")]
        public string Weights { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Lengths", "Lengths")]
        public string Lengths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Widths", "Widths")]
        public string Widths { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Heights", "Heights")]
        public string Heights { get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }

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

        [EntityPropertyExtension("Int1", "Int1")]
        public int Int1 { get; set; }

        [EntityPropertyExtension("Int2", "Int2")]
        public int Int2 { get; set; }

        [EntityPropertyExtension("Int3", "Int3")]
        public int Int3 { get; set; }

        [EntityPropertyExtension("Int4", "Int4")]
        public int Int4 { get; set; }

        [EntityPropertyExtension("Int5", "Int5")]
        public int Int5 { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }

        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4 { get; set; }

        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5 { get; set; }

    }
}

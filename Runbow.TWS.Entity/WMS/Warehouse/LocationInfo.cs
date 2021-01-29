using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库位信息
    /// </summary>
    public class LocationInfo
    {
        [EntityPropertyExtension("GoodsShelvesName", "GoodsShelvesName")]
        public string GoodsShelvesName { get; set; }
        [EntityPropertyExtension("LevelsNumber", "LevelsNumber")]
        public long LevelsNumber { get; set; }
        [EntityPropertyExtension("SerialNumber", "SerialNumber")]
        public long SerialNumber { get; set; }
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("GoodsShelfID", "GoodsShelfID")]
        public long GoodsShelfID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        [EntityPropertyExtension("AreaID", "AreaID")]
        public long AreaID { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("AreaName", "AreaName")]
        public string AreaName { get; set; }
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 库位类型
        /// </summary>
        [EntityPropertyExtension("LocationType", "LocationType")]
        public int LocationType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [EntityPropertyExtension("LocationStatus", "LocationStatus")]
        public string LocationStatus { get; set; }
        /// <summary>
        /// 库位分类
        /// </summary>
        [EntityPropertyExtension("Classification", "Classification")]
        public int Classification { get; set; }
        /// <summary>
        /// 库位类别
        /// </summary>
        [EntityPropertyExtension("Category", "Category")]
        public int Category { get; set; }
        /// <summary>
        /// 库位handling
        /// </summary>
        [EntityPropertyExtension("Handling", "Handling")]
        public int Handling { get; set; }
        /// <summary>
        /// ABC分类
        /// </summary>
        [EntityPropertyExtension("ABCClassification", "ABCClassification")]
        public string ABCClassification { get; set; }
        /// <summary>
        /// 是否多批次
        /// </summary>
        [EntityPropertyExtension("IsMultiLot", "IsMultiLot")]
        public bool IsMultiLot { get; set; }
        /// <summary>
        /// 是否多货品
        /// </summary>
        [EntityPropertyExtension("IsMultiSKU", "IsMultiSKU")]
        public bool IsMultiSKU { get; set; }
        /// <summary>
        /// 货位级别
        /// </summary>
        [EntityPropertyExtension("LocationLevel", "LocationLevel")]
        public string LocationLevel { get; set; }
        /// <summary>
        /// 放货次序
        /// </summary>
        [EntityPropertyExtension("GoodsPutOrder", "GoodsPutOrder")]
        public string GoodsPutOrder { get; set; }
        /// <summary>
        /// 拣货次序
        /// </summary>
        [EntityPropertyExtension("GoodsPickOrder", "GoodsPickOrder")]
        public string GoodsPickOrder { get; set; }
        /// <summary>
        /// 体积
        /// </summary>
        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }
        /// <summary>
        /// 库位最大ID
        /// </summary>
        [EntityPropertyExtension("MaxID", "MaxID")]
        public string MaxID { get; set; }
        [EntityPropertyExtension("MaxNumber", "MaxNumber")]
        public string MaxNumber { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        [EntityPropertyExtension("Length", "Length")]
        public string Length { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [EntityPropertyExtension("Width", "Width")]
        public string Width { get; set; }
        /// <summary>
        ///高度
        /// </summary>
        [EntityPropertyExtension("Height", "Height")]
        public string Height { get; set; }
        /// <summary>
        ///X坐标
        /// </summary>
        [EntityPropertyExtension("X_Coordinate", "X_Coordinate")]
        public string X_Coordinate { get; set; }
        /// <summary>
        ///Y坐标
        /// </summary>
        [EntityPropertyExtension("Y_Coordinate", "Y_Coordinate")]
        public string Y_Coordinate { get; set; }
        /// <summary>
        ///Z坐标
        /// </summary>
        [EntityPropertyExtension("Z_coordinate", "Z_coordinate")]
        public string Z_coordinate { get; set; }
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
        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 是否整箱 (0零散 1整箱)
        /// </summary>
        [EntityPropertyExtension("Int1", "Int1")]
        public int Int1 { get; set; }
        /// <summary>
        /// 库位容积率
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        /// <summary>
        /// 项目所属
        /// </summary>
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
    }
}

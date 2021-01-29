using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
   public  class ScanInfo
    {
        /*ID */
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /*运单号*/
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        /*箱数*/
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public long BoxNumber { get; set; }

        /*已扫描箱数*/
        [EntityPropertyExtension("ScanBoxNumber", "ScanBoxNumber")]
        public long ScanBoxNumber { get; set; }


        /*拖号*/
        [EntityPropertyExtension("TrailerNo", "TrailerNo")]
        public string TrailerNo { get; set; }

        /*车牌号*/
        [EntityPropertyExtension("PlateNumber", "PlateNumber")]
        public string PlateNumber { get; set; }

        /*承运商ID*/
        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }

        /*承运商ID*/
        [EntityPropertyExtension("Shipper", "Shipper")]
        public string Shipper { get; set; }


        /*运单扫码关闭标记*/
        [EntityPropertyExtension("CloseFlag", "CloseFlag")]
        public int CloseFlag { get; set; }

        /*运单扫码完成次数标记*/
        [EntityPropertyExtension("CompleteFlag", "CompleteFlag")]
        public int CompleteFlag { get; set; }

        /*操作人*/
        [EntityPropertyExtension("Operator", "Operator")]
        public string Operator { get; set; }

        /*操作日期*/
        [EntityPropertyExtension("OperateTime", "OperateTime")]
        public DateTime OperateTime { get; set; }


        /*创建人*/
        [EntityPropertyExtension("Creater", "Creater")]
        public string Creater { get; set; }

        /*创建时间*/
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }


        /*修改人*/
        [EntityPropertyExtension("Modifier", "Modifier")]
        public string Modifier { get; set; }

        /*修改时间*/
        [EntityPropertyExtension("ModifyTime", "ModifyTime")]
        public DateTime ModifyTime { get; set; }

        /*备注*/
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
    }
}

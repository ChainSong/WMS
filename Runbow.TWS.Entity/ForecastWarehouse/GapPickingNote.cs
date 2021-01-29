using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.ForecastWarehouse
{
    public class GapPickingNote
    {
        #region Model
        [EntityPropertyExtension("ID", "ID")]
        public Int64 ID { get; set; }

        [EntityPropertyExtension("StoreCode", "StoreCode")]
        public string StoreCode { get; set; }

        [EntityPropertyExtension("StoreName", "StoreName")]
        public string StoreName { get; set; }

        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }

        [EntityPropertyExtension("TransferorReturn", "TransferorReturn")]
        public string TransferorReturn { get; set; }

        [EntityPropertyExtension("ServiceDetail", "ServiceDetail")]
        public string ServiceDetail { get; set; }

        [EntityPropertyExtension("CartonQuantity", "CartonQuantity")]
        public string CartonQuantity { get; set; }

        [EntityPropertyExtension("DestinationCode", "DestinationCode")]
        public string DestinationCode { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("Brand", "Brand")]
        public string Brand { get; set; }

        [EntityPropertyExtension("ExpectedDeliveryDate", "ExpectedDeliveryDate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [EntityPropertyExtension("ExpectedArrivalDate", "ExpectedArrivalDate")]
        public DateTime ExpectedArrivalDate { get; set; }

        [EntityPropertyExtension("CreatTime", "CreatTime")]
        public DateTime CreatTime { get; set; }
        #endregion
    }
}

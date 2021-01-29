using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class PodForecastInfo
    {
        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }

        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        [EntityPropertyExtension("EmailTitle", "EmailTitle")]
        public string EmailTitle { get; set; }

        [EntityPropertyExtension("EmailAddress", "EmailAddress")]
        public string EmailAddress { get; set; }

        [EntityPropertyExtension("EmailContent", "EmailContent")]
        public string EmailContent { get; set; }

        [EntityPropertyExtension("IDs", "IDs")]
        public string IDs { get; set; }
    }
}
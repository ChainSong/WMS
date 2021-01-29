using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class PodDescription
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }
    }
}
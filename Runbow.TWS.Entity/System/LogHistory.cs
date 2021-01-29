using System;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity
{
    public class LogHistory
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("Time", "Time")]
        public DateTime Time { get; set; }

        [EntityPropertyExtension("Action", "Action")]
        public string Action { get; set; }
    }
}

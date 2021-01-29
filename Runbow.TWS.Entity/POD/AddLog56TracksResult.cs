using System.Collections.Generic;

namespace Runbow.TWS.Entity
{
    public class AddLog56TracksResult
    {
        public IEnumerable<Log56Track> TracksHaveAdded { get; set; }

        public IEnumerable<Log56Track> TracksWithIssues { get; set; }

        public IEnumerable<Log56Track> TracksNotAdded { get; set; }
    }
}
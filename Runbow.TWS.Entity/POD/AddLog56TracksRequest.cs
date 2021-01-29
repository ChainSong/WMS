using System.Collections.Generic;

namespace Runbow.TWS.Entity
{
    public class AddLog56TracksRequest
    {
        public IEnumerable<Log56Track> UsefulTracks { get; set; }

        public IEnumerable<Log56Track> UselessTracks { get; set; }

        public IEnumerable<Log56Track> AllTracks { get; set; }
    }
}
using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class WXAccessToken
    {
        [EntityPropertyExtension("Access_token", "Access_token")]
        public string Access_token { get; set; }

        [EntityPropertyExtension("Expires_in", "Expires_in")]
        public DateTime Expires_in { get; set; }
    }
}

using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddAMSUploadRequest
    {
        public IEnumerable<AMSUpload> amsUpload { get; set; }

        public bool IsCoverOld { get; set; }

        public string Ids { get; set; }
    }
}

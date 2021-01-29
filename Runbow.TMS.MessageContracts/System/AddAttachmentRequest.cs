using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddAttachmentRequest
    {
        public IEnumerable<Attachment> attachments { get; set; }

        public bool IsCoverOld { get; set; }
    }
}
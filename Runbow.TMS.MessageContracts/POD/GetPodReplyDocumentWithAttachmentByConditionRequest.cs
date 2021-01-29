using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetPodReplyDocumentWithAttachmentByConditionRequest
    {
        public PodReplyDocumentSearchCondition SearchCondition { get; set; }

        public long ProjectID { get; set; }
    }
}
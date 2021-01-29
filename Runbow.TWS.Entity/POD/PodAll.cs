using System.Collections.Generic;

namespace Runbow.TWS.Entity
{
    public class PodAll
    {
        public Pod Pod { get; set; }

        public PodReplyDocument PodReplyDocument { get; set; }

        public PodFeadBack PodFeadBack { get; set; }

        public IEnumerable<PodDetail> PodDetails { get; set; }

        public IEnumerable<PodTrack> PodTracks { get; set; }

        public IEnumerable<PodStatusLog> PodStatusLogs { get; set; }

        public IEnumerable<PodException> PodExceptions { get; set; }

        public PodFee PodFee { get; set; }

        public IEnumerable<Pod> PodPod { get; set; }

        public IEnumerable<PodStatusTrack> PodStatusTracks { get; set; }

        public IEnumerable<PodReplyDocument> PodReplyDocuments { get; set; }

        public IEnumerable<PodFeadBack> PodFeadBacks { get; set; }

        public WXPODBarCode WXPODBarCode { get; set; }

        /// <summary>
        /// 百姓网订单对应的快递信息
        /// </summary>
        public Pod podBX { get; set; }
    }
}
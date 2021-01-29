namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class BatchEditPodsViewModel : QueryPodViewModel
    {
        public string ActionButtonName { get; set; }

        public long DestPodState { get; set; }

        public string DestPodStateName { get; set; }

        public bool IsSplit { get; set; }

        public bool IsPODDistributionVehicle { get; set; }

        public bool IsAllocateShipper { get; set; }

        public int? HasAllocateShipper { get; set; }

        public bool IsSettled { get; set; }

        public bool WaybillReach { get; set; }

        public int? SettltedType { get; set; }

        public bool IsExternFee { get; set; }

        public int ExternFeeType { get; set; }

        public bool IsManualSettled { get; set; }

        public int ManualSettledType { get; set; } 
    }
}
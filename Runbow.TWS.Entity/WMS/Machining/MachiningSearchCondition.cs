using System;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class MachiningSearchCondition : WMS_MachiningHeaderAndDetail
    {
        #region Model
    
        public DateTime?  StartExpectDate { get; set; }
        public DateTime?  EndExpectDate { get; set; }
      
        #endregion
    }
}

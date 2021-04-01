using Runbow.TWS.Entity.WMS.UnitAndSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.UnitAndSpecifications
{
    public class UnitAndSpecificationsModel
    {
        public UnitAndSpecificationsInfo unitAndSpecificationsInfo { get; set; }

        public IEnumerable<UnitAndSpecificationsInfo> unitAndSpecificationsInfos { get; set; }

        public IEnumerable<SelectListItem> CustomerItems
        {
            get;
            set; 
        }
    }
}
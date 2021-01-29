using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class SKUFloar
    {

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// Floar
        /// </summary>
        [EntityPropertyExtension("Floar", "Floar")]
        public string Floar { get; set; }

        




    }
}

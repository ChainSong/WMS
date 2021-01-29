using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class EditSettledPodRequest
    {
        public SettledPod SettledPod { get; set; }

        public string Updator { get; set; }

        public int SettledType { get; set; }
    }
}

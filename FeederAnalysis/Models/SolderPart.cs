using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class SolderPart
    {
        public string MATERIAL_ORDER_ID { get; set; }
        public string PART_ID { get; set; }
        public string UPN_ID { get; set; }
        public DateTime UNFREEZE_TIME { get; set; }
        public DateTime MIX_END_TIME { get; set; }
    }
}

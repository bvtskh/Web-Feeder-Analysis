using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class VerifyLoadedEntity
    {
        public string LINE_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string PART_ID { get; set; }
        public string MACHINE_ID { get; set; }
        public int MACHINE_SLOT { get; set; }
        public string PRODUCTION_ORDER_ID { get; set; }
    }
}
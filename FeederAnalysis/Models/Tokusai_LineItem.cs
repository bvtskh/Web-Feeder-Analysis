using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class Tokusai_LineItem
    {
        public string LINE_ID { get; set; }
        public string PART_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public DateTime UPD_TIME { get; set; }
        public bool IS_TOKUSAI { get; set; }
        public string WO { get; set; }
        public bool IS_DM_ACCEPT { get; set; }
    }
}
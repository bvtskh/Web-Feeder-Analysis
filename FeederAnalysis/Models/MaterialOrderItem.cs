using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class MaterialOrderItem
    {
        public string UPN_ID { get; set; }
        public string PART_ID { get; set; }
        public string FEEDER_ID { get; set; }
        public string LINE_ID { get; set; }
        public string MACHINE_ID { get; set; }
        public string MACHINE_SETTING_NAME { get; set; }
        public int MACHINE_SLOT { get; set; }
        public string PRODUCT_ID { get; set; }
        public string MATERIAL_ORDER_ID { get; set; }
        public string PRODUCTION_ORDER_ID { get; set; }
        public double QUANTITY { get; set; }
        public string COMPONENT_ID { get; set; }
    }
}

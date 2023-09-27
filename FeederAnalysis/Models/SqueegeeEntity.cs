using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class SqueegeeEntity
    {
        public string SQUEEGEE_NO { get; set; }
        public string MATERIAL_ORDER_ID { get; set; }
        public string SIDE { get; set; }
        public string LINE_ID { get; set; }
        public DateTime LOAD_TIME { get; set; }
        public string PRODUCT_ID { get; set; }
        public string PRODUCTION_ORDER_ID { get; set; }
        public double QUANTITY { get; set; }
        public int MAX_USE_COUNT { get; set; }
        public DateTime START_TIME { get; set; }
        public string VERSION { get; set; }
        public string CUSTOMER_ID { get; set; }
    }
}

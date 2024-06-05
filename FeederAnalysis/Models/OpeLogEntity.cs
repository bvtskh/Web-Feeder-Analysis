using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class OpeLogEntity
    {
        public string ALTER_PART_ID {  get; set; }
        public Guid ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string FAULT_REASON { get; set; }
        public string OPERATOR_NAME { get; set; }
        public DateTime OPERATE_TIME { get; set; }
        public string PRODUCTION_ORDER_ID { get; set; }
        public string PART_ID { get; set; }
        public string LINE_ID { get; set; }
        public string CUSTOMER_ID { get; set; }
        public int MACHINE_SLOT { get; set; }
        public string MACHINE_ID { get; set; }
        public int TASK { get; set; }
        public string MATERIAL_ORDER_ID { get; set; }
    }
}

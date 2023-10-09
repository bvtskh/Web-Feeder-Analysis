using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class Tokusai_LineHistory
    {
        public string ID { get; set; }
        public string LINE_ID { get; set; }
        public string PART_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public DateTime UPD_TIME { get; set; }
        public string CHANGE_NAME { get; set; }
        public int CHANGE_ID { get; set; }
        public bool IS_CONFIRM { get; set; }
        public string MATERIAL_ORDER_ID { get; set; }
        public int MACHINE_SLOT { get; set; }
        public string MACHINE_ID { get; set; }
        public bool IS_DM_ACCEPT { get; set; }
        public string WO { get; set; }
    }
}
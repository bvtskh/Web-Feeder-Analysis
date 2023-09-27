using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public partial class Tokusai_Item
    {
        [Key]
        [StringLength(64)]
        public string UPN_ID { get; set; }

        [StringLength(20)]
        public string EM_NO { get; set; }

        [StringLength(64)]
        public string MATERIAL_ORDER_ID { get; set; }

        [StringLength(64)]
        public string PART_ID { get; set; }

        public int MACHINE_SLOT { get; set; }

        [StringLength(64)]
        public string MACHINE_ID { get; set; }

        [StringLength(64)]
        public string PRODUCT_ID { get; set; }

        [StringLength(64)]
        public string LINE_ID { get; set; }

        public DateTime UPD_TIME { get; set; }

        public bool IS_FAILED { get; set; }

        public int ERR_TYPE { get; set; }

        public string PRODUCTION_ORDER_ID { get; set; }

        public double QUANTITY { get; set; }
        public Tokusai_Item()
        {
            IS_FAILED = false;
            UPD_TIME = DateTime.Now;
            EM_NO = string.Empty;
        }
    }
}

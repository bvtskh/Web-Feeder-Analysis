namespace FeederAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_Feeder
    {
        [Key]
        [StringLength(50)]
        public string FeederNo { get; set; }

        [StringLength(150)]
        public string FeederName { get; set; }

        [StringLength(50)]
        public string Vendor { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        public DateTime DatePlan { get; set; }

        [StringLength(150)]
        public string Status_TT { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Maxdayuse { get; set; }
    }
}

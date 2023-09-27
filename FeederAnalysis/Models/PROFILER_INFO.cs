namespace FeederAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROFILER_INFO
    {


        [StringLength(50)]
        public string UMC_NO { get; set; }

        public DateTime UPD_TIME { get; set; }


        [StringLength(50)]
        public string HOST_NAME { get; set; }

        [StringLength(50)]
        public string LOCATION { get; set; }
    }
}

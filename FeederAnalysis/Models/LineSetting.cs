namespace FeederAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LineSetting")]
    public partial class LineSetting
    {
        [Key]
        [StringLength(50)]
        public string LINE_ID { get; set; }

        public bool IS_CHECK_RELOAD { get; set; }

        public bool IS_CHECK_FEEDER { get; set; }

        public int FEEDER_DAY_USE { get; set; }

        [StringLength(200)]
        public string DES { get; set; }

        public DateTime UPDATE_TIME { get; set; }

        [Required]
        [StringLength(100)]
        public string UPDATER { get; set; }
    }
}

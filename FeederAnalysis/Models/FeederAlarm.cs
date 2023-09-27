namespace FeederAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeederAlarm")]
    public partial class FeederAlarm
    {
        [Key]
        [StringLength(50)]
        public string FEEDER_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LINE_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string MACHINE_SETTING { get; set; }

        [Required]
        [StringLength(50)]
        public string MACHINE_ID { get; set; }

        public int MACHINE_SLOT { get; set; }

        [Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EX_DATE { get; set; }

        public int STATE { get; set; }

        [Required]
        [StringLength(50)]
        public string ABOUT { get; set; }
        [Browsable(false)]
        public DateTime? UPD_TIME { get; set; } = DateTime.Now;
    }
}

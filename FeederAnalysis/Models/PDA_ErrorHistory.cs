namespace FeederAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PDA_ErrorHistory
    {
        public long id { get; set; }

        [Required]
        [StringLength(50)]
        public string SystemId { get; set; }

        public DateTime ErrorTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Line { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        [StringLength(50)]
        public string WO { get; set; }

        [StringLength(100)]
        public string PartCode { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ErrorContent { get; set; }

        [Required]
        [StringLength(50)]
        public string OperatorCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Customer { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public int? Slot { get; set; }

        public int? ProcessID { get; set; }
    }
}

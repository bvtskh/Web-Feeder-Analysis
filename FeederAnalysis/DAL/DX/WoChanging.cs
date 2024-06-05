namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WoChanging")]
    public partial class WoChanging
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(10)]
        public string ORDER_NO { get; set; }

        [StringLength(50)]
        public string ECO_NO { get; set; }

        public int QUANTITY { get; set; }

        [Column(TypeName = "date")]
        public DateTime DUE_DATE { get; set; }

        public DateTime UPD_DATE { get; set; }

        [StringLength(50)]
        public string UPDATER_NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string HOSTNAME { get; set; }

        public int TYPE_ID { get; set; }

        [Required]
        [StringLength(30)]
        public string TYPE_NAME { get; set; }

        public int DEPT_ORD { get; set; }

        [Required]
        [StringLength(50)]
        public string DEPT_NAME { get; set; }

        public int PE_OPTION { get; set; }
    }
}

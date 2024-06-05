namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WoChangingHistory")]
    public partial class WoChangingHistory
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ORDER_NO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ECO_NO { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QUANTITY { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime DUE_DATE { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime UPD_DATE { get; set; }

        [StringLength(50)]
        public string UPDATER_NAME { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string HOSTNAME { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TYPE_ID { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(30)]
        public string TYPE_NAME { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DEPT_ORD { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string DEPT_NAME { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PE_OPTION { get; set; }
    }
}

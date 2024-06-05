namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Area")]
    public partial class Area
    {
        public int ID { get; set; }

        [Column("AREA")]
        [StringLength(20)]
        public string AREA1 { get; set; }

        [StringLength(20)]
        public string CUSTOMER { get; set; }
    }
}

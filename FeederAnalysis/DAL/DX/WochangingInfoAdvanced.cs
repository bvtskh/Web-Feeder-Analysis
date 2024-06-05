namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WochangingInfoAdvanced")]
    public partial class WochangingInfoAdvanced
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ECO_NO { get; set; }

        [StringLength(100)]
        public string CUSTOMER { get; set; }

        [StringLength(100)]
        public string MODEL { get; set; }
    }
}

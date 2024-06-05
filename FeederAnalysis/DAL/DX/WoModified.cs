namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WoModified")]
    public partial class WoModified
    {
        public int Id { get; set; }

        public Guid? WoChangingID { get; set; }

        [StringLength(30)]
        public string OldWO { get; set; }

        [StringLength(30)]
        public string NewWO { get; set; }

        [StringLength(50)]
        public string ECO { get; set; }

        public DateTime? UPDATE_TIME { get; set; }

        [StringLength(50)]
        public string HOSTNAME { get; set; }

        [StringLength(50)]
        public string UPDATER { get; set; }
    }
}

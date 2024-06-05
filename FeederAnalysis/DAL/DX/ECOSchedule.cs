namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ECOSchedule")]
    public partial class ECOSchedule
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IMPLEMENT_DATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RECEIVE_DOCUMENT_DATE { get; set; }

        [StringLength(50)]
        public string MODEL { get; set; }

        [StringLength(300)]
        public string CONTENT_CHANGE { get; set; }

        [StringLength(50)]
        public string ECN_DCI_NO { get; set; }

        [Column(TypeName = "date")]
        public DateTime? START_APPROVE_DATE { get; set; }

        [StringLength(50)]
        public string ECO_NO { get; set; }

        [StringLength(300)]
        public string REMARK { get; set; }
    }
}

namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WO_Relationship
    {
        public int ID { get; set; }

        [StringLength(30)]
        public string ORDER_NO { get; set; }

        public int? TYPE_ID { get; set; }

        [StringLength(30)]
        public string TYPE_NAME { get; set; }

        public Guid? ORDER_BASE { get; set; }

        public int? QTY { get; set; }

        [StringLength(50)]
        public string UPDATE_NAME { get; set; }

        [StringLength(50)]
        public string HOST_NAME { get; set; }

        public DateTime? UPDATE_TIME { get; set; }

        public int? CHANGE_ID { get; set; }

        [StringLength(30)]
        public string CHANGE_NAME { get; set; }
    }
}

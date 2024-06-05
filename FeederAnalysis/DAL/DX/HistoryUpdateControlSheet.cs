namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistoryUpdateControlSheet")]
    public partial class HistoryUpdateControlSheet
    {
        public int Id { get; set; }

        public int? Last_Update_Id { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}

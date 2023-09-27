namespace FeederAnalysis.DAL.LCA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Bet.Util.Extension;
    using FeederAnalysis.Business;

    public partial class Tbl_Mankichi
    {
        public long Id { get; set; }
        public int Code  { get; set; }
        public string AltCode { get; set; }

        [StringLength(80)]
        public string Name { get; set; } = "";

        public DateTime? EnterDate { get; set; }

        public DateTime? QuitJob { get; set; }

        [StringLength(50)]
        public string Dept { get; set; } = "";

        [StringLength(50)]
        public string Position { get; set; } = "";

        public DateTime? Birthday { get; set; }

        [StringLength(50)]
        public string Sex { get; set; } = "";

        [StringLength(50)]
        public string EduLevel { get; set; } = "";

        [StringLength(20)]
        public string TinhCode { get; set; } = "";

        [StringLength(20)]
        public string HuyenCode { get; set; } = "";

        [StringLength(20)]
        public string XaCode { get; set; } = "";

        [StringLength(50)]
        public string MaritalStatus { get; set; } = "";

        [StringLength(100)]
        public string Customer { get; set; } = "";

        [StringLength(200)]
        public string Address { get; set; } = "";

        public int? Distance { get; set; }
        public string CaLV { get; set; }
    }
}

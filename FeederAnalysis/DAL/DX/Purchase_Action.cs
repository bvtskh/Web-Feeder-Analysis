namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Purchase_Action
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Action_Date { get; set; }

        public int? ECO_ControlSheet_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(200)]
        public string ECN { get; set; }

        [StringLength(100)]
        public string ECO_No { get; set; }

        [StringLength(300)]
        public string Old_Part_BOM1 { get; set; }

        [StringLength(200)]
        public string New_Part { get; set; }

        public double? Old_Price { get; set; }

        public double? New_Price { get; set; }

        [StringLength(100)]
        public string Old_Vendor { get; set; }

        [StringLength(100)]
        public string New_Vendor { get; set; }

        [StringLength(200)]
        public string Report { get; set; }

        [StringLength(300)]
        public string Reason { get; set; }

        [StringLength(200)]
        public string ETA_UMCVN { get; set; }

        [StringLength(150)]
        public string Est_Using_NewPart_Date { get; set; }

        [StringLength(200)]
        public string Transfered_ECO_Date { get; set; }

        [StringLength(300)]
        public string Purpose_Transfered_ECO { get; set; }
    }
}

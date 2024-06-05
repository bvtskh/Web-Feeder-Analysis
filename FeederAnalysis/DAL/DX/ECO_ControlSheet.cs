namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ECO_ControlSheet
    {
        public int Id { get; set; }

        public int? No { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ECN_ReceiveDate { get; set; }

        [StringLength(200)]
        public string ECN_ERI_No { get; set; }

        [StringLength(5)]
        public string History { get; set; }

        [StringLength(5)]
        public string Ver_EE { get; set; }

        [StringLength(5)]
        public string Ver_EA { get; set; }

        [StringLength(70)]
        public string Apply { get; set; }

        [StringLength(200)]
        public string VE_Follows_ECN_CVN { get; set; }

        [StringLength(50)]
        public string ECO_No { get; set; }

        [StringLength(100)]
        public string ModelName { get; set; }

        [StringLength(300)]
        public string OldPartCode { get; set; }

        [StringLength(200)]
        public string NewPartCode { get; set; }

        [StringLength(300)]
        public string ContentOfChange { get; set; }

        [StringLength(600)]
        public string Location { get; set; }

        [StringLength(50)]
        public string ProcessForAssembly { get; set; }

        [StringLength(150)]
        public string ECO_Issue { get; set; }

        [StringLength(200)]
        public string FAT_Implement { get; set; }

        [StringLength(200)]
        public string ImplementDate { get; set; }

        [StringLength(200)]
        public string ShippingDate { get; set; }

        [StringLength(200)]
        public string ECO_Status { get; set; }

        [StringLength(30)]
        public string Confirm { get; set; }

        [StringLength(50)]
        public string Issue_To { get; set; }

        [StringLength(700)]
        public string FATContentInformation { get; set; }

        [StringLength(100)]
        public string ModelFull { get; set; }

        [StringLength(150)]
        public string TVP_No { get; set; }

        [StringLength(150)]
        public string TVPResult { get; set; }

        [StringLength(150)]
        public string TVP_RecieveResultDate { get; set; }

        [StringLength(150)]
        public string BOM_Change_When_ECO_Implement { get; set; }
    }
}

namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string USERNAME { get; set; }

        [Required]
        [StringLength(50)]
        public string PASSWORD { get; set; }

        [StringLength(100)]
        public string FULLNAME { get; set; }

        [Required]
        [StringLength(10)]
        public string CODE { get; set; }

        public int? ACCOUNT_TYPE { get; set; }

        [StringLength(200)]
        public string ACCESS { get; set; }
    }
}

namespace FeederAnalysis.DAL.DX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Model_Family
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(100)]
        public string Model_Type { get; set; }

        [StringLength(100)]
        public string Origin_Model { get; set; }

        [StringLength(100)]
        public string Branch_Model { get; set; }
    }
}

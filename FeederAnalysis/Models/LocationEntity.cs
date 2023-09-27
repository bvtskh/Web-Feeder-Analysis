using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    [Table("Location")]
    public partial class LocationEntity
    {
        [Key]
        [StringLength(50)]
        public string LineId { get; set; }

        [StringLength(50)]
        public string LocationId { get; set; }

    }
}

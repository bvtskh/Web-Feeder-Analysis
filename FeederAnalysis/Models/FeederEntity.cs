using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class FeederEntity
    {
        public string FeederNo { get; set; }
        public DateTime DatePlan { get; set; }
    }
}

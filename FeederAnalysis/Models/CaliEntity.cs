using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class CaliEntity
    {
        public string SERIAL { get; set; }
        public string PART_NO { get; set; }
        public string MODEL { get; set; }
        public DateTime CALI_RECOMMEND { get; set; }
    }
}

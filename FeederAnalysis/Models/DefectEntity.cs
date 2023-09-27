using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class DefectEntity
    {
        public string BOARD_NO { get; set; }
        public string PRODUCT_ID { get; set; }
        public string DEFECT_CODE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string STATION_NO { get; set; }
        public DateTime UPDATE_TIME { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class PartQuantityModel
    {
        public string PART_ID { get; set; }
        public string UPN_ID { get; set; }
        public string PO_NO { get; set; }
        public int PO_LINE { get; set; }
        public double QUANTITY { get; set; }
        public DateTime REC_DATE { get; set; }
        public string TN_NO { get; set; }
        public string STD_CODE { get; set; }
    }
}
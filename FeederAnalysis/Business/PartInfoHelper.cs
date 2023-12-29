using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class PartInfoHelper
    {
        internal void PartQuantity_Update()
        {
            Stopwatch t = new Stopwatch();
            t.Start();
           
            var partSMT = Repository.ShowPartSMT();
            var stockSMT = Repository.GetQuantitySMT(partSMT);

            DataTable dt = new DataTable();
            dt.Columns.Add("UPN_ID", typeof(string));
            dt.Columns.Add("PART_ID", typeof(string));
            dt.Columns.Add("QUANTITY", typeof(float));
            foreach (var material in stockSMT)
            {
                dt.Rows.Add(new object[] {
                    material.UPN_ID, material.PART_ID, material.CURRENT_QUANTITY});
            }
            var stockInSAP = Repository.ShowPartQuantity(dt);
            Repository.UpdateStock(stockInSAP);
            t.Stop();
            Console.Write(t.ElapsedMilliseconds);
        }
    }
}
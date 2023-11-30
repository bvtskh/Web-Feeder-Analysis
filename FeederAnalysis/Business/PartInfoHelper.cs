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
            var currentMaterials = Repository.ShowPartQuantityInUMes();
            DataTable dt = new DataTable();
            dt.Columns.Add("UPN_ID", typeof(string));
            dt.Columns.Add("PART_ID", typeof(string));
            dt.Columns.Add("QUANTITY", typeof(float));
            foreach (var material in currentMaterials)
            {
                if(material.UPN_ID == "NP8Q1FR")
                {
                    Console.Write("");
                }
                dt.Rows.Add(new object[] {
                    material.UPN_ID,  material.PART_ID,material.QUANTITY});
            }
            var result = Repository.ShowPartQuantity(dt);
            Repository.UpdateStock(result);
            t.Stop();
            Console.Write(t.ElapsedMilliseconds);
        }
    }
}
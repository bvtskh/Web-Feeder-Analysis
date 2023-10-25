using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class VerifyOrderItemHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal void LoadedOrderItem()
        {
            try
            {
                var currentMaterials = Repository.FindAllMaterialItem();
                DataTable dt = new DataTable();
                dt.Columns.Add("LINE_ID", typeof(string));
                dt.Columns.Add("PRODUCTION_ORDER_ID", typeof(string));
                dt.Columns.Add("PART_ID", typeof(string));
                dt.Columns.Add("MACHINE_ID", typeof(string));
                dt.Columns.Add("MACHINE_SLOT", typeof(int));
                dt.Columns.Add("UPD_TIME", typeof(DateTime));
                dt.Columns.Add("PRODUCT_ID", typeof(string));
                dt.Columns.Add("IS_VERIFIED", typeof(bool));
                foreach (var material in currentMaterials)
                {
                    dt.Rows.Add(new object[] {
                    material.LINE_ID,  material.PRODUCTION_ORDER_ID,material.PART_ID,
                    material.MACHINE_ID,material.MACHINE_SLOT,DateTime.Now,material.PRODUCT_ID,false});
                }
                Repository.LoadedOrderItem_Update(dt);

            }
            catch (Exception ex)
            {
                log.Error("LoadedOrderItem", ex);
            }

        }

        internal void VerifiedOrderItem_Update()
        {
            try
            {
                Stopwatch t = new Stopwatch();
                t.Start();
                var currentMaterials = Repository.FindVerifiedOrderItem()
                     .GroupBy(c => new
                     {
                         c.LINE_ID,
                         c.PRODUCT_ID,
                         c.PART_ID,
                         c.MACHINE_ID,
                         c.MACHINE_SLOT
                     }).Select(gcs => new
                     {
                         LINE_ID = gcs.Key.LINE_ID,
                         PRODUCT_ID = gcs.Key.PRODUCT_ID,
                         PART_ID = gcs.Key.PART_ID,
                         MACHINE_ID = gcs.Key.MACHINE_ID,
                         MACHINE_SLOT = gcs.Key.MACHINE_SLOT,
                         LIST = gcs.ToList(),
                     });
                DataTable dt = new DataTable();
                dt.Columns.Add("LINE_ID", typeof(string));
                dt.Columns.Add("WO", typeof(string));
                dt.Columns.Add("PART_ID", typeof(string));
                dt.Columns.Add("MACHINE_ID", typeof(string));
                dt.Columns.Add("MACHINE_SLOT", typeof(string));
                dt.Columns.Add("UPD_TIME", typeof(DateTime));
                dt.Columns.Add("PRODUCT_ID", typeof(string));
                dt.Columns.Add("IS_VERIFIED", typeof(bool));
                foreach (var material in currentMaterials)
                {
                    var firstItem = material.LIST.FirstOrDefault();
                    dt.Rows.Add(new object[] {
                    material.LINE_ID,  firstItem.PRODUCTION_ORDER_ID,material.PART_ID,
                    material.MACHINE_ID,material.MACHINE_SLOT,DateTime.Now,material.PRODUCT_ID,false
                });
                }
                Repository.VerifiedOrderItem_Update(dt);
                t.Stop();
                Debug.WriteLine(t.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                log.Error("VerifiedOrderItem_Update", ex);
            }

        }


    }
}
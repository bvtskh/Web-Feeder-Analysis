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
                dt.Columns.Add("UPD_VERIFY_TIME", typeof(DateTime));
                foreach (var material in currentMaterials)
                {
                    dt.Rows.Add(new object[] {
                    material.LINE_ID,  material.PRODUCTION_ORDER_ID,material.PART_ID,
                    material.MACHINE_ID,material.MACHINE_SLOT,DateTime.Now,material.PRODUCT_ID,false,DateTime.Now});
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
                var currentMaterials = Repository.FindVerifiedOrderItem().ToList();
                var currentLoadedOrderItem = Repository.FindLoadedOrderItem();
                foreach (var item in currentLoadedOrderItem)
                {
                    if(currentMaterials.Where(m => m.LINE_ID == item.LINE_ID && m.PART_ID == item.PART_ID
                      && m.PRODUCT_ID == item.PRODUCT_ID 
                      && m.MACHINE_ID == item.MACHINE_ID
                      && m.MACHINE_SLOT == item.MACHINE_SLOT).FirstOrDefault() != null)
                    {
                        Repository.UpdateVerifiedOrderItem(item);
                    }
                }
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
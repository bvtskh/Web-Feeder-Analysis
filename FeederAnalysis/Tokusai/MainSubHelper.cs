using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using FeederAnalysis.Business;
using FeederAnalysis.Cache;
using FeederAnalysis.Models;
using Newtonsoft.Json.Linq;

namespace FeederAnalysis.Tokusai
{
    public class MainSubHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void MainSub_LineItem_Update()
        {
            try
            {
                Stopwatch t = new Stopwatch();
                t.Start();
                var currentMaterials = Repository.FindAllMaterialItem();
                var listPartMainSub = Repository.GetAllPartMainSub();
                var materials = currentMaterials
                    .Join(listPartMainSub, material => material.PART_ID,
                    mainsub => mainsub.PART_FROM,
                    (material, mainsub) => new MaterialOrderItem
                    {
                        PART_ID = material.PART_ID,
                        LINE_ID = material.LINE_ID,
                        PRODUCT_ID = material.PRODUCT_ID,
                        PRODUCTION_ORDER_ID = material.PRODUCTION_ORDER_ID,
                        MATERIAL_ORDER_ID = material.MATERIAL_ORDER_ID,
                        ALTER_PART_ID = material.ALTER_PART_ID
                    }).ToList();
                var currentMaterialGroupByPart = materials
                    .GroupBy(c => new
                    {
                        c.LINE_ID,
                        c.PRODUCT_ID,
                        c.PART_ID
                    }).Select(gcs => new
                    {
                        LINE_ID = gcs.Key.LINE_ID,
                        PRODUCT_ID = gcs.Key.PRODUCT_ID,
                        PART_ID = gcs.Key.PART_ID,
                        LIST = gcs.ToList(),
                    });
                DataTable dtMainSub = new DataTable();
                dtMainSub.Columns.Add("LINE_ID", typeof(string));
                dtMainSub.Columns.Add("PART_ID", typeof(string));
                dtMainSub.Columns.Add("PRODUCT_ID", typeof(string));
                dtMainSub.Columns.Add("UPD_TIME", typeof(DateTime));
                dtMainSub.Columns.Add("WO", typeof(string));
                dtMainSub.Columns.Add("MATERIAL_ORDER_ID", typeof(string));
                dtMainSub.Columns.Add("ALTER_PART_ID", typeof(string));
                foreach (var material in currentMaterialGroupByPart)
                {
                    var firstItem = material.LIST.FirstOrDefault();
                    dtMainSub.Rows.Add(new object[] {
                    material.LINE_ID,  firstItem.PART_ID,material.PRODUCT_ID,
                    DateTime.Now,firstItem.PRODUCTION_ORDER_ID,firstItem.MATERIAL_ORDER_ID,firstItem.ALTER_PART_ID});
                }
                // check chuyển đổi WO
                var currentMaterialGroupByModel = materials.GroupBy(m => new
                {
                    m.LINE_ID,
                    m.PRODUCT_ID
                }).Select(gcs => new
                {
                    LINE_ID = gcs.Key.LINE_ID,
                    PRODUCT_ID = gcs.Key.PRODUCT_ID,
                    LIST_PART = gcs.ToList(),
                }).ToList();
                foreach (var item in currentMaterialGroupByModel)
                {
                    var listOldPart = Repository.GetAllPartLineItem(item.LINE_ID, item.PRODUCT_ID);
                    var listNewPart = item.LIST_PART.Select(s => s.PART_ID).ToList();
                    foreach(var model in listPartMainSub)
                    {
                        if (IsChangeMainSub(model, listOldPart, listNewPart))
                        {
                            Repository.MainSubSave(item.LIST_PART.FirstOrDefault(), model.PART_FROM, model.PART_TO);
                        }
                    }
                   
                }

                Repository.MainSub_LineItem_Update(dtMainSub);
                t.Stop();
                log.Debug("MainSub_LineItem_Update_Finish_" + t.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                log.Error("MainSub_LineItem_Update", ex);
            }

        }

        private bool IsChangeMainSub(MainSub_Model model, List<string> listOldPart, List<string> listNewPart)
        {
            if (listOldPart.Contains(model.PART_FROM) && listNewPart.Contains(model.PART_TO)) return true;
            return false;
        }
     

    }
}

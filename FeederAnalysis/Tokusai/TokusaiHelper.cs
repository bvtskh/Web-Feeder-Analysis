﻿using System;
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
    public class TokusaiHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MaterialOrderItem> currentMaterials = new List<MaterialOrderItem>();
        private DateTime currentDate;
        public List<Tokusai_Item> GetAll()
        {
            List<Tokusai_Item> result = new List<Tokusai_Item>();
            var materialItem = Repository.FindAllMaterialItem();
            foreach (var item in materialItem.Where(r => r.UPN_ID != ""))
            {
                var upnEntity = UpnCache.FindBc(item.UPN_ID);
                var entity = new Tokusai_Item()
                {
                    LINE_ID = item.LINE_ID,
                    UPN_ID = item.UPN_ID,
                    MACHINE_ID = item.MACHINE_ID,
                    MACHINE_SLOT = item.MACHINE_SLOT,
                    MATERIAL_ORDER_ID = item.MATERIAL_ORDER_ID,
                    PRODUCT_ID = item.PRODUCT_ID,
                    PART_ID = item.PART_ID,
                    EM_NO = upnEntity.emNo,
                    PRODUCTION_ORDER_ID = item.PRODUCTION_ORDER_ID,
                    QUANTITY = item.QUANTITY
                };

                // Là tokusai
                if (!string.IsNullOrEmpty(upnEntity.emNo))
                {
                    // danh sách model được DM cho phép sử dụng
                    var lstModelAccept = SingletonHelper.ErpInstance.FindTokusai(upnEntity.emNo, upnEntity.partFm, upnEntity.partTo).Select(r => r.PRODUCT_ID).ToList();

                    var asm = SingletonHelper.ErpInstance.FGFind(item.PRODUCT_ID);
                    var productCompare = item.PRODUCT_ID;
                    if (asm != null)
                    {
                        lstModelAccept.Add(Utility.MakePCB(asm.PCBA1));
                        lstModelAccept.Add(Utility.MakePCB(asm.PCBA2));
                        productCompare = Utility.MakePCB(item.COMPONENT_ID);
                    }


                    if (lstModelAccept.Count != 0 && lstModelAccept.All(r => !r.Equals(productCompare, StringComparison.OrdinalIgnoreCase)))
                    {
                        entity.IS_FAILED = true;
                        entity.ERR_TYPE = 1;
                    }
                    result.Add(entity);
                }
                //else
                //{
                //    partTN.Add(upnEntity.partNo);
                //}
                //var flag = item.PRODUCT_ID.Contains("RM3-8495") && partTN.Any(r => string.Equals(r, "VE5-2100-332", StringComparison.OrdinalIgnoreCase)) && !TokusaiFn();
                //if (flag)
                //{
                //    entity.IS_FAILED = true;
                //    entity.ERR_TYPE = 2;
                //    lst1.Add(entity);
                //}
            }
            materialItem.Where(r => r.PRODUCT_ID.Contains("RM3-8495")).Where(r => r.PART_ID == "VE5-2100-332").ToList().ForEach(r =>
            {
                result.Add(new Tokusai_Item()
                {
                    EM_NO = "#NA",
                    PART_ID = r.PART_ID,
                    ERR_TYPE = 2,
                    UPN_ID = r.UPN_ID,
                    LINE_ID = r.LINE_ID,
                    MACHINE_ID = r.MACHINE_ID,
                    MACHINE_SLOT = r.MACHINE_SLOT,
                    MATERIAL_ORDER_ID = r.MATERIAL_ORDER_ID,
                    PRODUCTION_ORDER_ID = r.PRODUCTION_ORDER_ID,
                    PRODUCT_ID = r.PRODUCT_ID,
                    QUANTITY = r.QUANTITY,
                    IS_FAILED = true
                });
            });

            return result;
        }
        private void ClearTokusai()
        {
            using (DataContext context = new DataContext())
            {
                context.Database.ExecuteSqlCommand($"TRUNCATE TABLE [SMT].[dbo].[Tokusai_Item]");
                context.SaveChanges();
            }
        }
        private void InsertTokusai(List<Tokusai_Item> lst)
        {
            using (DataContext context = new DataContext())
            {
                foreach (var item in lst)
                {
                    string sql = $@"IF NOT EXISTS (SELECT * FROM [SMT].[dbo].[Tokusai_Item] WHERE UPN_ID = '{item.UPN_ID}' )
                                    INSERT INTO [SMT].[dbo].[Tokusai_Item] 
                                    VALUES(
                                    '{item.LINE_ID}',
                                    '{item.UPN_ID}','{item.EM_NO}','{item.MATERIAL_ORDER_ID}',
                                    '{item.PART_ID}','{item.MACHINE_SLOT}','{item.MACHINE_ID}',
                                    '{item.PRODUCT_ID}','{item.IS_FAILED}',
                                    '{item.ERR_TYPE}','{item.PRODUCTION_ORDER_ID}','{item.QUANTITY}',
                                    '{item.UPD_TIME}'
                                    )";
                    context.Database.ExecuteSqlCommand(sql);
                }
                //context.TokusaiItems.AddRange(lst);
                context.SaveChanges();
            }
        }
        private void WriteLog(List<Tokusai_Item> lst)
        {
            lst.Where(r => r.IS_FAILED).ToList().ForEach(r =>
            {
                JObject @object = new JObject();
                @object.Add("ERR_NAME", "TOKUSAI_NG");
                @object.Add("ERR_TYPE", "1");
                @object.Add("UPN_ID", r.UPN_ID);
                @object.Add("PART_ID", r.PART_ID);
                @object.Add("MACHINE_SLOT", r.MACHINE_SLOT);
                @object.Add("MACHINE_ID", r.MACHINE_ID);
                @object.Add("PRODUCT_ID", r.PRODUCT_ID);
                @object.Add("MATERIAL_ORDER_ID", r.MATERIAL_ORDER_ID);
                @object.Add("LINE_ID", r.LINE_ID);
                @object.Add("PRODUCTION_ORDER_ID", r.PRODUCTION_ORDER_ID);
                @object.Add("QUANTITY", r.QUANTITY);
                log.Error(@object.ToString());
            });
        }
        public void Save()
        {
            try
            {
                var lst = GetAll();
                ClearTokusai();
                InsertTokusai(lst);
                WriteLog(lst);
            }
            catch (Exception ex)
            {
                log.Error("Tokusai Job Err:", ex);
            }
        }
        public void Tokusai_LineItem_Change()
        {
            try
            {
                var currentLines = Repository.FindAllMaterialItemChange();
                if (currentDate == DateTime.MinValue)
                {
                    var maxDate = Repository.GetMaxTokusaiUpdate();
                    currentLines = currentLines.Where(m => m.OPERATE_TIME > maxDate).ToList();
                }
                foreach (var item in currentLines)
                {
                    var upnEntity = IsTokusai(item);
                    var upnOldEntity = IsTokusai(item);
                    if (upnOldEntity != upnEntity)
                    {
                        if (upnEntity) item.CHANGE_ID = 1;
                        else item.CHANGE_ID = 0;
                        Repository.TokusaiSave(item);
                    }
                }
                currentDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                log.Error("Tokusai_LineItem_Change", ex);
            }

        }
        public void Tokusai_LineItem_Update()
        {
            try
            {
                Stopwatch t = new Stopwatch();
                currentMaterials = Repository.FindAllMaterialItem();
                var listCurrentMaterials = currentMaterials
               .GroupBy(c => new
               {
                   c.LINE_ID,
                   c.PART_ID,
               }).Select(gcs => new
               {
                   LINE_ID = gcs.Key.LINE_ID,
                   PART_ID = gcs.Key.PART_ID,
                   LIST = gcs.ToList(),
               });

                DataTable dt = new DataTable();
                dt.Columns.Add("LINE_ID", typeof(string));
                dt.Columns.Add("PART_ID", typeof(string));
                dt.Columns.Add("PRODUCT_ID", typeof(string));
                dt.Columns.Add("UPD_TIME", typeof(DateTime));
                dt.Columns.Add("IS_TOKUSAI", typeof(bool));
                dt.Columns.Add("WO", typeof(string));
                dt.Columns.Add("IS_DM_ACCEPT", typeof(bool));
                dt.Columns.Add("MATERIAL_ORDER_ID", typeof(string));

                foreach (var material in listCurrentMaterials)
                {
                    MaterialOrderItem upnFirst = material.LIST.FirstOrDefault();
                    dt.Rows.Add(new object[] {
                    material.LINE_ID,  material.PART_ID,upnFirst.PRODUCT_ID,
                    DateTime.Now,IsTokusai(material.LIST),upnFirst.PRODUCTION_ORDER_ID, IsDMAccept(material.LIST),upnFirst.MATERIAL_ORDER_ID});
                }
                Repository.Tokusai_LineItem_Update(dt);
                System.Diagnostics.Debug.WriteLine(t.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                log.Error("Ope Job Err", ex);
            }

        }

        public void MainSub_LineItem_Update()
        {
            try
            {
                currentMaterials = Repository.FindAllMaterialItem();
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

        private bool IsTokusai(List<MaterialOrderItem> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var upnEntity = UpnCache.FindBc(item.UPN_ID);
                    if (!string.IsNullOrEmpty(upnEntity.emNo)) return true;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("");
            }

            return false;
        }

        private bool IsTokusai(FindAllMaterialOrderItemChange item)
        {
            try
            {
                var upnEntity = UpnCache.FindBc(item.UPN_ID);
                if (!string.IsNullOrEmpty(upnEntity.emNo)) return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine("");
            }

            return false;
        }

        private bool IsDMAccept(List<MaterialOrderItem> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var upnEntity = UpnCache.FindBc(item.UPN_ID);
                    if (!string.IsNullOrEmpty(upnEntity.emNo))
                    {
                        var lstModelAccept = SingletonHelper.ErpInstance.FindTokusai(upnEntity.emNo, upnEntity.partFm, upnEntity.partTo).Select(r => r.PRODUCT_ID).ToList();
                        if (lstModelAccept.Count != 0 &&
                            !lstModelAccept.Contains(item.PRODUCT_ID))
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }

            return true;
        }

    }
}

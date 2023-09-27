using Quartz;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using FeederAnalysis.Models;
using Newtonsoft.Json.Linq;
using FeederAnalysis.Tokusai;

namespace FeederAnalysis.Business
{
    public class TokusaiJob : IJob
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(IJobExecutionContext context)
        {
           new TokusaiHelper().Save();
            //try
            //{
            //log.Debug($"Tokusai Start At {DateTime.Now}");
            //var lst = Repository.FindTokusai();
            //Repository.SaveTokusai(lst);
            //TokusaiHelper.Save();
            //lst.Where(r => r.IS_FAILED).ToList().ForEach(r =>
            //{
            //    JObject @object = new JObject();
            //    @object.Add("ERR_NAME", "TOKUSAI_NG");
            //    @object.Add("ERR_TYPE", "1");
            //    @object.Add("UPN_ID", r.UPN_ID);
            //    @object.Add("PART_ID", r.PART_ID);
            //    @object.Add("MACHINE_SLOT", r.MACHINE_SLOT);
            //    @object.Add("MACHINE_ID", r.MACHINE_ID);
            //    @object.Add("PRODUCT_ID", r.PRODUCT_ID);
            //    @object.Add("MATERIAL_ORDER_ID", r.MATERIAL_ORDER_ID);
            //    @object.Add("LINE_ID", r.LINE_ID);
            //    @object.Add("PRODUCTION_ORDER_ID", r.PRODUCTION_ORDER_ID);
            //    @object.Add("QUANTITY", r.QUANTITY);
            //    log.Error(@object.ToString());
            //});
            // log.Debug($"Tokusai Job Finish At {DateTime.Now}");
            //}
            //catch (Exception ex)
            //{
            //log.Error("Tokusai Job Err:", ex);
            // Debug.WriteLine(ex.Message);
            //}
        }
    }
}

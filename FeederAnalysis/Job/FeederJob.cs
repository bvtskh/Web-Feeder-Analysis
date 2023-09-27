using Quartz;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using FeederAnalysis.Models;

namespace FeederAnalysis.Business
{
    public class FeederJob : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static int FeederUse = 10;
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var materialItem = Repository.FindAllMaterialItem().Where(r => !string.IsNullOrEmpty(r.FEEDER_ID));
                var allFeeder = FeederHelper.GetAllFeeder();
                DateTime dateCurr = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                List<FeederAlarm> feederAlarms = new List<FeederAlarm>();
                foreach (var item in materialItem)
                {
                    var feeder = allFeeder.FirstOrDefault(r => r.FeederNo.Equals(item.FEEDER_ID, StringComparison.OrdinalIgnoreCase));
                    if (feeder != null)
                    {
                        var datePlan = feeder.DatePlan.AddDays(1);
                        if (DateTime.Compare(dateCurr, Convert.ToDateTime(datePlan)) >= 0)
                        {
                            feederAlarms.Add(new FeederAlarm()
                            {
                                FEEDER_ID = item.FEEDER_ID,
                                LINE_ID = item.LINE_ID,
                                EX_DATE = feeder.DatePlan,
                                MACHINE_ID = item.MACHINE_ID,
                                MACHINE_SETTING = item.MACHINE_SETTING_NAME,
                                MACHINE_SLOT = item.MACHINE_SLOT,
                                ABOUT = "Over",
                                STATE = 3
                            });
                        }
                        else if (DateTime.Compare(dateCurr.AddDays(FeederUse), Convert.ToDateTime(datePlan)) >= 0)
                        {
                            string about = $"{datePlan.Subtract(dateCurr).TotalDays} days";
                            feederAlarms.Add(new FeederAlarm()
                            {
                                FEEDER_ID = item.FEEDER_ID,
                                LINE_ID = item.LINE_ID,
                                EX_DATE = feeder.DatePlan,
                                MACHINE_ID = item.MACHINE_ID,
                                MACHINE_SETTING = item.MACHINE_SETTING_NAME,
                                MACHINE_SLOT = item.MACHINE_SLOT,
                                ABOUT = about,
                                STATE = 2
                            });
                        }
                    }
                    else
                    {
                        feederAlarms.Add(new FeederAlarm()
                        {
                            FEEDER_ID = item.FEEDER_ID,
                            LINE_ID = item.LINE_ID,
                            EX_DATE = DateTime.Now,
                            MACHINE_ID = item.MACHINE_ID,
                            MACHINE_SETTING = item.MACHINE_SETTING_NAME,
                            MACHINE_SLOT = item.MACHINE_SLOT,
                            ABOUT = "Over",
                            STATE = 3
                        });
                    }
                }

               Repository.SaveFeeder(feederAlarms);
                // stopwatch.Stop();
                //Debug.WriteLine($"Feeder Job Finish. Elap: {stopwatch.ElapsedMilliseconds} ms");
                // log.Debug($"Feeder Job Finish. Elap: {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log.Error("Feeder Job Err:", ex);
            }
        }
    }
}

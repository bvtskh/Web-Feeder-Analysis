using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Common.Logging;
using FeederAnalysis.DAL.LCA;
using Quartz;

namespace FeederAnalysis.Business
{
    public class GaJob : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var stopwatch = CommonHelper.TimerStart();
                MankichiHelper.SaveLCA();
                MankichiHelper.SavePIBase();
                MankichiHelper.SaveStaffShift();
                log.Debug($"Ga Job Finish. Elap: {CommonHelper.TimerEnd(stopwatch)} ms");
            }
            catch (Exception ex)
            {
                log.Error("Ga Job Error", ex);
            }
        }
    }
}

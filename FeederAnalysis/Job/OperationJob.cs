using System;
using System.Diagnostics;
using System.Linq;
using Quartz;

namespace FeederAnalysis.Business
{
    public class OperationJob : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //var stopwatch = CommonHelper.TimerStart();
                Repository.SaveOpeHistories();
                //log.Debug($"Ope Job Finish. Elap: {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                log.Error("Ope Job Err", ex);
            }
        }
    }
}

using FeederAnalysis.Business;
using FeederAnalysis.Tokusai;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Job
{
    public class VerifedOrderItemJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var listVerify = Repository.FindLoadedOrderItem();
            if(listVerify.Count == 0)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.PauseJob(new JobKey("verifedOrderItemJob"));
            }
            new VerifyOrderItemHelper().VerifiedOrderItem_Update();
        }
    }
}
using FeederAnalysis.Business;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Job
{
    public class CheckPartQuantityJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            new PartInfoHelper().PartQuantity_Update();
        }
    }
}
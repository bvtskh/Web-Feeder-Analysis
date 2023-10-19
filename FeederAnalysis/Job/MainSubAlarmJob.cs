using FeederAnalysis.Tokusai;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Job
{
    public class MainSubAlarmJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            new MainSubHelper().MainSub_LineItem_Update();
        }
    }
}
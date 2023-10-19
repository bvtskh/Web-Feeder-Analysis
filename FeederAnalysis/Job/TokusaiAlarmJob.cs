using FeederAnalysis.Tokusai;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Job
{
    public class TokusaiAlarmJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var helper = new TokusaiHelper();
            helper.Tokusai_LineItem_Update();
            helper.MainSub_LineItem_Update();
        }
    }
}
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
            new TokusaiHelper().Tokusai_LineItem_OP_LOGS_Update();
        }
    }
}
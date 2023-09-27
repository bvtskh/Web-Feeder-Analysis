using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Common.Logging;
using FeederAnalysis.Business;
using FeederAnalysis.DAL.LCA;
using Quartz;

namespace FeederAnalysis.Job
{
    public class EduSolderJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            EduHelper.DoSolderJob();
        }
    }
}

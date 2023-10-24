﻿using FeederAnalysis.Business;
using FeederAnalysis.Tokusai;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Job
{
    public class LoadedOrderItemJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            new VerifyOrderItemHelper().LoadedOrderItem();
        }
    }
}
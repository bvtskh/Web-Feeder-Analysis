﻿using Bet.Util.Extension;
using FeederAnalysis.Job;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class JobScheduler
    {
        public static void Start()

        {
            int feederInterval = Bet.Util.Config.GetValue("FeederInterval").ToInt();
            int tokusaiInterval = Bet.Util.Config.GetValue("TokusaiInterval").ToInt();
            int opeInterval = 10;
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail feederJob = JobBuilder.Create<FeederJob>()
                .WithIdentity("job1")
                .Build();
            ITrigger feederTrigger = TriggerBuilder.Create()
                .WithIdentity("trigger1")
                .StartNow()
                .WithSimpleSchedule(r => r.WithIntervalInMinutes(feederInterval)
                .RepeatForever())
                .Build();
            IJobDetail tokusaiJob = JobBuilder.Create<TokusaiJob>()
               .WithIdentity("tokusaiJob")
               .Build();

            ITrigger tokusaiTrigger = TriggerBuilder.Create()
                .WithIdentity("tokusaiTrigger")
                .StartNow()
                .WithSimpleSchedule(r => r.WithIntervalInMinutes(tokusaiInterval)
                .RepeatForever())
                .Build();
            IJobDetail tokusaiAlarmJob = JobBuilder.Create<TokusaiAlarmJob>()
                 .WithIdentity("TokusaiAlarmJob")
                .Build();
            ITrigger tokusaiAlarmTrigger = TriggerBuilder.Create()
               .WithIdentity("tokusaiAlarmTrigger")
               .StartNow()
               .WithSimpleSchedule(r => r.WithIntervalInMinutes(tokusaiInterval)
               .RepeatForever())
               .Build();
            IJobDetail opeJob = JobBuilder.Create<OperationJob>()
               .WithIdentity("opeJob")
               .Build();

            ITrigger opeTrigger = TriggerBuilder.Create()
               .WithIdentity("opeTrigger")
               .StartNow()
               .WithSimpleSchedule(r => r.WithIntervalInMinutes(opeInterval)
               .RepeatForever())
               .Build();

            IJobDetail kyoJob = JobBuilder.Create<KyoJob>()
              .WithIdentity("kyoJob")
              .Build();

            ITrigger kyoTrigger = TriggerBuilder.Create()
               .WithIdentity("kyoTrigger")
               .StartNow()
               .WithSimpleSchedule(r => r.WithIntervalInMinutes(10)
               .RepeatForever())
               .Build();
            IJobDetail caliJob = JobBuilder.Create<CaliJob>()
             .WithIdentity("caliJob")
             .Build();

          
            //IJobDetail gaJob = JobBuilder.Create<GaJob>()
            //  .WithIdentity("gaJob")
            //  .Build();


            
            IJobDetail solderJob = JobBuilder.Create<SolderJob>()
            .WithIdentity("soilderJob")
            .Build();
            ITrigger soilderTrigger = TriggerBuilder.Create()
              .WithIdentity("soilderTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(r => r.WithIntervalInMinutes(15)
              .RepeatForever())
              .Build();
            IJobDetail eyeJob = JobBuilder.Create<EduEyeJob>()
           .WithIdentity("eyeJob")
           .Build();
            ITrigger eyeTrigger = TriggerBuilder.Create()
              .WithIdentity("eyeTrigger", "group1")
              .StartNow()
              .WithCronSchedule("0 15 8 ? * 2")
              .Build();

            IJobDetail eduJob = JobBuilder.Create<EduSolderJob>()
           .WithIdentity("eduJob")
           .Build();
            ITrigger eduTrigger = TriggerBuilder.Create()
              .WithIdentity("eduTrigger")
              .StartNow()
               .WithCronSchedule("0 10 8 ? * 2")
              .Build();

            IJobDetail profileJob = JobBuilder.Create<ProfilerJob>()
            .WithIdentity("profileJob")
            .Build();

            ITrigger profileTrigger = TriggerBuilder.Create()
                .WithIdentity("profileTrigger")
                .StartNow()
                .WithDailyTimeIntervalSchedule(r => r.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(10, 15))
                )
                .Build();
            scheduler.ScheduleJob(feederJob, feederTrigger);
            scheduler.ScheduleJob(tokusaiJob, tokusaiTrigger);
            scheduler.ScheduleJob(tokusaiAlarmJob, tokusaiAlarmTrigger);
            scheduler.ScheduleJob(opeJob, opeTrigger);
            //scheduler.ScheduleJob(gaJob, gaTrigger);
            scheduler.ScheduleJob(solderJob, soilderTrigger);
            scheduler.ScheduleJob(kyoJob, kyoTrigger);
            scheduler.ScheduleJob(profileJob, profileTrigger);
            scheduler.ScheduleJob(eduJob, eduTrigger);
            scheduler.ScheduleJob(eyeJob, eyeTrigger);
        }
    }
}
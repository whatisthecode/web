using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Schedules
{
    public class JobScheduler
    {
        public static void Start()
        {
            IJobDetail showHideProduct = JobBuilder.Create<ShowHideProductJob>()
                 .WithDescription("Show Hide Product Job with time")
                 .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(
                s => s.WithIntervalInMinutes(1)
                        .OnEveryDay()
                )
                .ForJob(showHideProduct)
                .WithIdentity("trigger1")
                .StartNow()
                .WithCronSchedule("0 0/1 * * * ?") //Time: Every 5 minutes job execute
                .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(showHideProduct, trigger);
            sc.Start();
        }
    }
}
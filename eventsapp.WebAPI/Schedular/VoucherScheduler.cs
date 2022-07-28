//using Eventsapp.Services.Schedular;
using Eventsapp.Services.Schedular;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Schedular
{
    public class VoucherScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<VoucherJobShcedular>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                //.WithCronSchedule("40 14 * * * ? *")
                //.WithCronSchedule("59 23 * * * ? *") // 11:59 PM
                .WithCronSchedule("0 0 0 * * ?") // 11:59 PM
                .WithPriority(1)
                //.WithCronSchedule("0 5 21 1/1 * ? *") // 12:00 AM
                //.WithCronSchedule("0 0 0 * * ?") // 12:00 AM
                //.WithCronSchedule("0 59 23 1/1 * ? *") // 11:59 PM
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
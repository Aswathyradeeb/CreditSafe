using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Schedular
{
    class SchedulerConfigration
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<VoucherJobShcedular>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(59)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}

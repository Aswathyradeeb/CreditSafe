using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Notifications
{
    
    public class EventNotificationScehdular
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail EventJob = JobBuilder.Create<EventJob>().Build();
            ITrigger EventTrigger = TriggerBuilder.Create()
                .WithIdentity("EventJob", "group2")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInHours(24)
                .RepeatForever())
                .EndAt(DateBuilder.DateOf(22, 0, 0))
                .Build();
            scheduler.ScheduleJob(EventJob, EventTrigger);

            IJobDetail AgendaJob = JobBuilder.Create<AgendaJob>().Build();
            ITrigger AgendaTrigger = TriggerBuilder.Create()
                .WithIdentity("AgendaJob", "group2")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(30)
                .RepeatForever())
                .EndAt(DateBuilder.DateOf(22, 0, 0))
                .Build();
            scheduler.ScheduleJob(AgendaJob, AgendaTrigger);
        }
    }
}
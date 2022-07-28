using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Notifications
{
    public class EmailNotification : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            IdentityMessage message = new IdentityMessage() { Body = "Please make this survay for event <a href=\"" + "hih" + "\"></a>", Subject = "Event Survay" };
            DateTime datePlus = DateTime.Now.AddMinutes(2); //Subtract(new TimeSpan(0, 1, 0));
            DateTime dateMin = DateTime.Now.Subtract(new TimeSpan(0, 2, 0));
            EventsApp.Domain.Entities.eventsappEntities entity = new EventsApp.Domain.Entities.eventsappEntities();
            //List<Event> eventLsst = entity.Events.Where(x => x.StartDate > dateMin && x.StartDate < datePlus).ToList();
            //if (eventLsst.Count > 0)
            //{
            //    new EmailGroupService().SendGroupAsync(message, new List<string>() { "mustafa@inlogic.ae", "", "" });
            //}

        //    new PushNotificationService().sendNotificationAndroid(new NotifyIOSDto());

        //    DateTime datePlus = DateTime.Now.AddMinutes(2); //Subtract(new TimeSpan(0, 1, 0));
        //    DateTime dateMin = DateTime.Now.Subtract(new TimeSpan(0, 2, 0));
        //    EventsApp.Domain.Entities.eventsappEntities entity = new EventsApp.Domain.Entities.eventsappEntities();
        //    List<Event> eventLsst = entity.Events.Where(x => x.Id == 70).ToList();
        //    if (eventLsst.Count > 0)
        //    {
        //        Event _eveny = eventLsst.FirstOrDefault();

        //        string virtualFilePath = "/Uploads/Photos/" + eventLsst.FirstOrDefault().BannerPhotoUrl;
        //        string absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
        //        NotifyIOSDto notifyIOSDto = new NotifyIOSDto();
        //        notifyIOSDto.NameEn = eventLsst.FirstOrDefault().NameEn;
        //        notifyIOSDto.NameAr = eventLsst.FirstOrDefault().NameAr;
        //        notifyIOSDto.StartDate = eventLsst.FirstOrDefault().StartDate;
        //        notifyIOSDto.BannerPhotoFullPath = "";
        //        notifyIOSDto.UserId = "8e604c80-0f56-4dae-b2ce-63df56865ae4";
        //        notifyIOSDto.EventId = eventLsst.FirstOrDefault().Id;

        //        PushNotificationService sendNot = new PushNotificationService();

        //        sendNot.sendNotification(notifyIOSDto, new string[] { "328025f6e96abb30853b52d2ea3b81b73d86013de53ae4738b7c9190ac573210", "" });


        //        //foreach (var mob in mobUserList) 
        //        //{
        //        //}

        //        //List<User> userLst = entity.Users.Where(x => x.EventUsers.Where(y => y.EventId == _eveny.Id).Any()).ToList();
        //        //if (userLst.Count > 0)
        //        //{
        //        //    foreach (var item in userLst)
        //        //    {
        //        //        var mobUserList = entity.IOSDevices.Where(x => x.UserId == item.Id);

        //        //        if (mobUserList.Count() > 0) // is mobille user ?
        //        //        {
        //        //            string virtualFilePath = "/Uploads/Photos/" + eventLsst.FirstOrDefault().BannerPhotoUrl;
        //        //            string absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
        //        //            NotifyIOSDto notifyIOSDto = new NotifyIOSDto();
        //        //            notifyIOSDto.NameEn = eventLsst.FirstOrDefault().NameEn;
        //        //            notifyIOSDto.NameAr = eventLsst.FirstOrDefault().NameAr;
        //        //            notifyIOSDto.StartDate = eventLsst.FirstOrDefault().StartDate;
        //        //            notifyIOSDto.BannerPhotoFullPath = "";
        //        //            notifyIOSDto.UserId = item.Id;
        //        //            notifyIOSDto.EventId = eventLsst.FirstOrDefault().Id;

        //        //            PushNotificationService sendNot = new PushNotificationService();
        //        //            foreach (var mob in mobUserList)
        //        //            {
        //        //                sendNot.sendNotification(notifyIOSDto, new string[] { "e9d6a713b10ae99a76ccc4d36ef88618c0375393c4ca0a472d98deb80c4e1468" });
        //        //            }
        //        //        }

        //        //  }
        //        //}
        //    }
        }

}
}
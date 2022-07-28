using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class NotifyIOSDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameEn { get; set; }
        [DataMember]
        public string NameAr { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public string BannerPhoto  { get; set; }
        [DataMember]
        public string BannerPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BannerPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BannerPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        [DataMember]
        public int EventId;
        [DataMember]
        public string UserId;
        [DataMember]
        public bool? IsRead;
    }
}
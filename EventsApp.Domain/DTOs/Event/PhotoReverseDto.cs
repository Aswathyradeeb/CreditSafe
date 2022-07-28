using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class PhotoReverseDto
    {

        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string PhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + PhotoName;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(PhotoName))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<int> EventId { get; set; }
        public Nullable<bool> Approved { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public virtual EventLightDto Event { get; set; }
    }
}
using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class PresentationDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string AttachmentUrl { get; set; }
        public int EventId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> AgendaId { get; set; }

        public virtual AgendumDto Agendum { get; set; }
        public string AttachmentUrlFulPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/Uploads/Attachment/" + AttachmentUrl;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(AttachmentUrl))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
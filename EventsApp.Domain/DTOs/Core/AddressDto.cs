using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class AddressDto
    { 
        public int Id { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Street { get; set; }
        public string LocationPhoto { get; set; }
        public string LocationPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + LocationPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(LocationPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string Lat { get; set; }
        public string Lng { get; set; }

        public   CountryDto Country { get; set; }
        public   StateDto State { get; set; }
    }
}
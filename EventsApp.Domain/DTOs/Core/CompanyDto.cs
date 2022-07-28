using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyCode { get; set; }
        public string PhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Photo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Photo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public Nullable<int> AddressId { get; set; }
        public AddressDto Address { get; set; }
        public int EventId { get; set; }
        public ICollection<UserInfoDto> Users { get; set; }
    }

    public class CompanySingleDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyCode { get; set; }
        public string PhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Photo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Photo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public Nullable<int> AddressId { get; set; }
    }
}
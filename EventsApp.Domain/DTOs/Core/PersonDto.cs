using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class PersonDto
    {

        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<byte> Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Resume { get; set; }
        public string ResumeFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Resume;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Resume))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string Photo { get; set; }
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
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string EmiratesID { get; set; }
        public Nullable<bool> IsResidentOfUAE { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public Nullable<int> AverageRating
        {
            get
            {
                if (SpeakerRatings != null && SpeakerRatings.Count > 0)
                {
                    return Convert.ToInt32((SpeakerRatings.Average(item => item.Rating)));
                }
                else
                {
                    return 1;
                }
            }
        }

        public List<SpeakerRatingDto> SpeakerRatings { get; set; }

    }
}
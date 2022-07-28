using System.Collections.Generic;

namespace EventsApp.Domain.DTOs
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ISOCode2 { get; set; }
        public string ISOCode3 { get; set; }
        public string FlagUrl { get; set; }
        public string BigFlagUrl { get; set; }
        public string PhoneCode { get; set; }
        public short NumericCode { get; set; }
        public short RegionId { get; set; }
        //public virtual ICollection<StateDto> States { get; set; }

    }

    public class CountryWithStateDto
    {
        public int Id { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ISOCode2 { get; set; }
        public string ISOCode3 { get; set; }
        public string FlagUrl { get; set; }
        public string BigFlagUrl { get; set; }
        public string PhoneCode { get; set; }
        public short NumericCode { get; set; }
        public short RegionId { get; set; }
        public virtual ICollection<StateDto> States { get; set; }

    }
}
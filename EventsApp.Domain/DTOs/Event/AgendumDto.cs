using System;
using System.Collections.Generic;

namespace EventsApp.Domain.DTOs
{
    public class AgendumDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string DateStringFormat { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int EventId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public string Location { get; set; }

        public virtual PersonDto Person { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<int> ReservationCount { get; set; }
        public Nullable<int> AttendanceCount { get; set; }
        //public ICollection<EventUserLightDto> EventUsers { get; set; }   // Breaking for large number of Attendees
    }
    public class AgendumSingleDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int EventId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<int> SessionId { get; set; }

        public string Location { get; set; }
        public virtual PersonDto Person { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<int> ReservationCount { get; set; }
        public Nullable<int> AttendanceCount { get; set; }
        //public ICollection<EventUserLightDto> EventUsers { get; set; }
    }
    public class AgendumLightDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public TimeSpan FromTime24Hr { get; set; }
        public TimeSpan ToTime24Hr { get; set; }
        public string Location { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<int> ReservationCount { get; set; }
        public Nullable<int> AttendanceCount { get; set; }
    }
}
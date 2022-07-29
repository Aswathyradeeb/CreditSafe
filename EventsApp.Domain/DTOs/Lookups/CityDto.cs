using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class CityDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CountryCode2 { get; set; }
        public string State { get; set; }
        public int TouristRating { get; set; }
        public System.DateTime DateEstablished { get; set; }
        public int Population { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class CityWeatherDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string CountryCode2 { get; set; }
        public string CountryCode3 { get; set; }
        public string CurrencyCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WeatherDescription { get; set; }
        public string Temperature { get; set; }
        public string FeelsLike { get; set; }
        public string Humidity { get; set; }
        public string State { get; set; }
        public int TouristRating { get; set; }
        public System.DateTime DateEstablished { get; set; }
        public int Population { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
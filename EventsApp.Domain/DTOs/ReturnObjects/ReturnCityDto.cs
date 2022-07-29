using EventsApp.Domain.DTOs.Athlete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnCityDto
    {
        public List<CityDto> cities { get; set; }
        public int cityCount { get; set; }
    }
    public class ReturnCountryDto
    {
        public List<CountryDto> countries { get; set; }
        public int countryCount { get; set; }
    }
    public class ReturnCityWeatherDto
    {
        public List<CityWeatherDto> cities { get; set; }
        public int cityCount { get; set; }
    }
}

using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface ICityService
    {
        Task<ReturnCityDto> GetAllCities(FilterParams filterParams, int page, int pageSize, string searchText);
        Task<CityDto> CreateUpdateCity(CityDto _city);
        Task<string> DeleteCity(int cityId);
        Task<ReturnCountryDto> GetAllCountries(FilterParams filterParams, int page, int pageSize);
        Task<ReturnCityWeatherDto> GetAllCityWeather(FilterParams filterParams, int page, int pageSize, string searchText);
    }
}

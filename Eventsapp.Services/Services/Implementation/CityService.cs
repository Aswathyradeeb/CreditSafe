using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace EventsApp.Services
{
    public class CityService : ICityService
    {
        private readonly IKeyedRepository<City, int> _cityRepository;
       
        public CityService(IKeyedRepository<City, int> _cityRepository)
        {
            this._cityRepository = _cityRepository;
        }
        public async Task<ReturnCountryDto> GetAllCountries(FilterParams filterParams, int page, int pageSize)
        {
            string countryApiResponse = string.Empty;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://restcountries.com/v3.1/all");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";

          
            using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
            {
                using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    countryApiResponse = streamIn.ReadToEnd();
                    streamIn.Close();
                }
            }
            dynamic dynamicListObj = new JavaScriptSerializer().Deserialize<object>(countryApiResponse);
            List<CountryDto> countryLst = new List<CountryDto>();
            foreach (var item in dynamicListObj)
            {
                string Countryname = "", CCa2 = "", CCa3 = "", currencyCode = "";

                foreach (var childItem in item)
                { 
                    if (childItem.Key == "name")
                        {
                        foreach (var data in childItem.Value)
                        {
                            if (data.Key == "official")
                            {
                                Countryname = data.Value;
                                break;
                            }
                        }  
                         }
                    if (childItem.Key == "cca2")
                        CCa2 = childItem.Value;
                    if (childItem.Key == "cca3")
                        CCa3 = childItem.Value;
                    if (childItem.Key == "currencies")
                    {
                        foreach(var data in childItem.Value)
                        {
                            currencyCode = data.Key;
                        }
                    }
                   

                }
                countryLst.Add(new CountryDto()
                {
                    Cname = Countryname,
                    CountryCode2 = CCa2,
                    CountryCode3 = CCa3,
                    CurrencyCode = currencyCode
                });


            }
                return new ReturnCountryDto { countries = countryLst, countryCount = countryLst.Count };
        }

        public async Task<ReturnCityDto> GetAllCities(FilterParams filterParams, int page, int pageSize, string searchText)
        {
            List<City> cities = new List<City>();

            cities = (await this._cityRepository.QueryAsync(x => x.IsActive != false )).OrderByDescending(y => y.DateEstablished).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<City> filteredCities = new List<City>();
                filteredCities = cities.Where(o => o.CityName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.State.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                var filteredCitiesDtoLst = MapperHelper.Map<List<CityDto>>(filteredCities.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());

                
                return new ReturnCityDto { cities = filteredCitiesDtoLst, cityCount = filteredCitiesDtoLst.Count };
            }
            else
            {
                var CitiesDtoLst = MapperHelper.Map<List<CityDto>>(cities.Skip((page - 1) * pageSize)
                                 .Take(pageSize).ToList());

                
                return new ReturnCityDto { cities = CitiesDtoLst, cityCount = CitiesDtoLst.Count };

            }

        }

        public async Task<ReturnCityWeatherDto> GetAllCityWeather(FilterParams filterParams, int page, int pageSize, string searchText)
        {
            List<City> cities = new List<City>();

            if (!string.IsNullOrEmpty(searchText))
            {
                cities = (await this._cityRepository.QueryAsync(x => x.IsActive != false)).OrderByDescending(y => y.DateEstablished).ToList();

                List<City> filteredCities = new List<City>();
                filteredCities = cities.Where(o => o.CityName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.State.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                var filteredCitiesDtoLst = MapperHelper.Map<List<CityWeatherDto>>(filteredCities.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());
                foreach(var city in filteredCitiesDtoLst)
                {
                    //-------------Getting the Country details of city

                    string countryApiResponse = string.Empty;
                    string latlongApiResponse = string.Empty;
                    string weatherApiResponse = string.Empty;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://restcountries.com/v3.1/alpha?codes="+city.CountryCode2);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            countryApiResponse = streamIn.ReadToEnd();
                            streamIn.Close();
                        }
                    }
                    dynamic dynamicListObj = new JavaScriptSerializer().Deserialize<object>(countryApiResponse);
                    
                    foreach (var childItem in dynamicListObj)
                    {
                        foreach (var data in childItem)
                        {
                            if (data.Key == "name")
                            {
                                foreach (var value in data.Value)
                                {
                                    if (value.Key == "official")
                                    {
                                        city.Country = value.Value;
                                        break;
                                    }
                                }
                            }

                            if (data.Key == "cca2")
                                city.CountryCode2 = data.Value;
                            if (data.Key == "cca3")
                                city.CountryCode3 = data.Value;
                            if (data.Key == "currencies")
                            {
                                foreach (var val in data.Value)
                                {
                                    city.CurrencyCode = val.Key;
                                }
                            }
                        }

                    }

                    //------Getting latitude and longitude details of city to call weather API

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.openweathermap.org/geo/1.0/direct?q="+city.CityName+"&limit=5&appid=644cdc65b2bc1c50172ce57adc143127");
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            latlongApiResponse = streamIn.ReadToEnd();
                            streamIn.Close();
                        }
                    }
                     dynamicListObj = new JavaScriptSerializer().Deserialize<object>(latlongApiResponse);
                    string lat = "", lon = "";
                    foreach (var childItem in dynamicListObj)
                    {

                        if (((string)childItem["country"]) == city.CountryCode2) 
                        foreach (var data in childItem)
                        {
                            if (data.Key == "lat")
                                    city.Latitude = Convert.ToString(data.Value);
                            if (data.Key == "lon")
                                    city.Longitude = Convert.ToString(data.Value);
                            if (data.Key == "state")
                                    city.State = Convert.ToString(data.Value);

                            }
                    }
                    //----Getting weather based on latitude and longitude values

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?lat="+ city.Latitude + "&lon="+ city.Longitude + "&appid=644cdc65b2bc1c50172ce57adc143127");
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            weatherApiResponse = streamIn.ReadToEnd();
                            streamIn.Close();
                        }
                    }
                    dynamicListObj = new JavaScriptSerializer().Deserialize<object>(weatherApiResponse);
                    foreach (var childItem in dynamicListObj)
                    {
                        

                            if (childItem.Key == "weather")
                                foreach (var data in childItem.Value)
                                {
                                foreach (var val in data)
                                {
                                    if (val.Key == "description")
                                        city.WeatherDescription = val.Value;
                                }
                                }
                            if (childItem.Key == "main")
                                foreach (var data in childItem.Value)
                                {
                                    if (data.Key == "temp")
                                        city.Temperature =Convert.ToString(data.Value);
                                    if (data.Key == "feels_like")
                                        city.FeelsLike = Convert.ToString(data.Value);
                                if (data.Key == "humidity")
                                        city.Humidity = Convert.ToString(data.Value);
                            }
                        
                      
                    }

                }

                return new ReturnCityWeatherDto { cities = filteredCitiesDtoLst, cityCount = filteredCitiesDtoLst.Count };
            }
            else
            {
                List<CityWeatherDto> city = new List<CityWeatherDto>();


                return new ReturnCityWeatherDto { cities = city, cityCount = city.Count };

            }

        }

        public async Task<CityDto> CreateUpdateCity(CityDto _city)
        {

            if(_city.Id==0)
            {
                City cityObj = MapperHelper.Map<City>(_city);
                this._cityRepository.Insert(cityObj);
                this._cityRepository.Commit();
                return MapperHelper.Map<CityDto>(cityObj);
            }
            else
            {
                City city = MapperHelper.Map<City>(_city);
                var CityEntity = await this._cityRepository.GetAsync(_city.Id);
                CityEntity.TouristRating = city.TouristRating;
                CityEntity.Population = city.Population;
                return MapperHelper.Map<CityDto>(CityEntity);
            }
          
        }

       
       
       

      
        public async Task<string> DeleteCity(int cityId)
        {
            var city = await this._cityRepository.GetAsync(cityId);
            city.IsActive = false;
            return "Deleted Successfully";
        }
       
    }
}

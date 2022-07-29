using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using EventsApp.WebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestEventsApp
    {
        private readonly ICityService _cityServices;
        private readonly IKeyedRepository<City, int> _cityRepository;
        public UnitTestEventsApp()
        {
        }
        public UnitTestEventsApp(ICityService _cityServices,
           IKeyedRepository<City, int> _cityRepository)
        {
            this._cityServices = _cityServices;
            this._cityRepository = _cityRepository;
        }
        
        [TestMethod]
        public void GetCities_ShouldReturnAllCities()
        {
            var testCities = GetFilters();
            var controller = new CityController(_cityServices, _cityRepository);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var result = controller.GetCities(testCities);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GetCountries_ShouldReturnAllCountries()
        {
            var testCities = GetFilters();
            var controller = new CityController(_cityServices, _cityRepository);

            var result = controller.GetCountries(testCities);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GetCityWeather_ShouldReturnCityWeatherbyCityname()
        {
            var testCities = GetFilters();
            var controller = new CityController(_cityServices, _cityRepository);

            var result = controller.GetCityWeather(testCities);
            Assert.IsNotNull(result);
        }
        
             [TestMethod]
        public void CreateUpdateCity_ShouldCreateUpdateCityBasedonCityId()
        {
            var testCities = GetCity();
            var controller = new CityController(_cityServices, _cityRepository);

            var result = controller.CreateUpdateCity(testCities);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void DeleteCity_ShouldDeleteCityBasedonCityId()
        {
            var controller = new CityController(_cityServices, _cityRepository);

            var result = controller.DeleteCity(1);
            Assert.IsNotNull(result);
        }
        private RequestFilter GetFilters()
        {
            var testFilters = new RequestFilter();
            testFilters.Page = 1;
            testFilters.PageSize = 12;
            testFilters.Searchtext = "London";
            return testFilters;
        }
        private CityDto GetCity()
        {
            var testCity = new CityDto();
            testCity.Id = 1;
            testCity.CityName = "London";
            testCity.Population = 1000;
            testCity.CountryCode2 = "GB";
            testCity.IsActive = true;
            return testCity;
        }
    }
    
}

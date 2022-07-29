using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.DTOs.Payment;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;

namespace EventsApp.Domain.DTOs
{
    public class MapperHelper
    {
        public static void MapInitialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, CityDto>().ReverseMap();
                cfg.CreateMap<City, CityWeatherDto>().ReverseMap();
            });
        }
        public static T Map<T>(object source)
        {
            return (T)AutoMapper.Mapper.Map(source, source.GetType(), typeof(T));
        }
        public static T Map<T>(object source, object destination)
        {
            AutoMapper.Mapper.Map(source, destination);
            return (T)destination;
        }
    }
}
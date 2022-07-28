using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Interface
{
    public interface IRestaurantService
    {
        Task<CompanyDto> CreateRestaurant(CompanyDto _event, string connString);
        Task<ReturnCompanyDto> GetAllRestaurants(FilterParams filterParams, int page, int pageSize);
        Task<List<CompanyDto>> GetRestaurants();
        Task<List<CompanyDto>> GetAllRestaurants();
        Task<CompanyDto> UpdateRestaurant(CompanyDto _company);
             Task<string> DeleteRestaurant(int companyId);
    }
}

using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateCompany(CompanyDto _event, string connString);
        Task<CompanyDto> GetCompanyId(int companyId);
        Task<CompanyDto> GetCompanyUser(int userid);
        Task<ReturnCompanyDto> GetAllCompanys(FilterParams filterParams, int page, int pageSize);
        Task<List<CompanyDto>> GetCompanys();
        Task<List<CompanyDto>> GetAllCompanies();
        Task<CompanyDto> UpdateCompany(CompanyDto _company);
        Task<List<PreferredLanguageDto>> SavePreferredLanguages(List<PreferredLanguageDto> PreferredLanguages);
        Task<string> DeleteCompany(int companyId);
    }
}

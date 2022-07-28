using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class CompanyTypeRepository : RepositoryBase<CompanyType, int> , ICompanyTypeRepository
    {
        public CompanyTypeRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}

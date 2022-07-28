using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class CompanyRepository : RepositoryBase<Company, int> , ICompanyRepository
    {
        public CompanyRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}

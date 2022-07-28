using System.Threading.Tasks;
using Eventsapp.Repositories.Repositories.Interface.Core;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class PackageRepository : RepositoryBase<Package, int>, IPackageRepository
    {
        public PackageRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }

       
    }
}

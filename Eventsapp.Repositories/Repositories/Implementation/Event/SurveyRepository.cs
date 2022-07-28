using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class SurveyRepository : RepositoryBase<Survey, int> , ISurveyRepository
    {
        public SurveyRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}

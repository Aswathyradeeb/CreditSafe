using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class SurveyResultsRepository : RepositoryBase<SurveyResult, int> , ISurveyResultsRepository
    {
        public SurveyResultsRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}

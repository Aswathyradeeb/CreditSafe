using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IPollService
    {
        void UpdatePoll(PollDto _poll);
        void CreatePoll(PollDto PollDto);
        Task<List<PollDto>> GetPolls(int eventId);
        Task<List<PollResultDto>> GetPollResult(int pollId, string userId);
    }
}

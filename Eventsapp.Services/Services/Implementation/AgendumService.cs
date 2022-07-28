using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public class AgendumService : IAgendumService
    {
        private readonly IAgendumRepository agendumRepository;

        public AgendumService(IAgendumRepository _agendumRepository)
        {
            this.agendumRepository = _agendumRepository;

        }

        public async Task<IEnumerable<AgendumDto>> GetAgendaByEventId(int eventId)
        {
            var lstAgendumList = new List<AgendumDto>();
            var agendums = await this.agendumRepository.QueryAsync(x => x.EventId == eventId);
            var agendumDto = MapperHelper.Map<List<AgendumDto>>(agendums).OrderBy(ag => ag.FromTime).ToList();
            var groupedCustomerList = agendumDto.GroupBy(x => x.StartDate).Select(grp => grp.ToList());
            var lst = new AgendumDto();

            foreach (List<AgendumDto> s in groupedCustomerList)
            {
                lst = new AgendumDto
                {
                    StartDate = s.First().StartDate.Value
                };

                var sessions = s.GroupBy(x => x.SessionId).Select(grp => grp.ToList());
                lstAgendumList.Add(lst);
            }


            //List<AgendumGetDto> getDto = new List<AgendumGetDto>();

            ////  string dt = DateTime.Parse().ToString("dd/MM/yyyy");
            //AgendumGetDto ag;
            //foreach (AgendumDto d in s)
            //{
            //    ag = new AgendumGetDto
            //    {
            //        Id = d.Id,
            //        fromTime = d.FromTime.ToString("h:mm tt"),
            //        toTime = d.ToTime.ToString("h:mm tt"),
            //        descriptionAr = d.DescriptionAr,
            //        descriptionEn = d.DescriptionEn,
            //        titleAr = d.TitleAr,
            //        titleEn = d.TitleEn,
            //        date = d.Date.ToShortDateString(),
            //        eventId = d.EventId,
            //        speakerId = d.SpeakerId,
            //        SessionId = d.SessionId

            //    };

            //    getDto.Add(ag);
            //}

            //lst.agendas = getDto;
            //lst.Date = DateTime.Parse(s.First().Date.ToShortDateString()).ToString("dd MMM yyyy"); ;
            //lst.Session = s.First().AgendaSession;
            //lstAgendumList.Add(lst);

            return lstAgendumList;
        }

        public async Task<List<AgendumLightDto>> GetAgendaEvent(int eventid)
        {
            var agendums = await this.agendumRepository.QueryAsync(x => x.EventId == eventid);
            List<AgendumLightDto> agendaDto = MapperHelper.Map<List<AgendumLightDto>>(agendums);
            foreach (var agendaItem in agendaDto)
            {
                agendaItem.FromTime24Hr = DateTime.Parse(agendaItem.FromTime).TimeOfDay;
                agendaItem.ToTime24Hr = DateTime.Parse(agendaItem.ToTime).TimeOfDay;
            }
            return agendaDto;
        }
    }
}

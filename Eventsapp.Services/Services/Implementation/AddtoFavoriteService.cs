using Eventsapp.Repositories;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Implementation
{
    public class AddtoFavoriteService : IAddtoFavoriteService
    {
        private readonly IKeyedRepository<InterestedAgenda, int> interestedAgenda;
        private readonly IEventRepository eventRepository;
        public AddtoFavoriteService(IEventRepository eventRepository, IKeyedRepository<InterestedAgenda, int> interestedAgenda)
        {
            this.eventRepository = eventRepository;
            this.interestedAgenda = interestedAgenda;
        }
        public async Task<List<FavoriteAgenda>> GetMyFavoriteEvents(string type, int id)
        {
            List<FavoriteAgenda> myFavorites;
            try
            {
                myFavorites = new List<FavoriteAgenda>();
                var allFavorites = type.ToLower() == "speaker" ? await this.interestedAgenda.QueryAsync(x => x.SpeakerId.Value == id) : await this.interestedAgenda.QueryAsync(x => x.UserId.Value == id);
                if (allFavorites != null && allFavorites.Count>0)
                {
                    foreach(var item in allFavorites)
                    {
                        myFavorites.Add(new FavoriteAgenda()
                        {
                            SpeakerId = item.SpeakerId  ,
                            UserId=item.UserId.HasValue? item.UserId.Value : 0,
                            EventId=item.EventId,
                            AgendaId=item.AgendaId,
                            CreatedOn=item.CreatedOn,
                            FavoriteAgendaId=item.Id

                        }) ;
                    }
                    return myFavorites;
                }                       
                else
                    return new List<FavoriteAgenda>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> AddToFavorite(FavoriteEvent data)
        {
            InterestedAgenda interestedAgenda = new InterestedAgenda();
            List<InterestedAgenda> myFavorites = new List<InterestedAgenda>();
            try
            {
                if (data.UserId.HasValue)
                {
                    myFavorites = await this.interestedAgenda.QueryAsync(x => x.UserId.Value == data.UserId.Value && x.AgendaId == data.AgendaId);
                    return InsertToFavorite(data, interestedAgenda, myFavorites);
                }
                else
                {
                    myFavorites = await this.interestedAgenda.QueryAsync(x => x.SpeakerId.Value == data.SpeakerId.Value && x.AgendaId == data.AgendaId);
                    return InsertToFavorite(data, interestedAgenda, myFavorites);
                }  
            }
            catch (Exception ex)
            {
                return ex.InnerException !=null? ex.InnerException.Message : ex.Message;
            }
        }

        private string InsertToFavorite(FavoriteEvent data, InterestedAgenda interestedAgenda, List<InterestedAgenda> myFavorites)
        {
            if (myFavorites.Count > 0 && myFavorites != null)
                return "The Agenda " + data.AgendaId + " is already in User's Favorite List";
            interestedAgenda.SpeakerId = data.SpeakerId;
            interestedAgenda.UserId = data.UserId;
            interestedAgenda.EventId = data.EventId;
            interestedAgenda.CreatedOn = DateTime.Now;
            interestedAgenda.AgendaId = data.AgendaId;
            this.interestedAgenda.Insert(interestedAgenda);
            return !data.SpeakerId.HasValue ?"Event: " + data.EventId + " for User: " + data.UserId + "has been submitted successfully at :" + DateTime.Now.ToShortDateString() : "Event: " + data.EventId + " for Speaker: " + data.SpeakerId + "has been submitted successfully at :" + DateTime.Now.ToShortDateString();
        }

        public async Task<bool> RemoveFromFavorite(int id)
        {
           
            try
            {
                var myFavorite = await this.interestedAgenda.GetAsync(id);
                if (myFavorite != null)
                {
                    this.interestedAgenda.Delete(myFavorite);
                    return true;
                }                   
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

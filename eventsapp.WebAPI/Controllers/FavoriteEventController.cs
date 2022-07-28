using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/FavoriteEvent")]
    public class FavoriteEventController : ApiController
    {
        private IAddtoFavoriteService favService;     
        private ICurrentUser user;
        public FavoriteEventController(IAddtoFavoriteService favService,ICurrentUser user)
        {
            this.favService = favService;
            this.user = user;
        }
        [HttpPost]
        [Route("AddtoFavorite")]
        public async Task<ResponseType<string>> AddtoFavorite(FavoriteEvent favoriteEvent)
        {
            try
            {
                if (favoriteEvent == null)
                    return ResponseType<string>.PerformError<string>("NoDataProvided","Cannot add a blank event, Please provide data for posting" + DateTime.Now.ToShortDateString());
                if(!favoriteEvent.UserId.HasValue && !favoriteEvent.SpeakerId.HasValue)
                    return ResponseType<string>.PerformError<string>("IdNotProvided","User Id and Speaker Id not provided, Please provide atleast one attendee ID ... Request Time: " + DateTime.Now.ToShortDateString());
                var result = await favService.AddToFavorite(favoriteEvent);
                return ResponseType<string>.PerformSuccessed<string>(result);
            }
            catch (Exception ex)
            {
                return ResponseType<string>.PerformError<string>("Exception","Some error has occurred while adding Favorite : "+ex.Message+ " ... Request Time: " + DateTime.Now.ToShortDateString());
            }
        }
        [HttpGet]
        [Route("RemoveFavorite/{id}")]
        public async Task<ResponseType<bool>> RemoveFavorite(int id)
        {
            try
            {
                if (id <= 0)
                    return ResponseType<bool>.PerformError<bool>("IdNotExist","Cannot remove a blank event, Please provide data for removing event" + DateTime.Now.ToShortDateString());
             
                var agendaDto =await favService.RemoveFromFavorite(id);
                return ResponseType<bool>.PerformSuccessed<bool>(agendaDto);
            }
            catch (Exception ex)
            {
                return ResponseType<bool>.PerformError<bool>("Exception", "Some error has occurred while adding Favorite : " + ex.Message + " ... Request Time: " + DateTime.Now.ToShortDateString());
            }
        }
        [HttpGet]
        [Route("GetFavoriteEvents/{type}/{id}")]
        public async Task<ResponseType<List<FavoriteAgenda>>> GetFavoriteEvents([FromUri]string type, int id)
        {
            try
            {
                if (type == null)
                    return ResponseType<List<FavoriteAgenda>>.PerformError<List<FavoriteAgenda>>("TypeisNull","Please provide valid type i.e. User or Speaker for Get Favorite Request ... request time : " + DateTime.Now.ToShortDateString());
                if (id == null || id<0)
                    return ResponseType<List<FavoriteAgenda>>.PerformError<List<FavoriteAgenda>>("IDNotExist", "Please provide valid ID for type : " + type + "... request time : " + DateTime.Now.ToShortDateString());
                var myFavorite = await favService.GetMyFavoriteEvents(type,id);
                if (myFavorite == null || myFavorite.Count() == 0)
                    return ResponseType<List<FavoriteAgenda>>.PerformError<List<FavoriteAgenda>>("EmptyResponse", "No Favorite Events found for " + type + "-" + id + " at" + DateTime.Now.ToShortDateString());               
                else
                    return ResponseType<List<FavoriteAgenda>>.PerformSuccessed<List<FavoriteAgenda>>(myFavorite);
            }
            catch (Exception ex)
            {
                return ResponseType<List<FavoriteAgenda>>.PerformError<List<FavoriteAgenda>>("Exception", "Some error has occurred while getting Favorite events : " + ex.Message + " ... Request Time: " + DateTime.Now.ToShortDateString());
            }
        }
    }
}

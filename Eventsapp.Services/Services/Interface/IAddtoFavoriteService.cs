using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Interface
{
    public interface IAddtoFavoriteService
    {
        Task<List<FavoriteAgenda>> GetMyFavoriteEvents(string type, int id);
        Task<string> AddToFavorite(FavoriteEvent data);
        Task<bool> RemoveFromFavorite(int id);
    }
}

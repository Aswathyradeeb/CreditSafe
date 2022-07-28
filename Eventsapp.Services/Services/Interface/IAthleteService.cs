using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.ReturnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Interface
{
    

    public interface IAthleteService
    {
        
        Task<ReturnAthletesDto> GetAllAthletes(FilterParams filterParams, int page, int pageSize, string searchText);
        Task<List<VoucherDto>> GetAllVouchers();
        Task<ReturnAthleteVoucherDto> GetAthleteVoucher(FilterParams filterParams, int page, int pageSize);
        Task<int> GetVoucherShareCount(int voucherId);
        List<UserDto> GetAllAthletesGuest();
        Task<int> ShareGuestVoucher(int guestId, int voucherId);
        Task<ReturnAthletesDto> GetGuestsOfAthlete(FilterParams filterParams, int page, int pageSize, int athleteId, string searchText);
        Task<ReturnAthletesDto> GetGuests(FilterParams filterParams, int page, int pageSize, string searchText);


    }
}

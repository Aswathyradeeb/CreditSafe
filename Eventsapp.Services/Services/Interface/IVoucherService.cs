
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Athlete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IVoucherService
    {
        Task<List<ClaimedVoucherDto>> GetClaimedVouchers(bool IsASuperAdmin);
        Task<bool> ResetVouchersCount(bool IsReset);
        Task<List<ClaimedVoucherDto>> GetPagedClaimedVoucher(ReportsFilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy);
        Task<int> GetNumbersOfRecords(ReportsFilterParams filterParams, string searchtext);
    }
}

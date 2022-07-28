
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Subscription;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace Eventsapp.Services
{
    public interface IUserService
    {
        UserInfo GetByUserName(string name);
        Task<UserInfo> GetByUserNameAsync(string name);
        Task<bool> UpdateUserProfileImageAsync(string name, string imageName);
        Task<bool> UserLock(int userId, bool IsLock);
        Task<UserDto> GetByUserId(int userId);
        Task<bool> UpdateUserProfile(UserDto userProfile);
        Task<List<UserDto>> GetPagedUsers(FilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy);
        Task<int> GetNumbersOfRecords(FilterParams filterParams, string searchtext); 
        Task<bool> SendEmailUserAction(UserActionsTakenDto userAction);
        Task<bool> SendSMSUserAction(UserActionsTakenDto userAction);
        Task<bool> SendSMSBatch(FilterParams filterParams, string searchtext, string SMSmessage); 
        Task<bool> SendEmailBatch(FilterParams filterParams, string searchtext, string Subject, string Body);  
        Task<bool> IsPhoneExist(string phoneNumber);

        Task<List<UserSubscriptionDto>> GetUserSubscriptions(FilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy);
        Task<int> GetSubscriptionNumbersOfRecords(FilterParams filterParams, string searchtext);

        Task<int> CreateSubscription(int eventPackageId);

        Task<string> AddCompany(string companyCode);

        Task<string> SetUserToken(int userId, string token);
        Task<UserDto> VerifyVoucher(int userId, int eventid);
    }
}

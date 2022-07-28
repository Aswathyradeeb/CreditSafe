using EventsApp.Framework.Utilities;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using EventsApp.Domain.DTOs.Subscription;

namespace Eventsapp.Services
{
    public class UserService : IUserService
    {
        private readonly IKeyedRepository<User, int> _userRepository;
        private readonly IKeyedRepository<Event, int> _eventRepository;
        private readonly IKeyedRepository<UserAction, short> _userActionRepository;
        private readonly IKeyedRepository<UserSubscription, short> _userSubscriptionRepository;
        private readonly IKeyedRepository<UserCompany, short> _userCompanyRepository;
        private readonly IKeyedRepository<Company, short> _companyRepository;
        private readonly IKeyedRepository<ClaimedVoucher, short> _claimedVoucherRepository;
        private readonly IKeyedRepository<LoggedUserConnection, short> _userTokenRepository;
        private readonly ICurrentUser user;
        private readonly IMailSender _mailSender;

        public UserService(IKeyedRepository<User, int> userRepository, IKeyedRepository<UserAction, short> userActionRepository,
            IMailSender mailSender, IKeyedRepository<Event, int> _eventRepository, ICurrentUser user,
            IKeyedRepository<UserSubscription, short> userSubscriptionRepository, IKeyedRepository<LoggedUserConnection, short> _userTokenRepository,
            IKeyedRepository<UserCompany, short> _userCompanyRepository, IKeyedRepository<Company, short> _companyRepository,
            IKeyedRepository<ClaimedVoucher, short> _claimedVoucherRepository)
        {
            this._userRepository = userRepository;
            this._userActionRepository = userActionRepository;
            this._mailSender = mailSender;
            this._eventRepository = _eventRepository;
            this._userSubscriptionRepository = userSubscriptionRepository;
            this._userCompanyRepository = _userCompanyRepository;
            this._companyRepository = _companyRepository;
            this.user = user;
            this._userTokenRepository = _userTokenRepository;
            this._claimedVoucherRepository = _claimedVoucherRepository;
        }

        public UserInfo GetByUserName(string name)
        {
            return MapperHelper.Map<UserInfo>(_userRepository.Query(x => x.Email == name));
        }

        public async Task<UserInfo> GetByUserNameAsync(string name)
        {
            User user = _userRepository.Query(c => c.Email == name).FirstOrDefault();
            await System.Threading.Tasks.Task.Run(() =>
           {
               user = _userRepository.Query((x => x.Email == name)).FirstOrDefault();

           });

            return MapperHelper.Map<UserInfo>(user);

        }

        public Task<bool> UpdateUserProfileImageAsync(string name, string imageName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserLock(int userId, bool isLock)
        {
            var user = (await this._userRepository.QueryAsync(x => x.Id == userId)).FirstOrDefault();
            user.IsActive = isLock;
            return true;
        }

        public async Task<UserDto> GetByUserId(int userId)
        {
            try
            {
                var item = await _userRepository.QueryAsync(x => x.Id == userId);

                if (item.Count() > 0)
                {
                    User user = item.FirstOrDefault();
                    UserDto userProfile = MapperHelper.Map<UserDto>(user);
                    if (user.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0)
                    {
                        userProfile.LimitForEventsReached = true;
                        var userSubscriptions = user.UserSubscriptions.Where(a => a.PaymentStatusId == (int)PaymentStatusEnum.Paid);
                        if (userSubscriptions.Count() > 0)
                        {
                            int? noOfEvents = userSubscriptions.Count();

                            var eventsCount = this._eventRepository.Query(x => x.CreatedBy == userId).Count();
                            userProfile.LimitForEventsReached = eventsCount >= noOfEvents;
                        }

                    }
                    return userProfile;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDto> VerifyVoucher(int userId,int eventid)
        {
            try
            {
                var item = await _userRepository.QueryAsync(x => x.Id == userId && x.EventId== eventid);

                if (item.Count() > 0)
                {
                    User user = item.FirstOrDefault();
                    UserDto userProfile = MapperHelper.Map<UserDto>(user);
                    if (user.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0)
                    {
                        userProfile.LimitForEventsReached = true;
                        var userSubscriptions = user.UserSubscriptions.Where(a => a.PaymentStatusId == (int)PaymentStatusEnum.Paid);
                        if (userSubscriptions.Count() > 0)
                        {
                            int? noOfEvents = userSubscriptions.Count();

                            var eventsCount = this._eventRepository.Query(x => x.CreatedBy == userId).Count();
                            userProfile.LimitForEventsReached = eventsCount >= noOfEvents;
                        }

                    }
                    return userProfile;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateUserProfile(UserDto userProfile)
        {
            User userProfileObj = await _userRepository.GetAsync(userProfile.Id);
            var dbUser = MapperHelper.Map<User>(userProfile);
            userProfileObj.UsedFoodVouchers = userProfile.UsedFoodVouchers + userProfileObj.UsedFoodVouchers;
            userProfileObj.UsedDrinkVouchers = userProfile.UsedDrinkVouchers + userProfileObj.UsedDrinkVouchers;
            userProfileObj.PreferredLanguages.Clear();
            foreach (PreferredLanguage item in dbUser.PreferredLanguages)
            {
                userProfileObj.PreferredLanguages.Add(item);
            }
            if (userProfile.UsedFoodVouchers != 0 || userProfile.UsedDrinkVouchers != 0)
            {
                var claimedVoucherObj = new ClaimedVoucher
                {
                    UserId = userProfile.Id,
                    EventId = (int)userProfile.EventId,
                    CreatedOn = System.DateTime.Now,
                    CreatedBy=this.user.UserInfo.Id,
                    ClaimedDrinkVoucher=userProfile.UsedDrinkVouchers,
                    ClaimedFoodVoucher=userProfile.UsedFoodVouchers

                };
                this._claimedVoucherRepository.Insert(claimedVoucherObj);
                this._claimedVoucherRepository.Commit();
            }

            userProfileObj.Update(dbUser);
            return true;
        }

        public async Task<bool> SendEmailBatch(FilterParams filterParams, string searchtext, string Subject, string Body)
        {
            var FilterdUsers = await GetPagedUsers(filterParams, 1, int.MaxValue - 1, true, searchtext, string.Empty);
            foreach (var customer in FilterdUsers)
            {
                //  BackgroundJob.Enqueue<IUserProfileService>(x => x.SendEmailBatchBackgroundJob(customer.Id, searchtext, isEmployee, Subject, Body));
                //context.GetUserManager<Microsoft.AspNet.Identity.UserManager<IdentityUser>>().SendEmailAsync(user.Id, subject, body);     
            }
            return true;
        }

        public async Task<bool> SendSMSBatch(FilterParams filterParams, string searchtext, string SMSmessage)
        {
            var FilterdUsers = await GetPagedUsers(filterParams, 1, int.MaxValue - 1, true, searchtext, string.Empty);
            foreach (var customer in FilterdUsers)
            {
                //BackgroundJob.Enqueue<IUserProfileService>(x => x.SendSMSBatchBackgroundJob(customer.UserId, searchtext, isEmployee, SMSmessage));
            }

            return true;
        }


        public async Task<List<UserDto>> GetPagedUsers(FilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy)
        {
            Expression<Func<User, bool>> filter = GetUsersFilter(filterParams, searchtext);
            List<UserDto> listOfPagedUsers = new List<UserDto>();

            switch (sortBy)
            {
                case "user.email":
                    Expression<Func<User, string>> serviceFilterOrder = b => b.Email;
                    var serviceItems = await this._userRepository.QueryAsync(filter, pageIndex - 1, pageCount, serviceFilterOrder, ascending);
                    if (serviceItems.Count() > 0)
                    {
                        listOfPagedUsers = serviceItems.Select(x => MapperHelper.Map<UserDto>(x)).ToList();
                    }
                    break;

                case "user.firstName":
                    Expression<Func<User, string>> arabicServiceFilterOrder = b => b.FirstName;
                    var arabicServiceItems = await this._userRepository.QueryAsync(filter, pageIndex - 1, pageCount, arabicServiceFilterOrder, ascending);
                    if (arabicServiceItems.Count() > 0)
                    {
                        listOfPagedUsers = arabicServiceItems.Select(x => MapperHelper.Map<UserDto>(x)).ToList();
                    }
                    break;

                case "user.lastName":
                    Expression<Func<User, string>> applicationFilterOrder = b => b.LastName;
                    var applicationItems = await this._userRepository.QueryAsync(filter, pageIndex - 1, pageCount, applicationFilterOrder, ascending);
                    if (applicationItems.Count() > 0)
                    {
                        listOfPagedUsers = applicationItems.Select(x => MapperHelper.Map<UserDto>(x)).ToList();
                    }
                    break;

                case "createdOn":
                    Expression<Func<User, DateTime>> createdOnFilterOrder = b => b.CreatedOn;
                    var createdOnItems = await this._userRepository.QueryAsync(filter, pageIndex - 1, pageCount, createdOnFilterOrder, ascending);
                    if (createdOnItems.Count() > 0)
                    {
                        listOfPagedUsers = createdOnItems.Select(x => MapperHelper.Map<UserDto>(x)).ToList();
                    }
                    break;

                default:
                    Expression<Func<User, int>> idFilterOrder = b => b.Id;
                    var idItems = await this._userRepository.QueryAsync(filter, pageIndex - 1, pageCount, idFilterOrder, ascending);
                    if (idItems.Count() > 0)
                    {
                        listOfPagedUsers = idItems.Select(x => MapperHelper.Map<UserDto>(x)).ToList();
                    }
                    break;
            }

            ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
            var loggedUser = await currentUser.GetUserInfoAsync();
            List<UserAction> userActions = _userActionRepository.GetAll().ToList();
            int[] roleIds = loggedUser.Roles.Select(x => x.Id).ToArray();


            foreach (var item in listOfPagedUsers)
            {
                item.UserActions = userActions.Where(a => (a.NameEn != "Unlock User") || (a.NameEn != "Lock User"))
                    .Select(x => MapperHelper.Map<UserActionDto>(x)).ToList();
            }


            return listOfPagedUsers;
        }

        public Expression<Func<User, bool>> GetUsersFilter(FilterParams filterParams, string searchtext)
        {
            Expression<Func<User, bool>> filter;

            filter = a => ((searchtext == null || a.Email.Contains(searchtext) || a.FirstName.Contains(searchtext) || a.LastName.Contains(searchtext) ||
                            a.PhoneNumber.Contains(searchtext)));

            if (filterParams != null)
            {
                int[] roleIds = (filterParams.Roles == null || filterParams.Roles.Count() == 0) ? new int[0] : filterParams.Roles.Select(x => x.Id).ToArray();
                bool emptyRole = (filterParams.Roles == null || filterParams.Roles.Count() == 0);

                Expression<Func<User, bool>> filterExt = a =>
                            (emptyRole || a.Roles.Where(b => roleIds.Contains((short)b.Id)).FirstOrDefault() != null);

                var parameter = Expression.Parameter(typeof(User));

                var leftVisitor = new ReplaceExpressionVisitor(filter.Parameters[0], parameter);
                var left = leftVisitor.Visit(filter.Body);

                var rightVisitor = new ReplaceExpressionVisitor(filterExt.Parameters[0], parameter);
                var right = rightVisitor.Visit(filterExt.Body);

                Expression<Func<User, bool>> mergedFilter = Expression.Lambda<Func<User, bool>>(Expression.AndAlso(left, right), parameter);

                return mergedFilter;
            }

            return filter;
        }

        public async Task<int> GetNumbersOfRecords(FilterParams filterParams, string searchtext)
        {
            Expression<Func<User, bool>> filter = GetUsersFilter(filterParams, searchtext);

            return await this._userRepository.GetCountAsync(filter);
        }

        public async Task<bool> UserLock(UserActionsTakenDto userAction, UserActionsEnum action)
        {

            try
            {
                using (var dataContext = new eventsappEntities())
                {
                    var user = dataContext.Users.Where(x => x.Id == userAction.UserId).FirstOrDefault();

                    if (action == UserActionsEnum.LockUser)
                        user.IsActive = false;
                    else
                        user.IsActive = true;

                    ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
                    var loggedUser = await currentUser.GetUserInfoAsync();

                    UserActionsTaken newAction = new UserActionsTaken();
                    newAction.ActionBy = loggedUser.Id;
                    newAction.ActionDate = DateTime.Now;
                    newAction.Note = userAction.Note;
                    newAction.UserActionId = (short)action;

                    user.UserActionsTakens.Add(newAction);
                    dataContext.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendEmailUserAction(UserActionsTakenDto userAction)
        {
            var user = this._userRepository.Get(userAction.ActionBy);
            ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
            var loggedUser = await currentUser.GetUserInfoAsync();

            UserActionsTaken newAction = new UserActionsTaken();
            newAction.ActionBy = loggedUser.Id;
            newAction.ActionDate = DateTime.Now;
            newAction.Note = userAction.Note;
            newAction.UserActionId = (short)UserActionsEnum.SendEmail;

            user.UserActionsTakens.Add(newAction);

            return true;
        }

        public async Task<bool> SendSMSUserAction(UserActionsTakenDto userAction)
        {
            var user = this._userRepository.Get(userAction.ActionBy);
            ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
            var loggedUser = await currentUser.GetUserInfoAsync();

            UserActionsTaken newAction = new UserActionsTaken();
            newAction.ActionBy = loggedUser.Id;
            newAction.ActionDate = DateTime.Now;
            newAction.Note = userAction.Note;
            newAction.UserActionId = (short)UserActionsEnum.SendSMS;

            user.UserActionsTakens.Add(newAction);

            return true;
        }


        public async Task<bool> IsPhoneExist(string phoneNumber)
        {
            try
            {
                using (var dataContext = new eventsappEntities())
                {
                    var mobile = ("+" + phoneNumber).Replace(" ", string.Empty);
                    var result = dataContext.Users.Where(x => x.PhoneNumber == mobile);
                    return result.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserSubscriptionDto>> GetUserSubscriptions(FilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy)
        {
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            Expression<Func<UserSubscription, bool>> filter = x => (isSuperAdmin == true || x.UserId == this.user.UserInfo.Id) && 
             (String.IsNullOrEmpty(searchtext) || x.PaymentStatus.NameEn.Contains(searchtext) || x.PaymentStatus.NameAr.Contains(searchtext)
             || x.EventPackage.NameEn.Contains(searchtext) || x.EventPackage.NameAr.Contains(searchtext)
             || x.Fees.ToString().Contains(searchtext) || x.User.FirstName.ToString().Contains(searchtext)
             || x.User.LastName.ToString().Contains(searchtext));
            List<UserSubscriptionDto> userSubscriptions = new List<UserSubscriptionDto>();

            Expression<Func<UserSubscription, int>> idFilterOrder = b => b.Id;
            var idItems = await this._userSubscriptionRepository.QueryAsync(filter, pageIndex - 1, pageCount, idFilterOrder, ascending);
            if (idItems.Count() > 0)
            {
                userSubscriptions = idItems.Select(x => MapperHelper.Map<UserSubscriptionDto>(x)).ToList();
            }

            return userSubscriptions;
        }
        public async Task<int> GetSubscriptionNumbersOfRecords(FilterParams filterParams, string searchtext)
        {
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            Expression<Func<UserSubscription, bool>> filter = x => (isSuperAdmin == true || x.UserId == this.user.UserInfo.Id) &&
             (String.IsNullOrEmpty(searchtext) || x.PaymentStatus.NameEn.Contains(searchtext) || x.PaymentStatus.NameAr.Contains(searchtext)
             || x.EventPackage.NameEn.Contains(searchtext) || x.EventPackage.NameAr.Contains(searchtext)
             || x.Fees.ToString().Contains(searchtext));
            return await this._userSubscriptionRepository.GetCountAsync(filter);
        }

        public async Task<int> CreateSubscription(int eventPackageId)
        {
            var userSubscriptionObj = new UserSubscription();
            userSubscriptionObj.CreatedOn = DateTime.Now;
            userSubscriptionObj.PaymentStatusId = (int)PaymentStatusEnum.Pending;
            userSubscriptionObj.UserId = this.user.UserInfo.Id;
            userSubscriptionObj.EventPackageId = eventPackageId;
            this._userSubscriptionRepository.Insert(userSubscriptionObj);
            this._userSubscriptionRepository.Commit();
            return userSubscriptionObj.Id;
        }

        public async Task<string> AddCompany(string companyCode)
        {
            var companyObj = this._companyRepository.Query(x => x.CompanyCode.ToLower() == companyCode.ToLower()).FirstOrDefault();
            if (companyObj == null)
            {
                throw new Exception("InvalidCompanyCode");
            }
            else
            {
                if(this.user.UserInfo.UserCompanies.Count(x=>x.CompanyId == companyObj.Id) > 0)
                {
                    throw new Exception("CompanyAlreadyExists");
                } 
                var userCompanyObj = new UserCompany();
                userCompanyObj.UserId = this.user.UserInfo.Id;
                userCompanyObj.CompanyId = companyObj.Id;
                this._userCompanyRepository.Insert(userCompanyObj);
                this._userCompanyRepository.Commit();
                return "AddedSuccesfully";
            }
        }
        public async Task<string> SetUserToken(int userId, string token)
        {
            try
            {
                var loggedUser = await this._userTokenRepository.QueryAsync(x => x.UserId == userId);
                if (loggedUser.Count > 0 && loggedUser != null)
                {
                    loggedUser.FirstOrDefault().Token = token;
                    this._userTokenRepository.Commit();
                    return "Token has been updated for user : " + userId + " at time : " + DateTime.Now.ToString();
                }
                else
                {
                    var newConnection = new LoggedUserConnection()
                    {
                        UserId = userId,
                        Token = token
                    };
                    this._userTokenRepository.Insert(newConnection);
                    return "Token has been inserted for user : " + userId +" at time : "+ DateTime.Now.ToString();
                }
            }
            catch(Exception ex)
            {
                return ex.InnerException!=null ?  ex.InnerException.Message :  ex.Message;
            }
           
        }
    }
}

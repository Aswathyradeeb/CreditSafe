using EventsApp.Framework.Utilities;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.Enums;

namespace Eventsapp.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IKeyedRepository<ClaimedVoucher, int> _ClaimedVoucherRepository;
        private readonly IKeyedRepository<Event, int> _eventRepository;
        private readonly IKeyedRepository<User, short> _userRepository;
        private readonly ICurrentUser User;

        public VoucherService(IKeyedRepository<ClaimedVoucher, int> ClaimedVoucherRepository,
             IKeyedRepository<Event, int> _eventRepository, ICurrentUser User,
            IKeyedRepository<User, short> _userRepository)
        {
            this._ClaimedVoucherRepository = ClaimedVoucherRepository;
            this._eventRepository = _eventRepository;
            this.User = User;
            this._userRepository = _userRepository;
        }

        public async Task<bool> ResetVouchersCount(bool IsReset)
        {
            if (IsReset)
            {
                var UserLst = (await this._userRepository.QueryAsync(u => u.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete ||
                                u.RegistrationTypeId == (int?)RegistrationTypeEnum.Official ||
                                u.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)).ToList();
                foreach (var item in UserLst)
                {
                    item.UsedDrinkVouchers = 0;
                    item.UsedFoodVouchers = 0;
                }
                this._userRepository.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ClaimedVoucherDto>> GetClaimedVouchers(bool IsASuperAdmin)
        {
            List<ClaimedVoucherDto> listOfPagedClaimedVouchers = new List<ClaimedVoucherDto>();


            ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
            var loggedUser = await currentUser.GetUserInfoAsync();

            if (IsASuperAdmin == true)
            {
                var dataList = await this._ClaimedVoucherRepository.GetAllAsync();
                listOfPagedClaimedVouchers = MapperHelper.Map<List<ClaimedVoucherDto>>(dataList);
            }
            else
            {
                var dataList = (await this._ClaimedVoucherRepository.QueryAsync(x => x.User1.Id == loggedUser.Id)).ToList();
                listOfPagedClaimedVouchers = MapperHelper.Map<List<ClaimedVoucherDto>>(dataList);
            }

            return listOfPagedClaimedVouchers;
        }

        public async Task<List<ClaimedVoucherDto>> GetPagedClaimedVoucher(ReportsFilterParams filterParams, int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy)
        {
            Expression<Func<ClaimedVoucher, bool>> filter = GetClaimedVouchersFilter(filterParams, searchtext);
            List<ClaimedVoucherDto> listOfPagedClaimedVouchers = new List<ClaimedVoucherDto>();

            switch (sortBy)
            {
                case "event.nameEn":
                    Expression<Func<ClaimedVoucher, string>> serviceFilterOrder = b => b.Event.NameEn;
                    var serviceItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, serviceFilterOrder, ascending);
                    if (serviceItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = serviceItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "user.firstName":
                    Expression<Func<ClaimedVoucher, string>> arabicServiceFilterOrder = b => b.User.FirstName;
                    var arabicServiceItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, arabicServiceFilterOrder, ascending);
                    if (arabicServiceItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = arabicServiceItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "user.email":
                    Expression<Func<ClaimedVoucher, string>> applicationFilterOrder = b => b.User.Email;
                    var applicationItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, applicationFilterOrder, ascending);
                    if (applicationItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = applicationItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "createdOn":
                    Expression<Func<ClaimedVoucher, DateTime>> createdOnFilterOrder = b => b.CreatedOn;
                    var createdOnItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, createdOnFilterOrder, ascending);
                    if (createdOnItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = createdOnItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "AssignedFoodVouchers":
                    Expression<Func<ClaimedVoucher, int?>> AssignedFoodVouchersFilterOrder = b => b.User.AssignedFoodVouchers;
                    var AssignedFoodVouchersItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, AssignedFoodVouchersFilterOrder, ascending);
                    if (AssignedFoodVouchersItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = AssignedFoodVouchersItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "AssignedDrinkVouchers":
                    Expression<Func<ClaimedVoucher, int?>> AssignedDrinkVouchersFilterOrder = b => b.User.AssignedDrinkVouchers;
                    var AssignedDrinkVouchersFilterOrderItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, AssignedDrinkVouchersFilterOrder, ascending);
                    if (AssignedDrinkVouchersFilterOrderItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = AssignedDrinkVouchersFilterOrderItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "ClaimedFoodVoucher":
                    Expression<Func<ClaimedVoucher, int?>> ClaimedFoodVoucherFilterOrder = b => b.ClaimedFoodVoucher;
                    var ClaimedFoodVoucherItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, ClaimedFoodVoucherFilterOrder, ascending);
                    if (ClaimedFoodVoucherItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = ClaimedFoodVoucherItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "ClaimedDrinkVoucher":
                    Expression<Func<ClaimedVoucher, int?>> ClaimedDrinkVoucherFilterOrder = b => b.ClaimedDrinkVoucher;
                    var ClaimedDrinkVoucherItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, ClaimedDrinkVoucherFilterOrder, ascending);
                    if (ClaimedDrinkVoucherItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = ClaimedDrinkVoucherItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "UsedFoodVouchers":
                    Expression<Func<ClaimedVoucher, int?>> UsedFoodVouchersFilterOrder = b => b.User.UsedFoodVouchers;
                    var UsedFoodVouchersItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, UsedFoodVouchersFilterOrder, ascending);
                    if (UsedFoodVouchersItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = UsedFoodVouchersItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                case "UsedDrinkVouchers":
                    Expression<Func<ClaimedVoucher, int?>> UsedDrinkVouchersFilterOrder = b => b.User.UsedDrinkVouchers;
                    var UsedDrinkVouchersItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, UsedDrinkVouchersFilterOrder, ascending);
                    if (UsedDrinkVouchersItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = UsedDrinkVouchersItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;

                default:
                    Expression<Func<ClaimedVoucher, int>> idFilterOrder = b => b.Id;
                    var idItems = await this._ClaimedVoucherRepository.QueryAsync(filter, pageIndex - 1, pageCount, idFilterOrder, ascending);
                    if (idItems.Count() > 0)
                    {
                        listOfPagedClaimedVouchers = idItems.Select(x => MapperHelper.Map<ClaimedVoucherDto>(x)).ToList();
                    }
                    break;
            }
            ICurrentUser currentUser = IoC.Instance.Resolve<ICurrentUser>();
            var loggedUser = await currentUser.GetUserInfoAsync();
            //List<UserAction> UserActions = _UserActionRepository.GetAll().ToList();
            //int[] roleIds = loggedUser.Roles.Select(x => x.Id).ToArray();


            //foreach (var item in listOfPagedClaimedVouchers)
            //{
            //    item.User.UserActions = UserActions.Where(a => (a.NameEn != "Unlock User") || (a.NameEn != "Lock User"))
            //        .Select(x => MapperHelper.Map<UserActionDto>(x)).ToList();
            //}


            return listOfPagedClaimedVouchers;
        }

        public Expression<Func<ClaimedVoucher, bool>> GetClaimedVouchersFilter(ReportsFilterParams filterParams, string searchtext)
        {
            Expression<Func<ClaimedVoucher, bool>> filter;

            filter = a => ((searchtext == null ||
            a.User.Email.Contains(searchtext) ||
            a.User.FirstName.Contains(searchtext) ||
            a.User.LastName.Contains(searchtext) ||
            a.Event.NameEn.Contains(searchtext) ||
            a.Event.DescriptionEn.Contains(searchtext)) && (
            (a.EventId == filterParams.EventId) &&
            (filterParams.RegistrationTypeId == null || a.User.RegistrationTypeId == (int)filterParams.RegistrationTypeId) &&
            (a.User1.Id == filterParams.RestaurantUserId)));

            if (filterParams != null)
            {
                int[] roleIds = (filterParams.Roles == null || filterParams.Roles.Count() == 0) ? new int[0] : filterParams.Roles.Select(x => x.Id).ToArray();
                bool emptyRole = (filterParams.Roles == null || filterParams.Roles.Count() == 0);
                Expression<Func<ClaimedVoucher, bool>> filterExt = a =>
                            (emptyRole || a.User.Roles.Where(b => roleIds.Contains((short)b.Id)).FirstOrDefault() != null);

                //int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
                //bool emptyEvent = (filterParams.Events == null || filterParams.Events.Count() == 0);

                //Expression<Func<ClaimedVoucher, bool>> filterEvent = a =>
                //            (emptyEvent || eventIds.Select(e => e == (short)a.Event.Id).FirstOrDefault() == true);

                //int[] regsitrationTypesIds = (filterParams.RegistrationTypes == null || filterParams.RegistrationTypes.Count() == 0) ? new int[0] : filterParams.RegistrationTypes.Select(x => x.Id).ToArray();
                //bool emptyRegsitrationType = (filterParams.RegistrationTypes == null || filterParams.RegistrationTypes.Count() == 0);

                //Expression<Func<ClaimedVoucher, bool>> filterRegistrationType = a =>
                //            (emptyRegsitrationType || regsitrationTypesIds.Select(e => e == (short)a.User.RegistrationTypeId).FirstOrDefault() == true);

                var parameter = Expression.Parameter(typeof(ClaimedVoucher));

                var leftVisitor = new ReplaceExpressionVisitor(filter.Parameters[0], parameter);
                var left = leftVisitor.Visit(filter.Body);

                //var leftVisitorEvent = new ReplaceExpressionVisitor(filterEvent.Parameters[0], parameter);
                //var middle1 = leftVisitorEvent.Visit(filter.Body);

                var rightVisitor = new ReplaceExpressionVisitor(filterExt.Parameters[0], parameter);
                var right = rightVisitor.Visit(filterExt.Body);

                Expression<Func<ClaimedVoucher, bool>> mergedFilter = Expression.Lambda<Func<ClaimedVoucher, bool>>(Expression.AndAlso(left, right), parameter);

                return mergedFilter;
            }

            return filter;
        }

        public async Task<int> GetNumbersOfRecords(ReportsFilterParams filterParams, string searchtext)
        {
            Expression<Func<ClaimedVoucher, bool>> filter = GetClaimedVouchersFilter(filterParams, searchtext);

            return await this._ClaimedVoucherRepository.GetCountAsync(filter);
        }
    }
}

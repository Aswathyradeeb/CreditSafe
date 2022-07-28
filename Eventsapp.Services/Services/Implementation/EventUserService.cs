using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Eventsapp.Services
{
    public class EventUserService : IEventUserService
    {
        private readonly IEventRepository eventRepository;
        private readonly IUserRepository userRepository;
        private readonly IEventUserRepository eventUserRepository;
        private readonly IKeyedRepository<UserSubscription, int> _userSubscriptionRepository;
        private readonly INotificationsRepository notificationsRepository;
        private static readonly IMailSender _mailSender = IoC.Instance.Resolve<IMailSender>();
        private readonly ICurrentUser user;

        public EventUserService(IEventRepository eventRepository, IEventRepository _eventRepository,
            IEventUserRepository eventUserRepository, IUserRepository userRepository,
            IKeyedRepository<UserSubscription, int> _userSubscriptionRepository, ICurrentUser user)
        {
            this.eventRepository = _eventRepository;
            this.eventUserRepository = eventUserRepository;
            this.userRepository = userRepository;
            this._userSubscriptionRepository = _userSubscriptionRepository;
            this.user = user;
        }

        public EventDto Update(EventDto _eventDto, int userId)
        {
            Event eventEntity = this.eventRepository.Get(_eventDto.Id);
            Event _event = MapperHelper.Map<Event>(_eventDto);

            eventEntity.EventUsers.Add(new EventUser() { UserId = userId, EventId = _event.Id });

            List<EventUser> deletedEventUser = _event.EventUsers.Where(x => eventEntity.EventUsers.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventUser item in deletedEventUser)
            {
                eventEntity.EventUsers.Remove(item);
            }

            //var user = userRepository.Query(x => x.Id == userId).FirstOrDefault();

            //var body = "Hi, This is Event Invation Email\n\n";
            //var eventDetails = "Subject: " + _eventDto.NameEn + '\n';
            //eventDetails += "Start Date: " + _eventDto.StartDate + '\n';
            //eventDetails += "End Date: " + _eventDto.EndDate + '\n';
            //body += eventDetails;

            //var guid = Guid.NewGuid();
            // var isEmailsSent = _mailSender.SendEmailAsync(guid.ToString(), body,  "noreply@hifive.ae", null, _eventDto.NameEn + " Registration", user.Email, true, "");

            //Send notifications

            var mobUserList = notificationsRepository.Query(x => x.UserId == userId);

            if (mobUserList.Count() > 0) // is mobille user ?
            {
                NotifyIOSDto notifyIOSDto = new NotifyIOSDto()
                {
                    NameEn = _eventDto.NameEn,
                    NameAr = _eventDto.NameAr,
                    StartDate = _eventDto.StartDate,
                    BannerPhoto = _eventDto.BannerPhoto,
                    UserId = userId.ToString(),
                    EventId = _eventDto.Id
                };

                PushNotificationService sendNot = new PushNotificationService();
                sendNot.sendNotification(notifyIOSDto, new string[] { mobUserList.FirstOrDefault().DeviceToken });
            }

            //End

            return MapperHelper.Map<EventDto>(eventEntity);

        }

        public EventDto delete(EventDto _eventDto, int userId)
        {
            Event eventEntity = this.eventRepository.Get(_eventDto.Id);
            EventUser eventUser = eventEntity.EventUsers.Where(x => x.EventId == _eventDto.Id && x.UserId == userId).FirstOrDefault();
            eventEntity.EventUsers.Remove(eventUser);
            return MapperHelper.Map<EventDto>(eventEntity);
        }

        public async Task<EventUserDto> GetEventUsersbyAttendeeID(int attId)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;

            EventUser eventUserEntity = this.eventUserRepository.Query(x => x.Id == attId
             && (isSuperAdmin == true || (isAdmin == true && x.Event.CreatedBy == this.user.UserInfo.Id))).FirstOrDefault();
            EventUserDto item = MapperHelper.Map<EventUserDto>(eventUserEntity);

            if (item.Event.UserSubscription != null)
            {
                switch (item.Event.UserSubscription.EventPackageId.Value)
                {
                    case (int)EventPackageEnum.Hachling:
                        item.checkInEnabled = false;
                        item.BadgePrintEnabled = false;
                        break;

                    case (int)EventPackageEnum.Baby:
                        item.checkInEnabled = true;
                        item.BadgePrintEnabled = false;
                        break;

                    case (int)EventPackageEnum.Business:
                    case (int)EventPackageEnum.BusinessPro:
                    case (int)EventPackageEnum.Flexible:
                        item.BadgePrintEnabled = true;
                        item.checkInEnabled = true;
                        break;
                }
            }
            else if (item.User.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0)
            {
                item.BadgePrintEnabled = true;
                item.checkInEnabled = true;
            }
            return item;

        }

        public async Task<List<EventAttendeesDto>> GetEventUsersByEventId(int eventId)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            List<EventAttendeesDto> eventUserDto;
            if (eventId == 0)
            {
                List<EventUser> eventUserEntity = await this.eventUserRepository.QueryAsync(x => x.EventId > 0

                && (isSuperAdmin == true || (isAdmin == true && x.Event.CreatedBy == this.user.UserInfo.Id)));
                eventUserDto = MapperHelper.Map<List<EventAttendeesDto>>(eventUserEntity);
            }
            else
            {
                List<EventUser> eventUserEntity = await this.eventUserRepository.QueryAsync(x => x.EventId == eventId

                 && (isSuperAdmin == true || (isAdmin == true && x.Event.CreatedBy == this.user.UserInfo.Id)));
                eventUserDto = MapperHelper.Map<List<EventAttendeesDto>>(eventUserEntity);
            }

            foreach (var item in eventUserDto)
            {
                item.BadgePrintEnabled = true;
                item.checkInEnabled = true;
            }
            //foreach (var item in eventUserDto)
            //{
            //    if (item.Event.UserSubscription != null)
            //    {
            //        switch (item.Event.UserSubscription.EventPackageId.Value)
            //        {
            //            case (int)EventPackageEnum.Hachling:
            //                item.checkInEnabled = false;
            //                item.BadgePrintEnabled = false;
            //                break;

            //            case (int)EventPackageEnum.Baby:
            //                item.checkInEnabled = true;
            //                item.BadgePrintEnabled = false;
            //                break;

            //            case (int)EventPackageEnum.Business:
            //            case (int)EventPackageEnum.BusinessPro:
            //                item.BadgePrintEnabled = true;
            //                item.checkInEnabled = true;
            //                break;

            //            case (int)EventPackageEnum.Flexible:
            //                if (item.UserSubscription != null && item.UserSubscription.PaymentStatusId == (int)PaymentStatusEnum.Paid)
            //                {
            //                    item.BadgePrintEnabled = true;
            //                    item.checkInEnabled = true;
            //                }
            //                else
            //                {
            //                    item.BadgePrintEnabled = false;
            //                    item.checkInEnabled = false;
            //                }
            //                break;
            //        }
            //    }
            //    else if (item.User.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0)
            //    {
            //        item.BadgePrintEnabled = true;
            //        item.checkInEnabled = true;
            //    }
            //}
            return eventUserDto;
        }

        public async Task<EventUserDto> RegisteredUserAttended(EventUserDto RegisteredUser)
        {
            EventUser eventUserEntity = new EventUser();
            try
            {
                eventUserEntity = (await this.eventUserRepository.QueryAsync(x => x.EventId == RegisteredUser.EventId && x.UserId == RegisteredUser.UserId && x.AgendaId == RegisteredUser.AgendaId)).FirstOrDefault();
                //EventUser _eUser = MapperHelper.Map<EventUser>(RegisteredUser);

                eventUserEntity.IsAttended = RegisteredUser.IsAttended;
                if (eventUserEntity.IsAttended == true)
                {
                    eventUserEntity.Agendum.AttendanceCount = eventUserEntity.Agendum.AttendanceCount + 1;
                }

                return MapperHelper.Map<EventUserDto>(eventUserEntity);
            }
            catch (Exception)
            {
                return MapperHelper.Map<EventUserDto>(eventUserEntity);
            }

        }

        public async Task<string> QRVisitorAttendance(int UserId, int EventId, int AgendaId, string lang)
        {
            var eventUserEntity = (await this.eventUserRepository.QueryAsync(x => x.EventId == EventId && x.UserId == UserId && x.AgendaId == AgendaId)).FirstOrDefault();
            if (eventUserEntity != null)
            {
                TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
                TimeSpan FromTime = DateTime.Parse(eventUserEntity.Agendum.FromTime).TimeOfDay;
                TimeSpan EndTime = DateTime.Parse(eventUserEntity.Agendum.ToTime).TimeOfDay;
                if (CurrentTime > FromTime && CurrentTime < EndTime)
                {
                    if (eventUserEntity.IsAttended != true)
                    {
                        eventUserEntity.Agendum.AttendanceCount = eventUserEntity.Agendum.AttendanceCount + 1;
                    } 
                    eventUserEntity.IsAttended = true;
                    if (lang == "en")
                    {
                        MessageService messageService = new MessageService();
                        var smsResult = await messageService.SendMessage("We welcome you to the Sharjah International Book Fair 39th Edition.", "en", eventUserEntity.User.PhoneNumber);
                    }
                    else
                    {
                        MessageService messageService = new MessageService();
                        var smsResult = await messageService.SendMessage("نرحب بكم في معرض الشارقة الدولي للكتاب الدورة 39.", "ar", eventUserEntity.User.PhoneNumber);

                    }
                    return "Visitor attendance is marked successfully";
                }
                else
                {
                    return "QR Code is not valid for current slot";
                }
            }
            else
            {
                return "QR Code is invalid. Vistor not found";
            }
        }

        public async Task<string> AddEventUser(EventUserDto RegisteredUser, int? packageId, string StandLocation, string StandNumber)
        {
            EventUser eventUserEntity = this.eventUserRepository.Query(x => x.EventId == RegisteredUser.EventId && x.UserId == RegisteredUser.UserId && x.RegistrationTypeId == RegisteredUser.RegistrationTypeId).FirstOrDefault();
            EventUserDto _usrDto = new EventUserDto();
            if (eventUserEntity == null)
            {
                EventUser qstnMap = MapperHelper.Map<EventUser>(RegisteredUser);
                this.eventUserRepository.Insert(qstnMap);
                var eventObj = this.eventRepository.Query(x => x.Id == RegisteredUser.EventId).FirstOrDefault();
                var userObj = this.userRepository.Query(x => x.Id == RegisteredUser.UserId).FirstOrDefault();
                if (RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete || RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete)
                {
                    EventPerson ep = new EventPerson
                    {
                        CreatedOn = DateTime.Now,
                        EventId = eventObj.Id,
                        IsApproved = null,
                        LastModified = DateTime.Now,
                        PersonId = userObj.PersonId.Value,
                        PersonTypeId = RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete ? (int)PersonTypeEnum.VIP : (int)PersonTypeEnum.Speaker
                    };
                    eventObj.EventPersons.Add(ep);
                }
                if (RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Sponsor || RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Exhibitor)
                {
                    EventCompany ec = new EventCompany
                    {
                        CreatedOn = DateTime.Now,
                        EventId = eventObj.Id,
                        IsApproved = null,
                        LastModified = DateTime.Now,
                        CompanyId = userObj.CompanyId.Value,
                        PackageId = packageId,
                        StandLocation = StandLocation,
                        StandNumber = StandNumber,
                        CompanyTypeId = RegisteredUser.RegistrationTypeId == (int)RegistrationTypeEnum.Sponsor ? (int)CompanyTypeEnum.Sponser : (int)CompanyTypeEnum.Exhibitor
                    };
                    eventObj.EventCompanies.Add(ec);
                }

                var value = await _mailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", userObj.FirstName + " " + userObj.LastName, userObj.Email, "Event Invitation", "<p> <strong>Congrats!</strong> You are successfully registered for Event : " + eventObj.NameEn + "</p>");

                _usrDto = MapperHelper.Map<EventUserDto>(RegisteredUser);
                return "Sucess";
            }
            else
            {
                return "Existing";
            }

        }

        public async Task<byte[]> GetExcelTemplate()
        {
            try
            {
                using (ExcelPackage xp = new ExcelPackage())
                {
                    ExcelWorksheet ws = xp.Workbook.Worksheets.Add("Event Participant");
                    ws.Cells[1, 1, 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1, 1, 4].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DodgerBlue);
                    ws.Cells[1, 1, 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[1, 1, 1, 1].Value = "FirstName";
                    ws.Cells[1, 2, 1, 2].Value = "LastName";
                    ws.Cells[1, 3, 1, 3].Value = "Email";
                    ws.Cells[1, 4, 1, 4].Value = "Phone";
                    ws.Cells[2, 1, 2, 1].Value = "John";
                    ws.Cells[2, 2, 2, 2].Value = "Doe";
                    ws.Cells[2, 3, 2, 3].Value = "john_doe@xxx.xxx";
                    ws.Cells[2, 4, 2, 4].Value = "+971XXXXXXXXX";
                    ws.Cells.AutoFitColumns();
                    return xp.GetAsByteArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<EventUserDto>> SetUserRoleAndEvent(List<string> emails, int eventId, string connString)
        {

            try
            {
                var userData = await userRepository.QueryAsync(x => emails.Count(async => async == x.Email) > 0);
                var eventObj = this.eventRepository.Query(a => a.Id == eventId).FirstOrDefault();
                if (userData.Count > 0)
                {
                    foreach (var item in userData)
                    {
                        try
                        {
                            var eventUserObj = eventUserRepository.Query(a => a.UserId == item.Id && a.EventId == eventId && a.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).FirstOrDefault();
                            if (eventUserObj == null)
                            {
                                UserSubscription subscriptionObj = null;
                                if (eventObj.HasPayment == true)
                                {
                                    subscriptionObj = new UserSubscription
                                    {
                                        CreatedOn = DateTime.Now,
                                        EventPackageId = null,
                                        UserId = item.Id,
                                        PaymentStatusId = Convert.ToInt32(PaymentStatusEnum.Pending),
                                        TransactionId = null,
                                        Fees = eventObj != null ? eventObj.ParticipantFees : null
                                    };
                                }
                                var eventUsers = new EventUser()
                                {
                                    UserId = item.Id,
                                    EventId = eventId,
                                    RegistrationTypeId = (int)RegistrationTypeEnum.Athlete,
                                    IsApproved = eventObj.HasPayment == true ? false : true,
                                    CreatedOn = DateTime.Now,
                                    UserSubscription = subscriptionObj
                                };
                                eventUserRepository.Insert(eventUsers);
                                if (item.Roles.Count == 0)
                                {
                                    AddUserRole(item.Id, eventObj.User.CompanyId, connString);
                                    var url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/setUserPassword?userId={0}", item.Id);
                                    string registeremailBody = "<p> <strong>Congrats!</strong> You are successfully registered on Event Management System, please proceed with setting your password by clicking <a href=\"" + url + "\">here</a></p>";
                                    await Task.Factory.StartNew(() =>
                                    {
                                        _mailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", item.FirstName + " " + item.LastName, item.Email, "User Registration", registeremailBody);
                                    });
                                }
                                string emailBody = "<p> <strong>Congrats!</strong> You are successfully registered for Event : " + eventObj.NameEn;

                                await Task.Factory.StartNew(() =>
                                {
                                    _mailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", item.FirstName + " " + item.LastName, item.Email, "Event Invitation", emailBody);
                                });
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                var userEvents = eventId != 0 ? await eventUserRepository.QueryAsync(x => x.EventId == eventId) : null;
                return userEvents.Count > 0 && userEvents != null ? MapperHelper.Map<List<EventUserDto>>(userEvents) : null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void AddUserRole(int userId, int? companyId, string connString)
        {
            var userObj = this.userRepository.Query(a => a.Id == userId).FirstOrDefault();
            if (companyId != null && companyId != 0)
            {
                userObj.UserCompanies = userObj.UserCompanies.Count == 0 ? new List<UserCompany>() : userObj.UserCompanies;
                userObj.UserCompanies.Add(new UserCompany
                {
                    UserId = userId,
                    CompanyId = companyId
                });
            }
            if (userObj.Roles.Count == 0)
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlCommand cmd = new SqlCommand("dbo.AddUserRole", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@RoleId", SqlDbType.NVarChar).Value = Convert.ToInt32(RolesEnum.User);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private int InsertBulkEventUsers(string connString, DataTable tvp)
        {
            SqlConnection conn = new SqlConnection(connString);
            int result = 0;
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[InsertBulkUserforEvents]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter tvparam = cmd.Parameters.AddWithValue("@List", tvp);
                tvparam.SqlDbType = SqlDbType.Structured;
                tvparam.TypeName = "[Account].[EventUsersType]";
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return result;
        }

        public async Task<List<EventUserDto>> GenerateEventUsers(List<UserEventDTO> usersList, string connString)
        {
            List<UserEventDTO> newUsers = usersList;
            var allEmails = usersList.Select(x => x.Email).ToList();
            var existingUsers = await this.userRepository.QueryAsync(x => allEmails.Count(a => a == x.Email) > 0);
            if (existingUsers.Count > 0)
            {
                var existingUserEmails = existingUsers.Select(x => x.Email).ToList();
                newUsers = usersList.Where(x => existingUserEmails.Count(a => a == x.Email) == 0).ToList();
            }
            if (newUsers.Count > 0)
            {
                SqlConnection conn = new SqlConnection(connString);
                DataTable tvp;
                List<UserDataTable> userDT;
                userDT = new List<UserDataTable>();
                UserDataTable user;
                foreach (var item in newUsers)
                {
                    try
                    {
                        user = new UserDataTable();
                        user.FirstName = item.FirstName;
                        user.LastName = item.LastName;
                        user.IsActive = true;
                        user.PhoneNumber = item.Phone;
                        user.Email = item.Email;
                        user.CreatedOn = DateTime.Now;
                        user.EmailConfirmed = false;
                        user.PhoneNumberConfirmed = false;
                        user.CompanyId = null;
                        user.PersonId = null;
                        user.EventId = item.EventId;
                        user.LockoutEnabled = false;
                        user.LockoutEndDateUtc = null;
                        user.RegistrationTypeId = Convert.ToInt32(RegistrationTypeEnum.Athlete);
                        user.PasswordHash = null;
                        user.SecurityStamp = new Guid().ToString();
                        user.TwoFactorEnabled = false;
                        user.UserName = item.Email;
                        userDT.Add(user);
                        tvp = ToDataTable<UserDataTable>(userDT);
                        var result = InsertBulkEventUsers(connString, tvp);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            var email = allEmails.ToList();
            var allUsers = await SetUserRoleAndEvent(email, (int)usersList.FirstOrDefault().EventId, connString);
            return allUsers;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public async Task<List<AttendeeHistory>> GetAttendeeHistories(int userId, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            List<AttendeeHistory> collection = new List<AttendeeHistory>();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.UserAttendHistory";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AttendeeHistory sm = new AttendeeHistory();
                            sm.UserId = Convert.ToInt32(reader["UserId"].ToString());
                            sm.StartDate = Convert.ToDateTime(reader["StartDate"].ToString());
                            sm.EventNameEn = reader["EventNameEn"].ToString();
                            sm.EventNameAr = reader["EventNameAr"].ToString();
                            sm.AttendedAs = reader["AttendedAs"].ToString();
                            collection.Add(sm);
                        }
                    }
                    conn.Close();
                }
                return collection;
            }
            catch (Exception)
            {
                return null;
            }


        }

        public async Task<List<EventUserDto>> UpdateAttendance(int EventId, int UserId, int IsAttended)
        {
            var eventUserEntity = (await this.eventUserRepository.QueryAsync(x => x.EventId == EventId && x.UserId == UserId));
            if (eventUserEntity != null && eventUserEntity.Count > 0)
            {
                foreach (var item in eventUserEntity)
                {
                    item.IsAttended = IsAttended == 1 ? true : false;
                }
                return MapperHelper.Map<List<EventUserDto>>(eventUserEntity);
            }
            else
            {
                return MapperHelper.Map<List<EventUserDto>>(eventUserEntity);
            }
        }

        public async Task<List<EventAttendeesDto>> GetPagedEventUsers(int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy, int EventId)
        {
            try
            {
                if (searchtext != null)
                    searchtext = searchtext.Trim();
                Expression<Func<EventUser, bool>> filter = await GetFilter(searchtext, EventId);
                List<EventAttendeesDto> listOfPagedAttendees = new List<EventAttendeesDto>();

                switch (sortBy)
                {
                    case "id":
                        Expression<Func<EventUser, int>> idFilterOrder = b => b.Id;
                        var idItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, idFilterOrder, ascending);
                        if (idItems.Count() > 0)
                        {
                            listOfPagedAttendees = idItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;

                    case "user.firstName":
                    default:
                        Expression<Func<EventUser, string>> firstNameFilterOrder = b => b.User.FirstName;
                        var firstNameDateItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, firstNameFilterOrder, ascending);
                        if (firstNameDateItems.Count() > 0)
                        {
                            listOfPagedAttendees = firstNameDateItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;

                    case "user.lastName":
                        Expression<Func<EventUser, string>> lastNameFilterOrder = b => b.User.LastName;
                        var lastNameItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, lastNameFilterOrder, ascending);
                        if (lastNameItems.Count() > 0)
                        {
                            listOfPagedAttendees = lastNameItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;

                    case "user.phoneNumber":
                        Expression<Func<EventUser, string>> phoneNumberFilterOrder = b => b.User.PhoneNumber;
                        var phoneNumberItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, phoneNumberFilterOrder, ascending);
                        if (phoneNumberItems.Count() > 0)
                        {
                            listOfPagedAttendees = phoneNumberItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;
                    case "user.email":
                        Expression<Func<EventUser, string>> emailFilterOrder = b => b.User.Email;
                        var emailItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, emailFilterOrder, ascending);
                        if (emailItems.Count() > 0)
                        {
                            listOfPagedAttendees = emailItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;
                    case "agendum":
                        Expression<Func<EventUser, string>> agendumFilterOrder = b => b.Agendum.TitleEn;
                        var agendumItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, agendumFilterOrder, ascending);
                        if (agendumItems.Count() > 0)
                        {
                            listOfPagedAttendees = agendumItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;
                    case "isAttended":
                        Expression<Func<EventUser, bool?>> isAttendedFilterOrder = b => b.IsAttended;
                        var isAttendedItems = await this.eventUserRepository.QueryAsync(filter, pageIndex - 1, pageCount, isAttendedFilterOrder, ascending);
                        if (isAttendedItems.Count() > 0)
                        {
                            listOfPagedAttendees = isAttendedItems.Select(x => MapperHelper.Map<EventAttendeesDto>(x)).ToList();
                        }
                        break;
                }

                foreach (var item in listOfPagedAttendees)
                {
                    item.BadgePrintEnabled = true;
                    item.checkInEnabled = true;
                }

                return listOfPagedAttendees.ToList();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> GetNumbersOfPagedEventUsers(string searchtext, int EventId)
        {
            Expression<Func<EventUser, bool>> filter = await GetFilter(searchtext, EventId);
            return await this.eventUserRepository.GetCountAsync(filter);
        }

        public async Task<Expression<Func<EventUser, bool>>> GetFilter(string searchtext, int EventId)
        {
            Expression<Func<EventUser, bool>> filter = null;

            if (EventId == 0)
            {
                filter = a => ((searchtext == null ||
                a.User.FirstName.Contains(searchtext) ||
                a.User.LastName.Contains(searchtext) ||
                a.User.Email.Contains(searchtext) ||
                a.User.PhoneNumber.Contains(searchtext) ||
                a.Agendum.TitleAr.Contains(searchtext) ||
                a.Agendum.TitleEn.Contains(searchtext) ||
                a.Agendum.ToTime.Contains(searchtext) ||
                a.Agendum.FromTime.Contains(searchtext) ||
                a.Event.NameAr.Contains(searchtext) ||
                a.Event.NameEn.Contains(searchtext) ||
                a.Event.DescriptionEn.Contains(searchtext) ||
                a.Event.DescriptionAr.Contains(searchtext) ||
                a.RegistrationType.NameEn.Contains(searchtext) ||
                a.RegistrationType.NameAr.Contains(searchtext)));
            }
            else if (EventId > 0)
            {
                filter = a => ((searchtext == null ||
                 a.User.FirstName.Contains(searchtext) ||
                 a.User.LastName.Contains(searchtext) ||
                 a.User.Email.Contains(searchtext) ||
                 a.User.PhoneNumber.Contains(searchtext) ||
                 a.Agendum.TitleAr.Contains(searchtext) ||
                 a.Agendum.TitleEn.Contains(searchtext) ||
                 a.Agendum.ToTime.Contains(searchtext) ||
                 a.Agendum.FromTime.Contains(searchtext) ||
                 a.Event.NameAr.Contains(searchtext) ||
                 a.Event.NameEn.Contains(searchtext) ||
                 a.Event.DescriptionEn.Contains(searchtext) ||
                 a.Event.DescriptionAr.Contains(searchtext) ||
                 a.RegistrationType.NameEn.Contains(searchtext) ||
                 a.RegistrationType.NameAr.Contains(searchtext)) &&
                 a.EventId == EventId);
            }
            return filter;
        }

        public async Task<EventUserDto> GetEventAttendee(int EventId,int UserId, int AgendaId)
        {
            EventUser eventUserEntity = (await this.eventUserRepository.QueryAsync(x => x.EventId == EventId&& x.UserId == UserId && x.AgendaId == AgendaId)).FirstOrDefault();
            return MapperHelper.Map<EventUserDto>(eventUserEntity);
        }
    }
}

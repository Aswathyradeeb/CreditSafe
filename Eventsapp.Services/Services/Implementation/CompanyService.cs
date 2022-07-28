using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventsApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEventCompanyRepository _EventCompanyRepository;
        private readonly IKeyedRepository<User, int> _userRepository;
        private readonly IKeyedRepository<PreferredLanguage, int> _PreferredLanguagerRepository;
        private readonly ICurrentUser user;
        private static readonly IMailSender _mailSender = IoC.Instance.Resolve<IMailSender>();
        public CompanyService(ICompanyRepository companyRepository, IEventCompanyRepository eventCompanyRepository,
            IKeyedRepository<User, int> userRepository, ICurrentUser user,
            IKeyedRepository<PreferredLanguage, int> _PreferredLanguagerRepository)
        {
            this._companyRepository = companyRepository;
            this._EventCompanyRepository = eventCompanyRepository;
            this._userRepository = userRepository;
            this.user = user;
            this._PreferredLanguagerRepository = _PreferredLanguagerRepository;
        }

        public async Task<CompanyDto> CreateCompany(CompanyDto _company, string connString)
        {
            int ranNo = RandomNumber(5, 100);
            string randomStr = RandomString(2, true);
            string compcode = _company.NameEn.Substring(0, 3) + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            User userProfileObj = null;
            // _company.CompanyCode = compcode;
            Company companyObj = MapperHelper.Map<Company>(_company);
            if (companyObj.Address != null)
            {
                if (companyObj.Address.Country != null)
                {
                    companyObj.Address.CountryId = companyObj.Address.Country.Id;
                    companyObj.Address.Country = null;
                }
                if (companyObj.Address.State != null)
                {
                    companyObj.Address.StateId = companyObj.Address.State.Id;
                    companyObj.Address.State = null;
                }
            }
            companyObj.CompanyCode = compcode;
            companyObj.EventCompanies = null;
            companyObj.Users = null;
            companyObj.CreatedBy = this.user.UserInfo.Id;
            companyObj.CreatedOn = DateTime.Now;
            companyObj.UniqueId = compcode;
            this._companyRepository.Insert(companyObj);
            this._companyRepository.Commit();
            int companyId = companyObj.Id;
            if (String.IsNullOrEmpty(_company.Email) == false)
            {
                userProfileObj = this._userRepository.Query(a => a.Email == _company.Email).FirstOrDefault();
                if (userProfileObj != null && userProfileObj.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0 && userProfileObj.CompanyId != null)
                {
                    throw new Exception("Company Already Exists");
                }
                if (userProfileObj == null)
                {
                    userProfileObj = new User();
                    userProfileObj.FirstName = _company.NameEn;
                    userProfileObj.LastName = "";
                    userProfileObj.IsActive = true;
                    userProfileObj.PhoneNumber = _company.Phone;
                    userProfileObj.Email = _company.Email;
                    userProfileObj.CreatedOn = DateTime.Now;
                    userProfileObj.EmailConfirmed = false;
                    userProfileObj.PhoneNumberConfirmed = false;
                    userProfileObj.CompanyId = null;
                    userProfileObj.PersonId = null;
                    userProfileObj.EventId = null;
                    userProfileObj.LockoutEnabled = false;
                    userProfileObj.LockoutEndDateUtc = null;
                    userProfileObj.RegistrationTypeId = Convert.ToInt32(RegistrationTypeEnum.Athlete);
                    userProfileObj.PasswordHash = null;
                    userProfileObj.SecurityStamp = new Guid().ToString();
                    userProfileObj.TwoFactorEnabled = false;
                    userProfileObj.CompanyId = companyId;
                    userProfileObj.UserName = _company.Email;
                    userProfileObj.UserCompanies = new List<UserCompany>();
                    userProfileObj.UserCompanies.Add(new UserCompany
                    {
                        CompanyId = this.user.UserInfo.CompanyId
                    });
                    this._userRepository.Insert(userProfileObj);
                    this._userRepository.Commit();
                    userProfileObj = this._userRepository.Query(a => a.Email == _company.Email).FirstOrDefault();
                    var url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/setUserPassword?userId={0}", userProfileObj.Id);
                    string registeremailBody = "<p> <strong>Congrats!</strong> You are successfully registered on Event Management System, please proceed with setting your password by clicking <a href=\"" + url + "\">here</a></p>";
                    await _mailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", userProfileObj.FirstName + " " + userProfileObj.LastName, userProfileObj.Email, "User Registration", registeremailBody);
                    AddUserRole(userProfileObj.Id, connString);
                }
            }
            if (userProfileObj.Roles.Count(a => a.Id == (int)RolesEnum.User || a.Id == (int)RolesEnum.Administrator) > 0 && userProfileObj.CompanyId == null)
                userProfileObj.CompanyId = companyId;

            return MapperHelper.Map<CompanyDto>(companyObj);
        }

        public void AddUserRole(int userId, string connString)
        {
            var userObj = this._userRepository.Query(a => a.Id == userId).FirstOrDefault();
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

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public async Task<CompanyDto> GetCompanyId(int companyId)
        {
            var company = await this._companyRepository.GetAsync(companyId);
            if (company.DeletedOn != null)
            {
                return null;
            }
            var companyDto = MapperHelper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<CompanyDto> GetCompanyUser(int userid)
        {
            try
            {
                var cid = _companyRepository.Query(c => c.DeletedOn == null).Where(c => c.Users.Count(a => a.Id == userid) > 0).FirstOrDefault().Id;
                var companys = await this._companyRepository.GetAsync(cid);
                var companysDto = MapperHelper.Map<CompanyDto>(companys);
                return companysDto;
            }
            catch (Exception)
            {
                CompanyDto compdto = new CompanyDto();
                return compdto;
            }

        }

        public async Task<ReturnCompanyDto> GetAllCompanys(FilterParams filterParams, int page, int pageSize)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;

            int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
            bool emptyCompany = (filterParams.Events == null || filterParams.Events.Count() == 0);
            Func<Company, bool> filterExt = a => (emptyCompany || eventIds.Contains(a.Id))
            && a.DeletedOn == null
            && (isSuperAdmin == true || (isAdmin == true && a.CreatedBy == this.user.UserInfo.Id));
            var companys = await this._companyRepository.GetAllAsync();
            var companysDtoLst = MapperHelper.Map<List<CompanyDto>>(companys.Where(filterExt).Skip((page - 1) * pageSize)
                   .Take(pageSize).ToList());
            return new ReturnCompanyDto { companies = companysDtoLst, companiesCount = companys.Count() };
        }

        public async Task<List<CompanyDto>> GetCompanys()
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;

            var companys = await this._companyRepository.QueryAsync(a => a.DeletedOn == null
            && (isSuperAdmin == true || (isAdmin == true && a.CreatedBy == this.user.UserInfo.Id)));
            var companysDto = MapperHelper.Map<List<CompanyDto>>(companys);
            return companysDto;
        }

        public async Task<List<CompanyDto>> GetAllCompanies()
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;
            var userCompanies = this.user.UserInfo.UserCompanies.ToList();

            var companys = await this._companyRepository.QueryAsync(a => a.DeletedOn == null && a.User != null);
            companys = companys.Where(a => isSuperAdmin == true || a.CreatedBy == this.user.UserInfo.Id ||
            userCompanies.Count(b => b.CompanyId == a.Id) > 0).ToList();
            var companysDto = MapperHelper.Map<List<CompanyDto>>(companys);
            return companysDto;
        }

        public async Task<CompanyDto> UpdateCompany(CompanyDto _company)
        {
            Company company = MapperHelper.Map<Company>(_company);
            var CompanyEntity = await this._companyRepository.GetAsync(_company.Id);
            CompanyEntity.NameEn = company.NameEn;
            CompanyEntity.NameAr = company.NameAr;
            CompanyEntity.Phone = company.Phone;
            CompanyEntity.Email = company.Email;
            CompanyEntity.Photo = company.Photo;
            if (company.Address != null)
            {
                if (CompanyEntity.Address == null)
                    CompanyEntity.Address = new Address();

                CompanyEntity.Address.CountryId = company.Address.Country != null ? company.Address.Country.Id : company.Address.CountryId;
                CompanyEntity.Address.Lat = company.Address.Lat;
                CompanyEntity.Address.Lng = company.Address.Lng;
                CompanyEntity.Address.LocationPhoto = company.Address.LocationPhoto;
                CompanyEntity.Address.StateId = company.Address.State != null ? company.Address.State.Id : company.Address.StateId;
                CompanyEntity.Address.Street = company.Address.Street;
            }
            CompanyEntity.ModifiedOn = DateTime.Now;
            if (CompanyEntity.UniqueId == null)
            {
                int ranNo = RandomNumber(5, 100);
                string randomStr = RandomString(8, true);
                string compcode = _company.NameEn.Substring(0, 3) + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
                CompanyEntity.CompanyCode = compcode;
                CompanyEntity.UniqueId = compcode;
            }
            return MapperHelper.Map<CompanyDto>(CompanyEntity);
        }

        public async Task<List<PreferredLanguageDto>> SavePreferredLanguages(List<PreferredLanguageDto> PreferredLanguagesDtos)
        {
            var PreferredLanguages = MapperHelper.Map<List<PreferredLanguage>>(PreferredLanguagesDtos);

            var User = await _userRepository.GetAsync(PreferredLanguagesDtos.FirstOrDefault().UserId);
            User.PreferredLanguages.Clear();
           
            foreach (PreferredLanguage item in PreferredLanguages)
            {
                User.PreferredLanguages.Add(item);
            }
            return PreferredLanguagesDtos;
        }

        public async Task<string> DeleteCompany(int companyId)
        {
            var company = await this._companyRepository.GetAsync(companyId);
            company.DeletedOn = DateTime.Now;
            return "Deleted Successfully";
        }
    }
}

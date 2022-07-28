using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EventsApp.WebAPI.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompaniesController : ApiController
    {
        private readonly ICompanyService _companyServices;
        private readonly IUserService _userService;
        private readonly IKeyedRepository<PreferredLanguage, int> _preferredLanguageRepository;
        private ICurrentUser user;

        public CompaniesController(ICompanyService companyServices, IUserService _userService, ICurrentUser user,
            IKeyedRepository<PreferredLanguage, int> _preferredLanguageRepository)
        {
            this._companyServices = companyServices;
            this._userService = _userService;
            this._preferredLanguageRepository = _preferredLanguageRepository;
            this.user = user;
        }

        // GET: api/Companies
        [Route("GetCompanies")]
        public async Task<IHttpActionResult> GetCompanies()
        {
            try
            {
                var _companies = await this._companyServices.GetCompanys();
                return Ok(_companies);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Companies
        [Route("GetAllCompanies")]
        public async Task<IHttpActionResult> GetAllCompanies()
        {
            try
            {
                var _companies = await this._companyServices.GetAllCompanies();
                return Ok(_companies);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetCompanyPreferredLanguages")]
        public async Task<IHttpActionResult> GetCompanyPreferredLanguages(string CompanyCode)
        {
            try
            {
                var preferredLanguages = await this._preferredLanguageRepository.QueryAsync(x => x.User.Company.CompanyCode == CompanyCode);
                var preferredLanguagesDto = MapperHelper.Map<List<PreferredLanguageDto>>(preferredLanguages);
                return Ok(preferredLanguagesDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SavePreferredLanguages")]
        public async Task<IHttpActionResult> SavePreferredLanguages(List<PreferredLanguageDto> PreferredLanguages) 
        {
            try
            {
                var preferredLanguages = await this._companyServices.SavePreferredLanguages(PreferredLanguages);
                return Ok(preferredLanguages);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetAllCompanies")]
        public async Task<IHttpActionResult> GetAllCompanies(RequestFilter request)
        {
            try
            {
                var returnValue = await this._companyServices.GetAllCompanys(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList();
                pagedRecord.TotalRecords = returnValue.companiesCount;
                pagedRecord.Content = returnValue.companies;
                pagedRecord.CurrentPage = request.Page;
                pagedRecord.PageSize = request.PageSize;
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/Companies/5
        [HttpPost]
        [Route("GetCompanyById")]
        public async Task<IHttpActionResult> GetCompany(int id)
        {
            try
            {
                var _company = this._companyServices.GetCompanyId(id);
                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetCompanyByUser")]
        public async Task<IHttpActionResult> GetCompanyByUser(int uid)
        {
            try
            {
                var _company = this._companyServices.GetCompanyUser(uid);
                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }




        [HttpPost]
        [Route("CreateCompany")]
        public async Task<IHttpActionResult> CreateCompany(CompanyDto compnay)
        {
            try
            {
                compnay.CreatedBy = this.user.UserInfo.Id.ToString();
                var _company = await this._companyServices.CreateCompany(compnay, ConfigurationManager.AppSettings["connectionString"].ToString());
                var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                // var speaker = await _userService.GetByUserId(eventPerson.PersonId);

                try
                {
                    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, "Company", compnay.NameEn, System.DateTime.Now.ToString(), "speaker-register.html");
                    var result = await SendEmail("Company Registered", body, user.Email);

                    body = await createRegisterEmailBody(compnay.NameEn, "", compnay.NameEn, System.DateTime.Now.ToString(), "speaker-notification.html", user.FirstName, "Sponsor/Exhibitor");
                    result = await SendEmail("Company Registered", body, compnay.Email);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }


                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdateCompany")]
        public async Task<IHttpActionResult> UpdateCompany(CompanyDto compnay)
        {
            try
            {
                var _company = await this._companyServices.UpdateCompany(compnay);
                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("DeleteCompany")]
        public async Task<IHttpActionResult> DeleteCompany(int id)
        {
            try
            {
                var data = await this._companyServices.DeleteCompany(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #region Mail
        public async Task<bool> SendEmail(string subject, string body, string recipient)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    bool result = true;
                    mailMessage.From = new MailAddress("noreply@hifive.ae");
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(recipient);

                    SmtpClient smtp = new SmtpClient("mail.hifive.ae", 25);
                    smtp.EnableSsl = Convert.ToBoolean("false");
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("noreply@hifive.ae", "Hifive@123");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mailMessage);
                    return result;


                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }
            }
        }

        private async Task<string> createRegisterEmailBody(string Name, string eventName, string CompName, string startdate, string path, string adminName = "", string RegType = "")
        {
            string body = string.Empty;
            //Generating a token
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/" + path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullNAME]", Name);
            body = body.Replace("[Name]", CompName);
            body = body.Replace("[startdate]", startdate);
            if (!string.IsNullOrEmpty(adminName))
            {
                body = body.Replace("[adminName]", adminName);
            }
            if (!string.IsNullOrEmpty(eventName))
            {
                body = body.Replace("[Eventname]", eventName);
            }
            if (!string.IsNullOrEmpty(RegType))
            {
                body = body.Replace("[RegType]", RegType);
            }
            //return Ok();
            return body;
            // return Task.Delay(10000)
            //.ContinueWith(t => body); 
        }
        #endregion
    }
}
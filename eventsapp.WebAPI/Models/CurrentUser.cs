
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using EventsApp.Domain.DTOs;
using EventsApp.Framework;
using EventsApp.Domain.Entities;

namespace eventsapp.WebAPI.Models
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IOwinContext owinContext;
        private readonly IKeyedRepository<User, int> _userRepository;
        private UserInfo _user;
        public CurrentUser(IOwinContext owinContext, IKeyedRepository<User, int> _userRepository)
        {
            this.owinContext = owinContext;
            this._userRepository = _userRepository;
            this._user = null;
        }

        public UserInfo UserInfo
        {
            get
            {
                if (_user == null)
                {
                    var xx = HttpContext.Current.Request.GetOwinContext();
                    //TODO:Use the interface 
                    //var principal = owinContext.Authentication.User;
                    var principal = xx.Authentication.User;
                    using (var dbContext = new eventsappEntities())
                    {
                        var userObj = dbContext.Users.Where(x => x.Email == principal.Identity.Name).FirstOrDefault();
                        if (userObj != null)
                        {
                            var user = MapperHelper.Map<UserInfo>(userObj);
                            _user = user;
                        }
                    }
                }
                return _user;
            }
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            if (_user == null)
            {
                var context = HttpContext.Current.Request.GetOwinContext();
                //TODO:Use the interface 
                //var principal = owinContext.Authentication.User;
                var principal = context.Authentication.User;
                using (var dbContext = new eventsappEntities())
                {
                    var userObj = dbContext.Users.Where(x => x.Email == principal.Identity.Name).FirstOrDefault();
                    if (userObj != null)
                    {
                        var user = MapperHelper.Map<UserInfo>(userObj);
                        _user = user;
                    }
                }
            }
            return _user;
        }

        public async void SendEmail(string subject, string body)
        {
            if (_user == null)
            {
                var context = System.Web.HttpContext.Current.Request.GetOwinContext();
                var principal = context.Authentication.User;
                using (var dbContext = new eventsappEntities())
                {
                    var userObj = dbContext.Users.Where(x => x.Email == principal.Identity.Name).FirstOrDefault();
                    if (userObj != null)
                    {
                        var user = MapperHelper.Map<UserInfo>(userObj);
                        await context.GetUserManager<UserManager<ApplicationUser, int>>().SendEmailAsync(user.Id, subject, body);
                        _user = user;
                    }
                }
            }
        }
        public async void SendSMS(string PhoneNumber, string SMS)
        {
            if (_user == null)
            {
                var message = new IdentityMessage
                {
                    Destination = PhoneNumber,
                    Body = SMS
                };
                var context = System.Web.HttpContext.Current.Request.GetOwinContext();
                var usermgr = context.GetUserManager<Microsoft.AspNet.Identity.UserManager<ApplicationUser, int>>();

                if (usermgr != null)
                    await usermgr.SmsService.SendAsync(message);
                else
                {
                    string result = string.Empty;

                    //var GoogleReply = client.DownloadString(string.Format("http://121.241.242.114:8080/sendsms?username={0}&password={1}&type=0&dlr=0&destination={2}&source={3}&message={4}",
                    //    SMSUserName, SMSPassword, message.Destination.Replace("+", ""), SMSSourceId, message.Body));
                }
            }
        }
    }
}

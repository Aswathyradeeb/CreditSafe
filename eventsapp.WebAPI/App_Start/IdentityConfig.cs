using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using eventsapp.WebAPI.Models;
using System.Net.Mail;
using System.Net;
using EventsApp.Framework.EmailsSender;

namespace eventsapp.WebAPI
{
    public class EmailService : IIdentityMessageService
    {
        IMailSender _sender;
        public EmailService(IMailSender sender)
        {
            this._sender = sender;
        }

        Task IIdentityMessageService.SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.Factory.StartNew(() =>
            {
                _sender.SendEmailAsync("Hifive", "noreply@hifive.ae", message.Destination, message.Destination, message.Subject, message.Body);
            });
        }
    }

    public class EmailGroupService
    {
        IMailSender _sender;
        public EmailGroupService(IMailSender sender)
        {
            this._sender = sender;
        }

        public Task SendGroupAsync(IdentityMessage message, List<string> distinations)
        {
            // Plug in your email service here to send an email.
            foreach (var item in distinations)
            {
                _sender.SendEmailAsync("Hifive", "noreply@hifive.ae", item, item, message.Subject, message.Body);
            }
            return Task.FromResult(0);
        }
    }


    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int,
    UserLogin, UserRole, UserClaim>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 7,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser, int>
            {
                //TODO:this should be template
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser, int>
            {
                //TODO:this should be template
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EmailService(new MailSender());
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("EmailConfirmation"))
                {
                    TokenLifespan = TimeSpan.FromMinutes(5)
                };
            }
            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, int> roleStore) : base(roleStore)
        {
        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<ApplicationRole, int, UserRole>(context.Get<ApplicationDbContext>()));
        }
    }
}

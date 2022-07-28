using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using eventsapp.WebAPI.Models;
using System.Security.Claims;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using eventsapp.WebAPI.Controllers;
using System.Linq;
using EventsApp.Framework;
using EventsApp.Domain.Enums;

namespace eventsapp.WebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            try{
                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
       
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (user.IsActive == false)
            {
                context.SetError("user_locked", "The user is locked");
                return;
            }
            if (!user.EmailConfirmed)
            {
                context.SetError("email_not_confirmed", "Email Not Confirmed");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            await userManager.UpdateAsync(user);


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));
            AuthenticationProperties properties = CreateProperties(user.UserName, user.FirstName, user.LastName, user.EmailConfirmed, user.PhoneNumber, user.PhoneNumberConfirmed, user.Roles.FirstOrDefault().RoleId, user.Id, user.CompanyId, user.EventId, user.RegistrationTypeId, user.PersonId);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception e)
            {

            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string firstName, string lastName, bool emailConfirmed, string phoneNumber, bool phoneNumberConfirmed, int roleName, int userId, int? companyId, int? eventId,int? registrationTypeId, int? personId )
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "firstName", firstName },
                { "lastName", lastName },
                { "emailConfirmed", emailConfirmed.ToString() },
                { "phoneNumber", phoneNumber!= null ? phoneNumber.ToString() : "null" },
                { "phoneNumberConfirmed", phoneNumberConfirmed.ToString() },
                { "roleName", roleName.ToString() },
                { "userId", userId.ToString() },
                { "companyId", companyId!= null ? companyId.ToString() : "null" },
                { "eventId", eventId!= null ? eventId.ToString() : "null"  },
                { "registrationTypeId", registrationTypeId!= null ? registrationTypeId.ToString() : "null"  },
                { "personId", personId!= null ? personId.ToString() : "null" }
            };
            return new AuthenticationProperties(data);
        }
    }
}
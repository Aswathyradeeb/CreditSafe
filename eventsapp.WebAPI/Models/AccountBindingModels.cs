using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Athlete;

namespace eventsapp.WebAPI.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        //[Required]
        public string PhoneNumber { get; set; }
        public string Response { get; set; } 
        public string Photo { get; set; }
        public string PhotoFullPath { get; set; }

        public int EventId { get; set; }
        public int? AgendaId { get; set; }
        public string CompanyCode { get; set; }
        public int RegistrationTypeId { get; set; }
        public Nullable<int> AssignedFoodVouchers { get; set; }
        public Nullable<int> AssignedDrinkVouchers { get; set; }
        public Nullable<int> GuestOf { get; set; }

        //public string SendEmail { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int? UserRoleId { get; set; }

        public string UserRole { get; set; }
        public string SelectedLang { get; set; }
        public int eventPackageId { get; set; }
        public bool IsIndividual { get; set; }
        public int VisitorCount { get; set; }
        public string Relation { get; set; }
        public ICollection<PreferredLanguageDto> PreferredLanguages { get; set; }
        //public ICollection<AthleteVoucherDto> AthleteVouchers { get; set; }
    }

    public class EmailViewModel
    {
        public string EmailId { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class UserTokenModel
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Token { get; set; } 
    }

    public class SendEmailViewModel : EmailViewModel
    {
        public int UserId { get; set; } 
    }

    public class SMSViewModel
    {
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string SMS { get; set; }

    }
    public class SendSMSViewModel : SMSViewModel
    {
        public int UserId { get; set; } 
        public string PhoneNumber { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class PasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }
    }
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
    }
     

    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordWithTokenViewModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
    public class ResetPasswordAdminViewModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; } 
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }

    public class ConfigureAdminModel
    {
        public string Email { get; set; } 
        public int UserId { get; set; }
        public Nullable<int> NoOfEvents { get; set; }
        public bool IsApproved { get; set; }
    }
}

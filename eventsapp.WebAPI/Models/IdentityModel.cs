using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eventsapp.WebAPI.Models
{
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
       
         
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Photo { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public string QrImage { get; set; }
        public Nullable<int> AssignedFoodVouchers { get; set; }
        public Nullable<int> AssignedDrinkVouchers { get; set; }
        public Nullable<int> GuestOf { get; set; }
        public Nullable<int> UserCode { get; set; }
        public string Relation { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserRole : IdentityUserRole<int>
    {
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public class UserLogin : IdentityUserLogin<int>
    {
    }


    public class UserStore : UserStore<ApplicationUser, ApplicationRole, int,
        UserLogin, UserRole, UserClaim>
    {
        public UserStore(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class RoleStore : RoleStore<ApplicationRole, int, UserRole>
    {
        public RoleStore(ApplicationDbContext context) : base(context)
        {
        }
    }
    public class ApplicationRole : IdentityRole<int, UserRole>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base() { Name = name; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
    UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext()
            : base("IdentityEntities")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // You can globally assign schema here
            modelBuilder.HasDefaultSchema("Account");

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
        }
    }

    public class ApplicationRoleDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
    public class UserRoleDto
    {
        public List<ApplicationRoleDto> ApplicationRole { get; set; }
        public int? CompanyId { get; set; }
    }
}
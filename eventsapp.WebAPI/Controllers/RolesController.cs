 
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Eventsapp.Services;
using eventsapp.WebAPI.Models;
using EventsApp.Domain.DTOs;

namespace eventsapp.WebAPI.Controllers
{
   // [Authorize] 
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        private readonly IUserService _userService;

        public RolesController(IUserService userService)
        {
            this._userService = userService;
        }

        public RolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IUserService userService)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            this._userService = userService;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [Route("Create")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(roleViewModel.Name);

                // Save the new Description property: 
                role.NameEn = roleViewModel.Name;
                role.NameAr = roleViewModel.Name;
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return Ok();
                }
                return Ok(ModelState);
            }
            return Ok();
        }

        [Route("GetAll")]
        // [Authorize(Roles = "Director, Employee")]
        public async Task<IHttpActionResult> GetAll()
        {
            if (ModelState.IsValid)
            {
                var roleStore = new RoleStore<ApplicationRole, int, UserRole>(new ApplicationDbContext());
                var roleMngr = new ApplicationRoleManager(roleStore);

                var roles = roleMngr.Roles.ToList();
                roles.ForEach(x => x.Users.Clear());
                return Ok(roles);
            }
            return Ok();
        }

        [Authorize(Roles = "Director, Employee")]
        [Route("Get")]
        public async Task<IHttpActionResult> Get(string searchtext = "", int page = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            try
            {
                var roleStore = new RoleStore<ApplicationRole, int, UserRole>(new ApplicationDbContext());
                var roleMngr = new ApplicationRoleManager(roleStore);

                IEnumerable<ApplicationRole> roles = roleMngr.Roles.ToList();

                var pagedRecord = new PagedList();
                pagedRecord.TotalRecords = roles.Count();
                pagedRecord.Content = roles.ToList();
                pagedRecord.CurrentPage = page;
                pagedRecord.PageSize = pageSize;

                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetAllUserRoles")]
        public async Task<IHttpActionResult> GetAllUserRoles(int userId)
        {
            try
            {
                var roleStore = new RoleStore<ApplicationRole, int, UserRole>(new ApplicationDbContext());
                var roleMngr = new ApplicationRoleManager(roleStore);
                List<ApplicationRole> userRoles = roleMngr.Roles.Where(u => u.Users.Where(y => y.UserId == userId).Any()).ToList();
                //  List<ApplicationRole> allRoles = roleMngr.Roles
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return null;
        }

        //  [Authorize(Roles = "Director")]
        [Route("UpdateUserRoles")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserRoles(UserRoleDto model)
        {
            var context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, UserLogin, UserRole, UserClaim>(context));

            try
            {
                var userId = model.ApplicationRole[0].UserId;
                var userRoles = await userManager.GetRolesAsync(userId);
                var deletedRoles = userRoles.Where(x => model.ApplicationRole.FirstOrDefault(y => y.FullName == x) == null).ToList();

                foreach (var item in deletedRoles.Where(x => x != "Admin" && x != "Director"))
                {
                    await userManager.RemoveFromRoleAsync(userId, item);
                }

                var addedRoles = model.ApplicationRole.Where(x => userRoles.FirstOrDefault(y => y == x.FullName) == null).ToList();

                foreach (var item in addedRoles)
                {
                    userManager.AddToRole(item.UserId, item.FullName);
                }

               

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[Authorize(Roles = "Director, Admin")]
        //[Route("UpdateEmployeeGroup")]
        //[HttpPost]
        //public async Task<IHttpActionResult> UpdateEmployeeGroup(UserProfileDto model)
        //{
        //    var context = new ApplicationDbContext();
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    try
        //    {
        //        var userRoles = await UserManager.GetRolesAsync(model.UserId);
        //        foreach (var item in userRoles)
        //        {
        //            await UserManager.RemoveFromRoleAsync(model.UserId, item);
        //        }

        //        var addedRoles = (await groupService.GetByNameAsync(model.Group.NameEn)).GroupsRoles;

        //        foreach (var item in addedRoles)
        //        {
        //            UserManager.AddToRole(model.UserId, item.Role.Name);
        //        }

        //        userProfilUAEMatrixIIDS.UpdateUserGroup(model);

        //        await context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return InternalServerError();
        //    }
        //}


        [Route("GetUserRoles")]
        public async Task<IHttpActionResult> GetUserRoles(int userId)
        {
            try
            {
                var roleStore = new RoleStore<ApplicationRole, int, UserRole>(new ApplicationDbContext());
                var roleMngr = new ApplicationRoleManager(roleStore);
                List<ApplicationRole> roles = roleMngr.Roles.Where(u => u.Users.Any(y => y.UserId == userId)).ToList();

                if (roles.Count > 0)
                {
                    var userRoles = roles.Select(x => new RoleInfo { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn, Name = x.Name });
                    var user = await _userService.GetByUserId(userId);
                    return Ok(new { UserRoles = userRoles });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
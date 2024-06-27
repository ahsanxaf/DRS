using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using DRS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DRS.Extensions;

namespace DRS.Controllers
{

    public class BaseController : Controller
    {
        protected readonly DRSdbContext dbContext;

        public BaseController(DRSdbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var dbUser = dbContext.Users.Include(u => u.Role).ThenInclude(r => r.Permissions)
                                             .FirstOrDefault(u => u.Email == emailClaim);

            if (dbUser != null)
            {
                /*ViewBag.LoggedInUserPermissions = dbUser.Role.Permissions.ToList();*/
                HttpContext.Session.Set("LoggedInUserPermissions", dbUser?.Role?.Permissions.ToList());
            }

            if (emailClaim != null)
            {
                var userDetails = dbContext.Users.FirstOrDefault(u => u.Email == emailClaim);
                if (userDetails != null)
                {
                    ViewBag.LoggedInUserDetails = userDetails;
                }
            }
        }
    }
}

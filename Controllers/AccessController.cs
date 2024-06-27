using DRS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DRS.Services;
using BCrypt.Net;

namespace DRS.Controllers
{
    public class AccessController : Controller
    {
        private readonly DRSdbContext dbContext;
        private readonly IUserLogService _userLogService;

        public AccessController(DRSdbContext dbContext, IUserLogService userLogService)
        {
            this.dbContext = dbContext;
            _userLogService = userLogService;
        }
        public IActionResult SignIn()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(User modelLogin)
        {
            try
            {
                var userCredentials = dbContext.Users
                                               .Include(u => u.Role)
                                               .FirstOrDefault(model => model.Email == modelLogin.Email);

                if (userCredentials != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(modelLogin.Password, userCredentials.Password))
                    {

                        if (userCredentials.AccountStatus == "Active")
                        {
                            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                            var userBrowser = HttpContext.Request.Headers["User-Agent"].ToString();
                            var logInTime = DateTime.UtcNow;

                            HttpContext.Session.SetString("UserId", userCredentials.UserId.ToString());

                            List<Claim> claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.NameIdentifier, userCredentials.UserId.ToString()),
                                new Claim(ClaimTypes.Email, userCredentials.Email),
                                new Claim(ClaimTypes.Role, userCredentials.Role.RoleName),
                                new Claim(ClaimTypes.Name, userCredentials.Name),

                            };

                            foreach (var permission in userCredentials.Role.Permissions)
                            {
                                if (permission.UserModule == true) claims.Add(new Claim("Permission", "UserModule"));
                                if (permission.DocumentCategoryModule == true) claims.Add(new Claim("Permission", "DocumentCategoryModule"));
                                if (permission.LoginAuditModule == true) claims.Add(new Claim("Permission", "LoginAuditModule"));
                                if (permission.DocumentModule == true) claims.Add(new Claim("Permission", "DocumentModule"));
                                if (permission.SearchModule == true) claims.Add(new Claim("Permission", "SearchModule"));
                                if (permission.BackupModule == true) claims.Add(new Claim("Permission", "BackupModule"));
                                if (permission.AddPermission == true) claims.Add(new Claim("Permission", "AddPermission"));
                                if (permission.DeletePermission == true) claims.Add(new Claim("Permission", "DeletePermission"));
                                if (permission.ViewPermission == true) claims.Add(new Claim("Permission", "ViewPermission"));
                            }

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            AuthenticationProperties properties = new AuthenticationProperties()
                            {
                                AllowRefresh = true,
                                IsPersistent = false,
                            };

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity), properties);

                            var userLog = new UserLog
                            {
                                UserId = userCredentials.UserId,
                                LoginStatus = "Success",
                                UserIp = userIp,
                                UserBrowser = userBrowser,
                                LogInTime = logInTime,
                            };
                            await _userLogService.LogUserActivity(userLog);

                            //HttpContext.Session.SetString("UserSession", "Active");

                            return RedirectToAction("Index", "Home");
                        }
                        else if (userCredentials.AccountStatus == "Deactivated")
                        {
                            ViewData["ValidateMessage"] = "Your account has been deactivated. Please contact with the Admin.";

                            var userLog = new UserLog
                            {
                                UserId = userCredentials.UserId,
                                LoginStatus = "Deactivated",
                                UserIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                                UserBrowser = HttpContext.Request.Headers["User-Agent"].ToString(),
                                LogInTime = DateTime.UtcNow
                            };
                            await _userLogService.LogUserActivity(userLog);

                            return View();
                        }
                        else
                        {
                            ViewData["ValidateMessage"] = "Invalid Email or Password";

                            var userLog = new UserLog
                            {
                                UserId = userCredentials.UserId,
                                LoginStatus = "Failed",
                                UserIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                                UserBrowser = HttpContext.Request.Headers["User-Agent"].ToString(),
                                LogInTime = DateTime.UtcNow
                            };
                            await _userLogService.LogUserActivity(userLog);
                        }

                    }


                }

                ViewData["ValidateMessage"] = "Invalid Email or Password";

            }
            catch (Exception ex)
            {
                ViewData["ValidateMessage"] = "Invalid Email or Password";
            }

            return View();
        }

    }
}
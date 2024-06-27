using DRS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using DRS.Services;
using System.Security.Claims;

namespace DRS.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserLogService _userLogService;

        public HomeController(DRSdbContext context, IWebHostEnvironment webHostEnvironment, IUserLogService userLogService)
            : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            _userLogService = userLogService;
        }

        public IActionResult Index()
        {
            var Counts = dbContext.Versions.Count();
            ViewBag.Counts = Counts;

            var totalversion = dbContext.Versions.Count();
            ViewBag.TotalVersion = totalversion;

            var total_doc = dbContext.Documents.Count();
            ViewBag.TotalDoc = total_doc;

            var total_am = dbContext.Versions.Count(d => d.VersionNo != 1);
            ViewBag.Amendments = total_am;

          /*  var rule = dbContext.Documents.Count(Document => Document.CategoryId == 7);
            ViewBag.Rule = rule;*/

            var rules = dbContext.Documents.Where(d => d.Subcategory.SubcategoryName == "Rules").Count();
            var constitution = dbContext.Documents.Where(d => d.Subcategory.SubcategoryName == "Constitution").Count();
            var ordinance = dbContext.Documents.Where(d => d.Subcategory.SubcategoryName == "Ordinance").Count();
            var presidentorder = dbContext.Documents.Where(d => d.Subcategory.SubcategoryName == "President Order").Count();
            ViewBag.Constitutions = constitution;
            ViewBag.Presidentorder = presidentorder;
            ViewBag.Ordinances = ordinance;
            ViewBag.Rule = rules;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            var user = HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var userLog = await _userLogService.GetActiveLogForUser(userId);
                    if (userLog != null)
                    {
                        userLog.LogOutTime = DateTime.UtcNow;
                        await _userLogService.UpdateUserLog(userLog);
                    }
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn", "Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

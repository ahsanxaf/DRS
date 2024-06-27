using DRS.Models;
using DRS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DRS.Controllers
{
    [Authorize]
    public class UserLogController : Controller
    {
        private readonly IUserLogService _userLogService;
        private readonly ILogService _logService;
        private readonly DRSdbContext _context;

        public UserLogController(IUserLogService userLogService, DRSdbContext context, ILogService logService)
        {
            _userLogService = userLogService;
            _context = context;
            _logService = logService;
        }

        public async Task<IActionResult> UserLog()
        {
            var userLogs = await _userLogService.GetAllUserLogs();

            var model = userLogs
                .OrderBy(log => log.LogsId)
                .Select(log => new UserLogViewModel
                {
                    LogsId = log.LogsId,
                    UserName = log.user?.Name,
                    LoginStatus = log.LoginStatus,
                    UserIp = log.UserIp,
                    UserBrowser = log.UserBrowser,
                    LogInTime = log.LogInTime?.ToLocalTime(),
                    LogOutTime = log.LogOutTime?.ToLocalTime()
                })
                .ToList();

            return View(model);
        }

        public async Task<IActionResult> Logs()
        {
            var logs = await _logService.GetAllLogs();

            var model = logs
                .Select(log => new LogViewModel
                {
                    LogId = log.LogId,
                    UserId = log.UserId,
                    UserName = log.User?.Name, 
                    Action = log.Action,
                    DocumentId = log.DocumentId,
                    DocumentTitle = log.Document?.DocTitle,
                    Timestamp = log.Timestamp
                })
                .ToList();

            return View(model);
        }


    }
}

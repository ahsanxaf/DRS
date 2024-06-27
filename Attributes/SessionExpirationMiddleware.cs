using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DRS.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;

public class SessionExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public SessionExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceScopeFactory serviceScopeFactory)
    {
        if (context.Session.IsAvailable && !context.User.Identity.IsAuthenticated && context.Session.Keys.Any())
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var userLogService = scope.ServiceProvider.GetRequiredService<IUserLogService>();
                var userIdClaim = context.Session.GetString("LoggedInUserId");

                if (int.TryParse(userIdClaim, out int userId))
                {
                    var userLog = await userLogService.GetActiveLogForUser(userId);
                    if (userLog != null)
                    {
                        userLog.LogOutTime = DateTime.UtcNow;
                        await userLogService.UpdateUserLog(userLog);
                    }
                }
            }
        }

        await _next(context);
    
    }
}

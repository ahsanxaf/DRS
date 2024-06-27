using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using DRS.Models;
using Microsoft.EntityFrameworkCore;
using DRS;

public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _permissions;

    public CustomAuthorizeAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new ForbidResult();
            return;
        }

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<DRSdbContext>();
        var userEmail = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var dbUser = dbContext.Users
                              .Include(u => u.Role)
                              .ThenInclude(r => r.Permissions)
                              .FirstOrDefault(u => u.Email == userEmail);

        if (dbUser == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var userPermissions = dbUser.Role.Permissions.FirstOrDefault();

        if (userPermissions == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        foreach (var permission in _permissions)
        {
            var property = userPermissions.GetType().GetProperty(permission);
            if (property == null || property.PropertyType != typeof(bool?))
            {
                context.Result = new ForbidResult();
                return;
            }

            var hasPermission = (bool?)property.GetValue(userPermissions) == true;
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}

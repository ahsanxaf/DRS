using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using DRS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using DRS.Services;
using DRS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on specific ports
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5026); // HTTP port
    serverOptions.ListenAnyIP(7150, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS port
    });
});

// Add services to the container
builder.Services.AddDbContext<DRSdbContext>(ConfigureDbContextOptions);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Access/SignIn";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUserLogService, UserLogService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseMiddleware<SessionExpirationMiddleware>();

// Configure localization
var supportedCultures = new[]
{
    new CultureInfo("en-US")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=SignIn}/{id?}");
app.MapControllerRoute(
    name: "GetVersionData",
    pattern: "Version/GetVersionData/{versionId}", // Adjust the route pattern as needed
    defaults: new { controller = "Version", action = "GetVersionData" }
);

app.Run();

void ConfigureDbContextOptions(DbContextOptionsBuilder options)
{
    var connectionString = builder.Configuration.GetConnectionString("DRSdbContext");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DRSdbContext' not found.");
    }

    options.UseSqlServer(connectionString);
}

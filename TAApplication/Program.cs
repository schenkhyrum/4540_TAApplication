/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Code to bootup the application. Setups database, identities, and roles. Also adds a special policy for the 
    application details page, which only allows professors, admins, and applicant u0000000 to accesss the page.
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PS9");
builder.Services.AddDbContext<TAApplicationContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<TAUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TAApplicationContext>();
builder.Services.AddControllersWithViews();

//Creates authorization policy only allowing admins, professors, and user u0000000. Modified from
//https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-2.1#using-a-func-to-fulfill-a-policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApplicantDetailsPolicy", policy =>
        policy.RequireAssertion(context => context.User.IsInRole("Admin")
        || context.User.IsInRole("Professor") || context.User.HasClaim(c => c.Value == "u0000000@utah.edu")));
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                     builder.Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });

var app = builder.Build();

// seed database if needed
using (var scope = app.Services.CreateScope())
{
    var DB = scope.ServiceProvider.GetRequiredService<TAApplicationContext>();
    var um = scope.ServiceProvider.GetRequiredService<UserManager<TAUser>>();
    var rm = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DB.InitializeUsers(um, rm);
    await DB.InitializeUserApplications(um);
    await DB.InitializeCourses();
    await DB.InitializeAvailability(um);
    await DB.InitializeEnrollmentsOverTime();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

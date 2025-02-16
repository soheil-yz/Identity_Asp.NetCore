﻿using Identity.Data;
using Identity.Helper;
using Identity.Models.Entities;
using Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Identity.Helper.AddMyClaims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataBaseContext>(e => e.UseSqlServer("Data Source=SOHEIL\\SQL2022;Initial Catalog=Identity2_DB;Integrated Security=true;TrustServerCertificate=True"));
builder.Services.AddIdentity<Users, Roles>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddRoles<Roles>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<MyPasswordValidator>();

builder.Services.AddScoped<EmailService>();
// Setting Identity
builder.Services.Configure<IdentityOptions>(O =>
{
    //O.User.AllowedUserNameCharacters = "abcd"
    O.User.RequireUniqueEmail = true;  //email is Uniq
    O.Password.RequireDigit = false;
    O.Password.RequireLowercase = false;
    O.Password.RequireNonAlphanumeric = false;  //!$#^#@
    O.Password.RequireUppercase = false;
    //LockOut Setting
    O.Lockout.MaxFailedAccessAttempts = 3;
    O.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(30);
});
//
builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(10);   //after 10min auto logout
    //o.AccessDeniedPath = "Account/AccessDenied";
    o.SlidingExpiration = true;  //***
});

//*****
//builder.Services.AddScoped<IUserClaimsPrincipalFactory<Users>, AddMyClaims>();  وقتی میخوای قانون اضافه کنی به مشکل میخوری پس با روش دیگر کلیم اضافه میکنیم 
builder.Services.AddScoped<IClaimsTransformation, AddClaim>();
builder.Services.AddSingleton<IAuthorizationHandler, UserCreditHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsBlogForUserAuthorizationHandler>();
//****

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Buyer1", policy =>
    {
        policy.RequireClaim("Buyer");
    });    
    o.AddPolicy("AdminUsers", policy =>
    {
        policy.RequireRole("Admin");
    });
    o.AddPolicy("BloodType", policy =>
    {
        policy.RequireClaim("Blood", "Ap", "O");
    }); 
    o.AddPolicy("Credit", policy =>
    {
        policy.Requirements.Add(new UserCreditRequerment(10000));
    });    
    o.AddPolicy("IsBlogForYou", policy =>
    {
        policy.Requirements.Add(new BlogRequirement());
    });

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

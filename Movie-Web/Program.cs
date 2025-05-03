using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Services.Implementations;
using Movie.Application.Services.Interfaces;
using Movie.Data.Context;
using Movie.Data.Repositories;
using Movie.Domain.Contracts;
using Movie_Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(Option =>
    {
        Option.LoginPath = "/login";
        Option.LogoutPath = "/logout";
        Option.ExpireTimeSpan = TimeSpan.FromDays(15);

    });


builder.Services.AddDbContext<MyMovieContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("MyMovieConnection")) );

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService,UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CheckAdminMiddleware>();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

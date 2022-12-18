using AutoMapper;
using Forums.Core;
using Forums.Mappers;
using Forums.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//to configure the database connection
builder.Services.AddDbContext<ProjectContext>(
    config =>
    {
        config.UseSqlServer(builder.Configuration.GetConnectionString("dev_conn"));
    });

//to identify database
builder.Services.AddIdentity<Users, IdentityRole>(
   options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ProjectContext>();

//for mapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ForumMapper());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); 

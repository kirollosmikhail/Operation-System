using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Contexts;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Presentation_Layer.MappingProfiles;


namespace Presentation_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });// dependency injection

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>()
            {
                new EmployeeProfile(),
                new UserProfile(),
                new RoleProfile()
            }));
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true; // @ # $
                Options.Password.RequireDigit = true; // 12345
                Options.Password.RequireLowercase = true; // asd
                Options.Password.RequireUppercase = true; // ASD
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "Account/Login";
                options.AccessDeniedPath = "Home/Error";
            });
            var app = Builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

            app.Run();


        }


    }
}

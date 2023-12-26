namespace HotelChief
{
    using HotelChief.API;
    using HotelChief.API.Helpers;
    using HotelChief.Application.Services;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.Data;
    using HotelChief.Infrastructure.EFEntities;
    using HotelChief.Infrastructure.Repositories;
    using HotelChief.Infrastructure.UoW;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("HotelChiefdb");
            builder.Services.AddDbContext<Infrastructure.Data.ApplicationDbContext>(x => x.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddIdentity<Guest, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResource));
                };
            })
                .AddViewLocalization();
            builder.Services.AddAutoMapper();
            builder.Services.AddScoped(typeof(IBaseCRUDRepository<>), typeof(BaseCrudRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseCRUDService<>), typeof(BaseCRUDService<>));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "IsAdminPolicy",
                    policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "IsAdmin" && c.Value == "true"))));
            });
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                new CultureInfo("en-US"),
                new CultureInfo("uk-UA"),
                };
                options.DefaultRequestCulture = new RequestCulture("uk");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            var app = builder.Build();

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

            app.UseRequestLocalization();
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
        }
    }
}
namespace HotelChief
{
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using Hangfire;
    using HotelChief.API;
    using HotelChief.API.Helpers;
    using HotelChief.API.Hubs;
    using HotelChief.Application.IServices;
    using HotelChief.Application.Services;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.Data;
    using HotelChief.Infrastructure.EFEntities;
    using HotelChief.Infrastructure.Repositories;
    using HotelChief.Infrastructure.UoW;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.OAuth.Claims;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.CookiePolicy;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Telegram.Bot;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("HotelChiefdb");
            builder.Services.AddDbContext<Infrastructure.Data.ApplicationDbContext>(x => x.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddIdentity<Guest, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.ClientId = "oidcClient";
                    options.ClientSecret = "SuperSecretPassword";
                    options.SignInScheme = "Cookies";
                    options.SignOutScheme = "Cookies";
                   // options.CallbackPath = "/signin-oidc";
                    options.ResponseType = "code";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UsePkce = true;
                    options.ResponseMode = "query";
                    options.RequireHttpsMetadata = false;  // ONLY FOR DEVELOPMENT
                    // options.Scope.Add("profile");
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("api1.read");

                    options.SaveTokens = true;

                    options.ClaimActions.MapAll();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                    };
                });

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

            string telegramBotApiKey = builder.Configuration["TelegramBotApiKey"];
            long telegramBotRoomId = Convert.ToInt64(builder.Configuration["TelegramRoomId"]);
            builder.Services.AddSingleton<ITelegramBotService>(provider =>
            {
                var telegramBotClient = new TelegramBotClient(telegramBotApiKey);
                return new TelegramBotService(telegramBotClient, telegramBotRoomId);
            });
            builder.Services.AddScoped(typeof(IBaseCRUDRepository<>), typeof(BaseCrudRepository<>));
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseCRUDService<>), typeof(BaseCRUDService<>));
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IHotelServiceOrderHistoryService, HotelServiceOrderHistoryService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IHotelServiceOrderService, HotelServiceOrderService>();
            builder.Services.AddScoped<IRoomCleaningService, RoomCleaningService>();
            builder.Services.AddScoped<ILiqPayService, LiqPayService>();
            builder.Services.AddScoped(typeof(ILoyaltyService<>), typeof(LoyaltyService<>));
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IGuestRepository, GuestRepository>();
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
            builder.Services.AddSignalR();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "IsAdminPolicy",
                    policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "IsAdmin" && c.Value == "true"))));
                options.AddPolicy(
                    "IsEmployeePolicy",
                    policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "IsEmployee" && c.Value == "true"))));
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
            app.UseHangfireDashboard();

            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ReviewHub>("/reviewHub");
                endpoints.MapHub<RoomReservationHub>("/roomReservationHub");
                endpoints.MapHub<GuestHotelServiceOrderHub>("/guestHotelServiceOrderHub");
                endpoints.MapHub<EmployeeHotelServiceOrderHub>("/employeeHotelServiceOrderHub");
            });
            app.MapRazorPages();
            RecurringJob.AddOrUpdate<IRoomCleaningService>(x => x.ScheduleRoomCleaningAsync(), Cron.Daily);
            app.Run();
        }
    }
}
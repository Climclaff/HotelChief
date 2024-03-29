using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Services;
using HotelChief.IdentityProvider.Helpers;
using HotelChief.IdentityProvider.Mapping;
using IdentityModel;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityProvider
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            ConfigurationHelper.Initialize(Configuration);
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddHttpClient("RegistrationClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["ClientBaseAddress"]);
            }
            );
            services.AddRazorPages();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                
            })
                .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = Configuration["Authentication:Google:ClientId"]; ;
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            var identityServerBuilder = services.AddIdentityServer(options => options.KeyManagement.Enabled = true);
            
            /*identityServerBuilder.AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes());*/

            identityServerBuilder.AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();
            identityServerBuilder.AddProfileService<ProfileService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            InitializeDbTestData(app);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
        
        /// <summary>
        /// A small bootstrapping method that will run EF migrations against the database
        /// and create your test data.
        /// </summary>
        private async static void InitializeDbTestData(IApplicationBuilder app)
        {
            
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [PersistedGrants]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [IdentityResourceClaims]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [IdentityResourceProperties]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientProperties]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientClaims]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientScopes]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientGrantTypes]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ApiResourceSecrets]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ApiResourceScopes]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ApiResourceClaims]");            
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientRedirectUris]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientPostLogoutRedirectUris]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientScopes]");
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClientSecrets]");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM [IdentityResources]");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM [ApiScopes]");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM [ApiResources]");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM [Clients]");



                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }


                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Resources.GetApiScopes())
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in Users.Get())
                    {
                        var identityUser = new IdentityUser(testUser.Username)
                        {
                            Id = testUser.SubjectId
                        };

                        userManager.CreateAsync(identityUser, "121212qqQQ_").Wait();
                        userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                    }
                }
            }
        }
    }
}

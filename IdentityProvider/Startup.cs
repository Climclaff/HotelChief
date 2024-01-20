using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityProvider
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            const string connectionString = "Data Source=CLIMCLAFF\\SQLEXPRESS;Initial Catalog=IdentityTestDb;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddHttpClient("RegistrationClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:7049/");
            }
            );
            services.AddRazorPages();
            var identityServerBuilder = services.AddIdentityServer(options => options.KeyManagement.Enabled = true);
            
            /*identityServerBuilder.AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes());*/

            identityServerBuilder.AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

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

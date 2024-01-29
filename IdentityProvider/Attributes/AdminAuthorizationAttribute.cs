using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using IdentityProvider;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HotelChief.IdentityProvider.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizationAttribute : TypeFilterAttribute
    {
        public AdminAuthorizationAttribute() : base(typeof(AdminAuthorizationFilter))
        {
        }
    }

    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public AdminAuthorizationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = context.HttpContext.User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


                var isAdminClaim = dbContext.UserClaims.Any(uc =>
                    uc.UserId == userId &&
                    uc.ClaimType == "IsAdmin" &&
                    uc.ClaimValue == "true");

                if (!isAdminClaim)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }

}

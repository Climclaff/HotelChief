using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityProvider.Services
{
    public class ProfileService : DefaultProfileService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileService(ILogger<DefaultProfileService> logger, ApplicationDbContext dbContext) : base(logger)
        {
            _dbContext = dbContext;
        }
        public async override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _dbContext.Users.FindAsync(subjectId);
            var dbClaims = await _dbContext.UserClaims.Where(x => x.UserId == subjectId).ToListAsync();
            List<Claim> claims = new List<Claim>();
            foreach(var cl in dbClaims)
            {
                claims.Add(cl.ToClaim());
            }
            context.IssuedClaims = claims;
        }

        public async override Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.Claims.Single(c => c.Type == "sub"/*JwtClaimTypes.Subject*/).Value;
            var user = await _dbContext.Users.FindAsync(subjectId);

            context.IsActive = user != null;
        }
    }
}

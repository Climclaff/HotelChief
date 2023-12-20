namespace HotelChief.Infrastructure.EFEntities
{
    using Microsoft.AspNetCore.Identity;

    public class Guest : IdentityUser<int>
    {
        public string? FullName { get; set; }

        public bool? IsAdmin { get; set; }
    }
}

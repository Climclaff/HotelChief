namespace HotelChief.Core.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class Guest : IdentityUser<int>
    {
        public string? FullName { get; set; }
    }
}

using System;

namespace HotelChief.IdentityProvider.ViewModels
{
    public class IdentityUserViewModel
    {
        public string Id { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? UserName { get; set; }

        public string? NormalizedEmail { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string? PasswordHash { get; set; }

        public string? SecurityStamp { get; set; }

        public string? ConcurrencyStamp { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public string? NormalizedUserName { get; set; }

        public DateTime? LockoutEnd { get; set; }

        public bool? LockoutEnabled { get; set; }

        public int? AccessFailedCount { get; set; }

        public string? IsEmployee { get; set; }
    }
}
